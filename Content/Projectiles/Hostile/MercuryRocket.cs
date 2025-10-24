using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class MercuryRocket : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile proj = Main.projectile[(int)Projectile.ai[0]];
            if (proj.active)
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
        }
    }
}