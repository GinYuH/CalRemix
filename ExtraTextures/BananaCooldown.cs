using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Localization;
using Terraria;
using Terraria.Graphics.Shaders;
using static Terraria.ModLoader.ModContent;


namespace CalRemix.ExtraTextures
{
    public class BananaClownCooldown : CooldownHandler
    {
        public static new string ID => "BananaClown";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Clown Consumption Cooldown");
        public override string Texture => "CalRemix/ExtraTextures/BananaCooldown";
        public override Color OutlineColor => new Color(66, 245, 114);
        public override Color CooldownStartColor => new Color(191, 245, 66);
        public override Color CooldownEndColor => new Color(255, 255, 23);

        //Charge down at first, and then charge back up
        private float AdjustedCompletion => instance.timeLeft > 3600 ? (instance.timeLeft - 3600) / 600f : 1 - instance.timeLeft / 3600f;

        public override void ApplyBarShaders(float opacity)
        {
            //Use the adjusted completion
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseOpacity(opacity);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseSaturation(AdjustedCompletion);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseColor(CooldownStartColor);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseSecondaryColor(CooldownEndColor);
            GameShaders.Misc["CalamityMod:CircularBarShader"].Apply();
        }
        public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
        {
            Texture2D sprite = Request<Texture2D>(Texture).Value;
            Texture2D outline = Request<Texture2D>(OutlineTexture).Value;
            Texture2D overlay = Request<Texture2D>(OverlayTexture).Value;

            //Draw the outline
            spriteBatch.Draw(outline, position, null, OutlineColor * opacity, 0, outline.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            //Draw the icon
            spriteBatch.Draw(sprite, position, null, Color.White * opacity, 0, sprite.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            //Draw the small overlay
            int lostHeight = (int)Math.Ceiling(overlay.Height * AdjustedCompletion);
            Rectangle crop = new Rectangle(0, lostHeight, overlay.Width, overlay.Height - lostHeight);
            spriteBatch.Draw(overlay, position + Vector2.UnitY * lostHeight * scale, crop, OutlineColor * opacity * 0.9f, 0, sprite.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }
    }
}