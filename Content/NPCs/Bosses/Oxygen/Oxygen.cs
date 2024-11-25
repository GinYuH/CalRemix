using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.World;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.Events;
using CalRemix.Core.Biomes;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalRemix.Content.Items.Materials;
using CalRemix.UI;
using System.Linq;
using System.IO;
using CalRemix.Content.Items.Placeables.Relics;
using CalRemix.Content.NPCs.TownNPCs;
using CalRemix.Core.World;
using CalRemix.Content.Items.Bags;
using CalRemix.Content.Items.Placeables.Trophies;
using CalRemix.Content.Items.Armor;
using CalRemix.Content.Items.Accessories;
using CalRemix.Content.Items.Lore;
using CalRemix.Content.Items.Weapons;

namespace CalRemix.Content.NPCs.Bosses.Oxygen
{
    [AutoloadBossHead]
    public class Oxygen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref float DepthLevel => ref NPC.Calamity().newAI[0];
        public ref float MaxDepthLevel => ref NPC.Calamity().newAI[2];

        public ref Player Target => ref Main.player[NPC.target];

        public Rectangle teleportPos = new Rectangle();

        public static readonly SoundStyle HitSound = new("CalRemix/Assets/Sounds/GenBosses/OxygenHit", 4);
        public static readonly SoundStyle FinalHitSound = new("CalRemix/Assets/Sounds/GenBosses/OxygenFinalHit", 4);
        public static readonly SoundStyle DeathSound = new("CalRemix/Assets/Sounds/GenBosses/OxygenDeath");
        public static readonly SoundStyle CrackSound = new("CalRemix/Assets/Sounds/GenBosses/OxygenCrack");
        public static readonly SoundStyle ShatterSound = new("CalRemix/Assets/Sounds/GenBosses/OxygenShatter");
        public static readonly SoundStyle AttackSound = new("CalRemix/Assets/Sounds/GenBosses/OxygenAttack", 4);

        public enum PhaseType
        {
            Idle = 0,
            Gusts = 1,
            Fling = 2,
            Bubbles = 3,
            Orbitals = 4
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oxygen");
            Main.npcFrameCount[Type] = 4;

            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;

