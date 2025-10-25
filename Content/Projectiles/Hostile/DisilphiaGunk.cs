using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class DisilphiaGunk : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/LightningProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.timeLeft = 480;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Black;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.Red, 0, texture.Size() / 2, 1.2f, 0);

            Projectile proj = Projectile;

            int num = texture.Height / Main.projFrames[proj.type];
            int y = num * proj.frame;
            float scale = proj.scale;
            float rotation = proj.rotation;
            Rectangle rectangle = new Rectangle(0, y, texture.Width, num);
            Vector2 origin = rectangle.Size() / 2f;
            SpriteEffects effects = SpriteEffects.None;
            if (proj.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            bool flag = false;
            if (CalamityConfig.Instance.Afterimages)
            {
                Vector2 vector = proj.Size / 2f;
                Color alpha = proj.GetAlpha(lightColor);
                int num2 = Math.Max(1, 2);
                Color color3 = alpha;
                int num3 = ProjectileID.Sets.TrailCacheLength[proj.type];
                float num4 = (float)num3 * 1.5f;
                for (int k = 0; k < num3; k += num2)
                {
                    Vector2 position3 = proj.oldPos[k] + vector - Main.screenPosition + new Vector2(0f, proj.gfxOffY);
                    float mult = 1;
                    if (k > 0)
                    {
                        float num5 = num3 - k;
                        mult = num5 / num4;
                        color3 *= mult;
                    }

                    Main.spriteBatch.Draw(texture, position3, rectangle, Color.Red * mult, rotation, origin, scale * 1.2f, effects, 0f);
                    Main.spriteBatch.Draw(texture, position3, rectangle, color3, rotation, origin, scale, effects, 0f);
                }
            }

            if (!CalamityConfig.Instance.Afterimages || ProjectileID.Sets.TrailCacheLength[proj.type] <= 0 || flag)
            {
                Vector2 vector2 = proj.Center;
                Main.spriteBatch.Draw(texture, vector2 - Main.screenPosition + new Vector2(0f, proj.gfxOffY), rectangle, proj.GetAlpha(lightColor), rotation, origin, scale, effects, 0f);
            }

            return false;
        }
    }
}
