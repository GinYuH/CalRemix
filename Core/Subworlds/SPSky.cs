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
    public class SPSky : CustomSky
    {
        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.Current == ModContent.GetInstance<SingularPointSubworld>();
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
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, (int)Main.screenWidth, (int)Main.screenHeight), Color.Black);
        }

        public override Color OnTileColor(Color inColor) => DrawColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
