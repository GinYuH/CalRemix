using System;
using System.Security.Policy;
using CalamityMod;
using CalamityMod.Projectiles.Melee;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Core.Biomes;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Carcinogen
{
    public class HorizonSky : CustomSky
    {
        public float BackgroundIntensity;
        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.IsActive<HorizonSubworld>();
            }
        }

        public static Color current = new();

        public override void Update(GameTime gameTime)
        {
            if (!CanSkyBeActive)
            {
                BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity - 0.08f, 0f, 1f);
                Deactivate(Array.Empty<object>());
                return;
            }

            if (!NPC.AnyNPCs(ModContent.NPCType<Crevivence>()))
                Crevivence.SunOpacity = 1;

            BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity + 0.01f, 0f, 1f);

            Opacity = BackgroundIntensity;
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (minDepth < 0)
            {
                float yCompletion = Utils.GetLerpValue(0, Main.maxTilesY * 16 * 0.9f, Main.LocalPlayer.Center.Y, true);
                for (int i = 0; i < 100; i++)
                {
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Vector2(0, Main.screenHeight / 100f * i), new Rectangle(0, 0, (int)Main.screenWidth, 2 + (int)(Main.screenHeight / 100f)), Color.Lerp(Color.PaleGoldenrod, new Color(128, 113, 120), 1 - (i / 99f)), 0, Vector2.Zero, 1, 0, 0); ;
                }
                Texture2D sun = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;
                spriteBatch.EnterShaderRegion(BlendState.Additive);
                //float unlocked = Main.maxTilesY * 0.9f * 16 - Main.screenPosition.Y;
                Vector2 sunPosition = new Vector2(Main.screenWidth * 0.02f, Main.screenHeight * 0.8f);
                Vector2 sunEndPosition = new Vector2(Main.screenWidth * 0.02f,  Main.screenHeight * 1f);
                sunPosition = Vector2.Lerp(sunPosition, sunEndPosition, 1 - yCompletion);
                spriteBatch.Draw(sun, sunPosition, null, Color.Yellow, 0, sun.Size() / 2, 5 * Crevivence.SunOpacity, 0, 0);
                spriteBatch.Draw(sun, sunPosition, null, Color.White, 0, sun.Size() / 2, 4.5f * Crevivence.SunOpacity, 0, 0);
                spriteBatch.ExitShaderRegion();
            }
        }

        public override Color OnTileColor(Color inColor) => inColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
