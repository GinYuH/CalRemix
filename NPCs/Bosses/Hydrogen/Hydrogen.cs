using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.World;
using CalRemix.Projectiles.Hostile;
using CalamityMod.Events;
using CalamityMod.BiomeManagers;
using System;
using CalamityMod.Items.Placeables;
using CalRemix.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalRemix.UI;
using System.Linq;
using CalRemix.Items.Placeables.Relics;
using CalRemix.NPCs.TownNPCs;
using CalRemix.World;
using Terraria.Utilities;
using CalamityMod.Items.Fishing.SunkenSeaCatches;
using CalamityMod.Items.Placeables.Banners;
using CalRemix.Projectiles.Weapons;
using CalRemix.Items.Bags;
using CalRemix.Items.Placeables.Trophies;

namespace CalRemix.NPCs.Bosses.Hydrogen
{
    [AutoloadBossHead]
    public class Hydrogen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref Player Target => ref Main.player[NPC.target];

        public Rectangle teleportPos = new Rectangle();

        public static readonly SoundStyle HitSound = new("CalRemix/Sounds/IonogenHit", 3);
        public static readonly SoundStyle ExplosionSound = new("CalRemix/Sounds/HydrogenExplode");

        public static WeightedRandom<int> sunkenSeaFish = new WeightedRandom<int>();

        public enum PhaseType
        {
            Sealed = 0,
            Idle = 1,
            MissileLaunch = 2,
            Mines = 3,
            Death = 4
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrogen");

            // A bunch of fucking fish
            sunkenSeaFish.Add(ModContent.ItemType<PrismaticGuppy>());
            sunkenSeaFish.Add(ModContent.ItemType<Serpentuna>(), 0.5f); // Quest
            sunkenSeaFish.Add(ModContent.ItemType<SunkenSailfish>());
            sunkenSeaFish.Add(ModContent.ItemType<EutrophicSandfish>(), 0.5f); // Quest
            sunkenSeaFish.Add(ModContent.ItemType<GreenwaveLoach>(), 0.1f); // Money fish
            sunkenSeaFish.Add(ModContent.ItemType<SparklingEmpress>(), 0.05f); // Rare weapon
            sunkenSeaFish.Add(ModContent.ItemType<SurfClam>(), 0.5f); // Surf Clam <3
            sunkenSeaFish.Add(ModContent.ItemType<SerpentsBite>(), 0.1f); // Tool
            sunkenSeaFish.Add(ModContent.ItemType<PrismBackBanner>(), 0.01f); // Banners have greatly reduced weights
            sunkenSeaFish.Add(ModContent.ItemType<BlindedAnglerBanner>(), 0.01f);
            sunkenSeaFish.Add(ModContent.ItemType<GhostBellBanner>(), 0.01f);
            sunkenSeaFish.Add(ModContent.ItemType<BabyGhostBellBanner>(), 0.01f);
            sunkenSeaFish.Add(ModContent.ItemType<ClamBanner>(), 0.01f);
            sunkenSeaFish.Add(ModContent.ItemType<EutrophicRayBanner>(), 0.01f);
            sunkenSeaFish.Add(ModContent.ItemType<SeaFloatyBanner>(), 0.01f);
            sunkenSeaFish.Add(ModContent.ItemType<SeaMinnowBanner>(), 0.01f);

