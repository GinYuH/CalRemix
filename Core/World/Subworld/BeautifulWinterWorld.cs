using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalRemix.Content.Tiles;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalRemix.Content.Walls;
using System.Configuration;
using System;
using Terraria.WorldBuilding;
using static tModPorter.ProgressUpdate;
using CalamityMod.Tiles.AstralDesert;
using static Terraria.WorldGen;
using ReLogic.Utilities;
using Terraria.GameContent.Generation;
using Terraria.GameContent.UI.Elements;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using static CalRemix.Core.Subworlds.SubworldHelpers;

namespace CalRemix.Core.World.Subworld
{
    public class BeautifulWinterWorld : ModSystem
    {
        public static void GenerateWinterWorld()
        {
            //TODO
            //-SurfaceCaves have a ton of while loops that take super long to calc on smaller worlds
            //-Lakes loves to go out of bounds
            //-so does Flowers
            
            // The rectangle that the strait spawns in
            Rectangle straitRect = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);

            /*GenerateBasicTerrain_Recoded();
            Tunnels();
            DirtWallBackgrounds();
            // riight after the mega barebones world shaping youd wanna do other dirt-stone-based stuff
            // as well as other world-shaping things
            RocksInDirt();
            DirtInRocks();
            Clay();
            SmallHoles();
            DirtLayerCaves();
            RockLayerCaves();
            //SurfaceCaves(); dont use rn
            WavyCaves();
            Grass(); // dont use without grass
            FloatingIslands(5);
            //Lakes(5); dont use rn
            CleanUpDirt();
            DirtRockWallRunner();
            // super basic stuff done genning, dirt is pretty much finalized except sloping
            GenerateTanzanite(straitRect); // modded
            SmoothWorld();
            SettleLiquids();
            Waterfalls();
            WallVariety(); // dont use without SmallHoles
            QuickCleanup();
            SpreadingGrass(); // dont use without grass
            Piles();
            SpawnPoint();
            // foliage stuff next
            // by now the worlds major stuff is genned entirely, now we just place flavor around
            GrassWall(); // dont use without grass
            Sunflowers(); // dont use without grass
            PlantingTrees();
            Weeds(); // dont use without grass
            Vines(); // dont use without grass
            //Flowers(); dont use rn
            Mushrooms(); // dont use without grass
            Stalac();
            SettleLiquidsAgain();
            // final cleanup; never run anything after here unless ur dumb 
            TileCleanup();
            FinalCleanup();*/


            // heres a more stripped example, no grass-related stuff and auto turned into an ice biome
            // we gen everything first, then replace everything with snow and ice ns hit
            GenerateBasicTerrain_Recoded();
            Tunnels();
            DirtWallBackgrounds();
            // riight after the mega barebones world shaping youd wanna do other dirt-stone-based stuff
            // as well as other world-shaping things
            RocksInDirt();
            DirtInRocks();
            SmallHoles();
            DirtLayerCaves();
            RockLayerCaves();
            //SurfaceCaves(); dont use rn
            WavyCaves();
            FloatingIslands(5);
            GenerateIceBiome();
            //Lakes(5); dont use rn
            CleanUpDirt();
            DirtRockWallRunner();
            // super basic stuff done genning, dirt is pretty much finalized except sloping
            GenerateTanzanite(straitRect); // modded
            SmoothWorld();
            SettleLiquids();
            Waterfalls();
            QuickCleanup();
            Piles();
            SpawnPoint();
            // foliage stuff next
            // by now the worlds major stuff is genned entirely, now we just place flavor around
            PlantingTrees();
            Stalac();
            SettleLiquidsAgain();
            // final cleanup; never run anything after here unless ur dumb 
            TileCleanup();
            FinalCleanup();
        }

