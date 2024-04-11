using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalRemix.Tiles;
using System.Threading;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.DraedonStructures;
using CalamityMod.World;
using CalamityMod.Tiles.SunkenSea;
using CalamityMod;
using ReLogic.Utilities;
using System.CodeDom;
using CalRemix.Walls;
using ReLogic.Content;

namespace CalRemix
{
    public class BaronStrait : ModSystem
    {
        public static void GenerateBaronStrait(bool left)
        {
            int posX = left ? 0 : Main.maxTilesX - WorldGen.beachDistance;
            Rectangle straitRect = new Rectangle(posX, 0, WorldGen.beachDistance, (int)Main.worldSurface);
            GenerateBlocks(straitRect);
            GenerateTanzanite(straitRect);
            GenerateBrine(straitRect);
            Cornerlands(left);
        }

        public static void GenerateBlocks(Rectangle straitRect)
        {
            bool[,] map = new bool[WorldGen.beachDistance, (int)(Main.worldSurface)];
            bool[,] sandMap = new bool[WorldGen.beachDistance, (int)(Main.worldSurface)];
            bool[,] rackMap = new bool[WorldGen.beachDistance, (int)(Main.worldSurface)];

            for (int i = 0; i < straitRect.Width; i++)
            {
                for (int j = 0; j < straitRect.Height; j++)
                {
                    float noise = CalamityUtils.PerlinNoise2D(i / 180f, j / 120f, 3, Int32.Parse(WorldGen.currentWorldSeed)) * 0.5f + 0.5f;
                    float noise2 = CalamityUtils.PerlinNoise2D(i / 180f, j / 120f, 2, Int32.Parse(WorldGen.currentWorldSeed)) * 0.5f + 0.5f;
                    float noise3 = CalamityUtils.PerlinNoise2D(i / 100f, j / 100f, 2, Int32.Parse(WorldGen.currentWorldSeed)) * 0.5f + 0.5f;
                    map[i, j] = MathHelper.Distance(noise, 0.56f) < 0.1f;
                    sandMap[i, j] = MathHelper.Distance(noise2, 0.56f) < 0.1f;
                    rackMap[i, j] = MathHelper.Distance(noise3, 0.56f) < 0.1f;
                }
            }
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
                        int sur = SurroundingTileCounts(map, i, j);
                        int sur2 = SurroundingTileCounts(sandMap, i, j);
                        int sur3 = SurroundingTileCounts(rackMap, i, j);

                        if (sur > 4)
                        {
                            t.ResetToType((ushort)TileType<BanishedPlatingPlaced>());
                            WorldGen.SquareTileFrame(straitRect.X + i, straitRect.Y + j);
                            map[i, j] = true;
                        }
                        else if (sur < 4)
                        {
                            t.ClearEverything();
                            map[i, j] = false;
                        }
                        if (t.WallType == 0)
                            t.WallType = (ushort)WallType<BanishedPlatingWallPlaced>();
                        
                        if (sur2 > 4)
                        {
                            if (t.TileType == TileType<BanishedPlatingPlaced>())
                            {
                                t.ResetToType((ushort)TileType<BaronsandPlaced>());
                            }
                            if (t.WallType ==  WallType<BanishedPlatingWallPlaced>())
                                t.WallType = (ushort)WallType<BaronsandWallPlaced>();
                            WorldGen.SquareTileFrame(straitRect.X + i, straitRect.Y + j);
                            sandMap[i, j] = true;
                        }
                        if (sur3 < 4)
                        {
                            if (t.TileType == TileType<BaronsandPlaced>())
                            {
                                t.ResetToType((ushort)TileType<BrinerackPlaced>());
                            }
                            rackMap[i, j] = true;
                        }

                        WorldGen.SquareWallFrame(straitRect.X + i, straitRect.Y + j);
                    }
                }
            }
        }
        public static void GenerateTanzanite(Rectangle straitRect)
        {
            int crystalCount = 0;
            int maxCrystals = 66;
            int crystalFrequency = 2222;
            for (int rf = 0; rf < 8; rf++)
            {
                if (crystalCount > maxCrystals)
                    break;
                for (int i = straitRect.X; i < straitRect.X + straitRect.Width; i++)
                {
                    if (crystalCount > maxCrystals)
                        break;
                    for (int j = straitRect.Y; j < straitRect.Y + straitRect.Height; j++)
                    {
                        if (crystalCount > maxCrystals)
                            break;
                        if (Main.rand.NextBool(crystalFrequency))
                        {
                            if (!WorldGen.InWorld(i, j - 1) || !WorldGen.InWorld(i, j))
                                continue;
                            if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j].HasTile)
                            {
                                int crystalWidth = Main.rand.Next(3, 6);
                                int crystalHeight = Main.rand.Next(7, 16);
                                for (int x = i; x < crystalWidth + i; x++)
                                {
                                    for (int y = j - crystalHeight; y < j; y++)
                                    {
                                        if (WorldGen.InWorld(x, y))
                                        {
                                            if (!Main.tile[x, y].HasTile || (Main.tile[x, y].TileType == TileType<BaronsandPlaced>() || Main.tile[x, y].TileType == TileType<BanishedPlatingPlaced>() || Main.tile[x, y].TileType == TileType<BrinerackPlaced>()))
                                            {
                                                Main.tile[x, y].ResetToType((ushort)TileType<TanzaniteGlassPlaced>());
                                                if (y == j - crystalHeight)
                                                {
                                                    if (x == i)
                                                    {
                                                        if (Main.tile[x,y].HasTile)
                                                            WorldGen.SlopeTile(x, y, (int)SlopeType.SlopeDownRight);
                                                    }
                                                    if (x == crystalWidth + i - 1)
                                                    {
                                                        if (Main.tile[x, y].HasTile)
                                                            WorldGen.SlopeTile(x, y, (int)SlopeType.SlopeDownLeft);
                                                    }
                                                }
                                                WorldGen.SquareTileFrame(x, y);
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
            int brineChance = 2222;
            int brineCount = 0;
            for (int rf = 0; rf < 8; rf++)
            {
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
                            if (WorldGen.InWorld(i, j-1) && WorldGen.InWorld(i+1, j) && WorldGen.InWorld(i-1, j) && WorldGen.InWorld(i, j) && WorldGen.InWorld(i, j + 1))
                            if (!Main.tile[i, j - 1].HasTile && !Main.tile[i, j].HasTile && !Main.tile[i + 1, j].HasTile && !Main.tile[i - 1, j].HasTile && !Main.tile[i, j+1].HasTile)
                            {
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

        public static void Cornerlands(bool left)
        {
            int posX = left ? 0 : Main.maxTilesX - WorldGen.beachDistance / 2;
            int maxX = left ? WorldGen.beachDistance / 4 : Main.maxTilesX;
            for (int i = posX; i < maxX; i++)
            {
                for (int j = 1; j < WorldGen.beachDistance / 4; j++)
                {
                    if (WorldGen.InWorld(i, j))
                    {
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
        public static int SurroundingTileCounts(bool[,] map, int x, int y)
        {
            int wallCount = 0;
            for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
            {
                for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < WorldGen.beachDistance && neighbourY >= 0 && neighbourY < (int)(Main.worldSurface))
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