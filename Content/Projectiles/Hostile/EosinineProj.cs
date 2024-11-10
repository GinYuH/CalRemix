using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class EosinineProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eosinine");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.hostile = true;
        }

        public override void AI()
        {
            float yRange = 5;
            float ySpeedMult = 0.05f;
            Projectile.ai[1]++;
            Projectile.velocity.Y = (float)Math.Cos(Projectile.ai[1] * ySpeedMult) * yRange;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() * 0.5f;
            Color color = Color.MediumPurple * 0.3f;
            Vector2 scale = new Vector2(Projectile.scale * MathHelper.Max(0.2f, 1 - Math.Abs((Projectile.velocity.Y / 10))), Projectile.scale * MathHelper.Max(0.2f, Math.Abs(Projectile.velocity.Y / 10))) * 2;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 2 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(texture, position + vector2, null, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, position, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
