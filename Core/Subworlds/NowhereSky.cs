using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using static Terraria.Main;

namespace CalRemix.Core.Subworlds
{
    public class NowhereSky : CustomSky
    {
        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.Current == ModContent.GetInstance<NowhereSubworld>();
            }
        }

        public static readonly Color DrawColor = Color.White;

        public override void Update(GameTime gameTime)
        {
            if (!CanSkyBeActive)
            {
                Deactivate(Array.Empty<object>());
                return;
            }

            Opacity = 1;

        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            float value = 188 / 255f;
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, (int)Main.screenWidth, (int)Main.screenHeight), new Color(value, value, value));
            Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;

            float sunY = MathHelper.Lerp(Main.screenHeight * 0.2f, (Main.maxTilesY * 16) * 0.3f - Main.screenPosition.Y - Main.screenHeight * 0.3f, Utils.GetLerpValue(0.3f, 0.301f, Main.LocalPlayer.Center.Y / (Main.maxTilesY * 16f), true));

            spriteBatch.EnterShaderRegion(BlendState.Additive);
            spriteBatch.Draw(bloom, new Vector2(Main.screenWidth / 2f, sunY), null, Color.White, 0, bloom.Size() / 2, 0.5f, 0, 0);
            spriteBatch.EnterShaderRegion(BlendState.NonPremultiplied);
            spriteBatch.Draw(bloom, new Vector2(Main.screenWidth / 2f, sunY), null, Color.Black, 0, bloom.Size() / 2, 0.3f, 0, 0);
            spriteBatch.ExitShaderRegion();
        }

        public override Color OnTileColor(Color inColor) => DrawColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
