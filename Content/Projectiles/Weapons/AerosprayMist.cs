using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class AerosprayMist : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mist");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.Resize((int)(18 + Projectile.ai[0]), (int)(18 + Projectile.ai[0]));
            Projectile.scale += Projectile.ai[0] * 0.001f;
            if (Projectile.ai[0] % 4 == 0)
            {
                Projectile.damage--;
            }
            if (Projectile.ai[0] > 10)
            { 
                MediumMistParticle SandCloud = new MediumMistParticle(Projectile.Center, (-Projectile.velocity * 0.2f).RotatedByRandom(0.2f), Color.Cyan, Color.White, Projectile.scale, MathHelper.Lerp(180, 44, Projectile.ai[0] / 120), Main.rand.NextFloat(0.03f, -0.03f));
                GeneralParticleHandler.SpawnParticle(SandCloud);
            }
        }
    }
}