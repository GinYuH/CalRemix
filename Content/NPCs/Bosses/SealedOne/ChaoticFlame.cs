using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.SealedOne
{
    public class ChaoticFlame : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public ref float Origin => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.timeLeft = 2400;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.alpha = 55;
        }

        public override void AI()
        {
            Projectile.velocity.Y = -3;

            if (Timer == 0)
                Timer = Main.rand.Next(0, 200);

            if (Origin == 0)
                Origin = Projectile.position.X;

            Projectile.position.X = (float)(Origin + (Math.Sin(Timer / 25) * 100));

            if (Timer % 4 == 0)
            {
                if (Projectile.frame >= 3)
                    Projectile.frame = 0;
                else
                    Projectile.frame++;
            }

            Timer++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Rectangle sourceRectangle = new Rectangle(0, texture.Height / 4 * Projectile.frame, texture.Width, texture.Height / 4);
            Color color = Color.OrangeRed * Projectile.Opacity;
            Vector2 origin = sourceRectangle.Size() * 0.5f;
            Vector2 scale = Vector2.One;

            Main.spriteBatch.Draw(texture, position, sourceRectangle, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}