using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class MonorianGemProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 2;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 2 == 0 && Projectile.ai[0] > 1)
                GeneralParticleHandler.SpawnParticle(new HealingPlus(Projectile.position, 1f, Projectile.velocity.SafeNormalize(Vector2.UnitY) * 2, Color.LightPink, Color.DeepPink, 20));
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {

                GeneralParticleHandler.SpawnParticle(new HealingPlus(Projectile.Center, Main.rand.NextFloat(0.5f, 0.8f), Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(4, 6), Color.LightPink * 0.6f, Color.DeepPink * 0.6f, 20));
            }
        }
    }
}