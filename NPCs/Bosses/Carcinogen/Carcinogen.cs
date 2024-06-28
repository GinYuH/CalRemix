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
using CalRemix.UI;
using System.Linq;
using CalRemix.Items.Placeables.Relics;
using CalRemix.NPCs.TownNPCs;
using CalRemix.World;

namespace CalRemix.NPCs.Bosses.Carcinogen
{
    [AutoloadBossHead]
    public class Carcinogen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref Player Target => ref Main.player[NPC.target];

        public Rectangle teleportPos = new Rectangle();

        public static readonly SoundStyle HitSound = new("CalRemix/Sounds/CarcinogenHit", 3);
        public static readonly SoundStyle DeathSound = new("CalRemix/Sounds/CarcinogenDeath");

        public enum PhaseType
        {
            Idle = 0,
            Slam = 1,
            FireBlender = 2,
            Charge = 3
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carcinogen");
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
            NPC.LifeMaxNERB(6000, 8000, 300000);
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
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToWater = true;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToCold = true;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot("CalRemix/Sounds/Music/OncologicReinforcement");
            }
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Biomes.AsbestosBiome>().Type };
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<CarcinogenShield>(), ai0: NPC.whoAmI);
        }

        public override void AI()
        {
            Main.LocalPlayer.AddBuff(BuffID.Blackout, 22);
            Main.LocalPlayer.wingTime = 0;
            Main.LocalPlayer.mount.Dismount(Main.LocalPlayer);
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
                        int tpDistX = 800;
                        int tpDistY = 500;
                        int beginTeleporting = lifeRatio < 0.9f ? 90 : 180;
                        int teleportTelegraphDuration = lifeRatio < 0.9f ? 90 : 120;
                        int postTeleportDuration = lifeRatio < 0.9f ? 90 : 120;
                        int when2Teleport = beginTeleporting + teleportTelegraphDuration;
                        int when2EndPhase = when2Teleport + postTeleportDuration;
                        NPC.ai[1]++;
                        NPC.ai[2]++;
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 4;
                        if (NPC.ai[1] == beginTeleporting || (NPC.ai[1] > beginTeleporting && teleportPos == default))
                        {
                            teleportPos = new Rectangle((int)(Target.Center.X + Main.rand.Next(-tpDistX, tpDistX)), (int)(Target.Center.Y + Main.rand.Next(-tpDistY, tpDistY)), NPC.width, NPC.height);
                        }
                        if (NPC.ai[1] > beginTeleporting)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, DustID.Dirt);
                                Main.dust[d].noGravity = true;
                            }
                        }
                        if (NPC.ai[1] > when2Teleport)
                        {
                            DustExplosion();
                            NPC.position = new Vector2(teleportPos.X, teleportPos.Y);
                            DustExplosion();
                            NPC.ai[1] = 0;
                        }
                        if (NPC.ai[2] > when2EndPhase)
                        {
                            Phase = (int)PhaseType.Slam;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.netUpdate = true;
                        }
                    }
                    break;
                case (int)PhaseType.Slam:
                    {
                        NPC.velocity.X *= 0.95f;
                        NPC.ai[1]++;
                        int maxDist = 600;
                        float fallSpeedRate = 0.3f;
                        int fallMaxSpeed = 4;
                        float riseSpeedRate = lifeRatio < 0.5f ? 0.4f : 0.3f;
                        int riseMaxSpeed = 10;
                        int speedB4Crash = 6;
                        int fallTime = lifeRatio < 0.5f ? 20 : 30;
                        if (NPC.ai[2] == 0)
                        {
                            if (NPC.ai[1] < fallTime)
                            {
                                if (NPC.velocity.Y < fallMaxSpeed)
                                {
                                    NPC.velocity.Y += fallSpeedRate;
                                }
                            }
                            else if (NPC.ai[1] >= fallTime)
                            {
                                if (NPC.velocity.Y > -riseMaxSpeed)
                                {
                                    NPC.velocity.Y -= riseSpeedRate;
                                }
                                if (NPC.velocity.Y < -speedB4Crash)
                                {
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
                        else
                        {
                            NPC.ai[3]++;
                            int spacing = 16;
                            int speed = 10;
                            int fireRate = 10;
                            int time = death && NPC.Calamity().newAI[1] == 0 ? 50 : 120;
                            if (NPC.ai[3] % fireRate == 0)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(NPC.Top.X + NPC.ai[3] * spacing, NPC.Calamity().newAI[0]), new Vector2(0, speed), ModContent.ProjectileType<AsbestosDrop>(), (int)(NPC.damage * 0.25f), 0f, Main.myPlayer, Target.whoAmI);
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(NPC.Top.X - NPC.ai[3] * spacing, NPC.Calamity().newAI[0]), new Vector2(0, speed), ModContent.ProjectileType<AsbestosDrop>(), (int)(NPC.damage * 0.25f), 0f, Main.myPlayer, Target.whoAmI);
                            }
                            NPC.velocity.Y *= 0.98f;
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
                case (int)PhaseType.FireBlender:
                    {
                        int normalSpeed = 4;
                        int fireSpeed = 2;
                        int firePoints = death ? 5 : 4;
                        float fireProjSpeed = death ? 10 : rev ? 9 : 8;
                        float fireRateMultiplier = 0.02f;
                        int projType = NPC.ai[2] == 1 ? ProjectileID.Flames : ProjectileID.Flames;
                        NPC.ai[1]++;
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
                                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, projType, (int)(0.25f * NPC.damage), 0, Main.myPlayer);
                                    Main.projectile[p].friendly = false;
                                    Main.projectile[p].hostile = true;
                                    Main.projectile[p].DamageType = DamageClass.Default;
                                    Main.projectile[p].tileCollide = false;
                                }
                                else
                                {
                                    TimedSmokeParticle smoke = new TimedSmokeParticle(NPC.Center, (velocity * 6).RotatedByRandom(MathHelper.PiOver4 / 8), new Color(0.01f, 0.01f, 0.01f), new Color(0.01f, 0.01f, 0.01f), Main.rand.NextFloat(0.8f, 1.6f), 125, 10);
                                    GeneralParticleHandler.SpawnParticle(smoke);
                                }
                            }
                        }
                        if (NPC.ai[1] > 480)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            Phase = (int)PhaseType.Charge;
                        }
                    }
                    break;
                case (int)PhaseType.Charge:
                    {
                        int spinTime = 60;
                        int dashSpeed = 18;
                        int bombRate = 20;
                        int phaseTime = spinTime + 90;
                        NPC.ai[1]++;
                        if (NPC.ai[1] < spinTime)
                        {
                            NPC.rotation -= 0.4f;
                            NPC.velocity *= 0.9f;
                        }
                        else if (NPC.ai[1] == spinTime)
                        {
                            SoundEngine.PlaySound(CalamityMod.Items.Weapons.Melee.Murasama.Swing, NPC.Center);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Cigar>(), (int)(NPC.damage * 0.5f), 0f, Main.myPlayer, Main.rand.NextBool().ToInt());
                            NPC.velocity = NPC.DirectionTo(Target.Center) * dashSpeed;
                            if (master)
                            {
                                int totalCinders = death ? 12 : rev ? 10 : 8;
                                int cinderSpeed = 16;
                                float variance = MathHelper.TwoPi / totalCinders;
                                for (int i = 0; i < totalCinders; i++)
                                {
                                    Vector2 velocity = new Vector2(0f, cinderSpeed);
                                    velocity = velocity.RotatedBy(variance * i);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<CigarCinder>(), (int)(0.25f * NPC.damage), 0, Main.myPlayer);
                                }
                            }
                        }
                        else
                        {
                            NPC.rotation = NPC.velocity.ToRotation();
                            if (NPC.ai[1] % bombRate == 0)
                            {
                                SoundEngine.PlaySound(CalamityMod.Items.Weapons.Melee.Murasama.Swing, NPC.Center);
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Cigar>(), (int)(NPC.damage * 0.5f), 0f, Main.myPlayer, Main.rand.NextBool().ToInt());
                            }
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

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<Asbestos>(), 1, 216, 224);
            npcLoot.AddIf(() => Main.masterMode || CalamityWorld.revenge, ModContent.ItemType<CarcinogenRelic>());
        }
        public override void OnKill()
        {
            RemixDowned.downedCarcinogen = true;
            CalRemixWorld.UpdateWorldBool();
        }

        public override bool SpecialOnKill()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<UNCANNY>()))
            {
                NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<UNCANNY>());
            }
            // work you stupid stupid
            RemixDowned.downedCarcinogen = true;
            CalRemixWorld.UpdateWorldBool();
            return false;
        }
    }
}
