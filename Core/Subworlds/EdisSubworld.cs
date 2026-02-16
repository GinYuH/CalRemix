using CalRemix.Content.Tiles;
using CalRemix.Content.Tiles.Subworlds.ClownWorld;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CalRemix.Core.Subworlds
{
    public class EdisSubworld : Subworld
    {
        public override int Height => 2000;
        public override int Width => 2000;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new EdisGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            SubworldUpdateMethods.UpdateTiles();
            SubworldUpdateMethods.UpdateTileEntities();

            Main.LocalPlayer.ManageSpecialBiomeVisuals("CalRemix:EdisSky", true);
            SkyManager.Instance.Activate("CalRemix:EdisSky", Main.LocalPlayer.position);

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
    public class EdisGeneration : GenPass
    {
        public EdisGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            CalRemixHelper.PerlinGeneration(new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY), 0.6f, 0.1f, new Vector2(840, 150), ModContent.TileType<FunnyBalloonTile>(), 0, CalRemixHelper.PerlinEase.EaseAirTopSolidBottom, 0.3f, 0.2f);
        }
    }
}