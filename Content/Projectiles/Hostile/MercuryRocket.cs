using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class MercuryRocket : ModProjectile
    {
        public static SoundStyle RockyExplosion = new SoundStyle("CalRemix/Assets/Sounds/RockyExplosion") { MaxInstances = 0 };
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(3))
            {
                GeneralParticleHandler.SpawnParticle(new TimedSmokeParticle(Projectile.Center, -Projectile.velocity.SafeNormalize(Vector2.UnitY).RotatedByRandom(MathHelper.PiOver2 * 0.5f) * Main.rand.NextFloat(3f, 5f), new Color(20, 20, 20), new Color(40, 40, 40), Main.rand.NextFloat(1f, 1.4f), 0.8f, 40, 0.02f));
            }
            Projectile proj = Main.projectile[(int)Projectile.ai[0]];
            if (!proj.active)
            {
                proj.Kill();
            }
            else
            {
                if (proj.Hitbox.Intersects(Projectile.Hitbox))
                {
                    proj.ai[2] = 1;
                    proj.ai[1] = 0;
                    Projectile.Kill();
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile p = Projectile;
            Main.LocalPlayer.Calamity().GeneralScreenShakePower += 1;
            p.position = p.Center;
            p.width = p.height = 200;
            p.position.X = p.position.X - (float)(p.width / 2);
            p.position.Y = p.position.Y - (float)(p.height / 2);
            p.maxPenetrate = -1;
            p.penetrate = -1;
            p.Damage();
            SoundEngine.PlaySound(RockyExplosion with { PitchVariance = 0.3f }, p.Center);
            for (int i = 0; i < 20; i++)
                GeneralParticleHandler.SpawnParticle(new CustomPulse(p.Center, Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(14f, 24f), Color.Orange, "CalamityMod/Particles/Light", Vector2.One, 0, Main.rand.NextFloat(2f, 5f), 0, 20));
            for (int i = 0; i < 30; i++)
                GeneralParticleHandler.SpawnParticle(new TimedSmokeParticle(p.Center, Main.rand.NextVector2Circular(30, 30), Color.Black, new Color(20, 20, 20), Main.rand.NextFloat(2f, 2.4f), 0.8f, 30, 0.02f));
            GeneralParticleHandler.SpawnParticle(new StrongBloom(p.Center, Vector2.Zero, Color.Orange, 3f, 5));
            GeneralParticleHandler.SpawnParticle(new PulseRing(p.Center, Vector2.Zero, Color.Gray * 0.4f, 0.4f, 5f, 10));
        }

        public static void MercuryExplosion(Projectile p, float scale = 1f)
        {
        }
    }
}