using CalamityMod;
using CalamityMod.Schematics;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.World
{
    public class FannySeed : ModSystem
    {

        public static void PlaceFanny(int x, int y, int width, int height)
        {
            // cut off at the top octagon
            for (int i = x - width; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    if (WorldGen.InWorld(i, j, 1) && Math.Abs(i - x) + Math.Abs(j - y) < Math.Sqrt(width * width + height * height))
                    {
                        //if (WorldGen.CheckTileBreakability(i, j) == 0)
                        {
                            //if (Main.tile[i, j].HasTile)
                            {
                                Main.tile[i, j].Get<TileWallBrightnessInvisibilityData>().IsTileFullbright = true;
                            }
                        }
                    }
                }
            }
            RightTriangleGen(x - width, y, (int)(width * 0.7f) * 2, (int)(height * 1.0f));
            RightTriangleGen(x, y + (int)(height * 0.22f), (int)(width * 0.5f) * 2, (int)(height * 0.7f));
        }

        // at the moment this only supports a /| angled triangle
        public static void RightTriangleGen(int x, int y, int width, int height)
        {
            float slope = -(float)height / width;
            float b = y - slope * x;
            for (int i = x; i < x + width; i++)
            {
                for (int j = y - height; j < y; j++)
                {
                    if (j >= slope * i + b)
                    {
                        if (WorldGen.InWorld(i, j))
                        {
                            //if (Main.tile[i, j].HasTile)
                            {
                                Main.tile[i, j].Get<TileWallBrightnessInvisibilityData>().IsTileFullbright = true;
                            }
                        }
                    }
                }
            }
        }

        public static void GenerateFannySeed()
        {
            int iterations = Main.maxTilesX * 10;
            int spaceFromBorder = 25;
            for (int i = 0; i < 3; i++)
            {
                int islandSize = WorldGen.genRand.Next(235, 266);
                Vector2 vector = new Vector2(115 + WorldGen.genRand.Next(20), (int)Main.worldSurface - 100 + WorldGen.genRand.Next(-20, 21));
                switch (i)
                {
                    case 1:
                        vector = new Vector2(Main.maxTilesX - 115 - WorldGen.genRand.Next(20), (int)Main.worldSurface - 100 + WorldGen.genRand.Next(-20, 21));
                        break;
                    case 2:
                        vector = new Vector2(Main.maxTilesX / 2 + WorldGen.genRand.Next(-50, 51), (int)Main.worldSurface - WorldGen.genRand.Next(101));
                        islandSize = WorldGen.genRand.Next(100, 201);
                        break;
                }
                for (int j = (int)vector.X - islandSize; (float)j <= vector.X + (float)islandSize; j++)
                {
                    for (int k = (int)vector.Y - islandSize; (float)k <= vector.Y + (float)islandSize; k++)
                    {
                        if (WorldGen.InWorld(j, k) && Vector2.Distance(vector, new Vector2(j, k)) < (float)islandSize)
                        {
                            Main.tile[j, k].Get<TileWallBrightnessInvisibilityData>().IsTileFullbright = true;
                        }
                    }
                }
            }
            for (int l = 0; l < iterations; l++)
            {
                Vector2 originPoint = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), WorldGen.genRand.Next(50, Main.maxTilesY - 50));
                int spacing = 10;
                int size = WorldGen.genRand.Next(30, 170);
                if (l > iterations / 2)
                {
                    size /= 2;
                    spacing /= 2;
                }
                bool invalidPoint = false;
                for (int i = (int)originPoint.X - size - spacing; (float)i <= originPoint.X + (float)size + (float)spacing; i++)
                {
                    for (int j = (int)originPoint.Y - size - spacing; (float)j <= originPoint.Y + (float)size + (float)spacing; j++)
                    {
                        if (WorldGen.InWorld(i, j) && Vector2.Distance(originPoint, new Vector2(i, j)) < (float)(size + spacing) && Main.tile[i, j].IsTileFullbright)
                        {
                            invalidPoint = true;
                            break;
                        }
                    }
                    if (invalidPoint)
                    {
                        break;
                    }
                }
                if (invalidPoint)
                {
                    continue;
                }
                PlaceFanny((int)originPoint.X, (int)originPoint.Y, size, size);
            }
            for (int i = spaceFromBorder; i < Main.maxTilesX - spaceFromBorder; i++)
            {
                for (int j = spaceFromBorder; j < Main.maxTilesY - 100; j++)
                {
                    if (!Main.tile[i, j].IsTileFullbright)
                    {
                        Main.tile[i, j].Clear(TileDataType.All);
                    }
                }
            }
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Main.tile[i, j].Get<TileWallBrightnessInvisibilityData>().IsTileFullbright = false;
                }
            }
        }
    }
}