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
                GeneralParticleHandler.SpawnParticle(new SmallSmokeParticle(Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(3f, 5f), new Color(20, 20, 20), new Color(40, 40, 40), Main.rand.NextFloat(1f, 1.4f), 0.8f, 0.02f));
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
            MercuryExplosion(Projectile);
        }

        public static void MercuryExplosion(Projectile p, float scale = 1f)
        {
            bool big = scale != 1f;
            Main.LocalPlayer.Calamity().GeneralScreenShakePower += big ? 4 : 1;
            p.position = p.Center;
            p.width = p.height = big ? 800 : 200;
            p.position.X = p.position.X - (float)(p.width / 2);
            p.position.Y = p.position.Y - (float)(p.height / 2);
            p.maxPenetrate = -1;
            p.penetrate = -1;
            p.Damage();
            SoundEngine.PlaySound(RockyExplosion with { PitchVariance = 0.3f, Pitch = (big ? -0.3f : 1f), Volume = (big ? 3f : 1f) }, p.Center);
            for (int i = 0; i < 20; i++)
                GeneralParticleHandler.SpawnParticle(new CustomPulse(p.Center, Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(14f, 24f) * (big ? 2 : 1), Color.Orange, "CalamityMod/Particles/Light", Vector2.One, 0, Main.rand.NextFloat(2f, 5f) * scale, 0, big ? 40 : 20));
            for (int i = 0; i < 50 * (big ? scale * 2 : scale); i++)
                GeneralParticleHandler.SpawnParticle(new TimedSmokeParticle(p.Center, Main.rand.NextVector2Circular(40, 40) * scale, Color.Black, new Color(20, 20, 20), Main.rand.NextFloat(1f, 2f) * scale, 0.8f, 60, 0.02f));
            GeneralParticleHandler.SpawnParticle(new StrongBloom(p.Center, Vector2.Zero, Color.Orange, 3f * scale, big ? 10 : 5));
            GeneralParticleHandler.SpawnParticle(new PulseRing(p.Center, Vector2.Zero, Color.Gray * 0.4f, 0.4f, big ? 10f : 5f, 10));
        }
    }
}