using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class TobaccoSeed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Seed");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha += 30;
            }
            else if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 30;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, 0, lightColor, 3);
            return false;
        }
    }
}