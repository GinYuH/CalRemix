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
using CalamityMod.Projectiles.Enemy;
using Newtonsoft.Json.Serialization;
using CalRemix.UI;
using System.Linq;
using CalamityMod.NPCs.Perforator;
using CalamityMod.Projectiles.Summon;
using SubworldLibrary;
using CalRemix.Subworlds;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.NPCs.Crags;
using CalRemix.Items.Placeables.Relics;

namespace CalRemix.NPCs.Bosses.Pathogen
{
    [AutoloadBossHead]
    public class Pathogen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public Entity Target = null;

        public Rectangle teleportPos = new Rectangle();

        public static readonly SoundStyle HitSound = new("CalRemix/Sounds/CarcinogenHit", 3);
        public static readonly SoundStyle DeathSound = new("CalRemix/Sounds/CarcinogenDeath");

        public enum PhaseType
        {
            Idle = 0,
            SplittingBlood = 1,
            Mosquito = 2,
            Caltrops = 3,
            Grinder = 4
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pathogen");
            if (Main.dedServ)
                return;
            HelperMessage.New("Pathogen",
                "It seems through your efforts, you've awoken Pathogen! I hope you've been helping the viruses, or you'll be in a sick situation! and not in the good way!",
                "FannyAwooga",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
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
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToCold = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/MicrobicReinforcement");
            }
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PandemicPanicBiome>().Type };
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<PathogenShield>(), ai0: NPC.whoAmI);
        }

        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<PathogenShield>()))
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<PathogenShield>(), ai0: NPC.whoAmI);
            }
            NPC.TargetClosest();
            float lifeRatio = NPC.life / NPC.lifeMax;
            bool rev = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;
            bool master = Main.masterMode || BossRushEvent.BossRushActive;
            bool expert = Main.expertMode || BossRushEvent.BossRushActive;
            if (Target == null || !Target.active || (Target is NPC n && n.life <= 0) || (Target is Player && PandemicPanic.PandemicPanic.InvadersWinning))
            {
                Target = PandemicPanic.PandemicPanic.BioGetTarget(false, NPC);
            }
            NPC.Calamity().newAI[3] = 0;
            switch (Phase)
            {
                case (int)PhaseType.Idle:
                    {
                        if (ValidTarget())
                        {
                            NPC.velocity = NPC.DirectionTo(Target.Center) * 6;
                            NPC.ai[1]++;
                        }
                        else
                        {
                            NPC.ai[2]++;
                            if (NPC.ai[2] % 90 == 0)
                            {
                                NPC.velocity = Main.rand.NextVector2CircularEdge(12, 12);
                            }
                            else
                            {
                                NPC.velocity *= 0.98f;
                            }
                        }
                        if (NPC.ai[1] > 120)
                        {
                            Phase = (int)PhaseType.SplittingBlood;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                        }
                    }
                    break;
                case (int)PhaseType.SplittingBlood:
                    {
                        int fireRate = death ? 35 : rev ? 40 : 45;
                        int totalRounds = 5;
                        int phaseTime = (fireRate * totalRounds) + 180;
                        NPC.ai[1]++;
                        if (ValidTarget())
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 6;
                        if (NPC.ai[1] % fireRate == 0 && NPC.ai[1] < fireRate * totalRounds)
                        {
                            int firePoints = master ? 6 : 4;
                            int fireProjSpeed = master ? 10 : 8;
                            float variance = MathHelper.TwoPi / firePoints;
                            float fireRateMultiplier = 0.02f;
                            for (int i = 0; i < firePoints; i++)
                            {
                                SoundEngine.PlaySound(PerforatorHive.IchorShoot, NPC.Center);
                                Vector2 velocity = new Vector2(0f, fireProjSpeed);
                                velocity = velocity.RotatedBy(variance * i + NPC.ai[1] * fireRateMultiplier);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<PathogenCell1>(), (int)(0.25f * NPC.damage), 0, ai1: Main.rand.NextBool().ToInt());
                            }
                        }
                        if (NPC.ai[1] > phaseTime)
                        {
                            Phase = (int)PhaseType.Mosquito;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                        }
                        break;
                    }
                case (int)PhaseType.Mosquito:
                    {
                        if (ValidTarget())
                        {
                            int timeBeforeHome = 60;
                            int waitTime = 10;
                            int diveGate = waitTime + 30;
                            int maxTimeBeforeCollide = diveGate + 180;
                            int groundDist = 100;
                            int drillTime = 150;
                            int maxHomeTime = 300;
                            Vector2 destination = Target.Center - Vector2.UnitY * 400;
                            NPC.ai[1]++;
                            if (NPC.ai[1] > timeBeforeHome && NPC.ai[2] == 0)
                            {
                                NPC.damage = 0;
                                NPC.velocity = NPC.DirectionTo(destination) * 18;
                                if (NPC.Center.Distance(destination) < 10)
                                {
                                    NPC.velocity = Target.velocity;
                                    NPC.ai[2] = 1;
                                }
                                else if (NPC.ai[1] > maxHomeTime)
                                {
                                    NPC.ai[2] = 1;
                                    NPC.ai[3] = waitTime;
                                }
                            }
                            if (NPC.ai[2] == 1)
                            {
                                NPC.damage = 100;
                                NPC.ai[3]++;
                                if (NPC.ai[3] > diveGate)
                                {
                                    if (NPC.velocity.Y < 16)
                                    {
                                        NPC.velocity.Y += 0.4f;
                                    }
                                    if (Collision.IsWorldPointSolid(NPC.Bottom + Vector2.UnitY * groundDist) || NPC.ai[3] > maxTimeBeforeCollide)
                                    {
                                        SoundEngine.PlaySound(DespairStone.ChainsawStartSound, NPC.Center);
                                        SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, NPC.Center);
                                        NPC.velocity = Vector2.Zero;
                                        NPC.ai[2] = 2;
                                        NPC.ai[3] = 0;
                                    }
                                }
                                else if (NPC.ai[3] > waitTime)
                                {
                                    NPC.velocity = Vector2.Zero;
                                }
                                else
                                {
                                    NPC.Center = Vector2.Lerp(NPC.Center, destination, 0.8f);
                                }
                            }
                            if (NPC.ai[2] == 2)
                            {
                                NPC.ai[3]++;
                                if (NPC.ai[3] % 5 == 0)
                                {
                                    SoundEngine.PlaySound(CnidarianJellyfishOnTheString.SlapSound, NPC.Center);
                                    if (NPC.ai[3] % 15 == 0)
                                    {
                                        SoundEngine.PlaySound(PerforatorHeadMedium.DeathSound, NPC.Center);
                                    }
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(11f, 23f), ModContent.ProjectileType<PathogenBloodDrop>(), (int)(NPC.damage * 0.5f), 0f);
                                    if (NPC.ai[3] > drillTime)
                                    {
                                        Phase = (int)PhaseType.Caltrops;
                                        NPC.ai[1] = 0;
                                        NPC.ai[2] = 0;
                                        NPC.ai[3] = 0;
                                    }
                                }

                                for (int i = 0; i < 6; i++)
                                {
                                    int bloodLifetime = Main.rand.Next(22, 36);
                                    float bloodScale = Main.rand.NextFloat(0.6f, 0.8f);
                                    Color bloodColor = Color.Lerp(Color.Red, Color.DarkRed, Main.rand.NextFloat());
                                    bloodColor = Color.Lerp(bloodColor, new Color(51, 22, 94), Main.rand.NextFloat(0.65f));

                                    if (Main.rand.NextBool(20))
                                        bloodScale *= 2f;

                                    Vector2 bloodVelocity = -Vector2.UnitY.RotatedByRandom(0.81f) * Main.rand.NextFloat(11f, 23f);
                                    bloodVelocity.Y -= 90f;
                                    BloodParticle blood = new BloodParticle(NPC.Bottom + Vector2.UnitY * groundDist, bloodVelocity, bloodLifetime, bloodScale, bloodColor);
                                    GeneralParticleHandler.SpawnParticle(blood);
                                }
                                for (int i = 0; i < 4; i++)
                                {
                                    float bloodScale = Main.rand.NextFloat(0.2f, 0.33f);
                                    Color bloodColor = Color.Lerp(Color.Red, Color.DarkRed, Main.rand.NextFloat(0.5f, 1f));
                                    Vector2 bloodVelocity = -Vector2.UnitY.RotatedByRandom(0.9f) * Main.rand.NextFloat(9f, 14.5f);
                                    bloodVelocity.Y -= 10f;
                                    BloodParticle2 blood = new BloodParticle2(NPC.Bottom + Vector2.UnitY * groundDist, bloodVelocity, 20, bloodScale, bloodColor);
                                    GeneralParticleHandler.SpawnParticle(blood);
                                }
                            }
                        }
                        else
                        {
                            Phase = (int)PhaseType.Mosquito;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.ai[3] = 0;
                        }
                        break;
                    }
                case (int)PhaseType.Caltrops:
                    {
                        int fireRate = 40;
                        int totalRounds = death ? 4 : rev ? 3 : 2;
                        int phaseTime = totalRounds * fireRate + 360;
                        int projPerRound = 3;
                        float kb = 8;
                        float recoverySpeed = 0.3f;
                        NPC.ai[1]++;
                        if (NPC.ai[1] % fireRate == 0 && NPC.ai[1] < fireRate * totalRounds + 5)
                        {
                            SoundEngine.PlaySound(CalamityMod.Sounds.CommonCalamitySounds.ExoHitSound, NPC.Center);
                            for (int i = 0; i < projPerRound; i++)
                            {
                                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver4) * 12, ModContent.ProjectileType<PathogenCaltrop>(), (int)(NPC.damage * 0.5f), 0f);
                            }
                            NPC.position.Y += kb;
                        }
                        else if (NPC.ai[1] > fireRate * totalRounds + 5)
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                        else
                        {
                            NPC.position.Y -= recoverySpeed;
                        }
                        if (NPC.ai[1] > phaseTime)
                        {
                            NPC.ai[1] = 0;
                            Phase = (int)PhaseType.Grinder;
                        }
                        break;
                    }
                case (int)PhaseType.Grinder:
                    {
                        if (ValidTarget())
                        {
                            int fallSpeed = 12;
                            int phaseTime = 480;
                            int xChange = 1000;
                            int thornRate = death ? 22 : rev ? 25 : 27;
                            NPC.ai[1]++;
                            if (NPC.ai[1] == 1)
                            {
                                SoundEngine.PlaySound(DespairStone.ChainsawStartSound, NPC.Center);
                                NPC.velocity.X = 1;
                            }
                            int width = NPC.width;
                            int height = NPC.height;
                            Vector2 collisionTile = new Vector2(NPC.Center.X - (float)(width / 2), NPC.position.Y + (float)NPC.height - (float)height);
                            if (Collision.SolidCollision(collisionTile, width, height) || Target.position.Y - NPC.position.Y < -400)
                            {
                                if (NPC.ai[1] % thornRate == 0)
                                {
                                    SoundEngine.PlaySound(PerforatorHive.GeyserShoot, NPC.Center);
                                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Bottom, -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver4) * 12, ModContent.ProjectileType<PathogenBloodThorn>(), (int)(NPC.damage * 0.5f), 0f, -1, 0, Main.rand.NextFloat() * 0.5f + 0.6f);
                                }
                                if (NPC.velocity.Y > 0f)
                                {
                                    NPC.velocity.Y = 0f;
                                }
                                if ((double)NPC.velocity.Y > -0.2)
                                {
                                    NPC.velocity.Y -= 0.025f;
                                }
                                else
                                {
                                    NPC.velocity.Y -= 0.2f;
                                }
                                if (NPC.velocity.Y < -4f)
                                {
                                    NPC.velocity.Y = -4f;
                                }
                                for (int i = 0; i < 6; i++)
                                {
                                    int bloodLifetime = Main.rand.Next(22, 36);
                                    float bloodScale = Main.rand.NextFloat(0.6f, 0.8f);
                                    Color bloodColor = Color.Lerp(Color.Red, Color.DarkRed, Main.rand.NextFloat());
                                    bloodColor = Color.Lerp(bloodColor, new Color(51, 22, 94), Main.rand.NextFloat(0.65f));

                                    if (Main.rand.NextBool(20))
                                        bloodScale *= 2f;

                                    Vector2 bloodVelocity = -Vector2.UnitY.RotatedByRandom(0.81f) * Main.rand.NextFloat(11f, 23f);
                                    bloodVelocity.Y -= 80f;
                                    bloodVelocity = bloodVelocity.RotatedBy(MathHelper.PiOver4 * -Math.Sign(NPC.velocity.X));
                                    BloodParticle blood = new BloodParticle(NPC.Bottom, bloodVelocity, bloodLifetime, bloodScale, bloodColor);
                                    GeneralParticleHandler.SpawnParticle(blood);
                                }
                                for (int i = 0; i < 4; i++)
                                {
                                    float bloodScale = Main.rand.NextFloat(0.2f, 0.33f);
                                    Color bloodColor = Color.Lerp(Color.Red, Color.DarkRed, Main.rand.NextFloat(0.5f, 1f));
                                    Vector2 bloodVelocity = -Vector2.UnitY.RotatedByRandom(0.9f) * Main.rand.NextFloat(9f, 14.5f);
                                    bloodVelocity.Y -= 10f;
                                    bloodVelocity = bloodVelocity.RotatedBy(MathHelper.PiOver4 * Math.Sign(NPC.velocity.X));
                                    bloodVelocity.X *= -1;
                                    BloodParticle2 blood = new BloodParticle2(NPC.Bottom, bloodVelocity, 20, bloodScale, bloodColor);
                                    GeneralParticleHandler.SpawnParticle(blood);
                                }
                            }
                            else
                            {
                                if (NPC.velocity.Y < 0f)
                                {
                                    NPC.velocity.Y = 0f;
                                }
                                if ((double)NPC.velocity.Y < 0.1)
                                {
                                    NPC.velocity.Y += 0.025f;
                                }
                                else
                                {
                                    NPC.velocity.Y += 0.5f;
                                }
                            }
                            if (NPC.velocity.Y > fallSpeed)
                            {
                                NPC.velocity.Y = fallSpeed;
                            }
                            float turnpoint = Target.Center.X + xChange * Math.Sign(NPC.velocity.X);
                            bool left = Target.Center.X - turnpoint > 0;
                            if (left ? NPC.Center.X < turnpoint : NPC.Center.X > turnpoint)
                            {
                                SoundEngine.PlaySound(PerforatorHive.IchorShoot, NPC.Center);
                                NPC.velocity.X = -NPC.velocity.X;
                                int amt = 5;
                                for (int i = 0; i < amt; i++)
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(11f, 23f), ModContent.ProjectileType<PathogenBloodDrop>(), (int)(NPC.damage * 0.5f), 0f);
                            }
                            else
                            {
                                if (Math.Abs(NPC.velocity.X) < 36)
                                    NPC.velocity.X *= 1.05f;
                            }
                            if (NPC.ai[1] > phaseTime)
                            {
                                NPC.ai[1] = 0;
                                Phase = (int)PhaseType.Idle;
                            }
                            if (Target is Player player)
                            {
                                player.Calamity().infiniteFlight = true;
                            }
                        }
                        else
                        {
                            NPC.ai[1] = 0;
                            Phase = (int)PhaseType.Idle;
                        }
                        break;
                    }
            }
        }

        public bool ValidTarget()
        {
            return !(Target == null || !Target.active || (Target is NPC n && n.life <= 0));
        }

        public void DustExplosion()
        {
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement("This elemental's traits stick close to its creator; espionage. After an unfavorable encounter with Yharim's forces, Calamitas' remarks on Mona's tendencies soon doomed her to a fate of virality. All that remains of the archmagus is an imitation of her bread and butter.")
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
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<BloodSample>(), 1, 4, 8);
            npcLoot.AddIf(() => Main.masterMode || CalamityWorld.revenge, ModContent.ItemType<PathogenRelic>());
        }
        public override void OnKill()
        {
            RemixDowned.downedPathogen = true;
            if (!PandemicPanic.PandemicPanic.InvadersWinning)
                PandemicPanic.PandemicPanic.EndEvent();
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool SpecialOnKill()
        {
            // work you stupid stupid
            RemixDowned.downedPathogen = true;
            if (!PandemicPanic.PandemicPanic.InvadersWinning)
                PandemicPanic.PandemicPanic.EndEvent();
            CalRemixWorld.UpdateWorldBool();
            return false;
        }

        public override bool NeedSaving()
        {
            return true;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return !PandemicPanic.PandemicPanic.InvadersWinning;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.friendly && PandemicPanic.PandemicPanic.InvadersWinning)
                return false;
            return null;
        }
    }
}
