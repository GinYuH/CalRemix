using System.Collections.Generic;
using Terraria;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.ModLoader;
using CalamityMod.Tiles.FurnitureExo;
using System;
using CalamityMod.Tiles.Ores;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ID;
using CalRemix.Core.World;
using CalRemix.Content.Tiles;
using Terraria.Graphics.Effects;
using CalamityMod;
using CalamityMod.Tiles.FurnitureAshen;
using CalRemix.UI.SubworldMap;

namespace CalRemix.Core.Subworlds
{
    public class AntSubworld : Subworld
    {
        public override int Height => 222;
        public override int Width => 822;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new AntGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();
            SubworldMapUI.TakeSubworldPicture("Ant");
        }

        public override void DrawMenu(GameTime gameTime)
        {
            base.DrawMenu(gameTime);
            string str = CalRemixHelper.LocalText("StatusText.Ant").Value;
            Vector2 size = FontAssets.MouseText.Value.MeasureString(str) * 2;
            //Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black, 0, Vector2.Zero, 1, 0, 0);
            /*Utils.DrawBorderString(Main.spriteBatch,
                str,
                Main.ScreenSize.ToVector2() * 0.5f - size * 0.5f, Color.White, 2);*/

        }
    }
    public class AntGeneration : GenPass
    {
        public AntGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    WorldGen.PlaceWall(i, j, WallID.DiamondGemspark);

                    if (j > (int)(Main.maxTilesY * 0.8f))
                    {
                        WorldGen.PlaceTile(i, j, TileID.DiamondGemspark);
                    }
                }
            }

            WorldGen.PlaceTile(Main.spawnTileX, Main.spawnTileY - 22, (ushort)ModContent.TileType<Ant>());

            for (int i = -1; i < 1; i++)
            {
                WorldGen.PlaceTile(Main.spawnTileX + i, Main.spawnTileY + 1, (ushort)ModContent.TileType<AshenPlatform>());
            }

            RandomSubworldDoors.GenerateDoorRandom(ModContent.TileType<AntDoor>());
        }
    }
}