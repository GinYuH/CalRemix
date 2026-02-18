using System.Collections.Generic;
using Terraria;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.ModLoader;
using CalRemix.Content.Tiles;

namespace CalRemix.Core.Subworlds
{
    public class FannySubworld : Subworld, IDisableBuilding
    {
        public override int Height => 900;
        public override int Width => 900;
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new FannyGeneration()
        };

        public override void OnEnter()
        {
            base.OnEnter();
        }
    }
    public class FannyGeneration : GenPass
    {
        public FannyGeneration() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "A Fan-tastic time awaits!"; // Sets the text displayed for this pass
            Main.worldSurface = Main.maxTilesY - 42; // Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; // Hides the cavern layer way out of bounds
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.spawnTileY; j < Main.maxTilesY; j++)
                {
                    WorldGen.PlaceTile(i, j, ModContent.TileType<MeldGunkPlaced>(), true, true);
                    WorldGen.SquareTileFrame(i, j, true);
                    NetMessage.SendTileSquare(-1, i, j, 1);
                }
            }
            for (int i = 0; i < Main.spawnTileX - 20; i++)
            {
                for (int j = 0; j < Main.spawnTileY; j++)
                {
                    WorldGen.PlaceTile(i, j, ModContent.TileType<MeldGunkPlaced>(), true, true);
                    WorldGen.SquareTileFrame(i, j, true);
                    NetMessage.SendTileSquare(-1, i, j, 1);
                }
            }
        }
    }
}