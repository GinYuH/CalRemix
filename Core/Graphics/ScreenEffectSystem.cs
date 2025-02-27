using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using static System.MathF;
using static Terraria.Utils;
using static Microsoft.Xna.Framework.MathHelper;
using static CalRemix.CalRemixHelper;

namespace CalRemix.Core.Graphics
{
    public class ScreenEffectSystem : ModSystem
    {
        #region Blur
        private static RenderTarget2D BlurRenderTarget;

        private static Vector2 BlurPosition;

        private static float BlurIntensity;

        private static int BlurLifeTime;

        private static int BlurTime;

        private static bool BlurActive;

        public static float BaseScaleAmount => /*CalRemixConfig.instance.VisualOverlayIntensity*/ 1 * 0.08f;

        private const float BaseBlurAmount = 4f;

        private static float BlurLifetimeRatio => (float)BlurTime / BlurLifeTime;

        /// <summary>
        /// Call this to set a blur effect. Any existing ones will be replaced.
        /// </summary>
        /// <param name="position">The focal position, in world co-ordinates</param>
        /// <param name="intensity">How intense to make the scale and blur effect. A 0-1 range should be used</param>
        /// <param name="lifetime">How long the effect should last</param>
        public static void SetBlurEffect(Vector2 position, float intensity, int lifetime)
        {
            if (CalamityConfig.Instance.ScreenshakePower <= 0 || /*CalRemixConfig.instance.VisualOverlayIntensity*/ 1 <= 0f)
                return;

            BlurPosition = position;
            BlurIntensity = intensity;
            BlurLifeTime = lifetime;
            BlurTime = 0;
            BlurActive = true;
        }
        #endregion

        #region Flash
        private static RenderTarget2D FlashRenderTarget;

        private static Vector2 FlashPosition;

        private static float FlashIntensity;

        private static int FlashLifeTime;

        private static int FlashTime;

        private static bool FlashActive;

        private static float FlashLifetimeRatio => (float)FlashTime / FlashLifeTime;

        /// <summary>
        /// Call this to set a flash effect. Any existing ones will be replaced.
        /// </summary>
        /// <param name="position">The focal position, in world co-ordinates</param>
        /// <param name="intensity">How bright to make the flash. A 0-1 range should be used</param>
        /// <param name="lifetime">How long the effect should last</param>
        public static void SetFlashEffect(Vector2 position, float intensity, int lifetime)
        {
            if (CalamityConfig.Instance.ScreenshakePower <= 0 || /*CalRemixConfig.instance.VisualOverlayIntensity*/ 1 <= 0f)
                return;

            FlashPosition = position;
            FlashIntensity = intensity * /*CalRemixConfig.instance.VisualOverlayIntensity*/ 1;
            FlashLifeTime = lifetime;
            FlashTime = 0;
            FlashActive = true;
        }
        #endregion

        #region Chromatic Aberration
        private static RenderTarget2D AberrationTarget;

        private static Vector2 AberrationPosition;

        private static float AberrationIntensity;

        private static int AberrationLifeTime;

        private static int AberrationTime;

        private static float AberrationLifetimeRatio => (float)AberrationTime / AberrationLifeTime;

        public static void SetChromaticAberrationEffect(Vector2 position, float intensity, int lifetime)
        {
            if (CalamityConfig.Instance.ScreenshakePower <= 0 || AberrationLifetimeRatio > 0f || /*CalRemixConfig.instance.VisualOverlayIntensity*/ 1 <= 0f)
                return;

            AberrationPosition = position;
            AberrationIntensity = intensity * /*CalRemixConfig.instance.VisualOverlayIntensity*/ 1;
            AberrationLifeTime = lifetime;
            AberrationTime = 1;
        }
        #endregion Chromatic Aberration

        public override void OnModLoad()
        {
            Main.OnResolutionChanged += ResizeRenderTarget;
            Terraria.Graphics.Effects.On_FilterManager.EndCapture += EndCaptureManager;
        }

        public override void OnModUnload()
        {
            Main.OnResolutionChanged -= ResizeRenderTarget;
            Terraria.Graphics.Effects.On_FilterManager.EndCapture -= EndCaptureManager;
            Main.QueueMainThreadAction(() =>
            {
                FlashRenderTarget?.Dispose();
                BlurRenderTarget?.Dispose();
                AberrationTarget?.Dispose();
            });
        }

        // The purpose of this is to make these all work together and apply in the correct order.
        private void EndCaptureManager(Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            // Draw the screen effects first.
            screenTarget1 = DrawBlurEffect(screenTarget1);

            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);

            ScreenShatterSystem.CreateSnapshotIfNecessary(screenTarget1);
        }

