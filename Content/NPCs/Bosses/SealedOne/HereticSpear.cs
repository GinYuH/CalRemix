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
    public class HereticSpear : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public ref float Flip => ref Projectile.ai[1];
        public ref float VassalDamage => ref Projectile.ai[2];

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            int timerStartMovingFast = 10;
            int timerFullyFadeIn = 40;
            int timerEndMovingFast = 100;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            int maxTransparency = 200;
            Projectile.alpha = (int)(-(Utils.GetLerpValue(0, timerFullyFadeIn, Timer, true) * maxTransparency) + 255);

            if (Projectile.alpha < maxTransparency)
                Projectile.damage = 0;
            else
                Projectile.damage = (int)VassalDamage;

            float velx = CalamityUtils.ExpInEasing(Utils.GetLerpValue(timerStartMovingFast, timerEndMovingFast, Timer, true), 1) * 50;
            Projectile.velocity.X = velx;
            Projectile.velocity.X += 1;
            if (Flip == 1)
                Projectile.velocity.X *= -1;

            Timer++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Color color = Color.OrangeRed * Projectile.Opacity;
            Vector2 origin = texture.Size() * 0.5f;
            Vector2 scale = Vector2.One;

            Main.spriteBatch.Draw(texture, position, null, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            return true;
        }
    }
}
