using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles
{
    public class CursedBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = -1;
            Projectile.width = Projectile.height = 4;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.scale *= 2;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.spriteDirection = Projectile.direction = Projectile.velocity.X.DirectionalSign();
            int speed = 10;
            Point tileCords = Projectile.Center.ToTileCoordinates();
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[1] = Main.rand.Next(1, 3);
            }
            Projectile.ai[2]++;
            if (Projectile.ai[2] > 11)
            {
                if (Projectile.velocity.Length() != 10)
                {
                    Vector2 norm = Projectile.velocity.SafeNormalize(Vector2.UnitY);
                    if (norm.Y > norm.X)
                    {
                        Projectile.velocity.Y = norm.Y.DirectionalSign() * speed;
                        Projectile.velocity.X = 0;
                    }
                    else
                    {
                        Projectile.velocity.X = norm.X.DirectionalSign() * speed;
                        Projectile.velocity.Y = 0;
                    }
                }
                if (tileCords.X % 10 == 0 && Projectile.velocity.Y == 0)
                {
                    Projectile.velocity.X = 0;
                    Projectile.velocity.Y = Main.rand.NextBool().ToDirectionInt() * speed;
                }
                if (tileCords.Y % 10 == 0 && Projectile.velocity.X == 0)
                {
                    Projectile.velocity.Y = Main.rand.NextBool().ToDirectionInt() * speed;
                    Projectile.velocity.X = Main.rand.NextBool().ToDirectionInt() * speed;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Projectile.ai[1] == 1 ? TextureAssets.Projectile[Type].Value : ModContent.Request<Texture2D>(Texture + 2).Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}