        public static void GenerateTanzanite(Rectangle straitRect)
        {
            int crystalCount = 0;
            int maxCrystals = 66;
            // Significantly more crystals on the baron seed
            if (WorldGen.currentWorldSeed.ToLower() == "banishedbaron")
            {
                maxCrystals *= 22 * (1 + WorldGen.GetWorldSize());
            }
            int crystalFrequency = 2222;
            // Loop 8 times to assure enough crystals are spawned
            for (int rf = 0; rf < 8; rf++)
            {
                // Break the loop if enough crystals have been spawned
                if (crystalCount > maxCrystals)
                    break;
                // Go through the tile array and randomly place crystals
                for (int i = straitRect.X; i < straitRect.X + straitRect.Width; i++)
                {
                    if (crystalCount > maxCrystals)
                        break;
                    for (int j = straitRect.Y; j < straitRect.Y + straitRect.Height; j++)
                    {
                        if (crystalCount > maxCrystals)
                            break;
                        // If a crystal rolls, attempt placing one
                        if (Main.rand.NextBool(crystalFrequency))
                        {
                            // Don't try to do anything if out of bounds
                            if (!WorldGen.InWorld(i, j - 1) || !WorldGen.InWorld(i, j))
                                continue;
                            // If the tile has no air above it or is air itself, don't try placing a crystal
                            if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j].HasTile)
                            {
                                // Randomize crystal width and height
                                int crystalWidth = Main.rand.Next(3, 6);
                                int crystalHeight = Main.rand.Next(7, 16);
                                for (int x = i; x < crystalWidth + i; x++)
                                {
                                    for (int y = j - crystalHeight; y < j; y++)
                                    {
                                        if (WorldGen.InWorld(x, y))
                                        {
                                            // Replace only air or Baron Strait tiles
                                            if (!Main.tile[x, y].HasTile || Main.tile[x, y].TileType == TileType<BaronsandPlaced>() || Main.tile[x, y].TileType == TileType<BanishedPlatingPlaced>() || Main.tile[x, y].TileType == TileType<BrinerackPlaced>())
                                            {
                                                Main.tile[x, y].ResetToType((ushort)TileType<TanzaniteGlassPlaced>());

                                                // Slope the top left and right tiles on the crystal
                                                // Arbitrary bound check because apparently SlopeTile is weird 
                                                if (x > 50 && x < Main.maxTilesX - 50 && y > 50 && y < Main.maxTilesY - 50)
                                                {
                                                    if (Main.tile[x, y].TileType == TileType<TanzaniteGlassPlaced>())
                                                        if (y == j - crystalHeight)
                                                        {
                                                            // Slope the left 
                                                            if (x == i)
                                                            {
                                                                if (Main.tile[x, y].HasTile)
                                                                    WorldGen.SlopeTile(x, y, (int)SlopeType.SlopeDownRight);
                                                            }
                                                            // Slope the right
                                                            if (x == crystalWidth + i - 1)
                                                            {
                                                                if (Main.tile[x, y].HasTile)
                                                                    WorldGen.SlopeTile(x, y, (int)SlopeType.SlopeDownLeft);
                                                            }
                                                        }
                                                }
                                                WorldGen.SquareTileFrame(x, y);
                                                // Assure crystals don't spawn floating in the air
                                                // Technically they can still float if they spawn above a chasm higher than 22 blocks but that should be rare
                                                if (y == j - 1)
                                                {
                                                    for (int q = 1; q < 22; q++)
                                                    {
                                                        if (WorldGen.InWorld(x, y + q))
                                                        {
                                                            if (!Main.tile[x, y + q].HasTile)
                                                            {
                                                                Main.tile[x, y + q].ResetToType((ushort)TileType<TanzaniteGlassPlaced>());
                                                                WorldGen.SquareTileFrame(x, y + q);
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                crystalCount++;
                            }
                        }
                    }
                }
            }
        }

        public static void PlaceTreesAround()
        {

        }
        public static void PlaceTree(Point location, int trunkHeightAmount, int individualHeight)
        {
            List<int> pointsForFronds = new();

            // trunk
            for (int i = 0; i < trunkHeightAmount; i++)
            {
                Point pointAccountForDistance = new Point(location.X, location.Y - (i * individualHeight));
                WorldUtils.Gen(pointAccountForDistance, new Shapes.Mound(5, individualHeight), new Actions.PlaceTile(TileID.PineTree));
                if (i != 0 && i % 2 == 0)
                {
                    pointsForFronds.Add(i);
                    WorldUtils.Gen(new Point(pointAccountForDistance.X + 3, pointAccountForDistance.Y + individualHeight), new Shapes.Mound(5, individualHeight), new Actions.PlaceTile(TileID.PineTree));
                    WorldUtils.Gen(new Point(pointAccountForDistance.X - 3, pointAccountForDistance.Y + individualHeight), new Shapes.Mound(5, individualHeight), new Actions.PlaceTile(TileID.PineTree));
                }
            }

            // leaves
            int iterationCount = pointsForFronds.Count() * 3;
            for (int i = 0; i < pointsForFronds.Count(); i++)
            {
                for (int ii = 0; ii < iterationCount; ii++)
                {
                    Point pointAccountForDistance = new Point(location.X, location.Y - (pointsForFronds[i] * individualHeight));
                    WorldUtils.Gen(new Point(pointAccountForDistance.X + ii, pointAccountForDistance.Y), new Shapes.Mound(5, (int)(individualHeight * 1.5f)), new Actions.PlaceTile(TileID.PineTree));
                    WorldUtils.Gen(new Point(pointAccountForDistance.X - ii, pointAccountForDistance.Y), new Shapes.Mound(5, (int)(individualHeight * 1.5f)), new Actions.PlaceTile(TileID.PineTree));
                }
                iterationCount -= 3;
            }
        }
    }
}