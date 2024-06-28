using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Subworlds
{
    public class FannySky : CustomSky
    {
        public float BackgroundIntensity;
        public float LightningIntensity;
        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.Current == ModContent.GetInstance<FannySubworld>();
            }
        }

        public static readonly Color DrawColor = Color.Black;
        public float fannyPos = 0;
        public float fannyPos2 = Main.screenWidth;
        public float fannyPos3 = Main.screenWidth;

        public override void Update(GameTime gameTime)
        {
            if (!CanSkyBeActive)
            {
                BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity - 0.08f, 0f, 1f);
                Deactivate(Array.Empty<object>());
                return;
            }

            LightningIntensity = MathHelper.Clamp(LightningIntensity * 0.95f - 0.025f, 0f, 1f);
            BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity + 0.01f, 0f, 1f);

            Opacity = BackgroundIntensity;
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= float.MaxValue)
            {
                fannyPos--;
                if (fannyPos < -Main.screenWidth)
                    fannyPos = Main.screenWidth;
                fannyPos2--;
                if (fannyPos2 < -Main.screenWidth)
                    fannyPos2 = Main.screenWidth;
                Texture2D fanny = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/FannyGoner").Value;
                Vector2 scale = new Vector2(Main.screenWidth * 1.1f / TextureAssets.MagicPixel.Value.Width, Main.screenHeight * 1.1f / TextureAssets.MagicPixel.Value.Height);
                Vector2 fscale = new Vector2(Main.screenWidth * 1.1f / fanny.Width, Main.screenHeight * 1.1f / fanny.Height);
                Vector2 screenArea = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
                Vector2 fscreenArea = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.25f);
                Color drawColor = Color.Black;
                Vector2 origin = TextureAssets.MagicPixel.Value.Size() * 0.5f;
                Vector2 forigin = fanny.Size() * 0.5f;

                // Draw a grey background as base.
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, screenArea, null, Color.Black, 0f, origin, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(fanny, fscreenArea + Vector2.UnitX * fannyPos, null, Color.Red * 0.22f, 0f, forigin, fscale, SpriteEffects.None, 0f);
                spriteBatch.Draw(fanny, fscreenArea + Vector2.UnitX * fannyPos2, null, Color.Red * 0.22f, 0f, forigin, fscale, SpriteEffects.None, 0f);
            }
        }

        public override Color OnTileColor(Color inColor) => new Color(Vector4.Lerp(DrawColor.ToVector4(), inColor.ToVector4(), 1f - BackgroundIntensity));

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
