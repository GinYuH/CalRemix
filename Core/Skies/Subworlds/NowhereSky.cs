using System;
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
            float value = MathHelper.Lerp(188 / 255f, 10 / 255f, Utils.GetLerpValue(maxTilesY * 0.4f, maxTilesY * 0.8f, LocalPlayer.Center.Y / 16f, true));
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), new Color(value, value, value));
            Texture2D bloom = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle").Value;
            Texture2D bloom2 = ModContent.Request<Texture2D>("CalamityMod/Particles/LargeBloom").Value;

            float sunY = MathHelper.Lerp(screenHeight * 0.2f, (maxTilesY * 16) * 0.3f - screenPosition.Y - screenHeight * 0.3f, Utils.GetLerpValue(0.3f, 0.301f, LocalPlayer.Center.Y / (maxTilesY * 16f), true));

            spriteBatch.Draw(bloom, new Vector2(screenWidth / 2f, sunY), null, Color.White with { A = 0 }, 0, bloom.Size() / 2, 1.3f, 0, 0);
            spriteBatch.Draw(bloom2, new Vector2(screenWidth / 2f, sunY), null, Color.Black, 0, bloom2.Size() / 2, 0.3f, 0, 0);
        }

        public override Color OnTileColor(Color inColor) => DrawColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !gameMenu;
    }
}
