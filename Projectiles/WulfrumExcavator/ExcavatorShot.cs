using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.WulfrumExcavator
{
    public class ExcavatorShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wulfrum Laser");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            // Decide frames.
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter / 5 % Main.projFrames[Type];

            // Fade in.
            Projectile.Opacity = MathHelper.Clamp(Projectile.Opacity + 0.1f, 0f, 1f);

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
