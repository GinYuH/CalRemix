using System.Collections.Generic;
using Terraria;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.GameContent.Generation;
using CalRemix.Core.World;

namespace CalRemix.Core.Subworlds
{
    public class BaronSubworld : Subworld
    {
        public override int Height => 2222;
        public override int Width => 2222;

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new PassLegacy("Banishing The Baron", (progress, config) =>
                {
                    progress.Message = "Creating a Baron Wasteland";
                    BaronStrait.GenerateBaronStrait(null, true);
                })
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            string str = "Baroning rn, please wait";
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Cyan, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(Main.spriteBatch,
                str,
                Main.ScreenSize.ToVector2() * 0.5f - size * 0.5f, Color.White, 2);

        }
    }
}