            if (Main.dedServ)
                return;
            HelperMessage.New("Hydrogen",
                "Hey! Do you see that spiky balloon chained over there? Word has it that hitting it with explosives will unleash a terrible evil, capable of destroying the entire sea! But something so destructive couldn't possibly exist, right?",
                "FannyNuhuh",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 150;
            NPC.width = 82;
            NPC.height = 88;
            NPC.defense = 15;
            NPC.DR_NERD(0.3f);
            NPC.LifeMaxNERB(40000, 48000, 300000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 40, 0, 0);
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.DeathSound = null;
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/AtomicReinforcement");
            }
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SunkenSeaBiome>().Type };
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<HydrogenShield>(), ai0: NPC.whoAmI);
        }

        public override void AI()
        {
            // Generic setup
            NPC.TargetClosest();
            float lifeRatio = NPC.life / NPC.lifeMax;
            bool rev = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            bool master = Main.masterMode || BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || BossRushEvent.BossRushActive;
            // If Hydrogen is at <= 1 health and tile destruction is enabled, do death animation
            if (NPC.life <= 1 && CalRemixWorld.hydrogenBomb && !BossRushEvent.BossRushActive)
            {
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                Phase = (int)PhaseType.Death;
            }
            else if (Target == null || Target.dead)
            {
                NPC.velocity.Y += 1;
                NPC.Calamity().newAI[3]++;
                if (NPC.Calamity().newAI[3] > 240)
                {
                    NPC.active = false;
                }
                return;
            }
            NPC.Calamity().newAI[3] = 0;
            // Attacks
            switch (Phase)
            {
                // Dormant...
                case (int)PhaseType.Sealed:
                    {
                        NPC.ai[1]++;
                        NPC.damage = 0;
                        NPC.boss = false;
                        NPC.velocity = Vector2.UnitY * (float)Math.Sin(NPC.ai[1] * 0.025f) * 0.25f;
                        NPC.chaseable = false;
                        // Automatically get pissed off if it somehow gets damaged despite the checks
                        if (lifeRatio < 0.9f || BossRushEvent.BossRushActive)
                        {
                            NPC.ai[1] = 0;
                            NPC.damage = 100;
                            NPC.boss = true;
                            NPC.chaseable = true;
                            Phase = (int)PhaseType.Idle;
                        }
                        break;
                    }
                // Move towards the player
                case (int)PhaseType.Idle:
                    {
                        int phaseTime = 90;
                        NPC.ai[1]++;
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 5;
                        if (NPC.ai[1] > phaseTime)
                        {
                            NPC.ai[1] = 0;
                            Phase = (int)PhaseType.MissileLaunch;
                        }
                        break;
                    }
                // Shoot out missiles puncutated by falling warheads
                case (int)PhaseType.MissileLaunch:
                    {
                        int rocketRate = 5; // Fire rate of projectiles
                        int fireDelay = 30; // Stagger barrages with this 
                        int rocketAmt = death ? 20 : rev ? 16 : expert ? 10 : 8; // Amount of rockets to be fired before stagger
                        int salvoAmount = 2; // Amount of rounds
                        float missileSpread = death ? 60f : 45f; // Spread
                        if (Main.masterMode)
                            salvoAmount *= 2;
                        NPC.ai[1]++;
                        NPC.velocity *= 0.95f;
                        if (NPC.ai[1] > fireDelay)
                        {
                            NPC.ai[2]++;
                            if (NPC.ai[2] % rocketRate == 0)
                            {
                                SoundEngine.PlaySound(CalamityMod.Items.Weapons.Ranged.Scorpio.RocketShoot);
                                {
                                    // The last few projectiles are gravity-affected warheads
                                    int type = NPC.ai[2] > (rocketAmt - 2) * rocketRate ? ModContent.ProjectileType<HydrogenWarhead>() : ModContent.ProjectileType<HydrogenShell>();
                                    Vector2 acidSpeed = (Vector2.UnitY * Main.rand.NextFloat(-10f, -8f)).RotatedByRandom(MathHelper.ToRadians(missileSpread));
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, acidSpeed, type, (int)(NPC.damage * 0.4f), 3f, Main.myPlayer, Target.whoAmI);
                                }
                                if (NPC.ai[2] > rocketAmt * rocketRate)
                                {
                                    NPC.ai[2] = 0;
                                    NPC.ai[1] = 0;
                                    NPC.ai[3]++; // Keeps track of how many salvos have been shot
                                }
                            }
                            // Transition after the max salvos have been met
                            if (NPC.ai[3] >= salvoAmount)
                            {
                                NPC.ai[1] = 0;
                                NPC.ai[2] = 0;
                                NPC.ai[3] = 0;
                                Phase = (int)PhaseType.Mines;
                            }
                        }
                        break;
                    }
                // Spawn an uneven row of mines from below that rise then explode
                case (int)PhaseType.Mines:
                    {
                        int whenToSummon = 90; // When the mines should spawn
                        int mineAmt = death ? 40 : rev ? 32 : 26; // Amount of mines
                        int mineRange = 4000; // Horizontal radius at which mines can spawn
                        int mineSpeed = 4; // Speed that the mines move up
                        int phaseTime = 360; // How long the phase lasts 
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 2; // Slowly move towards the player
                        NPC.ai[1]++;
                        // Summon the mines
                        if (NPC.ai[1] == whenToSummon)
                        {
                            SoundEngine.PlaySound(CalamityMod.NPCs.PlaguebringerGoliath.PlaguebringerGoliath.NukeWarningSound);
                            for (int i = 0; i < mineAmt; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), Target.Center + new Vector2(Main.rand.Next(-mineRange, mineRange), Main.rand.Next(400, 600)), Vector2.UnitY * -mineSpeed, ModContent.ProjectileType<HydrogenMine>(), (int)(NPC.damage * 0.5f), 0f, Main.myPlayer);
                            }
                        }
                        if (NPC.ai[1] > phaseTime)
                        {
                            NPC.ai[1] = 0;
                            Phase = (int)PhaseType.Idle;
                        }
                        break;
                    }
                // Nagasaki
                case (int)PhaseType.Death:
                    {
                        NPC.Calamity().newAI[1]++;
                        int doomsdayTimer = 870; // How long until Hydrogen's explosion ends
                        int spawnFridge = 120; // When the fridge spawns
                        int startExplosion = doomsdayTimer - 300; // When the actual explosion starts
                        int tikTok = startExplosion / 11; // Interval of when numbers appear
                        NPC.velocity *= 0.95f;
                        // Spawn the fridge
                        // Spawns to the left of the player if they're left of Hydrogen and right if they're on the right
                        if (NPC.Calamity().newAI[1] == spawnFridge)
                        {
                            bool playerLeft = Target.Center.X - NPC.Center.X < 0;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), Target.Center + new Vector2(playerLeft ? -400 : 400, -400), Vector2.UnitY * 4, ModContent.ProjectileType<Fridge>(), 0, 0f, Main.myPlayer, ai2: -1);
                        }
                        // Hiroshima
                        if (NPC.Calamity().newAI[1] > doomsdayTimer)
                        {
                            // Die
                            NPC.active = false;
                            NPC.HitEffect();
                            NPC.NPCLoot();

                            NPC.netUpdate = true;

                            // Destroy the Sunken Sea
                            CalRemixWorld.DestroyTheSunkenSea(NPC.Center / 16, 500);
                            foreach (Player p in Main.player)
                            {
                                if (p == null)
                                    continue;
                                if (!p.active)
                                    continue;
                                if (p.dead)
                                    continue;
                                // Spare anyone far away
                                if (p.Distance(NPC.Center) > 16 * 500)
                                    continue;
                                // Spare anyone hiding in a fridge
                                if (p.GetModPlayer<CalRemixPlayer>().fridge)
                                    continue;
                                // Else disentegrate {{sic}}
                                p.KillMe(PlayerDeathReason.ByCustomReason(p.name + " was atomized."), 9999, 1);
                            }

                            // Prevent netUpdate from being blocked by the spam counter.
                            if (NPC.netSpam >= 10)
                                NPC.netSpam = 9;

                            // Blast fishing
                            for (int i = 0; i < 44; i++)
                            {
                                Item.NewItem(NPC.GetSource_Death(), Main.rand.Next((int)Target.Center.X - 1000, (int)Target.Center.X + 1000), Main.rand.Next((int)Target.Center.Y - 1000, (int)Target.Center.Y + 500), 4, 4, sunkenSeaFish.Get());
                            }
                        }
                        // Kaboom
                        if (NPC.Calamity().newAI[1] == startExplosion)
                        {
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 222;
                            SoundEngine.PlaySound(ExplosionSound);
                        }
                        // Controls the intensity of the flash with it increasing rapidly 
                        if (NPC.Calamity().newAI[1] > startExplosion)
                        {
                            NPC.localAI[0] += 4.25f;
                        }
                        // Tick down, localai[1] is the amount of ticks so far
                        if ((10 - NPC.localAI[1]) > 0)
                        {
                            if (NPC.Calamity().newAI[1] % tikTok == 0)
                            {
                                SoundEngine.PlaySound(SoundID.Camera);
                                // The ticks grow increasingly more red until the end
                                CombatText.NewText(NPC.getRect(), Color.Lerp(Color.White, Color.Red, NPC.localAI[1] / 10), (int)(10 - NPC.localAI[1]));
                                NPC.localAI[1]++;
                            }
                        }
                        break;
                    }
            }
        }

        public void DustExplosion()
        {
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LunarRust, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement("This machination of Yharim's artillery proved to be a challenge to fuse both magic and science cohesively. While Ivy's soul comfortably sits outside the construct, Hydrogen's power resonates with her mana; down to its devastating explosion.")
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0 && Phase != (int)PhaseType.Death)
            {
                NPC.soundDelay = 3;
                SoundEngine.PlaySound(HitSound, NPC.Center);
            }
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LunarRust, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LunarRust, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<HydrogenTrophy>(), 10);
            npcLoot.AddConditionalPerPlayer(() => Main.expertMode, ModContent.ItemType<HydrogenBag>());
            npcLoot.AddNormalOnly(ModContent.ItemType<SeaPrism>(), 1, 8, 10);
            npcLoot.AddIf(() => Main.masterMode || CalamityWorld.revenge, ModContent.ItemType<HydrogenRelic>());
        }
        public override void OnKill()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<KABLOOEY>()))
            {
                NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<KABLOOEY>());
            }
            RemixDowned.downedHydrogen = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool SpecialOnKill()
        {
            // work you stupid stupid
            RemixDowned.downedHydrogen = true;
            CalRemixWorld.UpdateWorldBool();
            return false;
        }

        public override bool CheckDead()
        {
            if (CalRemixWorld.hydrogenBomb)
            {
                NPC.life = 1;
                NPC.Calamity().newAI[0] = 1;
                NPC.active = true;
                NPC.dontTakeDamage = true;

                NPC.netUpdate = true;

                // Prevent netUpdate from being blocked by the spam counter.
                if (NPC.netSpam >= 10)
                    NPC.netSpam = 9;
                return false;
            }
            return true;
        }

        public override bool CheckActive()
        {
            return NPC.Calamity().newAI[0] != 1;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (Phase == (int)PhaseType.Sealed || Phase == (int)PhaseType.Death)
                return false;
            return null;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<AsbestosDropFriendly>())
            {
                return null;
            }
            if ((Phase == (int)PhaseType.Sealed && !ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[projectile.type]) || Phase == (int)PhaseType.Death)
                return false;
            return null;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ModContent.ProjectileType<AsbestosDropFriendly>())
            {
                NPC.StrikeInstantKill();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Draws Hydrogen while in his inactive state
            if (Phase == (int)PhaseType.Sealed)
            {
                // Grab the position at which Hydrogen's chain should connect to
                Vector2 bottom = CalRemixWorld.hydrogenLocation != default && CalRemixWorld.hydrogenLocation != Vector2.Zero ? CalRemixWorld.hydrogenLocation : NPC.Center;
                // Don't draw if it's too far for those CHEATERS who spawn Hydrogen in other locations
                if (bottom == CalRemixWorld.hydrogenLocation && NPC.Distance(CalRemixWorld.hydrogenLocation) < 1000)
                {
                    bottom += new Vector2(10, 110);
                    Vector2 distToProj = NPC.Center;
                    float projRotation = NPC.AngleTo(bottom) - 1.57f;
                    bool doIDraw = true;
                    Texture2D texture = ModContent.Request<Texture2D>(Texture + "Chain").Value; //change this accordingly to your chain texture

                    while (doIDraw)
                    {
                        float distance = (bottom - distToProj).Length();
                        if (distance < (texture.Height + 1))
                        {
                            doIDraw = false;
                        }
                        else if (!float.IsNaN(distance))
                        {
                            Color drawColore = Lighting.GetColor((int)distToProj.X / 16, (int)(distToProj.Y / 16f));
                            distToProj += NPC.DirectionTo(bottom) * texture.Height;
                            Main.EntitySpriteDraw(texture, distToProj - Main.screenPosition,
                                new Rectangle(0, 0, texture.Width, texture.Height), drawColore, projRotation,
                                Utils.Size(texture) / 2f, 1f, SpriteEffects.None, 0);
                        }
                    }
                }
            }

            Texture2D tex = ModContent.Request<Texture2D>(Texture + "Goner").Value;
            Vector2 drawPos = NPC.Center - screenPos;
            // Shake
            if (NPC.localAI[1] > 0)
                drawPos += new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            spriteBatch.Draw(TextureAssets.Npc[Type].Value, drawPos, null, NPC.GetAlpha(drawColor), NPC.rotation, TextureAssets.Npc[Type].Value.Size() / 2, NPC.scale, SpriteEffects.None, 0f);
            // Enreden when ticking down
            // Small note: The usage of the word "Enreden" in the above comment was completely on accident and I wasn't thinking about Enreden (user) at all until after I wrote it lmao
            spriteBatch.Draw(tex, drawPos, null, Color.Red * (NPC.localAI[1] / 10), NPC.rotation, tex.Size() / 2, NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
