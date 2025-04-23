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
        internal static CalRemixConfig instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("MainMenuHeader")]

        [DefaultValue(true)]
        public bool forcedMenu;

        [DefaultValue(true)]
        public bool randomMenu;

        [DefaultValue(false)]
        public bool useSecondMenu;

        [Header("VisualEffectsHeader")]
        //[Label("Screen Shatter Effects")]
        //[BackgroundColor(224, 127, 180, 192)]
        [DefaultValue(true)]
        //[Tooltip("Enables screen shatter effects. Disable if they're too straining on the eyes.")]
        public bool ScreenShatterEffects;

        //[Label("Visual Overlay Intensity")]
        //[BackgroundColor(224, 127, 180, 192)]
        [DefaultValue(0.5f)]
        [Range(0f, 1f)]
        //[Tooltip("Changes the intensity of visual overlays such as blur and chromatic aberration.")]
        public float VisualOverlayIntensity;
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