using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalamityMod.World;
using CalamityMod.Events;
using CalRemix.UI;
using System.Linq;

namespace CalRemix.Content.NPCs.Bosses.Origen
{
    [AutoloadBossHead]
    public class Origen : ModNPC
    {
        public ref float Phase => ref NPC.ai[0];

        public ref Player Target => ref Main.player[NPC.target];

        public Rectangle teleportPos = new Rectangle();

        public enum PhaseType
        {
            Idle = 0,
            IceRain = 1,
            Spin = 2
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Origen");
            this.HideFromBestiary();
            if (Main.dedServ)
                return;
            HelperMessage.New("Origen",
                "It appears you are being punished for home invasion!",
                "FannyNuhuh",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 24f;
            NPC.damage = 0;
            NPC.width = 192;
            NPC.height = 196;
            NPC.defense = 15;
            NPC.DR_NERD(0.3f);
            NPC.LifeMaxNERB(1000, 4800, 300000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.DeathSound = CalamityMod.NPCs.Cryogen.Cryogen.DeathSound;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = false;
            if (!Main.dedServ)
                Music = CalRemixMusic.Origen;
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
                // Move towards player while firing radial ice blasts and teleporting once
                case (int)PhaseType.Idle:
                    {
                        int fireRate = death ? 80 : rev ? 90 : 100; // Ice blast fire rate
                        int totalRounds = death ? 4 : rev ? 3 : 2; // How many ice blast rounds should be fired
                        int phaseTime = fireRate * (totalRounds) + 22; // How long the attack lasts
                        int tpDistX = 1000; // Horizontal teleport radius
                        int tpDistY = 500; // Vertical teleport radius
                        NPC.ai[1]++;
                        NPC.velocity = NPC.DirectionTo(Target.Center) * 3;
                        // "Prevent cheap bullshit"
                        if (NPC.ai[1] > 60)
                        {
                            NPC.damage = 100;
                        }
                        // Transition to next attack
                        if (NPC.ai[1] > phaseTime)
                        {
                            DustExplosion();
                            Phase = (int)PhaseType.IceRain;
                            NPC.position = teleportPos.Center.ToVector2();
                            DustExplosion();
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                        }
                        // Teleport
                        if (NPC.ai[1] == phaseTime - 120)
                        {
                            teleportPos = new Rectangle((int)(Target.Center.X + Main.rand.Next(-tpDistX, tpDistX)), (int)(Target.Center.Y + Main.rand.Next(-tpDistY, tpDistY)), NPC.width, NPC.height);
                        }
                        // Teleport telegraph
                        if (NPC.ai[1] > phaseTime - 120)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                int d = Dust.NewDust(new Vector2(teleportPos.X, teleportPos.Y), teleportPos.Width, teleportPos.Height, DustID.IceGolem);
                                Main.dust[d].noGravity = true;
                            }
                        }
                        // Fire ice blasts in a circle around itself
                        if (NPC.ai[1] % fireRate == 0 && NPC.ai[1] < fireRate * totalRounds + 22)
                        {
                            int firePoints = 4 + (int)NPC.ai[2]; // Each time it fires, 2 more blasts are added in the next round
                            int fireProjSpeed = master ? 8 : 6; // Ice blast speed
                            float variance = MathHelper.TwoPi / firePoints;
                            for (int i = 0; i < firePoints; i++)
                            {
                                SoundEngine.PlaySound(SoundID.Item28, NPC.Center);
                                Vector2 velocity = new Vector2(0f, fireProjSpeed);
                                velocity = velocity.RotatedBy(variance * i);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ProjectileID.FrostBlastHostile, (int)(0.25f * NPC.damage), 0);
                            }
                            NPC.ai[2] += 2;
                        }
                        NPC.rotation += 0.1f;
                    }
                    break;
                // Position itself over the player while raining icicles down
                case (int)PhaseType.IceRain:
                    {
                        int fireRate = 5; // The rate at which icicles spawn
                        int phaseTime = master ? 210 : 180; // Attack duration
                        Vector2 destination = Target.Center - Vector2.UnitY * 400; // Destination location
                        NPC.velocity = NPC.DirectionTo(destination) * 4;
                        NPC.ai[1]++;
                        // Shoot icicles
                        if (NPC.ai[1] % fireRate == 0)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.position.X + Main.rand.Next(0, NPC.width), NPC.Bottom.Y), new Vector2(0, 8), ProjectileID.FrostShard, (int)(0.25f * NPC.damage), 0);
                        }
                        if (NPC.ai[1] > phaseTime)
                        {
                            NPC.ai[1] = 0;
                            Phase = (int)PhaseType.Spin;
                        }
                        NPC.rotation = 0;
                        break;
                    }
                // Spin in place while spewing out projectiles
                case (int)PhaseType.Spin:
                    {
                        int phaseTime = master ? 240 : 200; // Attack duration
                        int fireRate = 5; // The rate at which projectiles are fired
                        int speed = master ? 10 : 6; // Projectile speed
                        NPC.ai[1]++;
                        // Fire projectiles in random directions
                        if (NPC.ai[1] % fireRate == 0)
                        {
                            int type = Main.rand.NextBool() ? ProjectileID.FrostShard : ProjectileID.FrostBlastHostile;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2Circular(speed, speed).SafeNormalize(Vector2.One) * speed, type, (int)(0.25f * NPC.damage), 0);
                        }
                        if (NPC.ai[1] > phaseTime)
                        {
                            NPC.ai[1] = 0;
                            Phase = (int)PhaseType.Idle;
                        }
                        NPC.velocity = Vector2.Zero;
                        NPC.rotation += 0.2f;
                        break;
                    }
            }
        }

        public void DustExplosion()
        {
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceGolem, Main.rand.NextFloat(-22, 22), Main.rand.NextFloat(-22, 22), Scale: Main.rand.NextFloat(0.8f, 2f));
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = (Main.dust[d].position - NPC.Center).SafeNormalize(Vector2.One) * Main.rand.Next(10, 18);
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 3;
                SoundEngine.PlaySound(CalamityMod.NPCs.Cryogen.Cryogen.HitSound, NPC.Center);
            }
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceGolem, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceGolem, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override bool PreKill()
        {
            // This hurts, but it makes it more accurate to old Cryogen
            CalRemixHelper.SpawnNewNPC(NPC.GetSource_Death(), NPC.Center, ModContent.NPCType<OrigenCore>());
            return false;
        }
    }
}
