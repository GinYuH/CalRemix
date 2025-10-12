using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class JuggularPortal : ModProjectile
    {
        public override string Texture => "CalamityMod/ExtraTextures/GreyscaleVortex";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            if (Projectile.timeLeft > Projectile.ai[1])
            {
                Projectile.timeLeft = (int)Projectile.ai[1];
            }
            float lifeCompletion = Projectile.timeLeft / Projectile.ai[1];
            float becomeVisible = 0.8f;
            float fadeAway = 0.2f;
            if (lifeCompletion >= becomeVisible)
            {
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, Utils.GetLerpValue(1, becomeVisible, lifeCompletion, true));
            }
            else if (lifeCompletion <= fadeAway)
            {
                Projectile.alpha = (int)MathHelper.Lerp(0, 255, Utils.GetLerpValue(fadeAway, 0, lifeCompletion, true));
            }
            else
            {
                Projectile.alpha = 0;
            }
            Projectile.scale = Utils.GetLerpValue(255, 0, Projectile.alpha, true);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var shader = GameShaders.Misc[$"{Mod.Name}:JuggularPortal"];
            Color c = Projectile.ai[0] == 0 ? Color.Orange : Color.Violet;
            shader.UseColor(c * Projectile.Opacity);
            shader.Apply();
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive, shader.Shader);

            Texture2D portalTexture = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(portalTexture, Projectile.Center - Main.screenPosition, null, Color.White, 5 * Main.GlobalTimeWrappedHourly, portalTexture.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);

            Main.spriteBatch.ExitShaderRegion();
            return false;
        }
    }
}