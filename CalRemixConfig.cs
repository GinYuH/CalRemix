using static Terraria.ModLoader.ModContent;
using CalRemix.Core.World;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CalRemix
{
    public class CalRemixConfig : ModConfig
    {
        public static CalRemixConfig instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool forcedMenu;

        [DefaultValue(true)]
        public bool randomMenu;

        [DefaultValue(false)]
        public bool useSecondMenu;
    }
    public class RemixButton : UIElement
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Vector2 position = new Vector2(dimensions.X + 4f, dimensions.Y + 4f);
            Color color = !CalRemixWorld.stratusDungeonDisabled ? Color.White : Color.Gray;
            spriteBatch.Draw(Request<Texture2D>("CalRemix/Core/World/StratusDungeon").Value, position, color);
            Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 6, 6);
            if (maus.Intersects(dimensions.ToRectangle() with { Height = (int)(dimensions.Height * 1.8f), Width = (int)(dimensions.Width * 1.8f) }))
            {
                Vector2 startpos = Main.MouseScreen + new Vector2(22, 22);
                string desc = "Stratus Dungeon generation: ";
                float descWidth = FontAssets.MouseText.Value.MeasureString(desc).X;
                Utils.DrawBorderString(spriteBatch, desc, startpos, Color.White);
                Utils.DrawBorderString(spriteBatch, !CalRemixWorld.stratusDungeonDisabled ? "Enabled" : "Disabled", startpos + Vector2.UnitX * descWidth, !CalRemixWorld.stratusDungeonDisabled ? Color.Green : Color.Red);
            }
        }
    }
}