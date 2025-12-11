using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.World;
using CalamityMod.Particles;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Content.Items.Placeables;
using CalamityMod.Events;
using CalRemix.UI;
using System.Linq;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.NPCs.TownNPCs;
using CalRemix.Core.World;
using CalRemix.Content.Items.Weapons;
using CalRemix.Content.Items.Bags;
using CalRemix.Content.Items.Placeables.Trophies;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Lore;

namespace CalRemix.Content.NPCs.Bosses.Carcinogen
{
    [AutoloadBossHead]
    public class Carcinogen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref Player Target => ref Main.player[NPC.target];

        public Rectangle teleportPos = new Rectangle();

        public static readonly SoundStyle HitSound = new("CalRemix/Assets/Sounds/GenBosses/CarcinogenHit", 3);
        public static readonly SoundStyle DeathSound = new SoundStyle("CalRemix/Assets/Sounds/GenBosses/CarcinogenDeath") with { Volume = 0.8f };

        public enum PhaseType
        {
            Idle = 0,
            Slam = 1,
            FireBlender = 2,
            Charge = 3
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Carcinogen");
            if (Main.dedServ)
                return;
            HelperMessage.New("Carcinogen",
                "A giant floating chunk of asbestos with cigars orbitting it? Now I've seen it all...",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type)).SetHoverTextOverride("Indeed Fanny, Indeed.");
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 40;
            NPC.width = 86;
            NPC.height = 88;
            NPC.defense = 15;
            NPC.DR_NERD(0.3f);
            NPC.LifeMaxNERB(3000, 4000, 150000);
            double HPBoost = CalamityServerConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 40, 0, 0);
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.DeathSound = DeathSound;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToCold = true;
            if (!Main.dedServ)
                Music = CalRemixMusic.Carcinogen;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Core.Biomes.AsbestosBiome>().Type };
        }

        public override void OnSpawn(IEntitySource source)
        {
            CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromThis(), NPC.position, ModContent.NPCType<CarcinogenShield>(), ai0: NPC.whoAmI);
        }

        public override void AI()
        {
            // Handicaps
            Main.LocalPlayer.AddBuff(BuffID.Blackout, 22);
            Main.LocalPlayer.wingTime = 0;
            Main.LocalPlayer.mount.Dismount(Main.LocalPlayer); 
            if (Main.zenithWorld)
            {
                foreach (Player victim in Main.ActivePlayers) //fucking suffer
                {
                    if (victim.Calamity() != null)
                    {
                        victim.AddBuff(BuffID.Obstructed, 1);
                    }
                }
            }
            // Generic setup
            NPC.TargetClosest();
            float lifeRatio = NPC.life / NPC.lifeMax;
            bool rev = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            bool master = Main.masterMode || BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || BossRushEvent.BossRushActive;
            if (Target == null || Target.dead)
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
                // Move directly towards the player and teleport once
                case (int)PhaseType.Idle:
                    {
                        int tpDistX = 800; // The horizontal range centered around the player 
                        int tpDistY = 500; // The vertical range centered around the player
                        int beginTeleporting = lifeRatio < 0.9f ? 90 : 180; // When Carcinogen should start teleporting
                        int teleportTelegraphDuration = lifeRatio < 0.9f ? 90 : 120; // How long it takes for the teleport to actually occur 
                        int postTeleportDuration = lifeRatio < 0.9f ? 90 : 120; // How long Carcinogen remains in this attack until going to his next attack
                        int when2Teleport = beginTeleporting + teleportTelegraphDuration; // The exact moment Carcinogen teleports
                        int when2EndPhase = when2Teleport + postTeleportDuration; // When the phase should end
                        NPC.ai[1]++;
                        NPC.ai[2]++;
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 4; // Move directly towards the player
                        // Choose a teleport position
                        if (NPC.ai[1] == beginTeleporting || (NPC.ai[1] > beginTeleporting && teleportPos == default))
                        {
                            teleportPos = new Rectangle((int)(Target.Center.X + Main.rand.Next(-tpDistX, tpDistX)), (int)(Target.Center.Y + Main.rand.Next(-tpDistY, tpDistY)), NPC.width, NPC.height);
                        }
                        // Draw a dust telegraph for the teleport
                        if (NPC.ai[1] > beginTeleporting)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, DustID.Dirt);
                                Main.dust[d].noGravity = true;
                            }
                        }
                        // Teleport
                        if (NPC.ai[1] > when2Teleport)
                        {
                            DustExplosion();
                            NPC.position = new Vector2(teleportPos.X, teleportPos.Y);
                            DustExplosion();
                            NPC.ai[1] = 0;
                        }
                        // Go to slam attack
                        if (NPC.ai[2] > when2EndPhase)
                        {
                            Phase = (int)PhaseType.Slam;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.netUpdate = true;
                        }
                    }
                    break;
                // Slam into ceiling and spawn asbestos drop rain
                case (int)PhaseType.Slam:
                    {
                        NPC.velocity.X *= 0.95f;
                        NPC.ai[1]++;
                        int maxDist = 600; // Carcinogen automatically slams if he goes this far above the player
                        float fallSpeedRate = 0.3f; // How quickly Carcinogen falls before starting to rise
                        int fallMaxSpeed = 4; // Max fall speed
                        float riseSpeedRate = lifeRatio < 0.5f ? 0.4f : 0.3f; // How quickly Carcinogen rises
                        int riseMaxSpeed = 10; // Max speed at which Carcinogen rises
                        int speedB4Crash = 6; // Speed required for slam
                        int fallTime = lifeRatio < 0.5f ? 20 : 30; // How quickly it takes for Carcinogen to recover from slamming
                        // Rise behavior
                        if (NPC.ai[2] == 0)
                        {
                            // Fall downwards
                            if (NPC.ai[1] < fallTime)
                            {
                                if (NPC.velocity.Y < fallMaxSpeed)
                                {
                                    NPC.velocity.Y += fallSpeedRate;
                                }
                            }
                            // Start rising
                            else if (NPC.ai[1] >= fallTime)
                            {
                                if (NPC.velocity.Y > -riseMaxSpeed)
                                {
                                    NPC.velocity.Y -= riseSpeedRate;
                                }
                                if (NPC.velocity.Y < -speedB4Crash)
                                {
                                    // If Carcinogen hits a tile 10 blocks above the player or at the max distance, slam
                                    if ((Collision.IsWorldPointSolid(NPC.Top) && NPC.position.Y < Target.position.Y - 160) || NPC.position.Y < Target.position.Y - maxDist)
                                    {
                                        SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, NPC.Center);
                                        NPC.velocity.Y *= -0.7f;
                                        NPC.ai[2] = 1;
                                        NPC.Calamity().newAI[0] = NPC.position.Y;
                                    }
                                }
                            }
                        }
                        // Slam
                        else
                        {
                            NPC.ai[3]++;
                            int spacing = 16; // Spacing between each droplet
                            int speed = 10; // Droplet speed
                            int fireRate = 10; // How often a droplet is spawned
                            int time = death && NPC.Calamity().newAI[1] == 0 ? 50 : 120; // How long the droplet spawning lasts
                            // Spawn droplets in pairs, distance increasing with each pair
                            if (NPC.ai[3] % fireRate == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(NPC.Top.X + NPC.ai[3] * spacing, NPC.Calamity().newAI[0]), new Vector2(0, speed), ModContent.ProjectileType<AsbestosDrop>(), CalRemixHelper.ProjectileDamage(20, 50), 0f, Main.myPlayer, Target.whoAmI);
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(NPC.Top.X - NPC.ai[3] * spacing, NPC.Calamity().newAI[0]), new Vector2(0, speed), ModContent.ProjectileType<AsbestosDrop>(), CalRemixHelper.ProjectileDamage(20, 50), 0f, Main.myPlayer, Target.whoAmI);
                                }
                            }
                            NPC.velocity.Y *= 0.98f;
                            // Do the attack again in Death Mode, if it's been done twice or it's not Death Mode, go to the fire blender
                            if (NPC.ai[3] > time)
                            {
                                if (death)
                                {
                                    if (NPC.Calamity().newAI[1] == 0)
                                    {
                                        NPC.ai[1] = 0;
                                        NPC.ai[2] = 0;
                                        NPC.ai[3] = 0;
                                        NPC.Calamity().newAI[0] = 0;
                                        NPC.Calamity().newAI[1]++;
                                    }
                                    else
                                    {
                                        Phase = (int)PhaseType.FireBlender;
                                        NPC.ai[1] = 0;
                                        NPC.ai[2] = 0;
                                        NPC.ai[3] = 0;
                                        NPC.Calamity().newAI[0] = 0;
                                        NPC.Calamity().newAI[1] = 0;
                                    }
                                }
                                else
                                {
                                    Phase = (int)PhaseType.FireBlender;
                                    NPC.ai[1] = 0;
                                    NPC.ai[2] = 0;
                                    NPC.ai[3] = 0;
                                    NPC.Calamity().newAI[0] = 0;
                                }
                            }
                        }
                        break;
                    }
                // Shoot spinning flames around itself
                case (int)PhaseType.FireBlender:
                    {
                        float playerDistance = (NPC.Center - Target.Center).Length(); //Distance to the player (used for scaling speed)
                        float normalSpeed = 4 * playerDistance / 300; // Speed Carcinogen moves before shooting fire
                        float fireSpeed = 2 * playerDistance / 300; // Speed Carcinogen moves while shooting fire
                        int firePoints = death ? 5 : 4; // How many points of fire Carcinogen shoots out
                        float fireProjSpeed = death ? 13 : rev ? 11 : 10; // Fire projectile speed, effectively range
                        float fireRateMultiplier = 0.01f; // Makes the spacing between the points sane
                        int projType = ProjectileID.Flames; // Burn baby burn!
                        NPC.ai[1]++;
                        // Start firing once the player is less than 300 pixels away or 2 seconds have passed
                        if (NPC.ai[1] > 120 || NPC.Distance(Target.Center) < 300)
                        {
                            if (NPC.ai[2] == 0)
                            {
                                SoundEngine.PlaySound(CalamityMod.Items.Weapons.Ranged.DragonsBreath.WeldingStart, NPC.Center);
                                NPC.ai[2] = 1;
                            }
                        }
                        NPC.velocity = NPC.DirectionTo(Target.Center) * (NPC.ai[2] == 1 ? fireSpeed : normalSpeed);
                        float variance = MathHelper.TwoPi / firePoints;
                        // Shoot out streams of fire or harmless smoke if telegraph not done yet
                        if (NPC.ai[1] % 5 == 0)
                        {
                            if (NPC.ai[2] == 1)
                            {
                                SoundEngine.PlaySound(CalamityMod.Items.Weapons.Ranged.DragonsBreath.FireballSound with { MaxInstances = 20 }, NPC.Center);
                            }
                            for (int i = 0; i < firePoints; i++)
                            {
                                Vector2 velocity = new Vector2(0f, fireProjSpeed);
                                velocity = velocity.RotatedBy(variance * i + NPC.ai[1] * fireRateMultiplier);
                                if (NPC.ai[2] == 1)
                                {
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, projType, (int)(0.25f * NPC.damage), 0, Main.myPlayer);
                                        Main.projectile[p].friendly = false;
                                        Main.projectile[p].hostile = true;
                                        Main.projectile[p].DamageType = DamageClass.Default;
                                        Main.projectile[p].tileCollide = false;
                                    }
                                }
                                else
                                {
                                    // We're beating the HeavySmokeParticle allegations with this one
                                    // Barely
                                    TimedSmokeParticle smoke = new TimedSmokeParticle(NPC.Center, (velocity * 6).RotatedByRandom(MathHelper.PiOver4 / 8), new Color(0.01f, 0.01f, 0.01f), new Color(0.01f, 0.01f, 0.01f), Main.rand.NextFloat(0.8f, 1.6f), 125, 10);
                                    GeneralParticleHandler.SpawnParticle(smoke);
                                }
                            }
                        }
                        // Go to charge attack
                        if (NPC.ai[1] > 480)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            Phase = (int)PhaseType.Charge;
                        }
                    }
                    break;
                // Dash while spawning exploding cigars
                case (int)PhaseType.Charge:
                    {
                        int spinTime = 60; // How long Carcinogen telegraphs the dash   by spinning
                        int dashSpeed = 18; // The speed of the dash
                        int bombRate = 20; // How often a cigar is spawned
                        int phaseTime = spinTime + 90; // How long the attack lasts
                        NPC.ai[1]++;
                        // Spin
                        if (NPC.ai[1] < spinTime)
                        {
                            NPC.rotation -= 0.4f;
                            NPC.velocity *= 0.9f;
                        }
                        // Dash
                        else if (NPC.ai[1] == spinTime)
                        {
                            SoundEngine.PlaySound(CalamityMod.Items.Weapons.Melee.Murasama.Swing, NPC.Center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Cigar>(), CalRemixHelper.ProjectileDamage(30, 60), 0f, Main.myPlayer, Main.rand.NextBool().ToInt());
                            NPC.velocity = NPC.DirectionTo(Target.Center) * dashSpeed;
                            // Spawn a circle of cinders in M*ster mode
                            if (master)
                            {
                                int totalCinders = death ? 12 : rev ? 10 : 8;
                                int cinderSpeed = 16;
                                float variance = MathHelper.TwoPi / totalCinders;
                                for (int i = 0; i < totalCinders; i++)
                                {
                                    Vector2 velocity = new Vector2(0f, cinderSpeed);
                                    velocity = velocity.RotatedBy(variance * i);
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<CigarCinder>(), CalRemixHelper.ProjectileDamage(20, 50), 0, Main.myPlayer);
                                }
                            }
                        }
                        // Spawn cigars
                        else
                        {
                            NPC.rotation = NPC.velocity.ToRotation();
                            if (NPC.ai[1] % bombRate == 0)
                            {
                                SoundEngine.PlaySound(CalamityMod.Items.Weapons.Melee.Murasama.Swing, NPC.Center);
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Cigar>(), CalRemixHelper.ProjectileDamage(30, 60), 0f, Main.myPlayer, Main.rand.NextBool().ToInt());
                            }
                            // Change attack
                            if (NPC.ai[1] > phaseTime)
                            {
                                NPC.ai[1] = 0;
                                Phase = lifeRatio < 0.2f ? (int)PhaseType.Slam : (int)PhaseType.Idle;
                                NPC.rotation = 0;
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
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 3;
                SoundEngine.PlaySound(HitSound, NPC.Center);
            }
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void BossLoot(ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.DungeonDesertKey, 3);
            npcLoot.Add(ModContent.ItemType<CarcinogenTrophy>(), 10);
            npcLoot.AddConditionalPerPlayer(() => Main.expertMode, ModContent.ItemType<CarcinogenBag>());
            npcLoot.AddNormalOnly(ModContent.ItemType<Asbestos>(), 1, 216, 224);
            npcLoot.AddNormalOnly(ModContent.ItemType<FiberBaby>());
            npcLoot.AddNormalOnly(ModContent.ItemType<Chainsmoker>());
            npcLoot.AddNormalOnly(ModContent.ItemType<SoulofCarcinogen>());
            npcLoot.AddNormalOnly(ModContent.ItemType<CarcinogenMask>(), 7);
            npcLoot.AddIf(() => Main.masterMode || CalamityWorld.revenge, ModContent.ItemType<CarcinogenRelic>());
            npcLoot.AddConditionalPerPlayer(() => !RemixDowned.downedCarcinogen, ModContent.ItemType<KnowledgeCarcinogen>(), desc: DropHelper.FirstKillText);
        }
        public override void OnKill()
        {
            RemixDowned.downedCarcinogen = true;
            CalRemixWorld.UpdateWorldBool();
            if (!NPC.AnyNPCs(ModContent.NPCType<UNCANNY>()))
            {
                CalRemixHelper.SpawnNewNPC(NPC.GetSource_Death(), NPC.Center, ModContent.NPCType<UNCANNY>());
            }
        }

        public override bool CheckActive()
        {
            return Target == null || Target.dead || !Target.active;
        }
    }
}