        public static bool AnyBlurOrFlashActive() => BlurActive || FlashActive;

        public override void PostUpdateEverything()
        {
            if (BlurActive)
            {
                if (BlurTime >= BlurLifeTime)
                {
                    BlurActive = false;
                    BlurTime = 0;
                }
                else
                    BlurTime++;
            }

            if (FlashActive)
            {
                if (FlashTime >= FlashLifeTime)
                {
                    FlashActive = false;
                    FlashTime = 0;
                }
                else
                    FlashTime++;
            }
        }

        internal static RenderTarget2D DrawBlurEffect(RenderTarget2D screenTarget1)
        {
            if (BlurActive)
            {
                if (BlurRenderTarget is null)
                    ResizeRenderTarget(Vector2.Zero);

                // Draw the screen contents to the blur render target.
                BlurRenderTarget.SwapToRenderTarget();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(screenTarget1, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
                Main.spriteBatch.End();

                // Reset the render target.
                screenTarget1.SwapToRenderTarget();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

                // Draw the blur render target 7 times, getting progressively larger and more transparent.
                for (int i = -3; i <= 3; i++)
                {
                    if (i == 0)
                        continue;

                    // Increase the scale based on the intensity and lifetime of the blur.
                    float scaleAmount = BaseScaleAmount * BlurIntensity;
                    float blurAmount = BaseBlurAmount * BlurIntensity;
                    float scale = 1f + scaleAmount * (1f - BlurLifetimeRatio) * i / blurAmount;
                    Color drawColor = Color.White * 0.42f;
                    // Not doing this causes it to not properly fit on the screen. This extends it to be 100 extra in either direction.
                    Rectangle frameOffset = new(-100, -100, Main.screenWidth + 200, Main.screenHeight + 200);
                    // Use that and the position to set the origin to the draw position.
                    Vector2 origin = BlurPosition + new Vector2(100) - Main.screenPosition;
                    Main.spriteBatch.Draw(BlurRenderTarget, BlurPosition - Main.screenPosition, frameOffset, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
                }

                Main.spriteBatch.End();
            }

            // This draws over the blur, so doing them together isn't really ideal.
            else if (FlashActive)
            {
                if (FlashRenderTarget is null)
                    ResizeRenderTarget(Vector2.Zero);

                // Draw the screen contents to the blur render target.
                FlashRenderTarget.SwapToRenderTarget();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(screenTarget1, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
                Main.spriteBatch.End();

                // Reset the render target.
                screenTarget1.SwapToRenderTarget();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

                Color drawColor = new(1f, 1f, 1f, Clamp(Lerp(0.5f, 1f, (1f - FlashLifetimeRatio) * FlashIntensity), 0f, 1f));

                // Not doing this causes it to not properly fit on the screen. This extends it to be 100 extra in either direction.
                Rectangle frameOffset = new(-100, -100, Main.screenWidth + 200, Main.screenHeight + 200);
                // Use that and the position to set the origin to the draw position.
                Vector2 origin = FlashPosition + new Vector2(100) - Main.screenPosition;
                for (int i = 0; i < 2; i++)
                    Main.spriteBatch.Draw(FlashRenderTarget, FlashPosition - Main.screenPosition, frameOffset, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.End();
            }

            if (AberrationLifetimeRatio > 0f)
            {
                if (AberrationTarget is null)
                    ResizeRenderTarget(Vector2.Zero);

                AberrationTime++;
                if (AberrationLifetimeRatio >= 1f)
                {
                    AberrationTime = 0;
                    return screenTarget1;
                }

                // Draw the screen contents to the aberration render target.
                AberrationTarget.SwapToRenderTarget();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(screenTarget1, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
                Main.spriteBatch.End();

                // Reset the render target.
                screenTarget1.SwapToRenderTarget();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                var aberrationShader = GameShaders.Misc[$"{CalRemix.instance.Name}:ChromaticAberrationShader"];
                aberrationShader.Shader.Parameters["uIntensity"].SetValue((1f - AberrationLifetimeRatio) * AberrationIntensity);
                aberrationShader.Shader.Parameters["impactPoint"].SetValue(AberrationPosition / new Vector2(Main.screenWidth, Main.screenHeight));
                aberrationShader.Apply();

                Main.spriteBatch.Draw(AberrationTarget, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
            }

            return screenTarget1;
        }

        private static void ResizeRenderTarget(Vector2 obj)
        {
            BlurRenderTarget?.Dispose();
            FlashRenderTarget?.Dispose();
            AberrationTarget?.Dispose();

            BlurRenderTarget = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            FlashRenderTarget = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            AberrationTarget = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
        }
    }
}
