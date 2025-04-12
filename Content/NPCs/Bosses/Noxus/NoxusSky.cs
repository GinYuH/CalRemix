using System.Collections.Generic;
using System.Reflection;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalRemix;
using CalRemix.Content.Particles;
using static CalRemix.CalRemixHelper;
using CalRemix.Core.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using System;

namespace CalRemix.Content.NPCs.Bosses.Noxus
{
    public class NoxusSkyScene : ModSceneEffect
    {
        private static readonly FieldInfo particlesField = typeof(GeneralParticleHandler).GetField("particles", BindingFlags.NonPublic | BindingFlags.Static);

        public override bool IsSceneEffectActive(Player player) => NoxusSky.SkyIntensityOverride > 0f || NPC.AnyNPCs(ModContent.NPCType<EntropicGod>());

        public override void Load()
        {
            Terraria.GameContent.Events.On_MoonlordDeathDrama.DrawWhite += DrawFog;
        }

        private void DrawFog(Terraria.GameContent.Events.On_MoonlordDeathDrama.orig_DrawWhite orig, SpriteBatch spriteBatch)
        {
            orig(spriteBatch);

            var sky = (NoxusSky)SkyManager.Instance["CalRemix:NoxusSky"];

            spriteBatch.EnterShaderRegion();
            sky.DrawFog();
            CalRemixHelper.SetBlendState(spriteBatch, BlendState.Additive);

            // Draw twinkles on top of the fog.
            List<Particle> particles = (List<Particle>)particlesField.GetValue(null);
            foreach (Particle p in particles)
            {
                if (p is not TwinkleParticle t)
                    continue;

                t.Opacity *= 0.6f;
                t.CustomDraw(spriteBatch);
                t.Opacity /= 0.6f;
            }
            spriteBatch.ExitShaderRegion();

            ScreenOverlaysSystem.EmptyDrawCache(ScreenOverlaysSystem.DrawCacheAfterNoxusFog);
        }

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalRemix:NoxusSky", isActive);
        }
    }

    public class NoxusSky : CustomSky
    {
        public class FloatingRubble
        {
            public int Time;

            public int Lifetime;

            public int Variant;

            public float Depth;

            public Vector2 Position;

            public float Opacity => Utils.GetLerpValue(0f, 20f, Time, true) * Utils.GetLerpValue(Lifetime, Lifetime - 90f, Time, true);

            public void Update()
            {
                Position += Vector2.UnitY * MathF.Sin(MathHelper.TwoPi * Time / 180f) * 1.2f;
                Time++;
            }
        }

        private bool isActive;

        private float intensity;

        private float fogIntensity;

        private float fogSpreadDistance;

        private Vector2 fogCenter;

        private readonly List<FloatingRubble> rubble = new();

        public static float FlashIntensity
        {
            get;
            private set;
        }

        public static float SkyIntensityOverride
        {
            get;
            set;
        }

        public static Vector2 FlashNoiseOffset
        {
            get;
            private set;
        }

        public static Vector2 FlashPosition
        {
            get;
            private set;
        }

        public static Color FogColor => new(49, 40, 70);

        public static readonly SoundStyle ThunderSound = new SoundStyle("CalRemix/Assets/Sounds/Noxus/ThunderRumble", 3) with { Volume = 0.32f, PitchVariance = 0.35f };

        public override void Update(GameTime gameTime)
        {
            // Make the intensity go up or down based on whether the sky is in use.
            intensity = Utils.Clamp(intensity + isActive.ToDirectionInt() * 0.01f, 0f, 1f);

            // Make the fog intensity go down if the sky is not in use. It does not go up by default, however.
            fogIntensity = Utils.Clamp(fogIntensity - (!isActive).ToInt(), 0f, 1f);

            // Disable ambient sky objects like wyverns and eyes appearing in front of the dark cloud of death.
            if (isActive)
                SkyManager.Instance["Ambience"].Deactivate();

            // Make flashes exponentially decay into nothing.
            FlashIntensity *= 0.86f;

            // Randomly create flashes.
            int flashCreationChance = 90;
            int noxusIndex = NPC.FindFirstNPC(ModContent.NPCType<EntropicGod>());
            float flashIntensity = /*CalRemixConfig.instance.VisualOverlayIntensity*/ 1 * 71f;
            if (noxusIndex != -1)
            {
                NPC noxus = Main.npc[noxusIndex];
                flashCreationChance = (int)MathHelper.Lerp(210, 36, 1f - noxus.life / (float)noxus.lifeMax);
                flashIntensity = MathHelper.Lerp(35f, 90f, 1f - noxus.life / (float)noxus.lifeMax);
            }

            if (FlashIntensity <= 2f && fogIntensity < 1f && Main.rand.NextBool(flashCreationChance))
            {
                FlashIntensity = flashIntensity * (1f - fogIntensity);
                FlashNoiseOffset = Main.rand.NextVector2Square(0f, 1f);
                FlashPosition = Main.rand.NextVector2Square(0.2f, 0.8f);
                if (Main.instance.IsActive)
                    SoundEngine.PlaySound(ThunderSound with { Volume = ((1f - fogIntensity) * 0.6f), MaxInstances = 5 });
            }

            // Prepare the fog overlay.
            if (EntropicGod.Myself is not null)
            {
                fogIntensity = EntropicGod.Myself.ModNPC<EntropicGod>().FogIntensity;
                fogSpreadDistance = EntropicGod.Myself.ModNPC<EntropicGod>().FogSpreadDistance;
                fogCenter = EntropicGod.Myself.Center + EntropicGod.Myself.ModNPC<EntropicGod>().HeadOffset;
            }

            // Randomly create rubble around the player.
            if (Main.rand.NextBool(20) && rubble.Count <= 80)
            {
                FloatingRubble r = new()
                {
                    Depth = Main.rand.NextFloat(1.1f, 2.78f),
                    Variant = Main.rand.Next(3),
                    Position = new Vector2(Main.LocalPlayer.Center.X + Main.rand.NextFloatDirection() * 3300f, Main.rand.NextFloat(8000f)),
                    Lifetime = Main.rand.Next(240, 360)
                };
                rubble.Add(r);
            }

            // Update all rubble.
            rubble.RemoveAll(r => r.Time >= r.Lifetime);
            rubble.ForEach(r => r.Update());

            SkyIntensityOverride = MathHelper.Clamp(SkyIntensityOverride - 0.07f, 0f, 1f);
        }

        public override Color OnTileColor(Color inColor)
        {
            return Color.Lerp(inColor, Color.White, intensity * 0.5f);
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Main.spriteBatch.EnterShaderRegion();
            DrawBackground(Color.Lerp(Color.Lerp(Color.BlueViolet, Color.Indigo, 0.6f), Color.DarkGray, 0.2f));

            if (Main.gfxQuality >= 0.6f)
            {
                Main.spriteBatch.EnterShaderRegion(BlendState.Additive);

                DrawBackground(Color.Lerp(Color.MidnightBlue, Color.Pink, 0.3f), 2f, 1f, 1f, ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/GreyscaleTextures/TurbulentNoise"));
                DrawBackground(Color.Lerp(Color.Lerp(Color.BlueViolet, Color.Indigo, 0.3f), Color.DarkGray, 0.2f), 0.6f);

                Main.spriteBatch.ExitShaderRegion();
            }

            DrawRubble(minDepth, maxDepth);
        }

        public void DrawBackground(Color color, float localIntensity = 1f, float scrollSpeed = 1f, float noiseZoom = 1f, Asset<Texture2D> texture = null)
        {
            // Make the background colors more muted based on how strong the fog is.
            if (EntropicGod.Myself is not null)
            {
                float fogIntensity = EntropicGod.Myself.ModNPC<EntropicGod>().FogIntensity;
                float fogSpreadDistance = EntropicGod.Myself.ModNPC<EntropicGod>().FogSpreadDistance;
                float colorDarknessInterpolant = MathHelper.Clamp(fogSpreadDistance * Utils.GetLerpValue(0f, 0.15f, fogIntensity, true), 0f, 1f);
                color = Color.Lerp(color, Color.DarkGray, colorDarknessInterpolant * 0.7f);
            }

            Texture2D pixel = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Pixel").Value;
            Vector2 screenArea = new(Main.instance.GraphicsDevice.Viewport.Width, Main.instance.GraphicsDevice.Viewport.Height);
            Vector2 textureArea = screenArea / pixel.Size();

            var backgroundShader = GameShaders.Misc[$"{CalRemix.instance.Name}:NoxusBackgroundShader"];
            backgroundShader.Shader.Parameters["luminanceThreshold"].SetValue(0.9f);
            backgroundShader.Shader.Parameters["uIntensity"].SetValue(localIntensity * MathHelper.Clamp(intensity, SkyIntensityOverride, 1f));
            backgroundShader.Shader.Parameters["scrollSpeed"].SetValue(scrollSpeed);
            backgroundShader.Shader.Parameters["noiseZoom"].SetValue(noiseZoom * 0.16f);
            backgroundShader.Shader.Parameters["flashCoordsOffset"].SetValue(FlashNoiseOffset);
            backgroundShader.Shader.Parameters["flashPosition"].SetValue(FlashPosition);
            backgroundShader.Shader.Parameters["flashIntensity"].SetValue(FlashIntensity);
            backgroundShader.Shader.Parameters["flashNoiseZoom"].SetValue(0.02f);
            backgroundShader.Shader.Parameters["uScreenPosition"].SetValue(Main.screenPosition);
            backgroundShader.SetShaderTexture(texture ?? ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Neurons2"));
            backgroundShader.SetShaderTexture(ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/GreyscaleTextures/WavyBlotchNoise"), 2);
            backgroundShader.UseColor(color);
            backgroundShader.UseShaderSpecificData(new Vector4(screenArea.Y, screenArea.X, 0f, 0f));
            backgroundShader.Apply();
            Main.spriteBatch.Draw(pixel, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, textureArea, 0, 0f);
        }

        public void DrawFog()
        {
            Texture2D pixel = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Pixel").Value;
            Vector2 screenArea = new(Main.instance.GraphicsDevice.Viewport.Width, Main.instance.GraphicsDevice.Viewport.Height);
            Vector2 textureArea = screenArea / pixel.Size();

            var gd = Main.instance.GraphicsDevice;
            var backgroundShader = GameShaders.Misc[$"{CalRemix.instance.Name}:DarkFogShader"];
            gd.Textures[1] = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Smudges").Value;
            backgroundShader.Shader.Parameters["uTargetPosition"].SetValue((fogCenter - Main.screenPosition) / screenArea);
            backgroundShader.Shader.Parameters["uScreenResolution"].SetValue(screenArea);
            backgroundShader.Shader.Parameters["fogTravelDistance"].SetValue(fogSpreadDistance);
            backgroundShader.Apply();
            Main.spriteBatch.Draw(pixel, Vector2.Zero, null, FogColor * fogIntensity, 0f, Vector2.Zero, textureArea, 0, 0f);
        }

        public void DrawRubble(float minDepth, float maxDepth)
        {
            Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f);
            Rectangle cutoffArea = new(-1000, -1000, 4000, 4000);
            Texture2D texture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Bosses/Noxus/BackgroundRubble").Value;

            for (int i = 0; i < rubble.Count; i++)
            {
                if (rubble[i].Depth > minDepth && rubble[i].Depth < maxDepth)
                {
                    Vector2 rubbleScale = new(1f / rubble[i].Depth, 0.9f / rubble[i].Depth);
                    Vector2 position = (rubble[i].Position - screenCenter) * rubbleScale + screenCenter - Main.screenPosition;
                    if (cutoffArea.Contains((int)position.X, (int)position.Y))
                    {
                        Rectangle frame = texture.Frame(3, 1, rubble[i].Variant, 0);
                        Main.spriteBatch.Draw(texture, position, frame, Color.White * rubble[i].Opacity * intensity * 0.1f, 0f, frame.Size() * 0.5f, rubbleScale.X, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        public override float GetCloudAlpha() => 1f - MathHelper.Clamp(intensity, SkyIntensityOverride, 1f);

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
        }

        public override bool IsActive()
        {
            return isActive || intensity > 0f;
        }
    }
}
