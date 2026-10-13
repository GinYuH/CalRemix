using System.Collections.Generic;
using Terraria;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Graphics.Effects;

namespace CalRemix.Core.Subworlds
{
    public class FlashSubworld : Subworld
    {
        public override int Height => 222;
        public override int Width => 822;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new ScreamingGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            SkyManager.Instance["Ambience"].Deactivate();
            Main.LocalPlayer.ManageSpecialBiomeVisuals("CalRemix:FlashSky", true);
            SkyManager.Instance.Activate("CalRemix:FlashSky", Main.LocalPlayer.position);
            base.Update();
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            string str = CalRemixHelper.LocalText("StatusText.Screaming").Value;
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(Main.spriteBatch,
                str,
                Main.ScreenSize.ToVector2() * 0.5f - size * 0.5f, Color.White, 2);

        }
    }
}