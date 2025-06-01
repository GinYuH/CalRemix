using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalRemix.Content.Tiles;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalRemix.Content.Walls;

namespace CalRemix.Core.World.Subworld
{
    public class BeautifulWinterWorld : ModSystem
    {
        public static void GenerateWinterWorld()
        {
            // The rectangle that the strait spawns in
            Rectangle straitRect = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);

            // Generate the main terrain
            GenerateBlocks(straitRect);

            // Generate Tanzanite crystals
            GenerateTanzanite(straitRect);

            // Generate smol brine clouds
            GenerateBrine(straitRect);
        }

        public static void GenerateBlocks(Rectangle straitRect)
        {
            int sizeX = straitRect.Width;
            int sizeY = straitRect.Height;

            // Maps to store what blocks should and shouldn't be converted
            bool[,] map = new bool[sizeX, sizeY];
            bool[,] sandMap = new bool[sizeX, sizeY];
            bool[,] rackMap = new bool[sizeX, sizeY];

            // Grab three sets of perlin noise and populate the maps with them
            for (int i = 0; i < straitRect.Width; i++)
            {
                for (int j = 0; j < straitRect.Height; j++)
                {
                    float noise = CalamityUtils.PerlinNoise2D(i / 180f, j / 120f, 3, (int)Main.GlobalTimeWrappedHourly) * 0.5f + 0.5f;
                    float noise2 = CalamityUtils.PerlinNoise2D(i / 180f, j / 120f, 2, (int)Main.GlobalTimeWrappedHourly * 3) * 0.5f + 0.5f;
                    float noise3 = CalamityUtils.PerlinNoise2D(i / 100f, j / 100f, 2, (int)Main.GlobalTimeWrappedHourly * 2) * 0.5f + 0.5f;
                    map[i, j] = MathHelper.Distance(noise, 0.56f) < 0.1f;
                    sandMap[i, j] = MathHelper.Distance(noise2, 0.56f) < 0.1f;
                    rackMap[i, j] = MathHelper.Distance(noise3, 0.56f) < 0.1f;
                }
            }
            // Iterate through the maps and add blocks accordingly
            for (int gdv = 0; gdv < 1; gdv++)
            {
                for (int i = 0; i < straitRect.Width; i++)
                {
                    for (int j = 0; j < straitRect.Height; j++)
                    {
                        if (!WorldGen.InWorld(straitRect.X + i, straitRect.Y + j))
                        {
                            continue;
                        }
                        Tile t = Main.tile[straitRect.X + i, straitRect.Y + j];
                        if (t.HasTile)
                        {
                            continue;
                        }
                        int checX = WorldGen.currentWorldSeed.ToLower() == "banishedbaron" ? Main.maxTilesX : 0;
                        int checY = WorldGen.currentWorldSeed.ToLower() == "banishedbaron" ? Main.maxTilesY : 0;
                        // Cell stuff
                        int sur = SurroundingTileCounts(map, i, j, checX, checY);
                        int sur2 = SurroundingTileCounts(sandMap, i, j, checX, checY);
                        int sur3 = SurroundingTileCounts(rackMap, i, j, checX, checY);

                        // If there are more than 4 nearby Banished Platings, place some Banished Plating
                        if (sur > 4)
                        {
                            t.ResetToType((ushort)TileType<BanishedPlatingPlaced>());
                            WorldGen.SquareTileFrame(straitRect.X + i, straitRect.Y + j);
                            map[i, j] = true;
                        }
                        // If there are less than 4 Banished Platings nearby, remove any Banished Plating
                        else if (sur < 4)
                        {
                            t.ClearEverything();
                            map[i, j] = false;
                        }
                        // No matter what, add Banished Plating Walls
                        if (t.WallType == 0)
                            t.WallType = (ushort)WallType<BanishedPlatingWallPlaced>();

                        WorldGen.SquareWallFrame(straitRect.X + i, straitRect.Y + j);
                    }
                }
            }
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

        public static void GenerateBrine(Rectangle straitRect)
        {
            int maxBrine = 44;
            // Significantly more clouds on the baron seed
            if (WorldGen.currentWorldSeed.ToLower() == "banishedbaron")
            {
                maxBrine *= 22 * (1 + WorldGen.GetWorldSize());
            }
            int brineChance = 2222;
            int brineCount = 0;
            // Loop 8 times until enough brine has been spawned
            for (int rf = 0; rf < 8; rf++)
            {
                // Cut immediately if enough brine has been spawned
                if (brineCount > maxBrine)
                    break;
                for (int i = straitRect.X; i < straitRect.X + straitRect.Width; i++)
                {
                    if (brineCount > maxBrine)
                        break;
                    for (int j = straitRect.Y; j < straitRect.Y + straitRect.Height; j++)
                    {
                        if (brineCount > maxBrine)
                            break;
                        if (Main.rand.NextBool(brineChance))
                        {
                            // Inworld checks
                            if (WorldGen.InWorld(i, j - 1) && WorldGen.InWorld(i + 1, j) && WorldGen.InWorld(i - 1, j) && WorldGen.InWorld(i, j) && WorldGen.InWorld(i, j + 1))
                            {
                                // Check if surrounding tiles are all free
                                if (!Main.tile[i, j - 1].HasTile && !Main.tile[i, j].HasTile && !Main.tile[i + 1, j].HasTile && !Main.tile[i - 1, j].HasTile && !Main.tile[i, j + 1].HasTile)
                                {
                                    // Generate clouds of random widths and heights
                                    for (int p = i - Main.rand.Next(2, 4); p < i + Main.rand.Next(2, 4); p++)
                                    {
                                        for (int q = j - Main.rand.Next(2, 4); q < j + Main.rand.Next(2, 4); q++)
                                        {
                                            int dist = (p - i) * (p - i) + (q - j) * (q - j);
                                            if (dist > Main.rand.Next(2, 4) * Main.rand.Next(2, 4))
                                                continue;

                                            if (WorldGen.InWorld(p, q, 1))
                                            {
                                                Main.tile[p, q].ResetToType((ushort)TileType<BaronBrinePlaced>());
                                                WorldGen.SquareTileFrame(p, q);
                                            }
                                        }
                                    }
                                    brineCount++;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void Cornerlands(bool? left)
        {
            int posX = left != false ? 0 : Main.maxTilesX - WorldGen.beachDistance / 2;
            int maxX = left != false ? WorldGen.beachDistance / 4 : Main.maxTilesX;
            for (int i = posX; i < maxX; i++)
            {
                for (int j = 1; j < WorldGen.beachDistance / 4; j++)
                {
                    if (WorldGen.InWorld(i, j))
                    {
                        // All tiles take after the tile type at the very top of the world
                        if (Main.tile[i, 0].TileType != 0)
                            Main.tile[i, j].ResetToType(Main.tile[i, 0].TileType);
                        else
                            Main.tile[i, j].ClearTile();

                    }
                }
            }
        }

        // thank you random youtube video
        // https://www.youtube.com/watch?v=v7yyZZjF1z4
        public static int SurroundingTileCounts(bool[,] map, int x, int y, int checkDistX = 0, int chestDistY = 0)
        {
            if (chestDistY == 0)
            {
                chestDistY = (int)Main.worldSurface;
                checkDistX = WorldGen.beachDistance;
            }
            int wallCount = 0;
            for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
            {
                for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < checkDistX && neighbourY >= 0 && neighbourY < chestDistY)
                    {
                        if (neighbourX != x || neighbourY != y)
                        {
                            wallCount += map[neighbourX, neighbourY].ToInt();
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }
            return wallCount;
        }
    }
}