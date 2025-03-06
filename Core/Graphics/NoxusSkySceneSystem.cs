﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using static System.MathF;
using static Terraria.Utils;
using static Microsoft.Xna.Framework.MathHelper;
using static CalRemix.CalRemixHelper;
using CalRemix.Core.World;

namespace CalRemix.Core.Graphics
{
    public class NoxusSkySceneSystem : ModSystem
    {
        internal static Main.SceneArea PreviousSceneDetails
        {
            get;
            private set;
        }

        public static float EclipseDarknessInterpolant
        {
            get;
            set;
        }

        public static Vector2 GetSunPosition(Main.SceneArea sceneArea, float dayCompletion)
        {
            float verticalOffsetInterpolant;
            if (dayCompletion < 0.5f)
                verticalOffsetInterpolant = Pow(1f - dayCompletion * 2f, 2f);
            else
                verticalOffsetInterpolant = Pow(dayCompletion - 0.5f, 2f) * 4f;
            Texture2D sunTexture = TextureAssets.Sun.Value;
            int x = (int)(dayCompletion * sceneArea.totalWidth + sunTexture.Width * 2f + dayCompletion * 210f) - 325;
            int y = (int)(sceneArea.bgTopY + verticalOffsetInterpolant * 250f + Main.sunModY + 180f);
            return new(x, y);
        }

        public override void OnModLoad()
        {
            Terraria.On_Main.DrawSunAndMoon += DrawNoxusInBackgroundHook;
        }

        private void DrawNoxusInBackgroundHook(Terraria.On_Main.orig_DrawSunAndMoon orig, Main self, Main.SceneArea sceneArea, Color moonColor, Color sunColor, float tempMushroomInfluence)
        {
            orig(self, sceneArea, moonColor, sunColor, tempMushroomInfluence);
            PreviousSceneDetails = sceneArea;
            DrawNoxusInBackground(sceneArea);
        }

        public void DrawNoxusInBackground(Main.SceneArea sceneArea)
        {
            // Make the eclipse darkness effect naturally dissipate to ensure that it goes away even if the checks below are failed.
            EclipseDarknessInterpolant = Clamp(EclipseDarknessInterpolant - 0.04f, 0f, 1f);

            // Don't draw Noxus if he's fucking dead, has fallen from space already, or hasn't started orbiting the planet yet.
            if (RemixDowned.downedNoxegg || CalRemixWorld.metNoxus || !NoxusEggCutsceneSystem.NoxusBeganOrbitingPlanet)
                return;

            // Don't draw Noxus if he's behind the view position or if on the title screen.
            if (CelestialOrbitDetails.NoxusOrbitOffset.Z < 0f || Main.gameMenu)
                return;

            // Calculate draw values for Noxus.
            Texture2D noxusEggTexture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Noxus/NoxusEgg").Value;
            Color noxusDrawColor = Color.Lerp(Color.Black * 0.035f, new(64, 64, 64), Pow(EclipseDarknessInterpolant, 0.54f));

            Vector2 noxusDrawPosition = new Vector2(CelestialOrbitDetails.NoxusHorizontalOffset, CelestialOrbitDetails.NoxusVerticalOffset) + sceneArea.SceneLocalScreenPositionOffset;
            noxusDrawPosition.Y += sceneArea.bgTopY;

            // Make Noxus darker as an indication that he's becoming a silhouette if close to the sun.
            Vector2 sunPosition = GetSunPosition(sceneArea, (float)(Main.time / Main.dayLength));
            float distanceFromSun = sunPosition.Distance(noxusDrawPosition);
            float silhouetteInterpolant = GetLerpValue(85f, 21f, distanceFromSun, true);
            noxusDrawColor = Color.Lerp(noxusDrawColor, Color.Black, Pow(silhouetteInterpolant, 0.6f) * 0.85f);
            if (silhouetteInterpolant > 0f)
                noxusDrawColor.A = (byte)Lerp(noxusDrawColor.A, 255f, Pow(silhouetteInterpolant, 1.3f));

            // Calculate the eclipse darkness intensity.
            EclipseDarknessInterpolant = GetLerpValue(58f, 21f, distanceFromSun, true);

            // Draw a bloom flare over the sun if an eclipse is happening.
            if (EclipseDarknessInterpolant > 0f)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

                Texture2D bloomFlare = ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/BloomFlare").Value;
                Texture2D corona = ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/GreyscaleTextures/Corona").Value;
                Main.spriteBatch.Draw(corona, sunPosition, null, Color.Wheat * EclipseDarknessInterpolant * 0.58f, Main.GlobalTimeWrappedHourly * 0.03f, corona.Size() * 0.5f, 0.26f, 0, 0f);
                Main.spriteBatch.Draw(bloomFlare, sunPosition, null, Color.LightGoldenrodYellow * EclipseDarknessInterpolant * 0.5f, Main.GlobalTimeWrappedHourly * 1.2f, bloomFlare.Size() * 0.5f, 0.3f, 0, 0f);
                Main.spriteBatch.Draw(bloomFlare, sunPosition, null, Color.LightGoldenrodYellow * EclipseDarknessInterpolant * 0.4f, Main.GlobalTimeWrappedHourly * -0.92f, bloomFlare.Size() * 0.5f, 0.25f, 0, 0f);
            }

            // Draw the egg with a moderate amount of blur.
            float eggScale = Lerp(0.41f, 0.42f, Pow(EclipseDarknessInterpolant, 0.4f));
            float maxBlurOffset = Lerp(0.4f, 0.6f, EclipseDarknessInterpolant) / noxusEggTexture.Width * 6f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

            // Apply the blur shader.
            var blurShader = GameShaders.Misc[$"{Mod.Name}:HorizontalBlurShader"];
            blurShader.Shader.Parameters["maxBlurOffset"].SetValue(maxBlurOffset);
            blurShader.Apply();

            Main.spriteBatch.Draw(noxusEggTexture, noxusDrawPosition, null, noxusDrawColor, 0f, noxusEggTexture.Size() * 0.5f, eggScale, 0, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            backgroundColor = Color.Lerp(backgroundColor, new(8, 8, 11), EclipseDarknessInterpolant * 0.96f);
            tileColor = Color.Lerp(tileColor, Color.Black, EclipseDarknessInterpolant * 0.85f);
        }
    }
}
