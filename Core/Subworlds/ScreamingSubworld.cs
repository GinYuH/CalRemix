using System.Collections.Generic;
using Terraria;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ID;
using CalRemix.Core.World;
using CalRemix.Content.Tiles;
using Terraria.Graphics.Effects;

namespace CalRemix.Core.Subworlds
{
    public class ScreamingSubworld : Subworld
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
            Main.LocalPlayer.ManageSpecialBiomeVisuals("CalRemix:ScreamingFaceSky", true);
            SkyManager.Instance.Activate("CalRemix:ScreamingFaceSky", Main.LocalPlayer.position);
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
    public class ScreamingGeneration : GenPass
    {
        public ScreamingGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (j % 50 != 0)
                        continue;
                    WorldGen.PlaceTile(i, j, TileID.Platforms, true, true);
                    Main.tile[i, j].TileFrameY = 252;
                    WorldGen.SquareTileFrame(i, j, true);
                    NetMessage.SendTileSquare(-1, i, j, 1);
                }
            }
            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<ScreamDoor>());
        }
    }
}