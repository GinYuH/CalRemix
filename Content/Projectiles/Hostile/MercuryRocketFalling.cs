using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class MercuryRocketFalling : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Projectiles/Hostile/MercuryRocket";
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[1]++;
            if (Projectile.ai[1] > Projectile.ai[0])
            {
                Projectile.velocity.Y = 30;
            }
        }

        public override void OnKill(int timeLeft)
        {
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            // Only collide with tiles if past the player's vertical position
            return Projectile.position.Y > Main.player[(int)Projectile.ai[2]].Bottom.Y;
        }
    }
}