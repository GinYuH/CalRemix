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
using CalRemix.Projectiles.Hostile;
using CalRemix.Items.Placeables;
using CalamityMod.Events;
using CalRemix.Biomes;
using CalamityMod.BiomeManagers;
using CalamityMod.Items.Materials;
using System;

namespace CalRemix.NPCs.Bosses.Ionogen
{
    [AutoloadBossHead]
    public class Ionogen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref Player Target => ref Main.player[NPC.target];

        public Rectangle teleportPos = new Rectangle();

        public static readonly SoundStyle HitSound = new("CalRemix/Sounds/IonogenHit", 3);
        public static readonly SoundStyle DeathSound = new("CalRemix/Sounds/CarcinogenDeath");

        public enum PhaseType
        {
            Idle = 0,
            LightningBurst = 1,
            Fall = 2,
            Magnet = 3
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ionogen");
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 100;
            NPC.width = 86;
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
            NPC.DeathSound = DeathSound;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/OncologicReinforcement");
            }
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SulphurousSeaBiome>().Type };
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<IonogenShield>(), ai0: NPC.whoAmI);
        }

        public override void AI()
        {
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
            switch (Phase)
            {
                case (int)PhaseType.Idle:
                    {
                        NPC.ai[1]++;
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 8;
                        if (NPC.ai[1] > 120)
                        {
                            Phase = (int)PhaseType.LightningBurst;
                            NPC.ai[1] = 0;
                        }
                    }
                    break;
                case (int)PhaseType.LightningBurst:
                    {
                        NPC.ai[1]++;
                        NPC.velocity *= 0.97f;
                        int lightningTime = 120; //CHANGE LIFETIME WITH THIS !!!
                        int telegraphTime = 60; //CHANGE IonogenLightning.cs AS WELL!!!
                        int startLightning = 30;
                        int lightningRate = lightningTime + 30;
                        int rounds = 4;
                        int phaseTime = rounds * lightningRate;
                        if (NPC.ai[1] > startLightning)
                        {
                            int totalProjectiles = 8;

                            float radians = MathHelper.TwoPi / totalProjectiles;
                            float velocity = 6f;
                            double angleA = radians * 0.5;
                            double angleB = MathHelper.ToRadians(90f) - angleA;
                            float velocityX2 = (float)(velocity * Math.Sin(angleA) / Math.Sin(angleB));
                            Vector2 spinningPoint = new Vector2(-velocityX2, -velocity);
                            spinningPoint.Normalize();
                            NPC.Calamity().newAI[2] = 200;

                            if (NPC.ai[2] % lightningRate == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    int type = ModContent.ProjectileType<IonogenLightning>();
                                    Vector2 spawnPoint = NPC.Center + new Vector2(-1f, 23f);
                                    SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.LightningSound, NPC.Center);
                                    for (int k = 0; k < totalProjectiles; k++)
                                    {
                                        Vector2 laserVelocity = spinningPoint.RotatedBy(radians * k + Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2));
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, laserVelocity, type, (int)(NPC.damage * 0.25f), 0f, Main.myPlayer, 0f, NPC.whoAmI);
                                    }
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(Target.Center), type, (int)(NPC.damage * 0.25f), 0f, Main.myPlayer, 0f, NPC.whoAmI);
                                }
                            }
                            if (NPC.ai[2] % (lightningRate + telegraphTime) == 0)
                            {
                                SoundEngine.PlaySound(CalamityMod.NPCs.SupremeCalamitas.SupremeCalamitas.BrimstoneShotSound, NPC.Center);
                            }
                            if (CalamityUtils.AnyProjectiles(ModContent.ProjectileType<IonogenLightning>()) && NPC.ai[1] % 5 == 0)
                            {
                                SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, NPC.Center);
                            }
                            NPC.ai[2]++;
                        }   
                        if (NPC.ai[1] > phaseTime)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            Phase = (int)PhaseType.Fall;
                        }
                        RNGLightning();
                        break;
                    }
                case (int)PhaseType.Fall:
                    {
                        float maxFallSpeed = 16f;
                        float fallSpeed = 0.2f;
                        NPC.ai[1]++;
                        if (NPC.ai[1] > 240 || (Collision.IsWorldPointSolid(NPC.Bottom, true) && NPC.ai[1] > 60))
                        {
                            NPC.velocity = Vector2.Zero;
                            NPC.ai[2]++;
                        }
                        else
                        {
                            if (NPC.velocity.Y < maxFallSpeed)
                            {
                                NPC.velocity.Y += fallSpeed;
                            }
                        }
                        if (NPC.ai[1] > 360)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            Phase = (int)PhaseType.Idle;
                        }
                        RNGLightning();
                        break;
                    }
                case (int)PhaseType.Magnet:
                    {
                        break;
                    }
            }
        }

        public void RNGLightning()
        {
            int lightningChance = 100;
            if (Main.rand.NextBool(lightningChance))
            {
                SoundEngine.PlaySound(SoundID.Thunder, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), Target.Center + new Vector2(Main.rand.Next(-2000, 2001), -1000), new Vector2(0, 1), ModContent.ProjectileType<IonogenLightning>(), (int)(NPC.damage * 0.25f), 0f, Main.myPlayer, 0f, -1);
            }
        }

        public void DustExplosion()
        {
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement("After the Archwizard was dishonorably discharged from the war, he fell into a state of smoking and gambling. During a gambling night, he sealed himself inside of a chunk of asbestos to win a bet. He was never heard from again.")
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
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<EssenceofSunlight>(), 1, 4, 8);
        }
        public override void OnKill()
        {
            RemixDowned.downedIonogen = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool SpecialOnKill()
        {
            // work you stupid stupid
            RemixDowned.downedIonogen = true;
            CalRemixWorld.UpdateWorldBool();
            return false;
        }
    }
}
