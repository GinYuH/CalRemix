using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class ConfettiEvil : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = Main.rand.Next(1, 7);
                Projectile.frame = Main.rand.Next(0, 3);
                Projectile.direction = Main.rand.NextBool().ToDirectionInt();
            }
            Projectile.rotation += 0.01f * Projectile.direction;

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, texture.Frame(6, 3, (int)Projectile.ai[0] - 1, Projectile.frame), lightColor, Projectile.rotation, new Vector2(texture.Width / 12, texture.Height / 6), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