            if (Main.dedServ)
                return;
            HelperMessage.New("Oxygen",
                "Oxygen is quite the fickle foe. Many have died foolishly trying to take a crack at it on the surface. However, that glass shell doesn't seem to be built for pressure. Try leading it to the bottom of the Abyss!",
                "FannyIdle",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
            
            HelperMessage.New("OxygenEvil",
                "Ok look, Fanny may be an imbecile, but if you're gonna take any words of his to heart it should be these. Leading it to the abyss is the only way you're defeating this idiotic ball.",
                "EvilFannyIdle",
                HelperMessage.AlwaysShow).SpokenByEvilFanny(true).ChainAfter(delay: 2f);

        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 100;
            NPC.width = 82;
            NPC.height = 88;
            NPC.defense = 99999999;
            NPC.Calamity().DR = 0.99999f;
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
            NPC.DeathSound = DeathSound;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToWater = false;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToHeat = true;
            if (!Main.dedServ)
                Music = CalRemixMusic.Oxygen;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<GaleforceDayBiome>().Type };
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.Calamity().newAI[0]);
            writer.Write(NPC.Calamity().newAI[1]);
            writer.Write(NPC.Calamity().newAI[2]);
            writer.Write(NPC.Calamity().newAI[3]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.Calamity().newAI[0] = reader.ReadSingle();
            NPC.Calamity().newAI[1] = reader.ReadSingle();
            NPC.Calamity().newAI[2] = reader.ReadSingle();
            NPC.Calamity().newAI[3] = reader.ReadSingle();
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<OxygenShield>(), ai0: NPC.whoAmI);
        }

        public override void AI()
        {
            // Generic boss setup
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
            // Decrement the cooldown for Oxygen summoning shards on hit
            NPC.Calamity().newAI[1]--;
            // Supply breath and infinite flight based on depth level
            foreach (Player p in Main.player)
            {
                if (p == null)
                    continue;
                if (!p.active)
                    continue;
                if (p.dead)
                    continue;
                p.Calamity().infiniteFlight = true;
                if (p.Distance(NPC.Center) < 640)
                {
                    p.breath += (int)MathHelper.Clamp(p.breathMax / 120 * (1 + MaxDepthLevel), -0.001f, p.breathMax - p.breath);
                }
            }
            // Transition to new phases based on abyss layer
            // During Boss Rush, Oxygen automatically acts as if it's in layer 4
            if (Target.Calamity().ZoneAbyssLayer4 || BossRushEvent.BossRushActive)
            {
                NPC.defense = 0;
                NPC.Calamity().DR = 0;
                if (MaxDepthLevel == 3)
                {
                    SoundEngine.PlaySound(SoundID.Shatter);
                    NPC.life -= (int)(NPC.lifeMax * 0.2f);
                    NPC.HitEffect(dmg: (int)(NPC.lifeMax * 0.2f));
                    Shard(18, true);
                }
                DepthLevel = 4;
                if (MaxDepthLevel < 4)
                MaxDepthLevel = 4;
            }
            else if (Target.Calamity().ZoneAbyssLayer3)
            {
                if (MaxDepthLevel == 2)
                {
                    SoundEngine.PlaySound(SoundID.Shatter);
                    NPC.life -= (int)(NPC.lifeMax * 0.15f);
                    NPC.HitEffect(dmg : (int)(NPC.lifeMax * 0.15f));
                    Shard(12, true);
                }
                DepthLevel = 3;
                if (MaxDepthLevel < 3)
                    MaxDepthLevel = 3;
            }
            else if (Target.Calamity().ZoneAbyssLayer2)
            {
                if (MaxDepthLevel == 1)
                {
                    SoundEngine.PlaySound(SoundID.Shatter);
                    NPC.life -= (int)(NPC.lifeMax * 0.1f);
                    NPC.HitEffect(dmg: (int)(NPC.lifeMax * 0.1f));
                    Shard(8, true);
                }
                DepthLevel = 2;
                if (MaxDepthLevel < 2)
                    MaxDepthLevel = 2;
            }
            else if (Target.Calamity().ZoneAbyssLayer1)
            {
                if (MaxDepthLevel == 0)
                {
                    SoundEngine.PlaySound(SoundID.Shatter);
                    NPC.life -= (int)(NPC.lifeMax * 0.05f);
                    NPC.HitEffect(dmg: (int)(NPC.lifeMax * 0.05f));
                    Shard(4, true);
                }
                DepthLevel = 1;
                if (MaxDepthLevel < 1)
                    MaxDepthLevel = 1;
            }
            else
            {
                DepthLevel = 0;
            }
            // Attacks 
            switch (Phase)
            {
                // Move towards player
                case (int)PhaseType.Idle:
                    {
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 5;
                        int phaseTime = 180;
                        NPC.ai[1]++;
                        // Use gust attack by default with an increasing chance to do bubbles depending on depth
                        if (NPC.ai[1] > phaseTime)
                        {
                            Phase = DepthLevel > 1 && Main.rand.NextBool(5 - (int)DepthLevel) ? (int)PhaseType.Bubbles : (int)PhaseType.Gusts;
                            NPC.ai[1] = 0;
                        }
                        break;
                    }
                // Summon damaging clouds from both sides of the player
                case (int)PhaseType.Gusts:
                    {
                        int spawnClouds = 90; // When the clouds should spawn
                        int phaseTime = spawnClouds + 240; // How long the phase lasts
                        NPC.ai[1]++;
                        if (NPC.ai[1] == spawnClouds)
                        {
                            int cloudAmt = 12; // Amount of clouds per side
                            int cloudSpacing = rev ? 480 : 400; // Spacing in pixels
                            int cloudDist = 1200; // Horizontal distance from the player
                            int cloudStart = 800 + Main.rand.Next(0, 64); // The origin height of the top cloud
                            float cloudSpeed = death ? 16 : rev ? 12 : 10; // Speed of the clouds
                            SoundEngine.PlaySound(AttackSound, NPC.Center);
                            for (int i = 0; i < cloudAmt / 2; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(Target.Center.X + cloudDist, Target.Center.Y - cloudStart + i * cloudSpacing), new Vector2(-cloudSpeed, 0), ModContent.ProjectileType<OxygenCloud>(), (int)(NPC.damage * 0.2f), 0f, Main.myPlayer, Main.rand.Next(0, TextureAssets.Cloud.Length - 1));
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(Target.Center.X - cloudDist, Target.Center.Y - cloudStart + i * cloudSpacing + cloudSpacing / 3), new Vector2(cloudSpeed, 0), ModContent.ProjectileType<OxygenCloud>(), (int)(NPC.damage * 0.2f), 0f, Main.myPlayer, Main.rand.Next(0, TextureAssets.Cloud.Length - 1));
                            }
                        }
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 4;
                        if (NPC.ai[1] > phaseTime)
                        {
                            // Previously a normal attack, sentenced to gfb
                            if (Main.zenithWorld)
                                Phase = (int)PhaseType.Fling;
                            else
                            {
                                // Use orbital attack by default with an increasing chance to do bubbles depending on depth
                                Phase = DepthLevel > 1 && Main.rand.NextBool(6 - (int)DepthLevel) ? (int)PhaseType.Bubbles : (int)PhaseType.Orbitals;
                            }
                            NPC.ai[1] = 0;
                        }
                        break;
                    }
                // Move towards the player while flinging them in random directions
                case (int)PhaseType.Fling:
                    {
                        NPC.ai[1]++;
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 5;
                        int flingStrength = 30; // The speed to toss the player
                        int flingRate = death ? 50 : rev ? 60 : expert ? 70 : 80; // How often to toss the player
                        int totalFlings = 3; // How many times the player is tossed
                        if (NPC.ai[1] % flingRate == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item43, NPC.Center);
                            // Increment the player's velocity
                            Target.velocity += Main.rand.NextVector2Circular(flingStrength, flingStrength);
                            NPC.ai[2]++;
                            DustExplosion(Target.velocity.SafeNormalize(Vector2.Zero));
                        }
                        if (NPC.ai[2] >= totalFlings + (totalFlings - 1))
                        {
                            NPC.ai[2] = 0;
                            NPC.ai[1] = 0;
                            Phase = (int)PhaseType.Orbitals;
                        }

                        break;
                    }
                // Spawn bubbles in random locations then dash at the player
                case (int)PhaseType.Bubbles:
                    {
                        int spawnBubbles = 60; // When to spawn bubbles after starting the attack
                        int bubbleAmt = 64; // Amount of bubbles to spawn
                        int bubbleRangeX = 1000; // The horizontal distance at which bubbles can spawn, centered on the player
                        int bubbleRangeY = 500; // The vertical distance at which bubbles can spawn, centered on the player
                        int phaseTime = spawnBubbles + 200; // How long the phase lasts
                        int dash = spawnBubbles + 90; // When Oxygen should dash
                        NPC.ai[1]++;
                        if (NPC.ai[1] == spawnBubbles)
                        {
                            SoundEngine.PlaySound(SoundID.NPCDeath45, Target.Center);
                            // Spawn the bubbles
                            for (int i = 0; i < bubbleAmt; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), Target.Center + new Vector2(Main.rand.Next(-bubbleRangeX, bubbleRangeX), Main.rand.Next(-bubbleRangeY, bubbleRangeY)), Vector2.Zero, ModContent.ProjectileType<OxygenBubble>(), (int)(NPC.damage * 0.2f), 0f, Main.myPlayer);
                            }
                        }
                        // Stop moving before dash and spin
                        if (NPC.ai[1] < dash)
                        {
                            NPC.velocity *= 0.97f;
                            NPC.rotation += 0.6f;
                        }
                        // Dash
                        else if (NPC.ai[1] == dash)
                        {
                            SoundEngine.PlaySound(CalamityMod.Items.Weapons.Melee.Murasama.Swing, NPC.Center);
                            NPC.rotation = NPC.velocity.ToRotation();
                            NPC.velocity = NPC.DirectionTo(Target.Center) * 20;
                            // Predictive in Master
                            if (master)
                                NPC.velocity += Target.velocity;
                        }
                        // Slow speed over time
                        else
                        {
                            NPC.rotation = NPC.velocity.ToRotation();
                            NPC.velocity *= 0.99f;
                        }
                        if (NPC.ai[1] > phaseTime)
                        {
                            NPC.rotation = 0;
                            NPC.ai[1] = 0;
                            Phase = (int)PhaseType.Fling;
                        }
                        break;
                    }
                // Stay in place and summon rounds of orbital debris
                case (int)PhaseType.Orbitals:
                    {
                        NPC.velocity *= 0.97f;
                        int spawnTornado = 60; // When to spawn the first set of debris
                        int totalObjects = death ? 16 : rev ? 10 : 8; // How many objects are in each round
                        int tornadoRate = rev ? 50 : expert ? 60 : 70; // Time between rounds
                        int totalTornados = death ? 5 : rev ? 4 : 3; // How many rounds should be done
                        int phaseTime = tornadoRate * totalTornados + spawnTornado; // How long the phase lasts
                        NPC.ai[1]++;
                        if (NPC.ai[1] > spawnTornado && NPC.ai[1] % tornadoRate == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                            int dir = Main.rand.NextBool().ToInt();
                            for (int i = 0; i < totalObjects; i++)
                            {
                                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<OxygenDebris>(), (int)(NPC.damage * 0.2f), 0, Main.myPlayer, i + 1, totalObjects, Main.rand.NextFloat(0, 4f));
                                Main.projectile[p].localAI[0] = Main.rand.Next(1, 5); // Controls which sprite is used
                                Main.projectile[p].localAI[1] = dir; // Controls if the debris moves clockwise or counter clockwise
                            }
                        }
                        if (NPC.ai[1] > phaseTime)
                        {
                            NPC.ai[1] = 0;
                            Phase = DepthLevel > 2 ? (int)PhaseType.Gusts : (int)PhaseType.Idle;
                        }
                        break;
                    }

            }
        }

        public void DustExplosion(Vector2 extraSpeed)
        {
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18) * extraSpeed;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement("In a magical blunder, this construct found itself a resident of the elemental family as an unwilling eye of her storm. While Tempest carried a plethora of cargo on Yharim's behalf, she slipped into her airstream which sparked a feedback loop outside her control."),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
            });
        }

        // Release shards and take no damage before reaching layer 4
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (DepthLevel < 4)
            {
                modifiers.SetMaxDamage(1);
                Shard(trans: false);
                NPC.life += 1;
            }
        }

        // Release shards and take no damage before reaching layer 4
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (DepthLevel < 4)
            {
                modifiers.SetMaxDamage(1);
                Shard(trans: false);
                NPC.life += 1;
            }
        }

        public void Shard(int amt = 10, bool ignoreCooldown = false, bool trans = true)
        {
            if (NPC.Calamity().newAI[1] <= 0 || ignoreCooldown)
            {
                int shardSpeed = Main.rand.Next(7, 11);
                for (int i = 0; i < amt; i++)
                {
                    if (trans)
                    {
                        SoundStyle crac = MaxDepthLevel == 4 ? ShatterSound : CrackSound;
                        SoundEngine.PlaySound(crac, NPC.Center);
                    }
                    Vector2 square = new Vector2(Main.rand.Next((int)NPC.position.X, (int)NPC.position.X + NPC.width), Main.rand.Next((int)NPC.position.Y, (int)NPC.position.Y + NPC.height));

                    if (Main.netMode != NetmodeID.Server)
                    {
                        int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), square, new Vector2(Main.rand.Next(-shardSpeed, shardSpeed), Main.rand.Next(-shardSpeed, shardSpeed)), ModContent.ProjectileType<Oxshard>(), (int)(NPC.damage * 0.5f), 0f, Main.myPlayer, ai1: Main.rand.Next(1, 7));
                        Main.projectile[p].scale = Main.rand.NextFloat(1f, 2f);
                    }
                }
                NPC.Calamity().newAI[1] = 40;
            }
        }


        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 3;
                SoundStyle hite = MaxDepthLevel == 4 ? FinalHitSound : HitSound;
                SoundEngine.PlaySound(hite, NPC.Center);
            }
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, hit.HitDirection, -1f, 0, default, 1f);
                }
                for (int i = 0; i < 20; i++)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Circular(NPC.width, NPC.height).SafeNormalize(Vector2.UnitY) * Main.rand.Next(4, 8), Mod.Find<ModGore>("OxygenShrap" + Main.rand.Next(1, 7)).Type);
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2Circular(NPC.width, NPC.height).SafeNormalize(Vector2.UnitY) * Main.rand.Next(4, 8), Mod.Find<ModGore>("Oxygen" + Main.rand.Next(1, 7)).Type);
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.AddNormalOnly(ModContent.ItemType<EssenceofBabil>(), 1, 8, 10);
            npcLoot.Add(ItemID.HallowedKey, 3);
            npcLoot.AddIf(() => Main.masterMode || CalamityWorld.revenge, ModContent.ItemType<OxygenRelic>());
            npcLoot.AddConditionalPerPlayer(() => Main.expertMode, ModContent.ItemType<OxygenBag>());
            npcLoot.Add(ModContent.ItemType<OxygenTrophy>(), 10);
            npcLoot.AddNormalOnly(ModContent.ItemType<OxygenMask>(), 7);
            npcLoot.AddNormalOnly(ModContent.ItemType<SoulofOxygen>());
            npcLoot.AddNormalOnly(ModContent.ItemType<ShardofGlass>());
            npcLoot.AddNormalOnly(ModContent.ItemType<Aerospray>());
            npcLoot.AddConditionalPerPlayer(() => !RemixDowned.downedOxygen, ModContent.ItemType<KnowledgeOxygen>(), desc: DropHelper.FirstKillText);
        }
        public override void OnKill()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<BALLER>()))
            {
                NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<BALLER>());
            }
            RemixDowned.downedOxygen = true;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                CalRemixWorld.oxydayTime = 0;
            }
            else
            {
                ModPacket packet = CalRemix.instance.GetPacket();
                packet.Write((byte)RemixMessageType.OxydayTime);
                packet.Write(0);
                packet.Send();
            }
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 drawPos = NPC.Center - screenPos;
            // Draws Oxygen's core which is just some bloom
            spriteBatch.EnterShaderRegion(BlendState.Additive);
            Texture2D plasmom = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomRing").Value;
            Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle").Value;
            float scaleFactor = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2) * 0.075f;
            spriteBatch.Draw(plasmom, drawPos, null, NPC.GetAlpha(Color.Cyan * 0.78f), NPC.rotation, plasmom.Size() / 2, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(bloom, drawPos, null, NPC.GetAlpha(Color.Cyan * 1f), NPC.rotation, bloom.Size() / 2, 0.6f + scaleFactor, SpriteEffects.None, 0f);
            spriteBatch.ExitShaderRegion();
            // Before layer 4, draw its normal shell which changes appearance based on depth
            if (MaxDepthLevel < 4)
            {
                spriteBatch.Draw(TextureAssets.Npc[Type].Value, drawPos, TextureAssets.Npc[Type].Frame(1, 4, 0, (int)MathHelper.Clamp(MaxDepthLevel, 0, 3)), NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(TextureAssets.Npc[Type].Value.Width / 2, TextureAssets.Npc[Type].Value.Height / 8), NPC.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
