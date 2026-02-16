using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.WorldBuilding;
using static Terraria.WorldGen;

namespace CalRemix.Core.Subworlds
{
    public static class SubworldHelpers
    {
        #region stuff
        public enum TerrainFeatureType
        {
            Plateau,
            Hill,
            Dale,
            Mountain,
            Valley
        }
        public class SurfaceHistory
        {
            private readonly double[] _heights;

            private int _index;

            public double this[int index]
            {
                get
                {
                    return this._heights[(index + this._index) % this._heights.Length];
                }
                set
                {
                    this._heights[(index + this._index) % this._heights.Length] = value;
                }
            }

            public int Length => this._heights.Length;

            public SurfaceHistory(int size)
            {
                this._heights = new double[size];
            }

            public void Record(double height)
            {
                this._heights[this._index] = height;
                this._index = (this._index + 1) % this._heights.Length;
            }
        }
        public static void GenerateBasicTerrain()
        {
            //int num = configuration.Get<int>("FlatBeachPadding");
            // according to config files the padding value is 5
            int num = 5;
            TerrainFeatureType terrainFeatureType = TerrainFeatureType.Plateau;
            int num7 = 0;
            double num8 = (double)Main.maxTilesY * 0.3;
            num8 *= (double)WorldGen.genRand.Next(90, 110) * 0.005;
            double num9 = num8 + (double)Main.maxTilesY * 0.2;
            num9 *= (double)WorldGen.genRand.Next(90, 110) * 0.01;
            double num10 = num8;
            double num11 = num8;
            double num12 = num9;
            double num13 = num9;
            double num14 = (double)Main.maxTilesY * 0.23;
            SurfaceHistory surfaceHistory = new SurfaceHistory(500);
            num7 = GenVars.leftBeachEnd + num;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                num10 = Math.Min(num8, num10);
                num11 = Math.Max(num8, num11);
                num12 = Math.Min(num9, num12);
                num13 = Math.Max(num9, num13);
                if (num7 <= 0)
                {
                    terrainFeatureType = (TerrainFeatureType)WorldGen.genRand.Next(0, 5);
                    num7 = WorldGen.genRand.Next(5, 40);
                    if (terrainFeatureType == TerrainFeatureType.Plateau)
                    {
                        num7 *= (int)((double)WorldGen.genRand.Next(5, 30) * 0.2);
                    }
                }
                num7--;
                if ((double)i > (double)Main.maxTilesX * 0.45 && (double)i < (double)Main.maxTilesX * 0.55 && (terrainFeatureType == TerrainFeatureType.Mountain || terrainFeatureType == TerrainFeatureType.Valley))
                {
                    terrainFeatureType = (TerrainFeatureType)WorldGen.genRand.Next(3);
                }
                if ((double)i > (double)Main.maxTilesX * 0.48 && (double)i < (double)Main.maxTilesX * 0.52)
                {
                    terrainFeatureType = TerrainFeatureType.Plateau;
                }
                num8 += GenerateWorldSurfaceOffset(terrainFeatureType);
                double num2 = 0.17;
                num2 = 0.25f;
                double num3 = 0.26;
                num3 = 0.3f;
                /*if (i < GenVars.leftBeachEnd + num || i > GenVars.rightBeachStart - num)
                {
                    num8 = Utils.Clamp(num8, (double)Main.maxTilesY * 0.17, num14);
                }
                else */
                if (num8 < (double)Main.maxTilesY * num2)
                {
                    num8 = (double)Main.maxTilesY * num2;
                    num7 = 0;
                }
                else if (num8 > (double)Main.maxTilesY * num3)
                {
                    num8 = (double)Main.maxTilesY * num3;
                    num7 = 0;
                }
                while (WorldGen.genRand.NextBool(0, 3))
                {
                    num9 += (double)WorldGen.genRand.Next(-2, 3);
                }
                if (num9 < num8 + (double)Main.maxTilesY * 0.06)
                {
                    num9 += 1.0;
                }
                if (num9 > num8 + (double)Main.maxTilesY * 0.35)
                {
                    num9 -= 1.0;
                }
                surfaceHistory.Record(num8);
                FillColumn(i, num8, num9);
                if (i == GenVars.rightBeachStart - num)
                {
                    if (num8 > num14)
                    {
                        RetargetSurfaceHistory(surfaceHistory, i, num14);
                    }
                    terrainFeatureType = TerrainFeatureType.Plateau;
                    num7 = Main.maxTilesX - i;
                }
            }
            Main.worldSurface = (int)(num11 + 25.0);
            Main.rockLayer = num13;
            double num4 = (int)((Main.rockLayer - Main.worldSurface) / 6.0) * 6;
            Main.rockLayer = (int)(Main.worldSurface + num4);
            int num15 = (int)(Main.rockLayer + (double)Main.maxTilesY) / 2 + WorldGen.genRand.Next(-100, 20);
            int lavaLine = num15 + WorldGen.genRand.Next(50, 80);
            int num5 = 20;
            if (num12 < num11 + (double)num5)
            {
                double num16 = (num12 + num11) / 2.0;
                double num6 = Math.Abs(num12 - num11);
                if (num6 < (double)num5)
                {
                    num6 = num5;
                }
                num12 = num16 + num6 / 2.0;
                num11 = num16 - num6 / 2.0;
            }
            GenVars.rockLayer = num9;
            GenVars.rockLayerHigh = num13;
            GenVars.rockLayerLow = num12;
            GenVars.worldSurface = num8;
            GenVars.worldSurfaceHigh = num11;
            GenVars.worldSurfaceLow = num10;
            GenVars.waterLine = num15;
            GenVars.lavaLine = lavaLine;
        }
        public static void GenerateBasicTerrain_Recoded()
        {
            TerrainFeatureType terrainFeatureType = TerrainFeatureType.Plateau;
            int terrainFeatureChangeTimer = 0;

            // making a bunch of vassal variables to contain temp stuff and be modified
            double surfaceVassal = (double)Main.maxTilesY * 0.3;
            surfaceVassal *= (double)WorldGen.genRand.Next(90, 110) * 0.005;
            double rockVassal = surfaceVassal + (double)Main.maxTilesY * 0.2;
            rockVassal *= (double)WorldGen.genRand.Next(90, 110) * 0.01;
            double surfaceLowVassal = surfaceVassal;
            double surfaceHighVassal = surfaceVassal;
            double rockLowVassal = rockVassal;
            double rockHighVassal = rockVassal;
            SurfaceHistory surfaceHistory = new SurfaceHistory(500);

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                // each loop, choose the lower/higher value of the two
                surfaceLowVassal = Math.Min(surfaceVassal, surfaceLowVassal);
                surfaceHighVassal = Math.Max(surfaceVassal, surfaceHighVassal);
                rockLowVassal = Math.Min(rockVassal, rockLowVassal);
                rockHighVassal = Math.Max(rockVassal, rockHighVassal);

                // change terrain feature type if the timer for it runs out
                if (terrainFeatureChangeTimer <= 0)
                {
                    terrainFeatureType = (TerrainFeatureType)WorldGen.genRand.Next(0, 5);
                    terrainFeatureChangeTimer = WorldGen.genRand.Next(5, 40);
                    if (terrainFeatureType == TerrainFeatureType.Plateau)
                    {
                        terrainFeatureChangeTimer *= (int)((double)WorldGen.genRand.Next(5, 30) * 0.2);
                    }
                }
                terrainFeatureChangeTimer--;

                // change terrain feature types if its funny
                // if in the middle of the world, be a plateau 100% of the time
                if ((double)i > (double)Main.maxTilesX * 0.45 && (double)i < (double)Main.maxTilesX * 0.55 && (terrainFeatureType == TerrainFeatureType.Mountain || terrainFeatureType == TerrainFeatureType.Valley))
                {
                    terrainFeatureType = (TerrainFeatureType)WorldGen.genRand.Next(3);
                }
                if ((double)i > (double)Main.maxTilesX * 0.48 && (double)i < (double)Main.maxTilesX * 0.52)
                {
                    terrainFeatureType = TerrainFeatureType.Plateau;
                }

                // add the world surface offset to the surface vassal value
                // without doing this, you get no surface!! just a flat!!!! nothing!!!!! flat earth!!!!!!!!!
                surfaceVassal += GenerateWorldSurfaceOffset(terrainFeatureType);

                // i will be honest. im not fully sure what these do. names may be inaccurate
                // if above or below a certian threshold, apply a reduction or increase in size??
                double surfaceUpperKinda = 0.17;
                double surfaceLowerKinda = 0.26;
                if (surfaceVassal < (double)Main.maxTilesY * surfaceUpperKinda)
                {
                    surfaceVassal = (double)Main.maxTilesY * surfaceUpperKinda;
                    terrainFeatureChangeTimer = 0;
                }
                else if (surfaceVassal > (double)Main.maxTilesY * surfaceLowerKinda)
                {
                    surfaceVassal = (double)Main.maxTilesY * surfaceLowerKinda;
                    terrainFeatureChangeTimer = 0;
                }

                // more noise?
                while (WorldGen.genRand.NextBool(0, 3))
                {
                    rockVassal += (double)WorldGen.genRand.Next(-2, 3);
                }
                if (rockVassal < surfaceVassal + (double)Main.maxTilesY * 0.06)
                {
                    rockVassal += 1.0;
                }
                if (rockVassal > surfaceVassal + (double)Main.maxTilesY * 0.35)
                {
                    rockVassal -= 1.0;
                }

                // record surface stuff and gen the surface
                surfaceHistory.Record(surfaceVassal);
                FillColumn(i, surfaceVassal, rockVassal);
            }

            // set a bunch of variables that help the rest of world generation
            // mainly determining where layers for things start
            Main.worldSurface = (int)(surfaceHighVassal + 25.0);
            Main.rockLayer = rockHighVassal;
            double num4 = (int)((Main.rockLayer - Main.worldSurface) / 6.0) * 6;
            Main.rockLayer = (int)(Main.worldSurface + num4);
            int waterLine = (int)(Main.rockLayer + (double)Main.maxTilesY) / 2 + WorldGen.genRand.Next(-100, 20);
            int lavaLine = waterLine + WorldGen.genRand.Next(50, 80);
            int num5 = 20;
            if (rockLowVassal < surfaceHighVassal + (double)num5)
            {
                double num16 = (rockLowVassal + surfaceHighVassal) / 2.0;
                double num6 = Math.Abs(rockLowVassal - surfaceHighVassal);
                if (num6 < (double)num5)
                {
                    num6 = num5;
                }
                rockLowVassal = num16 + num6 / 2.0;
                surfaceHighVassal = num16 - num6 / 2.0;
            }

            GenVars.rockLayer = rockVassal;
            GenVars.rockLayerHigh = rockHighVassal;
            GenVars.rockLayerLow = rockLowVassal;
            GenVars.worldSurface = surfaceVassal;
            GenVars.worldSurfaceHigh = surfaceHighVassal;
            GenVars.worldSurfaceLow = surfaceLowVassal;
            GenVars.waterLine = waterLine;
            GenVars.lavaLine = lavaLine;
        }
        public static void FillColumn(int x, double worldSurface, double rockLayer, int dirt = TileID.Dirt, int stone = TileID.Stone)
        {
            Tile tile;
            for (int i = 0; (double)i < worldSurface; i++)
            {
                tile = Main.tile[x, i];
                tile.HasTile = false;
                tile = Main.tile[x, i];
                tile.TileFrameX = -1;
                tile = Main.tile[x, i];
                tile.TileFrameY = -1;
            }
            for (int j = (int)worldSurface; j < Main.maxTilesY; j++)
            {
                if ((double)j < rockLayer)
                {
                    tile = Main.tile[x, j];
                    tile.HasTile = true;
                    tile = Main.tile[x, j];
                    tile.TileType = (ushort)dirt;
                    tile = Main.tile[x, j];
                    tile.TileFrameX = -1;
                    tile = Main.tile[x, j];
                    tile.TileFrameY = -1;
                }
                else
                {
                    tile = Main.tile[x, j];
                    tile.HasTile = true;
                    tile = Main.tile[x, j];
                    tile.TileType = (ushort)stone;
                    tile = Main.tile[x, j];
                    tile.TileFrameX = -1;
                    tile = Main.tile[x, j];
                    tile.TileFrameY = -1;
                }
            }
        }
        public static void RetargetColumn(int x, double worldSurface)
        {
            Tile tile;
            for (int i = 0; (double)i < worldSurface; i++)
            {
                tile = Main.tile[x, i];
                tile.HasTile = false;
                tile = Main.tile[x, i];
                tile.TileFrameX = -1;
                tile = Main.tile[x, i];
                tile.TileFrameY = -1;
            }
            for (int j = (int)worldSurface; j < Main.maxTilesY; j++)
            {
                tile = Main.tile[x, j];
                if (tile.TileType == TileID.Stone)
                {
                    tile = Main.tile[x, j];
                    if (tile.HasTile)
                    {
                        continue;
                    }
                }
                tile = Main.tile[x, j];
                tile.HasTile = true;
                tile = Main.tile[x, j];
                tile.TileType = TileID.Dirt;
                tile = Main.tile[x, j];
                tile.TileFrameX = -1;
                tile = Main.tile[x, j];
                tile.TileFrameY = -1;
            }
        }
        public static double GenerateWorldSurfaceOffset(TerrainFeatureType featureType)
        {
            double num = 0.0;

            switch (featureType)
            {
                case TerrainFeatureType.Plateau:
                    while (WorldGen.genRand.Next(0, 7) == 0)
                    {
                        num += (double)WorldGen.genRand.Next(-1, 2);
                    }
                    break;
                case TerrainFeatureType.Hill:
                    while (WorldGen.genRand.Next(0, 4) == 0)
                    {
                        num -= 1.0;
                    }
                    while (WorldGen.genRand.Next(0, 10) == 0)
                    {
                        num += 1.0;
                    }
                    break;
                case TerrainFeatureType.Dale:
                    while (WorldGen.genRand.Next(0, 4) == 0)
                    {
                        num += 1.0;
                    }
                    while (WorldGen.genRand.Next(0, 10) == 0)
                    {
                        num -= 1.0;
                    }
                    break;
                case TerrainFeatureType.Mountain:
                    while (WorldGen.genRand.Next(0, 2) == 0)
                    {
                        num -= 1.0;
                    }
                    while (WorldGen.genRand.Next(0, 6) == 0)
                    {
                        num += 1.0;
                    }
                    break;
                case TerrainFeatureType.Valley:
                    while (WorldGen.genRand.Next(0, 2) == 0)
                    {
                        num += 1.0;
                    }
                    while (WorldGen.genRand.Next(0, 5) == 0)
                    {
                        num -= 1.0;
                    }
                    break;
            }
            return num;
        }
        private static void RetargetSurfaceHistory(SurfaceHistory history, int targetX, double targetHeight)
        {
            for (int i = 0; i < history.Length / 2; i++)
            {
                if (history[history.Length - 1] <= targetHeight)
                {
                    break;
                }
                for (int j = 0; j < history.Length - i * 2; j++)
                {
                    double num = history[history.Length - j - 1];
                    num -= 1.0;
                    history[history.Length - j - 1] = num;
                    if (num <= targetHeight)
                    {
                        break;
                    }
                }
            }
            for (int k = 0; k < history.Length; k++)
            {
                double worldSurface = history[history.Length - k - 1];
                RetargetColumn(targetX - k, worldSurface);
            }
        }

        public static void Tunnels(int dirt = TileID.Dirt)
        {
            int num1036 = (int)((double)Main.maxTilesX * 0.0015);
            for (int num1037 = 0; num1037 < num1036; num1037++)
            {
                if (GenVars.numTunnels >= GenVars.maxTunnels - 1)
                {
                    break;
                }
                int[] array = new int[10];
                int[] array2 = new int[10];
                int num1038 = WorldGen.genRand.Next(450, Main.maxTilesX - 450);
                int num1039 = 0;
                bool flag61;
                do
                {
                    flag61 = false;
                    for (int num1040 = 0; num1040 < 10; num1040++)
                    {
                        for (num1038 %= Main.maxTilesX; !Main.tile[num1038, num1039].HasTile; num1039++)
                        {
                        }
                        if (Main.tile[num1038, num1039].TileType == TileID.Sand)
                        {
                            flag61 = true;
                        }
                        array[num1040] = num1038;
                        array2[num1040] = num1039 - WorldGen.genRand.Next(11, 16);
                        num1038 += WorldGen.genRand.Next(5, 11);
                    }
                }
                while (flag61);
                GenVars.tunnelX[GenVars.numTunnels] = array[5];
                GenVars.numTunnels++;
                for (int num1041 = 0; num1041 < 10; num1041++)
                {
                    WorldGen.TileRunner(array[num1041], array2[num1041], WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 9), dirt, addTile: true, -2.0, -0.3);
                    WorldGen.TileRunner(array[num1041], array2[num1041], WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 9), dirt, addTile: true, 2.0, -0.3);
                }
            }
        }

        public static void DirtWallBackgrounds()
        {
            int num1024 = 0;
            for (int num1025 = 1; num1025 < Main.maxTilesX - 1; num1025++)
            {
                ushort num1026 = WallID.DirtUnsafe;
                double value20 = (double)num1025 / (double)Main.maxTilesX;
                bool flag58 = false;
                num1024 += WorldGen.genRand.Next(-1, 2);
                if (num1024 < 0)
                {
                    num1024 = 0;
                }
                if (num1024 > 10)
                {
                    num1024 = 10;
                }
                for (int num1027 = 0; (double)num1027 < Main.worldSurface + 10.0 && !((double)num1027 > Main.worldSurface + (double)num1024); num1027++)
                {
                    Tile tile37 = Main.tile[num1025, num1027];
                    if (tile37.HasTile)
                    {
                        tile37 = Main.tile[num1025, num1027];
                        num1026 = (ushort)((tile37.TileType != TileID.SnowBlock) ? WallID.DirtUnsafe : WallID.SnowWallUnsafe);
                    }
                    if (flag58)
                    {
                        tile37 = Main.tile[num1025, num1027];
                        if (tile37.WallType != WallID.JungleUnsafe)
                        {
                            tile37 = Main.tile[num1025, num1027];
                            tile37.WallType = num1026;
                        }
                    }
                    tile37 = Main.tile[num1025, num1027];
                    if (tile37.HasTile)
                    {
                        tile37 = Main.tile[num1025 - 1, num1027];
                        if (tile37.HasTile)
                        {
                            tile37 = Main.tile[num1025 + 1, num1027];
                            if (tile37.HasTile)
                            {
                                tile37 = Main.tile[num1025, num1027 + 1];
                                if (tile37.HasTile)
                                {
                                    tile37 = Main.tile[num1025 - 1, num1027 + 1];
                                    if (tile37.HasTile)
                                    {
                                        tile37 = Main.tile[num1025 + 1, num1027 + 1];
                                        if (tile37.HasTile)
                                        {
                                            flag58 = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void RocksInDirt()
        {
            double num1018 = (double)(Main.maxTilesX * Main.maxTilesY) * 0.00015;
            for (int num1019 = 0; (double)num1019 < num1018; num1019++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)GenVars.worldSurfaceLow + 1), WorldGen.genRand.Next(4, 15), WorldGen.genRand.Next(5, 40), 1);
            }
            num1018 = (double)(Main.maxTilesX * Main.maxTilesY) * 0.0002;
            for (int num1020 = 0; (double)num1020 < num1018; num1020++)
            {
                int num1021 = WorldGen.genRand.Next(0, Main.maxTilesX);
                int num1022 = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, (int)GenVars.worldSurfaceHigh + 1);
                if (!Main.tile[num1021, num1022 - 10].HasTile)
                {
                    num1022 = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, (int)GenVars.worldSurfaceHigh + 1);
                }
                WorldGen.TileRunner(num1021, num1022, WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 30), 1);
            }
            num1018 = (double)(Main.maxTilesX * Main.maxTilesY) * 0.0045;
            for (int num1023 = 0; (double)num1023 < num1018; num1023++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, (int)GenVars.rockLayerHigh + 1), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 23), 1);
            }
        }

        public static void DirtInRocks()
        {
            double num1014 = (double)(Main.maxTilesX * Main.maxTilesY) * 0.005;
            for (int num1015 = 0; (double)num1015 < num1014; num1015++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.rockLayerLow, Main.maxTilesY), WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(2, 40), 0);
            }
        }

        public static void Clay()
        {
            for (int num1007 = 0; num1007 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05); num1007++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)GenVars.worldSurfaceLow), WorldGen.genRand.Next(4, 14), WorldGen.genRand.Next(10, 50), 40);
            }
            if (WorldGen.remixWorldGen)
            {
                for (int num1008 = 0; num1008 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 7E-05); num1008++)
                {
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.rockLayer - 25, Main.maxTilesY - 350), WorldGen.genRand.Next(8, 15), WorldGen.genRand.Next(5, 50), 40);
                }
            }
            else
            {
                for (int num1009 = 0; num1009 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 5E-05); num1009++)
                {
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, (int)GenVars.worldSurfaceHigh + 1), WorldGen.genRand.Next(8, 14), WorldGen.genRand.Next(15, 45), 40);
                }
                for (int num1010 = 0; num1010 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05); num1010++)
                {
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, (int)GenVars.rockLayerHigh + 1), WorldGen.genRand.Next(8, 15), WorldGen.genRand.Next(5, 50), 40);
                }
            }
            for (int num1011 = 5; num1011 < Main.maxTilesX - 5; num1011++)
            {
                for (int num1012 = 1; (double)num1012 < Main.worldSurface - 1.0; num1012++)
                {
                    Tile tile35 = Main.tile[num1011, num1012];
                    if (tile35.HasTile)
                    {
                        for (int num1013 = num1012; num1013 < num1012 + 5; num1013++)
                        {
                            tile35 = Main.tile[num1011, num1013];
                            if (tile35.TileType == TileID.ClayBlock)
                            {
                                tile35 = Main.tile[num1011, num1013];
                                tile35.TileType = TileID.Dirt;
                            }
                        }
                        break;
                    }
                }
            }
        }

        public static void SmallHoles()
        {
            double worldSurfaceHigh2 = GenVars.worldSurfaceHigh;
            for (int num1002 = 0; num1002 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0015); num1002++)
            {
                double value19 = (double)num1002 / ((double)(Main.maxTilesX * Main.maxTilesY) * 0.0015);
                int type13 = -1;
                if (WorldGen.genRand.NextBool(5))
                {
                    type13 = -2;
                }
                int num1003 = WorldGen.genRand.Next(0, Main.maxTilesX);
                int num1004 = WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, Main.maxTilesY);
                if (!WorldGen.remixWorldGen && WorldGen.tenthAnniversaryWorldGen)
                {
                    while ((double)num1003 < (double)Main.maxTilesX * 0.2 && (double)num1003 > (double)Main.maxTilesX * 0.8 && (double)num1004 < GenVars.worldSurface)
                    {
                        num1003 = WorldGen.genRand.Next(0, Main.maxTilesX);
                        num1004 = WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, Main.maxTilesY);
                    }
                }
                else
                {
                    while (((num1003 < GenVars.smallHolesBeachAvoidance || num1003 > Main.maxTilesX - GenVars.smallHolesBeachAvoidance) && (double)num1004 < worldSurfaceHigh2) || ((double)num1003 > (double)Main.maxTilesX * 0.45 && (double)num1003 < (double)Main.maxTilesX * 0.55 && (double)num1004 < GenVars.worldSurface))
                    {
                        num1003 = WorldGen.genRand.Next(0, Main.maxTilesX);
                        num1004 = WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, Main.maxTilesY);
                    }
                }
                int num1005 = WorldGen.genRand.Next(2, 5);
                int num1006 = WorldGen.genRand.Next(2, 20);
                if (WorldGen.remixWorldGen && (double)num1004 > Main.rockLayer)
                {
                    num1005 = (int)((double)num1005 * 0.8);
                    num1006 = (int)((double)num1006 * 0.9);
                }
                WorldGen.TileRunner(num1003, num1004, num1005, num1006, type13);
                num1003 = WorldGen.genRand.Next(0, Main.maxTilesX);
                num1004 = WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, Main.maxTilesY);
                while (((num1003 < GenVars.smallHolesBeachAvoidance || num1003 > Main.maxTilesX - GenVars.smallHolesBeachAvoidance) && (double)num1004 < worldSurfaceHigh2) || ((double)num1003 > (double)Main.maxTilesX * 0.45 && (double)num1003 < (double)Main.maxTilesX * 0.55 && (double)num1004 < GenVars.worldSurface))
                {
                    num1003 = WorldGen.genRand.Next(0, Main.maxTilesX);
                    num1004 = WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, Main.maxTilesY);
                }
                num1005 = WorldGen.genRand.Next(8, 15);
                num1006 = WorldGen.genRand.Next(7, 30);
                if (WorldGen.remixWorldGen && (double)num1004 > Main.rockLayer)
                {
                    num1005 = (int)((double)num1005 * 0.7);
                    num1006 = (int)((double)num1006 * 0.9);
                }
                WorldGen.TileRunner(num1003, num1004, num1005, num1006, type13);
            }
        }

        public static void DirtLayerCaves()
        {
            double worldSurfaceHigh = GenVars.worldSurfaceHigh;
            int num996 = (int)((double)(Main.maxTilesX * Main.maxTilesY) * 3E-05);
            for (int num997 = 0; num997 < num996; num997++)
            {
                double value18 = (double)num997 / (double)num996;
                if (GenVars.rockLayerHigh <= (double)Main.maxTilesY)
                {
                    int type12 = -1;
                    if (WorldGen.genRand.NextBool(6))
                    {
                        type12 = -2;
                    }
                    int num998 = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int num999 = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, (int)GenVars.rockLayerHigh + 1);
                    while (((num998 < GenVars.smallHolesBeachAvoidance || num998 > Main.maxTilesX - GenVars.smallHolesBeachAvoidance) && (double)num999 < worldSurfaceHigh) || ((double)num998 >= (double)Main.maxTilesX * 0.45 && (double)num998 <= (double)Main.maxTilesX * 0.55 && (double)num999 < Main.worldSurface))
                    {
                        num998 = WorldGen.genRand.Next(0, Main.maxTilesX);
                        num999 = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, (int)GenVars.rockLayerHigh + 1);
                    }
                    int num1000 = WorldGen.genRand.Next(5, 15);
                    int num1001 = WorldGen.genRand.Next(30, 200);
                    if (WorldGen.remixWorldGen)
                    {
                        num1000 = (int)((double)num1000 * 1.1);
                        num1001 = (int)((double)num1001 * 1.9);
                    }
                    WorldGen.TileRunner(num998, num999, num1000, num1001, type12);
                }
            }
        }

        public static void RockLayerCaves()
        {
            int num988 = (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00013);
            for (int num989 = 0; num989 < num988; num989++)
            {
                double value17 = (double)num989 / (double)num988;
                if (GenVars.rockLayerHigh <= (double)Main.maxTilesY)
                {
                    int type10 = -1;
                    if (WorldGen.genRand.NextBool(10))
                    {
                        type10 = -2;
                    }
                    int num990 = WorldGen.genRand.Next(6, 20);
                    int num991 = WorldGen.genRand.Next(50, 300);
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.rockLayerHigh, Main.maxTilesY), num990, num991, type10);
                }
            }
            if (WorldGen.remixWorldGen)
            {
                num988 = (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00013 * 0.4);
                for (int num992 = 0; num992 < num988; num992++)
                {
                    if (GenVars.rockLayerHigh <= (double)Main.maxTilesY)
                    {
                        int type11 = -1;
                        if (WorldGen.genRand.NextBool(10))
                        {
                            type11 = -2;
                        }
                        int num993 = WorldGen.genRand.Next(7, 26);
                        int steps = WorldGen.genRand.Next(50, 200);
                        double num994 = (double)WorldGen.genRand.Next(100, 221) * 0.1;
                        double num995 = (double)WorldGen.genRand.Next(-10, 11) * 0.02;
                        int i7 = WorldGen.genRand.Next(0, Main.maxTilesX);
                        int j9 = WorldGen.genRand.Next((int)GenVars.rockLayerHigh, Main.maxTilesY);
                        WorldGen.TileRunner(i7, j9, num993, steps, type11, addTile: false, num994, num995, noYChange: true);
                        WorldGen.TileRunner(i7, j9, num993, steps, type11, addTile: false, 0.0 - num994, 0.0 - num995, noYChange: true);
                    }
                }
            }
        }

        public static void SurfaceCaves()
        {
            int num969 = (int)((double)Main.maxTilesX * 0.002);
            int num970 = (int)((double)Main.maxTilesX * 0.0007);
            int num971 = (int)((double)Main.maxTilesX * 0.0003);
            if (WorldGen.remixWorldGen)
            {
                num969 *= 3;
                num970 *= 3;
                num971 *= 3;
            }
            for (int num972 = 0; num972 < num969; num972++)
            {
                int num973 = WorldGen.genRand.Next(0, Main.maxTilesX);
                while (((double)num973 > (double)Main.maxTilesX * 0.45 && (double)num973 < (double)Main.maxTilesX * 0.55) || num973 < GenVars.leftBeachEnd + 20 || num973 > GenVars.rightBeachStart - 20)
                {
                    num973 = WorldGen.genRand.Next(0, Main.maxTilesX);
                }
                for (int num974 = 0; (double)num974 < GenVars.worldSurfaceHigh; num974++)
                {
                    if (Main.tile[num973, num974].HasTile)
                    {
                        WorldGen.TileRunner(num973, num974, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(5, 50), -1, addTile: false, (double)WorldGen.genRand.Next(-10, 11) * 0.1, 1.0);
                        break;
                    }
                }
            }
            for (int num975 = 0; num975 < num970; num975++)
            {
                int num976 = WorldGen.genRand.Next(0, Main.maxTilesX);
                while (((double)num976 > (double)Main.maxTilesX * 0.43 && (double)num976 < (double)Main.maxTilesX * 0.57000000000000006) || num976 < GenVars.leftBeachEnd + 20 || num976 > GenVars.rightBeachStart - 20)
                {
                    num976 = WorldGen.genRand.Next(0, Main.maxTilesX);
                }
                for (int num977 = 0; (double)num977 < GenVars.worldSurfaceHigh; num977++)
                {
                    if (Main.tile[num976, num977].HasTile)
                    {
                        WorldGen.TileRunner(num976, num977, WorldGen.genRand.Next(10, 15), WorldGen.genRand.Next(50, 130), -1, addTile: false, (double)WorldGen.genRand.Next(-10, 11) * 0.1, 2.0);
                        break;
                    }
                }
            }
            for (int num978 = 0; num978 < num971; num978++)
            {
                int num979 = WorldGen.genRand.Next(0, Main.maxTilesX);
                while (((double)num979 > (double)Main.maxTilesX * 0.4 && (double)num979 < (double)Main.maxTilesX * 0.6) || num979 < GenVars.leftBeachEnd + 20 || num979 > GenVars.rightBeachStart - 20)
                {
                    num979 = WorldGen.genRand.Next(0, Main.maxTilesX);
                }
                for (int num980 = 0; (double)num980 < GenVars.worldSurfaceHigh; num980++)
                {
                    if (Main.tile[num979, num980].HasTile)
                    {
                        WorldGen.TileRunner(num979, num980, WorldGen.genRand.Next(12, 25), WorldGen.genRand.Next(150, 500), -1, addTile: false, (double)WorldGen.genRand.Next(-10, 11) * 0.1, 4.0);
                        WorldGen.TileRunner(num979, num980, WorldGen.genRand.Next(8, 17), WorldGen.genRand.Next(60, 200), -1, addTile: false, (double)WorldGen.genRand.Next(-10, 11) * 0.1, 2.0);
                        WorldGen.TileRunner(num979, num980, WorldGen.genRand.Next(5, 13), WorldGen.genRand.Next(40, 170), -1, addTile: false, (double)WorldGen.genRand.Next(-10, 11) * 0.1, 2.0);
                        break;
                    }
                }
            }
            for (int num981 = 0; num981 < (int)((double)Main.maxTilesX * 0.0004); num981++)
            {
                int num982 = WorldGen.genRand.Next(0, Main.maxTilesX);
                while (((double)num982 > (double)Main.maxTilesX * 0.4 && (double)num982 < (double)Main.maxTilesX * 0.6) || num982 < GenVars.leftBeachEnd + 20 || num982 > GenVars.rightBeachStart - 20)
                {
                    num982 = WorldGen.genRand.Next(0, Main.maxTilesX);
                }
                for (int num983 = 0; (double)num983 < GenVars.worldSurfaceHigh; num983++)
                {
                    if (Main.tile[num982, num983].HasTile)
                    {
                        WorldGen.TileRunner(num982, num983, WorldGen.genRand.Next(7, 12), WorldGen.genRand.Next(150, 250), -1, addTile: false, 0.0, 1.0, noYChange: true);
                        break;
                    }
                }
            }
            double num984 = (double)Main.maxTilesX / 4200.0;
            for (int num985 = 0; (double)num985 < 5.0 * num984; num985++)
            {
                try
                {
                    int num986 = (int)Main.rockLayer;
                    int num987 = Main.maxTilesY - 400;
                    if (num986 >= num987)
                    {
                        num986 = num987 - 1;
                    }
                    WorldGen.Caverer(WorldGen.genRand.Next(GenVars.surfaceCavesBeachAvoidance2, Main.maxTilesX - GenVars.surfaceCavesBeachAvoidance2), WorldGen.genRand.Next(num986, num987));
                }
                catch
                {
                }
            }
        }

        public static void WavyCaves()
        {
            double num960 = (double)Main.maxTilesX / 4200.0;
            num960 *= num960;
            int num961 = (int)(35.0 * num960);
            if (Main.remixWorld)
            {
                num961 /= 3;
            }
            int num962 = 0;
            int num963 = 80;
            for (int num964 = 0; num964 < num961; num964++)
            {
                double num965 = (double)num964 / (double)(num961 - 1);
                int num966 = WorldGen.genRand.Next((int)Main.worldSurface + 100, Main.UnderworldLayer - 100);
                int num967 = 0;
                while (Math.Abs(num966 - num962) < num963)
                {
                    num967++;
                    if (num967 > 100)
                    {
                        break;
                    }
                    num966 = WorldGen.genRand.Next((int)Main.worldSurface + 100, Main.UnderworldLayer - 100);
                }
                num962 = num966;
                int num968 = 80;
                int startX = num968 + (int)((double)(Main.maxTilesX - num968 * 2) * num965);
                try
                {
                    WorldGen.WavyCaverer(startX, num966, 12 + WorldGen.genRand.Next(3, 6), 0.25 + WorldGen.genRand.NextDouble(), WorldGen.genRand.Next(300, 500), -1);
                }
                catch
                {
                }
            }
        }

        public static void Grass()
        {
            double num948 = (double)(Main.maxTilesX * Main.maxTilesY) * 0.002;
            for (int num949 = 0; (double)num949 < num948; num949++)
            {
                int num950 = WorldGen.genRand.Next(1, Main.maxTilesX - 1);
                int num951 = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, (int)GenVars.worldSurfaceHigh);
                if (num951 >= Main.maxTilesY)
                {
                    num951 = Main.maxTilesY - 2;
                }
                Tile tile33 = Main.tile[num950 - 1, num951];
                if (tile33.HasTile)
                {
                    tile33 = Main.tile[num950 - 1, num951];
                    if (tile33.TileType == TileID.Dirt)
                    {
                        tile33 = Main.tile[num950 + 1, num951];
                        if (tile33.HasTile)
                        {
                            tile33 = Main.tile[num950 + 1, num951];
                            if (tile33.TileType == TileID.Dirt)
                            {
                                tile33 = Main.tile[num950, num951 - 1];
                                if (tile33.HasTile)
                                {
                                    tile33 = Main.tile[num950, num951 - 1];
                                    if (tile33.TileType == TileID.Dirt)
                                    {
                                        tile33 = Main.tile[num950, num951 + 1];
                                        if (tile33.HasTile)
                                        {
                                            tile33 = Main.tile[num950, num951 + 1];
                                            if (tile33.TileType == TileID.Dirt)
                                            {
                                                tile33 = Main.tile[num950, num951];
                                                tile33.HasTile = true;
                                                tile33 = Main.tile[num950, num951];
                                                tile33.TileType = TileID.Grass;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                num950 = WorldGen.genRand.Next(1, Main.maxTilesX - 1);
                num951 = WorldGen.genRand.Next(0, (int)GenVars.worldSurfaceLow);
                if (num951 >= Main.maxTilesY)
                {
                    num951 = Main.maxTilesY - 2;
                }
                tile33 = Main.tile[num950 - 1, num951];
                if (tile33.HasTile)
                {
                    tile33 = Main.tile[num950 - 1, num951];
                    if (tile33.TileType == TileID.Dirt)
                    {
                        tile33 = Main.tile[num950 + 1, num951];
                        if (tile33.HasTile)
                        {
                            tile33 = Main.tile[num950 + 1, num951];
                            if (tile33.TileType == TileID.Dirt)
                            {
                                tile33 = Main.tile[num950, num951 - 1];
                                if (tile33.HasTile)
                                {
                                    tile33 = Main.tile[num950, num951 - 1];
                                    if (tile33.TileType == TileID.Dirt)
                                    {
                                        tile33 = Main.tile[num950, num951 + 1];
                                        if (tile33.HasTile)
                                        {
                                            tile33 = Main.tile[num950, num951 + 1];
                                            if (tile33.TileType == TileID.Dirt)
                                            {
                                                tile33 = Main.tile[num950, num951];
                                                tile33.HasTile = true;
                                                tile33 = Main.tile[num950, num951];
                                                tile33.TileType = TileID.Grass;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void FloatingIslands(int? amtOfIslands = null, bool doLakes = false, bool doDrunkWorldIslands = false)
        {
            GenVars.numIslandHouses = 0;
            GenVars.skyIslandHouseCount = 0;
            int num925 = (int)((double)Main.maxTilesX * 0.0008);
            int num926 = 0;
            double num927 = num925 + GenVars.skyLakes;
            if (amtOfIslands != null)
                num927 = (double)amtOfIslands;
            for (int num928 = 0; (double)num928 < num927; num928++)
            {
                int num929 = Main.maxTilesX;
                while (--num929 > 0)
                {
                    bool flag57 = true;
                    int num930 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.1), (int)((double)Main.maxTilesX * 0.9));
                    while (num930 > Main.maxTilesX / 2 - 150 && num930 < Main.maxTilesX / 2 + 150)
                    {
                        num930 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.1), (int)((double)Main.maxTilesX * 0.9));
                    }
                    for (int num931 = 0; num931 < GenVars.numIslandHouses; num931++)
                    {
                        if (num930 > GenVars.floatingIslandHouseX[num931] - 180 && num930 < GenVars.floatingIslandHouseX[num931] + 180)
                        {
                            flag57 = false;
                            break;
                        }
                    }
                    if (flag57)
                    {
                        flag57 = false;
                        int num932 = 0;
                        for (int num933 = 200; (double)num933 < Main.worldSurface; num933++)
                        {
                            if (Main.tile[num930, num933].HasTile)
                            {
                                num932 = num933;
                                flag57 = true;
                                break;
                            }
                        }
                        if (flag57)
                        {
                            int num934 = 0;
                            num929 = -1;
                            int val = WorldGen.genRand.Next(90, num932 - 100);
                            val = Math.Min(val, (int)GenVars.worldSurfaceLow - 50);
                            if (num926 >= num925 && doLakes)
                            {
                                GenVars.skyLake[GenVars.numIslandHouses] = true;
                                WorldGen.CloudLake(num930, val);
                            }
                            else
                            {
                                GenVars.skyLake[GenVars.numIslandHouses] = false;
                                if (doDrunkWorldIslands)
                                {
                                    if (WorldGen.genRand.NextBool(2))
                                    {
                                        num934 = 3;
                                        WorldGen.SnowCloudIsland(num930, val);
                                    }
                                    else
                                    {
                                        num934 = 1;
                                        WorldGen.DesertCloudIsland(num930, val);
                                    }
                                }
                                else
                                {
                                    WorldGen.CloudIsland(num930, val);
                                }
                            }
                            GenVars.floatingIslandHouseX[GenVars.numIslandHouses] = num930;
                            GenVars.floatingIslandHouseY[GenVars.numIslandHouses] = val;
                            GenVars.floatingIslandStyle[GenVars.numIslandHouses] = num934;
                            GenVars.numIslandHouses++;
                            num926++;
                        }
                    }
                }
            }
        }

        public static void Lakes(int? amtOfLakes = null)
        {
            double num740 = (double)Main.maxTilesX / 4200.0;
            int num741 = WorldGen.genRand.Next((int)(num740 * 3.0), (int)(num740 * 6.0));
            if (amtOfLakes != null)
                num741 = (int)amtOfLakes;
            for (int num742 = 0; num742 < num741; num742++)
            {
                int num743 = Main.maxTilesX / 4;
                if (GenVars.numLakes >= GenVars.maxLakes - 1)
                {
                    break;
                }
                double value14 = (double)num742 / (double)num741;
                while (num743 > 0)
                {
                    bool flag48 = false;
                    num743--;
                    int num744 = WorldGen.genRand.Next(GenVars.lakesBeachAvoidance, Main.maxTilesX - GenVars.lakesBeachAvoidance);
                    while ((double)num744 > (double)Main.maxTilesX * 0.45 && (double)num744 < (double)Main.maxTilesX * 0.55)
                    {
                        num744 = WorldGen.genRand.Next(GenVars.lakesBeachAvoidance, Main.maxTilesX - GenVars.lakesBeachAvoidance);
                    }
                    for (int num745 = 0; num745 < GenVars.numLakes; num745++)
                    {
                        if (Math.Abs(num744 - GenVars.LakeX[num745]) < 150)
                        {
                            flag48 = true;
                            break;
                        }
                    }
                    for (int num746 = 0; num746 < GenVars.numMCaves; num746++)
                    {
                        if (Math.Abs(num744 - GenVars.mCaveX[num746]) < 100)
                        {
                            flag48 = true;
                            break;
                        }
                    }
                    for (int num747 = 0; num747 < GenVars.numTunnels; num747++)
                    {
                        if (Math.Abs(num744 - GenVars.tunnelX[num747]) < 100)
                        {
                            flag48 = true;
                            break;
                        }
                    }
                    if (!flag48)
                    {
                        int num748 = (int)GenVars.worldSurfaceLow - 20;
                        while (!Main.tile[num744, num748].HasTile)
                        {
                            num748++;
                            if ((double)num748 >= Main.worldSurface || Main.tile[num744, num748].WallType > WallID.None)
                            {
                                flag48 = true;
                                break;
                            }
                        }
                        if (Main.tile[num744, num748].TileType == TileID.Sand)
                        {
                            flag48 = true;
                        }
                        if (!flag48)
                        {
                            int num749 = 50;
                            for (int num750 = num744 - num749; num750 <= num744 + num749; num750++)
                            {
                                for (int num751 = num748 - num749; num751 <= num748 + num749; num751++)
                                {
                                    if (Main.tile[num750, num751].TileType == TileID.Crimstone || Main.tile[num750, num751].TileType == TileID.Ebonstone) // TODO this is where the error happens, its getting out of bounds variables
                                    {
                                        flag48 = true;
                                        break;
                                    }
                                }
                            }
                            if (!flag48)
                            {
                                int num752 = num748;
                                num749 = 20;
                                while (!WorldGen.SolidTile(num744 - num749, num748) || !WorldGen.SolidTile(num744 + num749, num748))
                                {
                                    num748++;
                                    if ((double)num748 > Main.worldSurface - 50.0)
                                    {
                                        flag48 = true;
                                    }
                                }
                                if (num748 - num752 <= 10)
                                {
                                    num749 = 60;
                                    for (int num753 = num744 - num749; num753 <= num744 + num749; num753++)
                                    {
                                        int y25 = num748 - 20;
                                        if (Main.tile[num753, y25].HasTile || Main.tile[num753, y25].WallType > WallID.None)
                                        {
                                            flag48 = true;
                                        }
                                    }
                                    if (!flag48)
                                    {
                                        int num754 = 0;
                                        for (int num755 = num744 - num749; num755 <= num744 + num749; num755++)
                                        {
                                            for (int num756 = num748; num756 <= num748 + num749 * 2; num756++)
                                            {
                                                if (WorldGen.SolidTile(num755, num756))
                                                {
                                                    num754++;
                                                }
                                            }
                                        }
                                        int num757 = (num749 * 2 + 1) * (num749 * 2 + 1);
                                        if (!((double)num754 < (double)num757 * 0.8) && !GenVars.UndergroundDesertLocation.Intersects(new Rectangle(num744 - 8, num748 - 8, 16, 16)))
                                        {
                                            WorldGen.SonOfLakinater(num744, num748);
                                            GenVars.LakeX[GenVars.numLakes] = num744;
                                            GenVars.numLakes++;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void CleanUpDirt()
        {
            Tile tile25;
            for (int num678 = 3; num678 < Main.maxTilesX - 3; num678++)
            {
                double num679 = (double)num678 / (double)Main.maxTilesX;
                bool flag43 = true;
                for (int num680 = 0; (double)num680 < Main.worldSurface; num680++)
                {
                    if (!flag43)
                    {
                        tile25 = Main.tile[num678, num680];
                        if (tile25.WallType == WallID.None)
                        {
                            tile25 = Main.tile[num678, num680 + 1];
                            if (tile25.WallType == WallID.None)
                            {
                                tile25 = Main.tile[num678, num680 + 2];
                                if (tile25.WallType == WallID.None)
                                {
                                    tile25 = Main.tile[num678, num680 + 3];
                                    if (tile25.WallType == WallID.None)
                                    {
                                        tile25 = Main.tile[num678, num680 + 4];
                                        if (tile25.WallType == WallID.None)
                                        {
                                            tile25 = Main.tile[num678 - 1, num680];
                                            if (tile25.WallType == WallID.None)
                                            {
                                                tile25 = Main.tile[num678 + 1, num680];
                                                if (tile25.WallType == WallID.None)
                                                {
                                                    tile25 = Main.tile[num678 - 2, num680];
                                                    if (tile25.WallType == WallID.None)
                                                    {
                                                        tile25 = Main.tile[num678 + 2, num680];
                                                        if (tile25.WallType == WallID.None)
                                                        {
                                                            tile25 = Main.tile[num678, num680];
                                                            if (!tile25.HasTile)
                                                            {
                                                                tile25 = Main.tile[num678, num680 + 1];
                                                                if (!tile25.HasTile)
                                                                {
                                                                    tile25 = Main.tile[num678, num680 + 2];
                                                                    if (!tile25.HasTile)
                                                                    {
                                                                        tile25 = Main.tile[num678, num680 + 3];
                                                                        if (!tile25.HasTile)
                                                                        {
                                                                            flag43 = true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        continue;
                    }
                    tile25 = Main.tile[num678, num680];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num678, num680];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num678, num680];
                            if (tile25.WallType != WallID.JungleUnsafe)
                            {
                                tile25 = Main.tile[num678, num680];
                                if (tile25.WallType != WallID.HiveUnsafe)
                                {
                                    goto IL_00c2;
                                }
                            }
                        }
                    }
                    tile25 = Main.tile[num678, num680];
                    tile25.WallType = WallID.None;
                    goto IL_00c2;
                IL_035f:
                    tile25 = Main.tile[num678 + 3, num680];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num678 + 3, num680];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num678 + 3, num680];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_03d8;
                            }
                        }
                    }
                    if (WorldGen.genRand.NextBool(2))
                    {
                        tile25 = Main.tile[num678 + 3, num680];
                        tile25.WallType = WallID.None;
                    }
                    goto IL_03d8;
                IL_0201:
                    tile25 = Main.tile[num678 - 3, num680];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num678 - 3, num680];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num678 - 3, num680];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_027a;
                            }
                        }
                    }
                    if (WorldGen.genRand.NextBool(2))
                    {
                        tile25 = Main.tile[num678 - 3, num680];
                        tile25.WallType = WallID.None;
                    }
                    goto IL_027a;
                IL_0188:
                    tile25 = Main.tile[num678 - 2, num680];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num678 - 2, num680];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num678 - 2, num680];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_0201;
                            }
                        }
                    }
                    if (WorldGen.genRand.NextBool(2))
                    {
                        tile25 = Main.tile[num678 - 2, num680];
                        tile25.WallType = WallID.None;
                    }
                    goto IL_0201;
                IL_03d8:
                    tile25 = Main.tile[num678, num680];
                    if (tile25.HasTile)
                    {
                        flag43 = false;
                    }
                    continue;
                IL_02e6:
                    tile25 = Main.tile[num678 + 2, num680];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num678 + 2, num680];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num678 + 2, num680];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_035f;
                            }
                        }
                    }
                    if (WorldGen.genRand.NextBool(2))
                    {
                        tile25 = Main.tile[num678 + 2, num680];
                        tile25.WallType = WallID.None;
                    }
                    goto IL_035f;
                IL_00c2:
                    tile25 = Main.tile[num678, num680];
                    if (tile25.TileType == TileID.Sand)
                    {
                        continue;
                    }
                    tile25 = Main.tile[num678, num680];
                    if (tile25.TileType == TileID.Ebonsand)
                    {
                        continue;
                    }
                    tile25 = Main.tile[num678, num680];
                    if (tile25.TileType == TileID.Crimsand)
                    {
                        continue;
                    }
                    tile25 = Main.tile[num678 - 1, num680];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num678 - 1, num680];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num678 - 1, num680];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_0188;
                            }
                        }
                    }
                    tile25 = Main.tile[num678 - 1, num680];
                    tile25.WallType = WallID.None;
                    goto IL_0188;
                IL_027a:
                    tile25 = Main.tile[num678 + 1, num680];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num678 + 1, num680];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num678 + 1, num680];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_02e6;
                            }
                        }
                    }
                    tile25 = Main.tile[num678 + 1, num680];
                    tile25.WallType = WallID.None;
                    goto IL_02e6;
                }
            }
            for (int num681 = Main.maxTilesX - 5; num681 >= 5; num681--)
            {
                double num682 = (double)num681 / (double)Main.maxTilesX;
                bool flag44 = true;
                for (int num683 = 0; (double)num683 < Main.worldSurface; num683++)
                {
                    if (!flag44)
                    {
                        tile25 = Main.tile[num681, num683];
                        if (tile25.WallType == WallID.None)
                        {
                            tile25 = Main.tile[num681, num683 + 1];
                            if (tile25.WallType == WallID.None)
                            {
                                tile25 = Main.tile[num681, num683 + 2];
                                if (tile25.WallType == WallID.None)
                                {
                                    tile25 = Main.tile[num681, num683 + 3];
                                    if (tile25.WallType == WallID.None)
                                    {
                                        tile25 = Main.tile[num681, num683 + 4];
                                        if (tile25.WallType == WallID.None)
                                        {
                                            tile25 = Main.tile[num681 - 1, num683];
                                            if (tile25.WallType == WallID.None)
                                            {
                                                tile25 = Main.tile[num681 + 1, num683];
                                                if (tile25.WallType == WallID.None)
                                                {
                                                    tile25 = Main.tile[num681 - 2, num683];
                                                    if (tile25.WallType == WallID.None)
                                                    {
                                                        tile25 = Main.tile[num681 + 2, num683];
                                                        if (tile25.WallType == WallID.None)
                                                        {
                                                            tile25 = Main.tile[num681, num683];
                                                            if (!tile25.HasTile)
                                                            {
                                                                tile25 = Main.tile[num681, num683 + 1];
                                                                if (!tile25.HasTile)
                                                                {
                                                                    tile25 = Main.tile[num681, num683 + 2];
                                                                    if (!tile25.HasTile)
                                                                    {
                                                                        tile25 = Main.tile[num681, num683 + 3];
                                                                        if (!tile25.HasTile)
                                                                        {
                                                                            flag44 = true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        continue;
                    }
                    tile25 = Main.tile[num681, num683];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num681, num683];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num681, num683];
                            if (tile25.WallType != WallID.JungleUnsafe)
                            {
                                goto IL_062f;
                            }
                        }
                    }
                    tile25 = Main.tile[num681, num683];
                    tile25.WallType = WallID.None;
                    goto IL_062f;
                IL_0838:
                    tile25 = Main.tile[num681 + 2, num683];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num681 + 2, num683];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num681 + 2, num683];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_08b9;
                            }
                        }
                    }
                    if (WorldGen.genRand.NextBool(2))
                    {
                        tile25 = Main.tile[num681 + 2, num683];
                        tile25.WallType = WallID.None;
                    }
                    goto IL_08b9;
                IL_0743:
                    tile25 = Main.tile[num681 - 3, num683];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num681 - 3, num683];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num681 - 3, num683];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_07c4;
                            }
                        }
                    }
                    if (WorldGen.genRand.NextBool(2))
                    {
                        tile25 = Main.tile[num681 - 3, num683];
                        tile25.WallType = WallID.None;
                    }
                    goto IL_07c4;
                IL_07c4:
                    tile25 = Main.tile[num681 + 1, num683];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num681 + 1, num683];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num681 + 1, num683];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_0838;
                            }
                        }
                    }
                    tile25 = Main.tile[num681 + 1, num683];
                    tile25.WallType = WallID.None;
                    goto IL_0838;
                IL_06c2:
                    tile25 = Main.tile[num681 - 2, num683];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num681 - 2, num683];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num681 - 2, num683];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_0743;
                            }
                        }
                    }
                    if (WorldGen.genRand.NextBool(2))
                    {
                        tile25 = Main.tile[num681 - 2, num683];
                        tile25.WallType = WallID.None;
                    }
                    goto IL_0743;
                IL_093a:
                    tile25 = Main.tile[num681, num683];
                    if (tile25.HasTile)
                    {
                        flag44 = false;
                    }
                    continue;
                IL_062f:
                    tile25 = Main.tile[num681, num683];
                    if (tile25.TileType == TileID.Sand)
                    {
                        continue;
                    }
                    tile25 = Main.tile[num681 - 1, num683];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num681 - 1, num683];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num681 - 1, num683];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_06c2;
                            }
                        }
                    }
                    tile25 = Main.tile[num681 - 1, num683];
                    tile25.WallType = WallID.None;
                    goto IL_06c2;
                IL_08b9:
                    tile25 = Main.tile[num681 + 3, num683];
                    if (tile25.WallType != WallID.DirtUnsafe)
                    {
                        tile25 = Main.tile[num681 + 3, num683];
                        if (tile25.WallType != WallID.SnowWallUnsafe)
                        {
                            tile25 = Main.tile[num681 + 3, num683];
                            if (tile25.WallType != WallID.SnowWallUnsafe)
                            {
                                goto IL_093a;
                            }
                        }
                    }
                    if (WorldGen.genRand.NextBool(2))
                    {
                        tile25 = Main.tile[num681 + 3, num683];
                        tile25.WallType = WallID.None;
                    }
                    goto IL_093a;
                }
            }
        }

        public static void DirtRockWallRunner()
        {
            for (int num668 = 0; num668 < Main.maxTilesX; num668++)
            {
                int num669 = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                int num670 = WorldGen.genRand.Next(10, (int)Main.worldSurface);
                if (Main.tile[num669, num670].WallType == WallID.DirtUnsafe)
                {
                    WorldGen.DirtyRockRunner(num669, num670);
                }
            }
        }

        public static void SmoothWorld()
        {
            Main.tileSolid[GenVars.crackedType] = true;
            for (int num565 = 20; num565 < Main.maxTilesX - 20; num565++)
            {
                double value11 = (double)num565 / (double)Main.maxTilesX;
                for (int num566 = 20; num566 < Main.maxTilesY - 20; num566++)
                {
                    if (Main.tile[num565, num566].TileType != TileID.Spikes && Main.tile[num565, num566].TileType != TileID.Traps && Main.tile[num565, num566].TileType != TileID.WoodenSpikes && Main.tile[num565, num566].TileType != TileID.LivingWood && Main.tile[num565, num566].TileType != TileID.SandstoneBrick && Main.tile[num565, num566].TileType != TileID.SandStoneSlab)
                    {
                        if (!Main.tile[num565, num566 - 1].HasTile && Main.tile[num565 - 1, num566].TileType != TileID.Switches && Main.tile[num565 + 1, num566].TileType != TileID.Switches)
                        {
                            if (WorldGen.SolidTile(num565, num566) && TileID.Sets.CanBeClearedDuringGeneration[Main.tile[num565, num566].TileType])
                            {
                                if (!Main.tile[num565 - 1, num566].IsHalfBlock && !Main.tile[num565 + 1, num566].IsHalfBlock && Main.tile[num565 - 1, num566].Slope == 0 && Main.tile[num565 + 1, num566].Slope == 0)
                                {
                                    if (WorldGen.SolidTile(num565, num566 + 1))
                                    {
                                        if (!WorldGen.SolidTile(num565 - 1, num566) && !Main.tile[num565 - 1, num566 + 1].IsHalfBlock && WorldGen.SolidTile(num565 - 1, num566 + 1) && WorldGen.SolidTile(num565 + 1, num566) && !Main.tile[num565 + 1, num566 - 1].HasTile)
                                        {
                                            if (WorldGen.genRand.NextBool(2))
                                            {
                                                WorldGen.SlopeTile(num565, num566, 2);
                                            }
                                            else
                                            {
                                                WorldGen.PoundTile(num565, num566);
                                            }
                                        }
                                        else if (!WorldGen.SolidTile(num565 + 1, num566) && !Main.tile[num565 + 1, num566 + 1].IsHalfBlock && WorldGen.SolidTile(num565 + 1, num566 + 1) && WorldGen.SolidTile(num565 - 1, num566) && !Main.tile[num565 - 1, num566 - 1].HasTile)
                                        {
                                            if (WorldGen.genRand.NextBool(2))
                                            {
                                                WorldGen.SlopeTile(num565, num566, 1);
                                            }
                                            else
                                            {
                                                WorldGen.PoundTile(num565, num566);
                                            }
                                        }
                                        else if (WorldGen.SolidTile(num565 + 1, num566 + 1) && WorldGen.SolidTile(num565 - 1, num566 + 1) && !Main.tile[num565 + 1, num566].HasTile && !Main.tile[num565 - 1, num566].HasTile)
                                        {
                                            WorldGen.PoundTile(num565, num566);
                                        }
                                        if (WorldGen.SolidTile(num565, num566))
                                        {
                                            if (WorldGen.SolidTile(num565 - 1, num566) && WorldGen.SolidTile(num565 + 1, num566 + 2) && !Main.tile[num565 + 1, num566].HasTile && !Main.tile[num565 + 1, num566 + 1].HasTile && !Main.tile[num565 - 1, num566 - 1].HasTile)
                                            {
                                                WorldGen.KillTile(num565, num566);
                                            }
                                            else if (WorldGen.SolidTile(num565 + 1, num566) && WorldGen.SolidTile(num565 - 1, num566 + 2) && !Main.tile[num565 - 1, num566].HasTile && !Main.tile[num565 - 1, num566 + 1].HasTile && !Main.tile[num565 + 1, num566 - 1].HasTile)
                                            {
                                                WorldGen.KillTile(num565, num566);
                                            }
                                            else if (!Main.tile[num565 - 1, num566 + 1].HasTile && !Main.tile[num565 - 1, num566].HasTile && WorldGen.SolidTile(num565 + 1, num566) && WorldGen.SolidTile(num565, num566 + 2))
                                            {
                                                if (WorldGen.genRand.NextBool(5))
                                                {
                                                    WorldGen.KillTile(num565, num566);
                                                }
                                                else if (WorldGen.genRand.NextBool(5))
                                                {
                                                    WorldGen.PoundTile(num565, num566);
                                                }
                                                else
                                                {
                                                    WorldGen.SlopeTile(num565, num566, 2);
                                                }
                                            }
                                            else if (!Main.tile[num565 + 1, num566 + 1].HasTile && !Main.tile[num565 + 1, num566].HasTile && WorldGen.SolidTile(num565 - 1, num566) && WorldGen.SolidTile(num565, num566 + 2))
                                            {
                                                if (WorldGen.genRand.NextBool(5))
                                                {
                                                    WorldGen.KillTile(num565, num566);
                                                }
                                                else if (WorldGen.genRand.NextBool(5))
                                                {
                                                    WorldGen.PoundTile(num565, num566);
                                                }
                                                else
                                                {
                                                    WorldGen.SlopeTile(num565, num566, 1);
                                                }
                                            }
                                        }
                                    }
                                    if (WorldGen.SolidTile(num565, num566) && !Main.tile[num565 - 1, num566].HasTile && !Main.tile[num565 + 1, num566].HasTile)
                                    {
                                        WorldGen.KillTile(num565, num566);
                                    }
                                }
                            }
                            else if (!Main.tile[num565, num566].HasTile && Main.tile[num565, num566 + 1].TileType != TileID.SandstoneBrick && Main.tile[num565, num566 + 1].TileType != TileID.SandStoneSlab)
                            {
                                if (Main.tile[num565 + 1, num566].TileType != TileID.MushroomBlock && Main.tile[num565 + 1, num566].TileType != TileID.Spikes && Main.tile[num565 + 1, num566].TileType != TileID.WoodenSpikes && WorldGen.SolidTile(num565 - 1, num566 + 1) && WorldGen.SolidTile(num565 + 1, num566) && !Main.tile[num565 - 1, num566].HasTile && !Main.tile[num565 + 1, num566 - 1].HasTile)
                                {
                                    if (Main.tile[num565 + 1, num566].TileType == TileID.ShellPile)
                                    {
                                        WorldGen.PlaceTile(num565, num566, Main.tile[num565 + 1, num566].TileType);
                                    }
                                    else
                                    {
                                        WorldGen.PlaceTile(num565, num566, Main.tile[num565, num566 + 1].TileType);
                                    }
                                    if (WorldGen.genRand.NextBool(2))
                                    {
                                        WorldGen.SlopeTile(num565, num566, 2);
                                    }
                                    else
                                    {
                                        WorldGen.PoundTile(num565, num566);
                                    }
                                }
                                if (Main.tile[num565 - 1, num566].TileType != TileID.MushroomBlock && Main.tile[num565 - 1, num566].TileType != TileID.Spikes && Main.tile[num565 - 1, num566].TileType != TileID.WoodenSpikes && WorldGen.SolidTile(num565 + 1, num566 + 1) && WorldGen.SolidTile(num565 - 1, num566) && !Main.tile[num565 + 1, num566].HasTile && !Main.tile[num565 - 1, num566 - 1].HasTile)
                                {
                                    if (Main.tile[num565 - 1, num566].TileType == TileID.ShellPile)
                                    {
                                        WorldGen.PlaceTile(num565, num566, Main.tile[num565 - 1, num566].TileType);
                                    }
                                    else
                                    {
                                        WorldGen.PlaceTile(num565, num566, Main.tile[num565, num566 + 1].TileType);
                                    }
                                    if (WorldGen.genRand.NextBool(2))
                                    {
                                        WorldGen.SlopeTile(num565, num566, 1);
                                    }
                                    else
                                    {
                                        WorldGen.PoundTile(num565, num566);
                                    }
                                }
                            }
                        }
                        else if (!Main.tile[num565, num566 + 1].HasTile && WorldGen.genRand.NextBool(2) && WorldGen.SolidTile(num565, num566) && !Main.tile[num565 - 1, num566].IsHalfBlock && !Main.tile[num565 + 1, num566].IsHalfBlock && Main.tile[num565 - 1, num566].Slope == 0 && Main.tile[num565 + 1, num566].Slope == 0 && WorldGen.SolidTile(num565, num566 - 1))
                        {
                            if (WorldGen.SolidTile(num565 - 1, num566) && !WorldGen.SolidTile(num565 + 1, num566) && WorldGen.SolidTile(num565 - 1, num566 - 1))
                            {
                                WorldGen.SlopeTile(num565, num566, 3);
                            }
                            else if (WorldGen.SolidTile(num565 + 1, num566) && !WorldGen.SolidTile(num565 - 1, num566) && WorldGen.SolidTile(num565 + 1, num566 - 1))
                            {
                                WorldGen.SlopeTile(num565, num566, 4);
                            }
                        }
                        if (TileID.Sets.Conversion.Sand[Main.tile[num565, num566].TileType])
                        {
                            Tile.SmoothSlope(num565, num566, applyToNeighbors: false);
                        }
                    }
                }
            }
            for (int num567 = 20; num567 < Main.maxTilesX - 20; num567++)
            {
                for (int num568 = 20; num568 < Main.maxTilesY - 20; num568++)
                {
                    if (WorldGen.genRand.NextBool(2) && !Main.tile[num567, num568 - 1].HasTile && Main.tile[num567, num568].TileType != TileID.Traps && Main.tile[num567, num568].TileType != TileID.Spikes && Main.tile[num567, num568].TileType != TileID.WoodenSpikes && Main.tile[num567, num568].TileType != TileID.LivingWood && Main.tile[num567, num568].TileType != TileID.SandstoneBrick && Main.tile[num567, num568].TileType != TileID.SandStoneSlab && Main.tile[num567, num568].TileType != TileID.ObsidianBrick && Main.tile[num567, num568].TileType != TileID.HellstoneBrick && WorldGen.SolidTile(num567, num568) && Main.tile[num567 - 1, num568].TileType != TileID.Traps && Main.tile[num567 + 1, num568].TileType != TileID.Traps)
                    {
                        if (WorldGen.SolidTile(num567, num568 + 1) && WorldGen.SolidTile(num567 + 1, num568) && !Main.tile[num567 - 1, num568].HasTile)
                        {
                            WorldGen.SlopeTile(num567, num568, 2);
                        }
                        if (WorldGen.SolidTile(num567, num568 + 1) && WorldGen.SolidTile(num567 - 1, num568) && !Main.tile[num567 + 1, num568].HasTile)
                        {
                            WorldGen.SlopeTile(num567, num568, 1);
                        }
                    }
                    if (Main.tile[num567, num568].Slope == SlopeType.SlopeDownLeft && !WorldGen.SolidTile(num567 - 1, num568))
                    {
                        WorldGen.SlopeTile(num567, num568);
                        WorldGen.PoundTile(num567, num568);
                    }
                    if (Main.tile[num567, num568].Slope == SlopeType.SlopeDownRight && !WorldGen.SolidTile(num567 + 1, num568))
                    {
                        WorldGen.SlopeTile(num567, num568);
                        WorldGen.PoundTile(num567, num568);
                    }
                }
            }
            Main.tileSolid[137] = true;
            Main.tileSolid[190] = false;
            Main.tileSolid[192] = false;
            Main.tileSolid[GenVars.crackedType] = false;
        }

        public static void SettleLiquids()
        {
            Liquid.worldGenTilesIgnoreWater(ignoreSolids: true);
            Liquid.QuickWater(3);
            WorldGen.WaterCheck();
            int num588 = 0;
            Liquid.quickSettle = true;
            int num589 = 10;
            while (num588 < num589)
            {
                int num590 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                num588++;
                double num591 = 0.0;
                int num592 = num590 * 5;
                while (Liquid.numLiquid > 0)
                {
                    num592--;
                    if (num592 < 0)
                    {
                        break;
                    }
                    double num593 = (double)(num590 - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (double)num590;
                    if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num590)
                    {
                        num590 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                    }
                    if (num593 > num591)
                    {
                        num591 = num593;
                    }
                    else
                    {
                        num593 = num591;
                    }
                    int num594 = 10;
                    if (num588 > num594)
                    {
                        num594 = num588;
                    }
                    Liquid.UpdateLiquid();
                }
                WorldGen.WaterCheck();
            }
            Liquid.quickSettle = false;
            Liquid.worldGenTilesIgnoreWater(ignoreSolids: false);
            Main.tileSolid[484] = false;
        }

        public static void Waterfalls()
        {
            Main.tileSolid[191] = false;
            for (int num556 = 20; num556 < Main.maxTilesX - 20; num556++)
            {
                double num557 = (double)num556 / (double)Main.maxTilesX;
                for (int num558 = 20; num558 < Main.maxTilesY - 20; num558++)
                {
                    if (WorldGen.SolidTile(num556, num558) && !Main.tile[num556 - 1, num558].HasTile && WorldGen.SolidTile(num556, num558 + 1) && !Main.tile[num556 + 1, num558].HasTile && (Main.tile[num556 - 1, num558].LiquidAmount > 0 || Main.tile[num556 + 1, num558].LiquidAmount > 0))
                    {
                        bool flag35 = true;
                        int num559 = WorldGen.genRand.Next(8, 20);
                        int num560 = WorldGen.genRand.Next(8, 20);
                        num559 = num558 - num559;
                        num560 += num558;
                        for (int num561 = num559; num561 <= num560; num561++)
                        {
                            if (Main.tile[num556, num561].IsHalfBlock)
                            {
                                flag35 = false;
                            }
                        }
                        if ((Main.tile[num556, num558].TileType == TileID.ObsidianBrick || Main.tile[num556, num558].TileType == TileID.HellstoneBrick) && !WorldGen.genRand.NextBool(10))
                        {
                            flag35 = false;
                        }
                        if (flag35)
                        {
                            WorldGen.PoundTile(num556, num558);
                        }
                    }
                }
            }
            for (int num562 = 20; num562 < Main.maxTilesX - 20; num562++)
            {
                double num563 = (double)num562 / (double)Main.maxTilesX;
                for (int num564 = 20; num564 < Main.maxTilesY - 20; num564++)
                {
                    if (Main.tile[num562, num564].TileType != TileID.Spikes && Main.tile[num562, num564].TileType != TileID.WoodenSpikes && WorldGen.SolidTile(num562, num564) && WorldGen.SolidTile(num562, num564 + 1))
                    {
                        if (!WorldGen.SolidTile(num562 + 1, num564) && Main.tile[num562 - 1, num564].IsHalfBlock && Main.tile[num562 - 2, num564].LiquidAmount > 0)
                        {
                            WorldGen.PoundTile(num562, num564);
                        }
                        if (!WorldGen.SolidTile(num562 - 1, num564) && Main.tile[num562 + 1, num564].IsHalfBlock && Main.tile[num562 + 2, num564].LiquidAmount > 0)
                        {
                            WorldGen.PoundTile(num562, num564);
                        }
                    }
                }
            }
            Main.tileSolid[191] = true;
        }

        public static void GenerateIceBiome()
        {
            for (int y = 0; y <= Main.maxTilesY; y++)
            {
                for (int x = 0; x <= Main.maxTilesX; x++)
                {
                    Tile currentTile;
                    currentTile = Main.tile[x, y];
                    if (currentTile.WallType == WallID.DirtUnsafe)
                    {
                        currentTile = Main.tile[x, y];
                        currentTile.WallType = WallID.SnowWallUnsafe;
                    }
                    currentTile = Main.tile[x, y];
                    switch (currentTile.TileType)
                    {
                        case TileID.Dirt:
                        case TileID.Grass:
                        case TileID.CorruptGrass:
                        case TileID.ClayBlock:
                        case TileID.Sand:
                            currentTile = Main.tile[x, y];
                            currentTile.TileType = TileID.SnowBlock;
                            break;
                        case TileID.Stone:
                            currentTile = Main.tile[x, y];
                            currentTile.TileType = TileID.IceBlock;
                            break;
                    }

                }
            }
        }

        public static void WallVariety()
        {
            double num550 = (double)(Main.maxTilesX * Main.maxTilesY) / 5040000.0;
            int num551 = (int)(300.0 * num550);
            int num552 = num551;
            ShapeData shapeData = new ShapeData();
            bool foundInvalidTile = default(bool);
            while (num551 > 0)
            {
                Point point2 = WorldGen.RandomWorldPoint((int)GenVars.worldSurface, 2, 190, 2);
                while (Vector2D.Distance(new Vector2D((double)point2.X, (double)point2.Y), GenVars.shimmerPosition) < (double)WorldGen.shimmerSafetyDistance)
                {
                    point2 = WorldGen.RandomWorldPoint((int)GenVars.worldSurface, 2, 190, 2);
                }
                Tile tile19 = Main.tile[point2.X, point2.Y];
                Tile tile20 = Main.tile[point2.X, point2.Y - 1];
                ushort num553 = 0;
                if (tile19.TileType == TileID.JungleGrass)
                {
                    num553 = (ushort)(204 + WorldGen.genRand.Next(4));
                }
                else if (tile19.TileType == TileID.Stone && tile20.WallType == WallID.None)
                {
                    num553 = ((!WorldGen.remixWorldGen) ? (((double)point2.Y < GenVars.rockLayer) ? ((ushort)(196 + WorldGen.genRand.Next(4))) : ((point2.Y >= GenVars.lavaLine) ? ((ushort)(208 + WorldGen.genRand.Next(4))) : ((ushort)(212 + WorldGen.genRand.Next(4))))) : (((double)point2.Y > GenVars.rockLayer) ? ((ushort)(196 + WorldGen.genRand.Next(4))) : ((point2.Y <= GenVars.lavaLine || !WorldGen.genRand.NextBool(2)) ? ((ushort)(212 + WorldGen.genRand.Next(4))) : ((ushort)(208 + WorldGen.genRand.Next(4))))));
                }
                if (tile19.HasTile && num553 != 0 && !tile20.HasTile)
                {
                    foundInvalidTile = false;
                    bool flag34 = ((tile19.TileType != TileID.JungleGrass) ? WorldUtils.Gen(new Point(point2.X, point2.Y - 1), new ShapeFloodFill(1000), Actions.Chain(new Modifiers.IsNotSolid(), new Actions.Blank().Output(shapeData), new Actions.ContinueWrapper(Actions.Chain(new Modifiers.IsTouching(true, 60, 147, 161, 396, 397, 70, 191), new Modifiers.IsTouching(true, 147, 161, 396, 397, 70, 191), new Actions.Custom(delegate
                    {
                        foundInvalidTile = true;
                        return true;
                    }))))) : WorldUtils.Gen(new Point(point2.X, point2.Y - 1), new ShapeFloodFill(1000), Actions.Chain(new Modifiers.IsNotSolid(), new Actions.Blank().Output(shapeData), new Actions.ContinueWrapper(Actions.Chain(new Modifiers.IsTouching(true, 147, 161, 396, 397, 70, 191), new Actions.Custom(delegate
                    {
                        foundInvalidTile = true;
                        return true;
                    }))))));
                    if (shapeData.Count > 50 && flag34 && !foundInvalidTile)
                    {
                        WorldUtils.Gen(new Point(point2.X, point2.Y), new ModShapes.OuterOutline(shapeData, useDiagonals: true, useInterior: true), Actions.Chain(new Modifiers.SkipWalls(87), new Actions.PlaceWall(num553)));
                        num551--;
                    }
                    shapeData.Clear();
                }
            }
        }

        public static void QuickCleanup()
        {
            Main.tileSolid[137] = false;
            Main.tileSolid[130] = false;
            for (int num437 = 20; num437 < Main.maxTilesX - 20; num437++)
            {
                for (int num438 = 20; num438 < Main.maxTilesY - 20; num438++)
                {
                    Tile tile17;
                    if ((double)num438 < Main.worldSurface && WorldGen.oceanDepths(num437, num438))
                    {
                        tile17 = Main.tile[num437, num438];
                        if (tile17.TileType == TileID.Sand)
                        {
                            tile17 = Main.tile[num437, num438];
                            if (tile17.HasTile)
                            {
                                tile17 = Main.tile[num437, num438];
                                if (tile17.BottomSlope)
                                {
                                    tile17 = Main.tile[num437, num438];
                                    tile17.Slope = 0;
                                }
                                for (int num439 = num438 + 1; num439 < num438 + WorldGen.genRand.Next(4, 7); num439++)
                                {
                                    tile17 = Main.tile[num437, num439];
                                    if (tile17.HasTile)
                                    {
                                        tile17 = Main.tile[num437, num439];
                                        if (tile17.TileType == TileID.HardenedSand)
                                        {
                                            break;
                                        }
                                        tile17 = Main.tile[num437, num439];
                                        if (tile17.TileType == TileID.Sand)
                                        {
                                            break;
                                        }
                                    }
                                    tile17 = Main.tile[num437, num439 + 1];
                                    if (tile17.HasTile)
                                    {
                                        tile17 = Main.tile[num437, num439 + 1];
                                        if (tile17.TileType == TileID.HardenedSand)
                                        {
                                            break;
                                        }
                                        tile17 = Main.tile[num437, num439 + 1];
                                        if (tile17.TileType == TileID.Sand)
                                        {
                                            break;
                                        }
                                        tile17 = Main.tile[num437, num439 + 1];
                                        if (tile17.TileType == TileID.ShellPile)
                                        {
                                            break;
                                        }
                                    }
                                    tile17 = Main.tile[num437, num439 + 2];
                                    if (tile17.HasTile)
                                    {
                                        tile17 = Main.tile[num437, num439 + 2];
                                        if (tile17.TileType == TileID.HardenedSand)
                                        {
                                            break;
                                        }
                                        tile17 = Main.tile[num437, num439 + 2];
                                        if (tile17.TileType == TileID.Sand)
                                        {
                                            break;
                                        }
                                        tile17 = Main.tile[num437, num439 + 2];
                                        if (tile17.TileType == TileID.ShellPile)
                                        {
                                            break;
                                        }
                                    }
                                    tile17 = Main.tile[num437, num439];
                                    tile17.TileType = TileID.Dirt;
                                    tile17 = Main.tile[num437, num439];
                                    tile17.HasTile = true;
                                    tile17 = Main.tile[num437, num439];
                                    tile17.IsHalfBlock = false;
                                    tile17 = Main.tile[num437, num439];
                                    tile17.Slope = 0;
                                }
                            }
                        }
                    }
                    tile17 = Main.tile[num437, num438];
                    if (tile17.WallType != WallID.Sandstone)
                    {
                        tile17 = Main.tile[num437, num438];
                        if (tile17.WallType != WallID.HardenedSand)
                        {
                            goto IL_03c6;
                        }
                    }
                    tile17 = Main.tile[num437, num438];
                    if (tile17.TileType != TileID.Mud)
                    {
                        tile17 = Main.tile[num437, num438];
                        if (tile17.TileType != TileID.Silt)
                        {
                            tile17 = Main.tile[num437, num438];
                            if (tile17.TileType != TileID.Slush)
                            {
                                goto IL_030c;
                            }
                        }
                    }
                    tile17 = Main.tile[num437, num438];
                    tile17.TileType = TileID.HardenedSand;
                    goto IL_030c;
                IL_030c:
                    tile17 = Main.tile[num437, num438];
                    if (tile17.TileType != TileID.Granite)
                    {
                        tile17 = Main.tile[num437, num438];
                        if (tile17.TileType != TileID.Marble)
                        {
                            goto IL_035e;
                        }
                    }
                    tile17 = Main.tile[num437, num438];
                    tile17.TileType = TileID.HardenedSand;
                    goto IL_035e;
                IL_03c6:
                    if ((double)num438 < Main.worldSurface)
                    {
                        tile17 = Main.tile[num437, num438];
                        if (tile17.HasTile)
                        {
                            tile17 = Main.tile[num437, num438];
                            if (tile17.TileType == TileID.Sand)
                            {
                                tile17 = Main.tile[num437, num438 + 1];
                                if (tile17.WallType == WallID.None && !WorldGen.SolidTile(num437, num438 + 1))
                                {
                                    ushort num440 = 0;
                                    int num441 = 3;
                                    for (int num442 = num437 - num441; num442 <= num437 + num441; num442++)
                                    {
                                        for (int num443 = num438 - num441; num443 <= num438 + num441; num443++)
                                        {
                                            tile17 = Main.tile[num442, num443];
                                            if (tile17.WallType > WallID.None)
                                            {
                                                tile17 = Main.tile[num442, num443];
                                                num440 = tile17.WallType;
                                                break;
                                            }
                                        }
                                    }
                                    if (num440 > 0)
                                    {
                                        tile17 = Main.tile[num437, num438 + 1];
                                        tile17.WallType = num440;
                                        tile17 = Main.tile[num437, num438];
                                        if (tile17.WallType == WallID.None)
                                        {
                                            tile17 = Main.tile[num437, num438];
                                            tile17.WallType = num440;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    tile17 = Main.tile[num437, num438];
                    if (tile17.TileType != TileID.Platforms)
                    {
                        bool[] canBeClearedDuringGeneration = TileID.Sets.CanBeClearedDuringGeneration;
                        tile17 = Main.tile[num437, num438];
                        if (canBeClearedDuringGeneration[tile17.TileType])
                        {
                            tile17 = Main.tile[num437, num438];
                            if (!tile17.TopSlope)
                            {
                                tile17 = Main.tile[num437, num438];
                                if (!tile17.IsHalfBlock)
                                {
                                    tile17 = Main.tile[num437, num438];
                                    if (tile17.BottomSlope)
                                    {
                                        if (!WorldGen.SolidTile(num437, num438 - 1))
                                        {
                                            tile17 = Main.tile[num437, num438];
                                            tile17.HasTile = false;
                                        }
                                        tile17 = Main.tile[num437 + 1, num438];
                                        if (tile17.TileType != TileID.Traps)
                                        {
                                            tile17 = Main.tile[num437 - 1, num438];
                                            if (tile17.TileType != TileID.Traps)
                                            {
                                                continue;
                                            }
                                        }
                                        tile17 = Main.tile[num437, num438];
                                        tile17.HasTile = false;
                                    }
                                    continue;
                                }
                            }
                            tile17 = Main.tile[num437, num438];
                            if (tile17.TileType == TileID.Hive)
                            {
                                tile17 = Main.tile[num437, num438];
                                if (tile17.IsHalfBlock)
                                {
                                    continue;
                                }
                            }
                            if (!WorldGen.SolidTile(num437, num438 + 1))
                            {
                                tile17 = Main.tile[num437, num438];
                                tile17.HasTile = false;
                            }
                            tile17 = Main.tile[num437 + 1, num438];
                            if (tile17.TileType != TileID.Traps)
                            {
                                tile17 = Main.tile[num437 - 1, num438];
                                if (tile17.TileType != TileID.Traps)
                                {
                                    continue;
                                }
                            }
                            tile17 = Main.tile[num437, num438];
                            tile17.HasTile = false;
                        }
                    }
                    continue;
                IL_035e:
                    if ((double)num438 <= Main.rockLayer)
                    {
                        tile17 = Main.tile[num437, num438];
                        tile17.LiquidAmount = 0;
                    }
                    else
                    {
                        tile17 = Main.tile[num437, num438];
                        if (tile17.LiquidAmount > 0)
                        {
                            tile17 = Main.tile[num437, num438];
                            tile17.LiquidAmount = byte.MaxValue;
                            tile17 = Main.tile[num437, num438];
                            tile17.LiquidType = LiquidID.Lava;
                        }
                    }
                    goto IL_03c6;
                }
            }
        }

        public static void SpreadingGrass()
        {
            if (!WorldGen.notTheBees || WorldGen.remixWorldGen)
            {
                Tile tile16;
                for (int num399 = 50; num399 < Main.maxTilesX - 50; num399++)
                {
                    for (int num400 = 50; (double)num400 <= Main.worldSurface; num400++)
                    {
                        tile16 = Main.tile[num399, num400];
                        if (tile16.HasTile)
                        {
                            tile16 = Main.tile[num399, num400];
                            int type4 = tile16.TileType;
                            tile16 = Main.tile[num399, num400];
                            if (tile16.HasTile && type4 == 60)
                            {
                                for (int num401 = num399 - 1; num401 <= num399 + 1; num401++)
                                {
                                    for (int num402 = num400 - 1; num402 <= num400 + 1; num402++)
                                    {
                                        tile16 = Main.tile[num401, num402];
                                        if (tile16.HasTile)
                                        {
                                            tile16 = Main.tile[num401, num402];
                                            if (tile16.TileType == TileID.Dirt)
                                            {
                                                tile16 = Main.tile[num401, num402 - 1];
                                                if (!tile16.HasTile)
                                                {
                                                    tile16 = Main.tile[num401, num402];
                                                    tile16.TileType = TileID.JungleGrass;
                                                }
                                                else
                                                {
                                                    tile16 = Main.tile[num401, num402];
                                                    tile16.TileType = TileID.Mud;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (type4 == 1 || type4 == 40 || TileID.Sets.Ore[type4])
                            {
                                int num403 = 3;
                                bool flag22 = false;
                                ushort num404 = 0;
                                for (int num405 = num399 - num403; num405 <= num399 + num403; num405++)
                                {
                                    for (int num406 = num400 - num403; num406 <= num400 + num403; num406++)
                                    {
                                        tile16 = Main.tile[num405, num406];
                                        if (tile16.HasTile)
                                        {
                                            tile16 = Main.tile[num405, num406];
                                            if (tile16.TileType == TileID.Sand || num404 == 53)
                                            {
                                                num404 = 53;
                                            }
                                            else
                                            {
                                                tile16 = Main.tile[num405, num406];
                                                if (tile16.TileType != TileID.Mud)
                                                {
                                                    tile16 = Main.tile[num405, num406];
                                                    if (tile16.TileType != TileID.JungleGrass)
                                                    {
                                                        tile16 = Main.tile[num405, num406];
                                                        if (tile16.TileType != TileID.SnowBlock)
                                                        {
                                                            tile16 = Main.tile[num405, num406];
                                                            if (tile16.TileType != TileID.IceBlock)
                                                            {
                                                                tile16 = Main.tile[num405, num406];
                                                                if (tile16.TileType != TileID.CrimsonGrass)
                                                                {
                                                                    tile16 = Main.tile[num405, num406];
                                                                    if (tile16.TileType != TileID.CorruptGrass)
                                                                    {
                                                                        continue;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                tile16 = Main.tile[num405, num406];
                                                num404 = tile16.TileType;
                                            }
                                        }
                                        else if (num406 < num400)
                                        {
                                            tile16 = Main.tile[num405, num406];
                                            if (tile16.WallType == WallID.None)
                                            {
                                                flag22 = true;
                                            }
                                        }
                                    }
                                }
                                if (flag22)
                                {
                                    switch (num404)
                                    {
                                        case 23:
                                        case 199:
                                            tile16 = Main.tile[num399, num400 - 1];
                                            if (tile16.HasTile)
                                            {
                                                num404 = 0;
                                            }
                                            break;
                                        case 59:
                                        case 60:
                                            if (num399 >= GenVars.jungleMinX && num399 <= GenVars.jungleMaxX)
                                            {
                                                tile16 = Main.tile[num399, num400 - 1];
                                                num404 = (ushort)(tile16.HasTile ? 59u : 60u);
                                            }
                                            break;
                                    }
                                    tile16 = Main.tile[num399, num400];
                                    tile16.TileType = num404;
                                }
                            }
                        }
                    }
                }
                for (int num407 = 10; num407 < Main.maxTilesX - 10; num407++)
                {
                    bool flag23 = true;
                    for (int num408 = 0; (double)num408 < Main.worldSurface - 1.0; num408++)
                    {
                        tile16 = Main.tile[num407, num408];
                        if (tile16.HasTile)
                        {
                            if (flag23)
                            {
                                tile16 = Main.tile[num407, num408];
                                if (tile16.TileType == TileID.Dirt)
                                {
                                    try
                                    {
                                        WorldGen.grassSpread = 0;
                                        WorldGen.SpreadGrass(num407, num408);
                                    }
                                    catch
                                    {
                                        WorldGen.grassSpread = 0;
                                        WorldGen.SpreadGrass(num407, num408, 0, 2, repeat: false);
                                    }
                                }
                            }
                            if ((double)num408 > GenVars.worldSurfaceHigh)
                            {
                                break;
                            }
                            flag23 = false;
                        }
                        else
                        {
                            tile16 = Main.tile[num407, num408];
                            if (tile16.WallType == WallID.None)
                            {
                                flag23 = true;
                            }
                        }
                    }
                }
                if (WorldGen.remixWorldGen)
                {
                    for (int num409 = 5; num409 < Main.maxTilesX - 5; num409++)
                    {
                        for (int num410 = (int)GenVars.rockLayerLow + WorldGen.genRand.Next(-1, 2); num410 < Main.maxTilesY - 200; num410++)
                        {
                            tile16 = Main.tile[num409, num410];
                            if (tile16.TileType == TileID.Dirt)
                            {
                                tile16 = Main.tile[num409, num410];
                                if (tile16.HasTile)
                                {
                                    tile16 = Main.tile[num409 - 1, num410 - 1];
                                    if (tile16.HasTile)
                                    {
                                        tile16 = Main.tile[num409, num410 - 1];
                                        if (tile16.HasTile)
                                        {
                                            tile16 = Main.tile[num409 + 1, num410 - 1];
                                            if (tile16.HasTile)
                                            {
                                                tile16 = Main.tile[num409 - 1, num410];
                                                if (tile16.HasTile)
                                                {
                                                    tile16 = Main.tile[num409 + 1, num410];
                                                    if (tile16.HasTile)
                                                    {
                                                        tile16 = Main.tile[num409 - 1, num410 + 1];
                                                        if (tile16.HasTile)
                                                        {
                                                            tile16 = Main.tile[num409, num410 + 1];
                                                            if (tile16.HasTile)
                                                            {
                                                                tile16 = Main.tile[num409 + 1, num410 + 1];
                                                                if (tile16.HasTile)
                                                                {
                                                                    continue;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    tile16 = Main.tile[num409, num410];
                                    tile16.TileType = TileID.Grass;
                                }
                            }
                        }
                    }
                    for (int num411 = 5; num411 < Main.maxTilesX - 5; num411++)
                    {
                        for (int num412 = (int)GenVars.rockLayerLow + WorldGen.genRand.Next(-1, 2); num412 < Main.maxTilesY - 200; num412++)
                        {
                            tile16 = Main.tile[num411, num412];
                            if (tile16.TileType == TileID.Grass)
                            {
                                tile16 = Main.tile[num411, num412 - 1];
                                if (!tile16.HasTile && WorldGen.genRand.NextBool(20))
                                {
                                    WorldGen.PlaceTile(num411, num412 - 1, 27, mute: true);
                                }
                            }
                        }
                    }
                    int conversionType = 1;
                    if (WorldGen.crimson)
                    {
                        conversionType = 4;
                    }
                    int num413 = Main.maxTilesX / 7;
                    for (int num414 = 10; num414 < Main.maxTilesX - 10; num414++)
                    {
                        for (int num415 = 10; num415 < Main.maxTilesY - 10; num415++)
                        {
                            if ((double)num415 < Main.worldSurface + (double)WorldGen.genRand.Next(3) || num414 < num413 + WorldGen.genRand.Next(3) || num414 >= Main.maxTilesX - num413 - WorldGen.genRand.Next(3))
                            {
                                if (WorldGen.drunkWorldGen)
                                {
                                    if (GenVars.crimsonLeft)
                                    {
                                        if (num414 < Main.maxTilesX / 2 + WorldGen.genRand.Next(-2, 3))
                                        {
                                            WorldGen.Convert(num414, num415, 4, 1, true, true);
                                        }
                                        else
                                        {
                                            WorldGen.Convert(num414, num415, 1, 1, true, true);
                                        }
                                    }
                                    else if (num414 < Main.maxTilesX / 2 + WorldGen.genRand.Next(-2, 3))
                                    {
                                        WorldGen.Convert(num414, num415, 1, 1, true, true);
                                    }
                                    else
                                    {
                                        WorldGen.Convert(num414, num415, 4, 1, true, true);
                                    }
                                }
                                else
                                {
                                    WorldGen.Convert(num414, num415, conversionType, 1, true, true);
                                }
                                tile16 = Main.tile[num414, num415];
                                tile16.TileColor = PaintID.None;
                                tile16 = Main.tile[num414, num415];
                                tile16.WallColor = PaintID.None;
                            }
                        }
                    }
                    if (WorldGen.remixWorldGen)
                    {
                        Main.tileSolid[225] = true;
                        int num416 = (int)((double)Main.maxTilesX * 0.31);
                        int num417 = (int)((double)Main.maxTilesX * 0.69);
                        _ = Main.maxTilesY;
                        int num418 = Main.maxTilesY - 135;
                        _ = Main.maxTilesY;
                        Liquid.QuickWater(-2);
                        for (int num419 = num416; num419 < num417 + 15; num419++)
                        {
                            for (int num420 = Main.maxTilesY - 200; num420 < num418; num420++)
                            {
                                tile16 = Main.tile[num419, num420];
                                tile16.LiquidAmount = 0;
                            }
                        }
                        Main.tileSolid[225] = false;
                        Main.tileSolid[484] = false;
                    }
                }
            }
        }

        public static void Piles()
        {
            Main.tileSolid[229] = false;
            Main.tileSolid[190] = false;
            Main.tileSolid[196] = false;
            Main.tileSolid[189] = false;
            Main.tileSolid[202] = false;
            Main.tileSolid[460] = false;
            Main.tileSolid[484] = false;
            for (int num304 = 0; (double)num304 < (double)Main.maxTilesX * 0.06; num304++)
            {
                int num305 = Main.maxTilesX / 2;
                bool flag11 = false;
                while (!flag11 && num305 > 0)
                {
                    num305--;
                    int num306 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                    int num307 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 300);
                    while (WorldGen.oceanDepths(num306, num307))
                    {
                        num306 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                        num307 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 300);
                    }
                    if (!Main.tile[num306, num307].HasTile)
                    {
                        int num308 = 186;
                        for (; !Main.tile[num306, num307 + 1].HasTile && num307 < Main.maxTilesY - 5; num307++)
                        {
                        }
                        int num309 = WorldGen.genRand.Next(22);
                        if (num309 >= 16 && num309 <= 22)
                        {
                            num309 = WorldGen.genRand.Next(22);
                        }
                        if ((Main.tile[num306, num307 + 1].TileType == TileID.Dirt || Main.tile[num306, num307 + 1].TileType == TileID.Stone || Main.tileMoss[Main.tile[num306, num307 + 1].TileType]) && WorldGen.genRand.NextBool(5))
                        {
                            num309 = WorldGen.genRand.Next(23, 29);
                            num308 = 187;
                        }
                        if (num307 > Main.maxTilesY - 300 || Main.wallDungeon[Main.tile[num306, num307].WallType] || Main.tile[num306, num307 + 1].TileType == TileID.WoodBlock || Main.tile[num306, num307 + 1].TileType == TileID.Platforms || Main.tile[num306, num307 + 1].TileType == TileID.Ebonstone || Main.tile[num306, num307 + 1].TileType == TileID.Crimstone)
                        {
                            num309 = WorldGen.genRand.Next(7);
                            num308 = 186;
                        }
                        if (Main.tile[num306, num307 + 1].TileType == TileID.SnowBlock || Main.tile[num306, num307 + 1].TileType == TileID.IceBlock || Main.tile[num306, num307 + 1].TileType == TileID.BreakableIce)
                        {
                            num309 = WorldGen.genRand.Next(26, 32);
                            num308 = 186;
                        }
                        if (Main.tile[num306, num307 + 1].TileType == TileID.JungleGrass)
                        {
                            num308 = 187;
                            num309 = WorldGen.genRand.Next(6);
                        }
                        if ((Main.tile[num306, num307 + 1].TileType == TileID.Ash || Main.tile[num306, num307 + 1].TileType == TileID.Hellstone) && WorldGen.genRand.Next(3) < 2)
                        {
                            num308 = 187;
                            num309 = WorldGen.genRand.Next(6, 9);
                        }
                        if (Main.tile[num306, num307 + 1].TileType == TileID.LihzahrdBrick)
                        {
                            num308 = 187;
                            num309 = WorldGen.genRand.Next(18, 23);
                        }
                        if (Main.tile[num306, num307 + 1].TileType == TileID.MushroomGrass)
                        {
                            num309 = WorldGen.genRand.Next(32, 35);
                            num308 = 186;
                        }
                        if (Main.tile[num306, num307 + 1].TileType == TileID.Sandstone || Main.tile[num306, num307 + 1].TileType == TileID.HardenedSand || Main.tile[num306, num307 + 1].TileType == TileID.DesertFossil)
                        {
                            num309 = WorldGen.genRand.Next(29, 35);
                            num308 = 187;
                        }
                        if (Main.tile[num306, num307 + 1].TileType == TileID.Granite)
                        {
                            num309 = WorldGen.genRand.Next(35, 41);
                            num308 = 187;
                        }
                        if (Main.tile[num306, num307 + 1].TileType == TileID.Marble)
                        {
                            num309 = WorldGen.genRand.Next(41, 47);
                            num308 = 187;
                        }
                        if (num308 == 186 && num309 >= 7 && num309 <= 15 && WorldGen.genRand.NextBool(75))
                        {
                            num308 = 187;
                            num309 = 17;
                        }
                        if (Main.wallDungeon[Main.tile[num306, num307].WallType] && !WorldGen.genRand.NextBool(3))
                        {
                            flag11 = true;
                        }
                        else
                        {
                            if (Main.tile[num306, num307].LiquidType != LiquidID.Shimmer)
                            {
                                WorldGen.PlaceTile(num306, num307, num308, mute: true, forced: false, -1, num309);
                            }
                            if (Main.tile[num306, num307].TileType == TileID.LargePiles || Main.tile[num306, num307].TileType == TileID.LargePiles2)
                            {
                                flag11 = true;
                            }
                            if (flag11 && num308 == 186 && num309 <= 7)
                            {
                                int num310 = WorldGen.genRand.Next(1, 5);
                                for (int num311 = 0; num311 < num310; num311++)
                                {
                                    int num312 = num306 + WorldGen.genRand.Next(-10, 11);
                                    int num313 = num307 - WorldGen.genRand.Next(5);
                                    if (!Main.tile[num312, num313].HasTile)
                                    {
                                        for (; !Main.tile[num312, num313 + 1].HasTile && num313 < Main.maxTilesY - 5; num313++)
                                        {
                                        }
                                        int x12 = WorldGen.genRand.Next(12, 36);
                                        WorldGen.PlaceSmallPile(num312, num313, x12, 0, 185);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int num314 = 0; (double)num314 < (double)Main.maxTilesX * 0.01; num314++)
            {
                int num315 = Main.maxTilesX / 2;
                bool flag12 = false;
                while (!flag12 && num315 > 0)
                {
                    num315--;
                    int num316 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                    int num317 = WorldGen.genRand.Next(Main.maxTilesY - 300, Main.maxTilesY - 10);
                    if (!Main.tile[num316, num317].HasTile)
                    {
                        int num318 = 186;
                        for (; !Main.tile[num316, num317 + 1].HasTile && num317 < Main.maxTilesY - 5; num317++)
                        {
                        }
                        int num319 = WorldGen.genRand.Next(22);
                        if (num319 >= 16 && num319 <= 22)
                        {
                            num319 = WorldGen.genRand.Next(22);
                        }
                        if (num317 > Main.maxTilesY - 300 || Main.wallDungeon[Main.tile[num316, num317].WallType] || Main.tile[num316, num317 + 1].TileType == TileID.WoodBlock || Main.tile[num316, num317 + 1].TileType == TileID.Platforms)
                        {
                            num319 = WorldGen.genRand.Next(7);
                        }
                        if ((Main.tile[num316, num317 + 1].TileType == TileID.Ash || Main.tile[num316, num317 + 1].TileType == TileID.Hellstone) && WorldGen.genRand.Next(3) < 2)
                        {
                            num318 = 187;
                            num319 = WorldGen.genRand.Next(6, 9);
                        }
                        if (Main.tile[num316, num317 + 1].TileType == TileID.SnowBlock || Main.tile[num316, num317 + 1].TileType == TileID.IceBlock || Main.tile[num316, num317 + 1].TileType == TileID.BreakableIce)
                        {
                            num319 = WorldGen.genRand.Next(26, 32);
                        }
                        WorldGen.PlaceTile(num316, num317, num318, mute: true, forced: false, -1, num319);
                        if (Main.tile[num316, num317].TileType == TileID.LargePiles || Main.tile[num316, num317].TileType == TileID.LargePiles2)
                        {
                            flag12 = true;
                        }
                        if (flag12 && num318 == 186 && num319 <= 7)
                        {
                            int num320 = WorldGen.genRand.Next(1, 5);
                            for (int num321 = 0; num321 < num320; num321++)
                            {
                                int num322 = num316 + WorldGen.genRand.Next(-10, 11);
                                int num323 = num317 - WorldGen.genRand.Next(5);
                                if (!Main.tile[num322, num323].HasTile)
                                {
                                    for (; !Main.tile[num322, num323 + 1].HasTile && num323 < Main.maxTilesY - 5; num323++)
                                    {
                                    }
                                    int x13 = WorldGen.genRand.Next(12, 36);
                                    WorldGen.PlaceSmallPile(num322, num323, x13, 0, 185);
                                }
                            }
                        }
                    }
                }
            }
            for (int num324 = 0; (double)num324 < (double)Main.maxTilesX * 0.003; num324++)
            {
                int num325 = Main.maxTilesX / 2;
                bool flag13 = false;
                while (!flag13 && num325 > 0)
                {
                    num325--;
                    int num326 = 186;
                    int num327 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                    int num328 = WorldGen.genRand.Next(10, (int)Main.worldSurface);
                    while (WorldGen.oceanDepths(num327, num328))
                    {
                        num327 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                        num328 = WorldGen.genRand.Next(10, (int)Main.worldSurface);
                    }
                    if (!Main.tile[num327, num328].HasTile)
                    {
                        for (; !Main.tile[num327, num328 + 1].HasTile && num328 < Main.maxTilesY - 5; num328++)
                        {
                        }
                        int num329 = WorldGen.genRand.Next(7, 13);
                        if (num328 > Main.maxTilesY - 300 || Main.wallDungeon[Main.tile[num327, num328].WallType] || Main.tile[num327, num328 + 1].TileType == TileID.WoodBlock || Main.tile[num327, num328 + 1].TileType == TileID.Platforms || Main.tile[num327, num328 + 1].TileType == TileID.Ebonstone || Main.tile[num327, num328 + 1].TileType == TileID.Crimstone || Main.tile[num327, num328 + 1].TileType == TileID.Crimsand || Main.tile[num327, num328 + 1].TileType == TileID.Ebonsand)
                        {
                            num329 = -1;
                        }
                        if (Main.tile[num327, num328 + 1].TileType == TileID.SnowBlock || Main.tile[num327, num328 + 1].TileType == TileID.IceBlock || Main.tile[num327, num328 + 1].TileType == TileID.BreakableIce)
                        {
                            num329 = WorldGen.genRand.Next(26, 32);
                        }
                        if (Main.tile[num327, num328 + 1].TileType == TileID.Sand)
                        {
                            num326 = 187;
                            num329 = WorldGen.genRand.Next(52, 55);
                        }
                        if (Main.tile[num327, num328 + 1].TileType == TileID.Grass || Main.tile[num327 - 1, num328 + 1].TileType == TileID.Grass || Main.tile[num327 + 1, num328 + 1].TileType == TileID.Grass)
                        {
                            num326 = 187;
                            num329 = WorldGen.genRand.Next(14, 17);
                        }
                        if (Main.tile[num327, num328 + 1].TileType == TileID.SandstoneBrick || Main.tile[num327, num328 + 1].TileType == TileID.SandStoneSlab)
                        {
                            num326 = 186;
                            num329 = WorldGen.genRand.Next(7);
                        }
                        if (num329 >= 0)
                        {
                            WorldGen.PlaceTile(num327, num328, num326, mute: true, forced: false, -1, num329);
                        }
                        if (Main.tile[num327, num328].TileType == num326)
                        {
                            flag13 = true;
                        }
                    }
                }
            }
            for (int num330 = 0; (double)num330 < (double)Main.maxTilesX * 0.0035; num330++)
            {
                int num331 = Main.maxTilesX / 2;
                bool flag14 = false;
                while (!flag14 && num331 > 0)
                {
                    num331--;
                    int num332 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                    int num333 = WorldGen.genRand.Next(10, (int)Main.worldSurface);
                    if (!Main.tile[num332, num333].HasTile && Main.tile[num332, num333].WallType > WallID.None)
                    {
                        int num334 = 186;
                        for (; !Main.tile[num332, num333 + 1].HasTile && num333 < Main.maxTilesY - 5; num333++)
                        {
                        }
                        int num335 = WorldGen.genRand.Next(7, 13);
                        if (num333 > Main.maxTilesY - 300 || Main.wallDungeon[Main.tile[num332, num333].WallType] || Main.tile[num332, num333 + 1].TileType == TileID.WoodBlock || Main.tile[num332, num333 + 1].TileType == TileID.Platforms)
                        {
                            num335 = -1;
                        }
                        if (Main.tile[num332, num333 + 1].TileType == TileID.Ebonstone)
                        {
                            num335 = WorldGen.genRand.Next(7);
                        }
                        if (Main.tile[num332, num333 + 1].TileType == TileID.SnowBlock || Main.tile[num332, num333 + 1].TileType == TileID.IceBlock || Main.tile[num332, num333 + 1].TileType == TileID.BreakableIce)
                        {
                            num335 = WorldGen.genRand.Next(26, 32);
                        }
                        if (Main.tile[num332, num333 + 1].TileType == TileID.Grass || Main.tile[num332 - 1, num333 + 1].TileType == TileID.Grass || Main.tile[num332 + 1, num333 + 1].TileType == TileID.Grass)
                        {
                            num334 = 187;
                            num335 = WorldGen.genRand.Next(14, 17);
                        }
                        if (Main.tile[num332, num333 + 1].TileType == TileID.SandstoneBrick || Main.tile[num332, num333 + 1].TileType == TileID.SandStoneSlab)
                        {
                            num334 = 186;
                            num335 = WorldGen.genRand.Next(7);
                        }
                        if (num335 >= 0)
                        {
                            WorldGen.PlaceTile(num332, num333, num334, mute: true, forced: false, -1, num335);
                        }
                        if (Main.tile[num332, num333].TileType == num334)
                        {
                            flag14 = true;
                        }
                        if (flag14 && num335 <= 7)
                        {
                            int num336 = WorldGen.genRand.Next(1, 5);
                            for (int num337 = 0; num337 < num336; num337++)
                            {
                                int num338 = num332 + WorldGen.genRand.Next(-10, 11);
                                int num339 = num333 - WorldGen.genRand.Next(5);
                                if (!Main.tile[num338, num339].HasTile)
                                {
                                    for (; !Main.tile[num338, num339 + 1].HasTile && num339 < Main.maxTilesY - 5; num339++)
                                    {
                                    }
                                    int x14 = WorldGen.genRand.Next(12, 36);
                                    WorldGen.PlaceSmallPile(num338, num339, x14, 0, 185);
                                }
                            }
                        }
                    }
                }
            }
            for (int num340 = 0; (double)num340 < (double)Main.maxTilesX * 0.6; num340++)
            {
                int num341 = Main.maxTilesX / 2;
                bool flag15 = false;
                while (!flag15 && num341 > 0)
                {
                    num341--;
                    int num342 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                    int num343 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 20);
                    if (Main.tile[num342, num343].WallType == WallID.LihzahrdBrickUnsafe && WorldGen.genRand.NextBool(2))
                    {
                        num342 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                        num343 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 20);
                    }
                    while (WorldGen.oceanDepths(num342, num343))
                    {
                        num342 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                        num343 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 20);
                    }
                    if (!Main.tile[num342, num343].HasTile)
                    {
                        for (; !Main.tile[num342, num343 + 1].HasTile && num343 < Main.maxTilesY - 5; num343++)
                        {
                        }
                        int num344 = WorldGen.genRand.Next(2);
                        int num345 = WorldGen.genRand.Next(36);
                        if (num345 >= 28 && num345 <= 35)
                        {
                            num345 = WorldGen.genRand.Next(36);
                        }
                        if (num344 == 1)
                        {
                            num345 = WorldGen.genRand.Next(25);
                            if (num345 >= 16 && num345 <= 24)
                            {
                                num345 = WorldGen.genRand.Next(25);
                            }
                        }
                        if (num343 > Main.maxTilesY - 300)
                        {
                            if (num344 == 0)
                            {
                                num345 = WorldGen.genRand.Next(12, 28);
                            }
                            if (num344 == 1)
                            {
                                num345 = WorldGen.genRand.Next(6, 16);
                            }
                        }
                        if (Main.wallDungeon[Main.tile[num342, num343].WallType] || Main.tile[num342, num343 + 1].TileType == TileID.WoodBlock || Main.tile[num342, num343 + 1].TileType == TileID.Platforms || Main.tile[num342, num343 + 1].TileType == TileID.Ebonstone || Main.tile[num342, num343 + 1].TileType == TileID.Crimstone || Main.tile[num342, num343].WallType == WallID.LihzahrdBrickUnsafe)
                        {
                            if (num344 == 0 && num345 < 12)
                            {
                                num345 += 12;
                            }
                            if (num344 == 1 && num345 < 6)
                            {
                                num345 += 6;
                            }
                            if (num344 == 1 && num345 >= 17)
                            {
                                num345 -= 10;
                            }
                        }
                        if (Main.tile[num342, num343 + 1].TileType == TileID.SnowBlock || Main.tile[num342, num343 + 1].TileType == TileID.IceBlock || Main.tile[num342, num343 + 1].TileType == TileID.BreakableIce)
                        {
                            if (num344 == 0 && num345 < 12)
                            {
                                num345 += 36;
                            }
                            if (num344 == 1 && num345 >= 20)
                            {
                                num345 += 6;
                            }
                            if (num344 == 1 && num345 < 6)
                            {
                                num345 += 25;
                            }
                        }
                        if (Main.tile[num342, num343 + 1].TileType == TileID.SandstoneBrick || Main.tile[num342, num343 + 1].TileType == TileID.SandStoneSlab)
                        {
                            if (num344 == 0)
                            {
                                num345 = WorldGen.genRand.Next(12, 28);
                            }
                            if (num344 == 1)
                            {
                                num345 = WorldGen.genRand.Next(12, 19);
                            }
                        }
                        if (Main.tile[num342, num343 + 1].TileType == TileID.Granite)
                        {
                            if (num344 == 0)
                            {
                                num345 = WorldGen.genRand.Next(60, 66);
                            }
                            if (num344 == 1)
                            {
                                num345 = WorldGen.genRand.Next(47, 53);
                            }
                        }
                        if (Main.tile[num342, num343 + 1].TileType == TileID.Marble)
                        {
                            if (num344 == 0)
                            {
                                num345 = WorldGen.genRand.Next(66, 72);
                            }
                            if (num344 == 1)
                            {
                                num345 = WorldGen.genRand.Next(53, 59);
                            }
                        }
                        if (Main.wallDungeon[Main.tile[num342, num343].WallType] && !WorldGen.genRand.NextBool(3))
                        {
                            flag15 = true;
                        }
                        else if (Main.tile[num342, num343].LiquidType != LiquidID.Shimmer)
                        {
                            flag15 = WorldGen.PlaceSmallPile(num342, num343, num345, num344, 185);
                        }
                        if (flag15 && num344 == 1 && num345 >= 6 && num345 <= 15)
                        {
                            int num346 = WorldGen.genRand.Next(1, 5);
                            for (int num347 = 0; num347 < num346; num347++)
                            {
                                int num348 = num342 + WorldGen.genRand.Next(-10, 11);
                                int num349 = num343 - WorldGen.genRand.Next(5);
                                if (!Main.tile[num348, num349].HasTile)
                                {
                                    for (; !Main.tile[num348, num349 + 1].HasTile && num349 < Main.maxTilesY - 5; num349++)
                                    {
                                    }
                                    int x15 = WorldGen.genRand.Next(12, 36);
                                    WorldGen.PlaceSmallPile(num348, num349, x15, 0, 185);
                                }
                            }
                        }
                    }
                }
            }
            for (int num350 = 0; (double)num350 < (double)Main.maxTilesX * 0.02; num350++)
            {
                int num351 = Main.maxTilesX / 2;
                bool flag16 = false;
                while (!flag16 && num351 > 0)
                {
                    num351--;
                    int num352 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                    int num353 = WorldGen.genRand.Next(15, (int)Main.worldSurface);
                    while (WorldGen.oceanDepths(num352, num353))
                    {
                        num352 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                        num353 = WorldGen.genRand.Next(15, (int)Main.worldSurface);
                    }
                    if (!Main.tile[num352, num353].HasTile)
                    {
                        for (; !Main.tile[num352, num353 + 1].HasTile && num353 < Main.maxTilesY - 5; num353++)
                        {
                        }
                        int num354 = WorldGen.genRand.Next(2);
                        int num355 = WorldGen.genRand.Next(11);
                        if (num354 == 1)
                        {
                            num355 = WorldGen.genRand.Next(5);
                        }
                        if (Main.tile[num352, num353 + 1].TileType == TileID.SnowBlock || Main.tile[num352, num353 + 1].TileType == TileID.IceBlock || Main.tile[num352, num353 + 1].TileType == TileID.BreakableIce)
                        {
                            if (num354 == 0 && num355 < 12)
                            {
                                num355 += 36;
                            }
                            if (num354 == 1 && num355 >= 20)
                            {
                                num355 += 6;
                            }
                            if (num354 == 1 && num355 < 6)
                            {
                                num355 += 25;
                            }
                        }
                        if (Main.tile[num352, num353 + 1].TileType == TileID.Grass && num354 == 1)
                        {
                            num355 = WorldGen.genRand.Next(38, 41);
                        }
                        if (Main.tile[num352, num353 + 1].TileType == TileID.SandstoneBrick || Main.tile[num352, num353 + 1].TileType == TileID.SandStoneSlab)
                        {
                            if (num354 == 0)
                            {
                                num355 = WorldGen.genRand.Next(12, 28);
                            }
                            if (num354 == 1)
                            {
                                num355 = WorldGen.genRand.Next(12, 19);
                            }
                        }
                        if (!Main.wallDungeon[Main.tile[num352, num353].WallType] && Main.tile[num352, num353 + 1].TileType != TileID.WoodBlock && Main.tile[num352, num353 + 1].TileType != TileID.Platforms && Main.tile[num352, num353 + 1].TileType != TileID.BlueDungeonBrick && Main.tile[num352, num353 + 1].TileType != TileID.GreenDungeonBrick && Main.tile[num352, num353 + 1].TileType != TileID.PinkDungeonBrick && Main.tile[num352, num353 + 1].TileType != TileID.CrackedBlueDungeonBrick && Main.tile[num352, num353 + 1].TileType != TileID.CrackedGreenDungeonBrick && Main.tile[num352, num353 + 1].TileType != TileID.CrackedPinkDungeonBrick && Main.tile[num352, num353 + 1].TileType != TileID.GoldBrick && Main.tile[num352, num353 + 1].TileType != TileID.SilverBrick && Main.tile[num352, num353 + 1].TileType != TileID.CopperBrick && Main.tile[num352, num353 + 1].TileType != TileID.TinBrick && Main.tile[num352, num353 + 1].TileType != TileID.TungstenBrick && Main.tile[num352, num353 + 1].TileType != TileID.PlatinumBrick && Main.tile[num352, num353 + 1].TileType != TileID.Sand && Main.tile[num352, num353 + 1].TileType != TileID.Ebonstone && Main.tile[num352, num353 + 1].TileType != TileID.Crimstone)
                        {
                            flag16 = WorldGen.PlaceSmallPile(num352, num353, num355, num354, 185);
                        }
                    }
                }
            }
            for (int num356 = 0; (double)num356 < (double)Main.maxTilesX * 0.15; num356++)
            {
                int num357 = Main.maxTilesX / 2;
                bool flag17 = false;
                while (!flag17 && num357 > 0)
                {
                    num357--;
                    int num358 = WorldGen.genRand.Next(25, Main.maxTilesX - 25);
                    int num359 = WorldGen.genRand.Next(15, (int)Main.worldSurface);
                    if (!Main.tile[num358, num359].HasTile && (Main.tile[num358, num359].WallType == WallID.DirtUnsafe || Main.tile[num358, num359].WallType == WallID.SnowWallUnsafe))
                    {
                        for (; !Main.tile[num358, num359 + 1].HasTile && num359 < Main.maxTilesY - 5; num359++)
                        {
                        }
                        int num360 = WorldGen.genRand.Next(2);
                        int num361 = WorldGen.genRand.Next(11);
                        if (num360 == 1)
                        {
                            num361 = WorldGen.genRand.Next(5);
                        }
                        if (Main.tile[num358, num359 + 1].TileType == TileID.SnowBlock || Main.tile[num358, num359 + 1].TileType == TileID.IceBlock || Main.tile[num358, num359 + 1].TileType == TileID.BreakableIce)
                        {
                            if (num360 == 0 && num361 < 12)
                            {
                                num361 += 36;
                            }
                            if (num360 == 1 && num361 >= 20)
                            {
                                num361 += 6;
                            }
                            if (num360 == 1 && num361 < 6)
                            {
                                num361 += 25;
                            }
                        }
                        if (Main.tile[num358, num359 + 1].TileType == TileID.Grass && num360 == 1)
                        {
                            num361 = WorldGen.genRand.Next(38, 41);
                        }
                        if (Main.tile[num358, num359 + 1].TileType == TileID.SandstoneBrick || Main.tile[num358, num359 + 1].TileType == TileID.SandStoneSlab)
                        {
                            if (num360 == 0)
                            {
                                num361 = WorldGen.genRand.Next(12, 28);
                            }
                            if (num360 == 1)
                            {
                                num361 = WorldGen.genRand.Next(12, 19);
                            }
                        }
                        if ((Main.tile[num358, num359].LiquidAmount != byte.MaxValue || Main.tile[num358, num359 + 1].TileType != TileID.Sand || Main.tile[num358, num359].WallType != WallID.None) && !Main.wallDungeon[Main.tile[num358, num359].WallType] && Main.tile[num358, num359 + 1].TileType != TileID.WoodBlock && Main.tile[num358, num359 + 1].TileType != TileID.Platforms && Main.tile[num358, num359 + 1].TileType != TileID.BlueDungeonBrick && Main.tile[num358, num359 + 1].TileType != TileID.GreenDungeonBrick && Main.tile[num358, num359 + 1].TileType != TileID.PinkDungeonBrick && Main.tile[num358, num359 + 1].TileType != TileID.CrackedBlueDungeonBrick && Main.tile[num358, num359 + 1].TileType != TileID.CrackedGreenDungeonBrick && Main.tile[num358, num359 + 1].TileType != TileID.CrackedPinkDungeonBrick && Main.tile[num358, num359 + 1].TileType != TileID.GoldBrick && Main.tile[num358, num359 + 1].TileType != TileID.SilverBrick && Main.tile[num358, num359 + 1].TileType != TileID.CopperBrick && Main.tile[num358, num359 + 1].TileType != TileID.TinBrick && Main.tile[num358, num359 + 1].TileType != TileID.TungstenBrick && Main.tile[num358, num359 + 1].TileType != TileID.PlatinumBrick && Main.tile[num358, num359 + 1].TileType != TileID.Ebonstone && Main.tile[num358, num359 + 1].TileType != TileID.Crimstone)
                        {
                            flag17 = WorldGen.PlaceSmallPile(num358, num359, num361, num360, 185);
                        }
                    }
                }
            }
            Main.tileSolid[190] = true;
            Main.tileSolid[192] = true;
            Main.tileSolid[196] = true;
            Main.tileSolid[189] = true;
            Main.tileSolid[202] = true;
            Main.tileSolid[225] = true;
            Main.tileSolid[460] = true;
            Main.tileSolid[138] = true;
        }

        public static void SpawnPoint()
        {
            int num295 = 5;
            bool flag10 = true;
            int num296 = Main.maxTilesX / 2;
            if (Main.tenthAnniversaryWorld && !WorldGen.remixWorldGen)
            {
                int num297 = GenVars.beachBordersWidth + 15;
                num296 = ((!WorldGen.genRand.NextBool(2)) ? (Main.maxTilesX - num297) : num297);
            }
            while (flag10)
            {
                int num298 = num296 + WorldGen.genRand.Next(-num295, num295 + 1);
                for (int num299 = 0; num299 < Main.maxTilesY; num299++)
                {
                    if (Main.tile[num298, num299].HasTile)
                    {
                        Main.spawnTileX = num298;
                        Main.spawnTileY = num299;
                        break;
                    }
                }
                flag10 = false;
                num295++;
                if ((double)Main.spawnTileY > Main.worldSurface)
                {
                    flag10 = true;
                }
                if (Main.tile[Main.spawnTileX, Main.spawnTileY - 1].LiquidAmount > 0)
                {
                    flag10 = true;
                }
            }
            int num300 = 10;
            while ((double)Main.spawnTileY > Main.worldSurface)
            {
                int num301 = WorldGen.genRand.Next(num296 - num300, num296 + num300);
                for (int num302 = 0; num302 < Main.maxTilesY; num302++)
                {
                    if (Main.tile[num301, num302].HasTile)
                    {
                        Main.spawnTileX = num301;
                        Main.spawnTileY = num302;
                        break;
                    }
                }
                num300++;
            }
            if (WorldGen.remixWorldGen)
            {
                int num303 = Main.maxTilesY - 10;
                while (WorldGen.SolidTile(Main.spawnTileX, num303))
                {
                    num303--;
                }
                Main.spawnTileY = num303 + 1;
            }
        }

        public static void GrassWall()
        {
            WorldGen.maxTileCount = 3500;
            Tile tile15;
            for (int num283 = 50; num283 < Main.maxTilesX - 50; num283++)
            {
                for (int num284 = 0; (double)num284 < Main.worldSurface - 10.0; num284++)
                {
                    if (!WorldGen.genRand.NextBool(4))
                    {
                        continue;
                    }
                    bool flag8 = false;
                    int num285 = -1;
                    int num286 = -1;
                    tile15 = Main.tile[num283, num284];
                    if (tile15.HasTile)
                    {
                        tile15 = Main.tile[num283, num284];
                        if (tile15.TileType == TileID.Grass)
                        {
                            tile15 = Main.tile[num283, num284];
                            if (tile15.WallType != WallID.DirtUnsafe)
                            {
                                tile15 = Main.tile[num283, num284];
                                if (tile15.WallType != WallID.GrassUnsafe)
                                {
                                    goto IL_0170;
                                }
                            }
                            for (int num287 = num283 - 1; num287 <= num283 + 1; num287++)
                            {
                                for (int num288 = num284 - 1; num288 <= num284 + 1; num288++)
                                {
                                    tile15 = Main.tile[num287, num288];
                                    if (tile15.WallType == WallID.None && !WorldGen.SolidTile(num287, num288))
                                    {
                                        flag8 = true;
                                    }
                                }
                            }
                            if (flag8)
                            {
                                for (int num289 = num283 - 1; num289 <= num283 + 1; num289++)
                                {
                                    for (int num290 = num284 - 1; num290 <= num284 + 1; num290++)
                                    {
                                        tile15 = Main.tile[num289, num290];
                                        if (tile15.WallType != WallID.DirtUnsafe)
                                        {
                                            tile15 = Main.tile[num289, num290];
                                            if (tile15.WallType != WallID.MudUnsafe)
                                            {
                                                continue;
                                            }
                                        }
                                        if (!WorldGen.SolidTile(num289, num290))
                                        {
                                            num285 = num289;
                                            num286 = num290;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    goto IL_0170;
                IL_0170:
                    if (flag8 && num285 > -1 && num286 > -1 && WorldGen.countDirtTiles(num285, num286) < WorldGen.maxTileCount)
                    {
                        try
                        {
                            ushort wallType = 63;
                            if (WorldGen.dontStarveWorldGen && !WorldGen.genRand.NextBool(3))
                            {
                                wallType = 62;
                            }
                            Spread.Wall2(num285, num286, wallType);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            for (int num291 = 5; num291 < Main.maxTilesX - 5; num291++)
            {
                for (int num292 = 10; (double)num292 < Main.worldSurface - 1.0; num292++)
                {
                    tile15 = Main.tile[num291, num292];
                    if (tile15.WallType == WallID.GrassUnsafe && WorldGen.genRand.NextBool(10))
                    {
                        tile15 = Main.tile[num291, num292];
                        tile15.WallType = WallID.FlowerUnsafe;
                    }
                    tile15 = Main.tile[num291, num292];
                    if (tile15.HasTile)
                    {
                        tile15 = Main.tile[num291, num292];
                        if (tile15.TileType == TileID.Dirt)
                        {
                            bool flag9 = false;
                            for (int num293 = num291 - 1; num293 <= num291 + 1; num293++)
                            {
                                int num294 = num292 - 1;
                                while (num294 <= num292 + 1)
                                {
                                    tile15 = Main.tile[num293, num294];
                                    if (tile15.WallType != WallID.GrassUnsafe)
                                    {
                                        tile15 = Main.tile[num293, num294];
                                        if (tile15.WallType != WallID.FlowerUnsafe)
                                        {
                                            num294++;
                                            continue;
                                        }
                                    }
                                    flag9 = true;
                                    break;
                                }
                            }
                            if (flag9)
                            {
                                WorldGen.SpreadGrass(num291, num292);
                            }
                        }
                    }
                }
            }
        }

        public static void Sunflowers()
        {
            double num268 = (double)Main.maxTilesX * 0.002;
            for (int num269 = 0; (double)num269 < num268; num269++)
            {
                int num270 = 0;
                int num271 = 0;
                _ = Main.maxTilesX / 2;
                int num272 = WorldGen.genRand.Next(Main.maxTilesX);
                num270 = num272 - WorldGen.genRand.Next(10) - 7;
                num271 = num272 + WorldGen.genRand.Next(10) + 7;
                if (num270 < 0)
                {
                    num270 = 0;
                }
                if (num271 > Main.maxTilesX - 1)
                {
                    num271 = Main.maxTilesX - 1;
                }
                int num273 = 1;
                int num274 = (int)Main.worldSurface - 1;
                for (int num275 = num270; num275 < num271; num275++)
                {
                    for (int num276 = num273; num276 < num274; num276++)
                    {
                        if (Main.tile[num275, num276].TileType == TileID.Grass && Main.tile[num275, num276].HasTile && !Main.tile[num275, num276 - 1].HasTile)
                        {
                            WorldGen.PlaceTile(num275, num276 - 1, 27, mute: true);
                        }
                        if (Main.tile[num275, num276].HasTile)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public static void PlantingTrees()
        {
            for (int num263 = 0; (double)num263 < (double)Main.maxTilesX * 0.003; num263++)
            {
                int num264 = WorldGen.genRand.Next(50, Main.maxTilesX - 50);
                int num265 = WorldGen.genRand.Next(25, 50);
                for (int num266 = num264 - num265; num266 < num264 + num265; num266++)
                {
                    for (int num267 = 20; (double)num267 < Main.worldSurface; num267++)
                    {
                        WorldGen.GrowEpicTree(num266, num267);
                    }
                }
            }
            WorldGen.AddTrees();
        }

        public static void Weeds()
        {
            for (int num248 = 0; num248 < Main.maxTilesX; num248++)
            {
                for (int num249 = 1; num249 < Main.maxTilesY; num249++)
                {
                    if (Main.tile[num248, num249].TileType == TileID.Grass && Main.tile[num248, num249].HasUnactuatedTile)
                    {
                        if (!Main.tile[num248, num249 - 1].HasTile)
                        {
                            WorldGen.PlaceTile(num248, num249 - 1, 3, mute: true);
                            Main.tile[num248, num249 - 1].CopyPaintAndCoating(Main.tile[num248, num249]);
                        }
                    }
                    else if (Main.tile[num248, num249].TileType == TileID.CorruptGrass && Main.tile[num248, num249].HasUnactuatedTile)
                    {
                        if (!Main.tile[num248, num249 - 1].HasTile)
                        {
                            WorldGen.PlaceTile(num248, num249 - 1, 24, mute: true);
                        }
                    }
                    else if (Main.tile[num248, num249].TileType == TileID.CrimsonGrass && Main.tile[num248, num249].HasUnactuatedTile)
                    {
                        if (!Main.tile[num248, num249 - 1].HasTile)
                        {
                            WorldGen.PlaceTile(num248, num249 - 1, 201, mute: true);
                        }
                    }
                    else if (Main.tile[num248, num249].TileType == TileID.AshGrass && Main.tile[num248, num249].HasUnactuatedTile && !Main.tile[num248, num249 - 1].HasTile)
                    {
                        WorldGen.PlaceTile(num248, num249 - 1, 637, mute: true);
                    }
                }
            }
        }

        public static void Vines()
        {
            for (int num217 = 5; num217 < Main.maxTilesX - 5; num217++)
            {
                int num218 = 0;
                ushort num219 = 52;
                int num220 = (int)Main.worldSurface;
                if (WorldGen.remixWorldGen)
                {
                    num220 = Main.maxTilesY - 200;
                }
                Tile tile13;
                for (int num221 = 0; num221 < num220; num221++)
                {
                    if (num218 > 0)
                    {
                        tile13 = Main.tile[num217, num221];
                        if (!tile13.HasTile)
                        {
                            tile13 = Main.tile[num217, num221];
                            tile13.HasTile = true;
                            tile13 = Main.tile[num217, num221];
                            tile13.TileType = num219;
                            tile13 = Main.tile[num217, num221];
                            tile13.CopyPaintAndCoating(Main.tile[num217, num221 - 1]);
                            num218--;
                            goto IL_00c8;
                        }
                    }
                    num218 = 0;
                    goto IL_00c8;
                IL_00c8:
                    tile13 = Main.tile[num217, num221];
                    if (!tile13.HasTile)
                    {
                        continue;
                    }
                    tile13 = Main.tile[num217, num221];
                    if (tile13.BottomSlope)
                    {
                        continue;
                    }
                    tile13 = Main.tile[num217, num221];
                    if (tile13.TileType != TileID.Grass)
                    {
                        tile13 = Main.tile[num217, num221];
                        if (tile13.TileType != TileID.LeafBlock || !WorldGen.genRand.NextBool(4))
                        {
                            continue;
                        }
                    }
                    if (!WorldGen.GrowMoreVines(num217, num221))
                    {
                        continue;
                    }
                    num219 = 52;
                    tile13 = Main.tile[num217, num221];
                    if (tile13.WallType != WallID.Flower)
                    {
                        tile13 = Main.tile[num217, num221];
                        if (tile13.WallType != WallID.FlowerUnsafe)
                        {
                            tile13 = Main.tile[num217, num221];
                            if (tile13.WallType != WallID.Grass)
                            {
                                tile13 = Main.tile[num217, num221];
                                if (tile13.WallType != WallID.GrassUnsafe)
                                {
                                    tile13 = Main.tile[num217, num221 + 1];
                                    if (tile13.WallType != WallID.Flower)
                                    {
                                        tile13 = Main.tile[num217, num221 + 1];
                                        if (tile13.WallType != WallID.FlowerUnsafe)
                                        {
                                            tile13 = Main.tile[num217, num221 + 1];
                                            if (tile13.WallType != WallID.Grass)
                                            {
                                                tile13 = Main.tile[num217, num221 + 1];
                                                if (tile13.WallType != WallID.GrassUnsafe)
                                                {
                                                    goto IL_0247;
                                                }
                                            }
                                        }
                                    }
                                    num219 = 382;
                                    goto IL_0247;
                                }
                            }
                        }
                    }
                    num219 = 382;
                    goto IL_0247;
                IL_0247:
                    if (WorldGen.remixWorldGen && WorldGen.genRand.NextBool(5))
                    {
                        num219 = 382;
                    }
                    if (WorldGen.genRand.Next(5) < 3)
                    {
                        num218 = WorldGen.genRand.Next(1, 10);
                    }
                }
                num218 = 0;
                for (int num222 = 5; num222 < Main.maxTilesY - 5; num222++)
                {
                    if (num218 > 0)
                    {
                        tile13 = Main.tile[num217, num222];
                        if (!tile13.HasTile)
                        {
                            tile13 = Main.tile[num217, num222];
                            tile13.HasTile = true;
                            tile13 = Main.tile[num217, num222];
                            tile13.TileType = TileID.JungleVines;
                            num218--;
                            goto IL_02e9;
                        }
                    }
                    num218 = 0;
                    goto IL_02e9;
                IL_02e9:
                    tile13 = Main.tile[num217, num222];
                    if (!tile13.HasTile)
                    {
                        continue;
                    }
                    tile13 = Main.tile[num217, num222];
                    if (tile13.TileType != TileID.JungleGrass)
                    {
                        continue;
                    }
                    tile13 = Main.tile[num217, num222];
                    if (tile13.BottomSlope || !WorldGen.GrowMoreVines(num217, num222))
                    {
                        continue;
                    }
                    if (WorldGen.notTheBees && num222 < Main.maxTilesY - 10)
                    {
                        tile13 = Main.tile[num217, num222 - 1];
                        if (tile13.HasTile)
                        {
                            tile13 = Main.tile[num217, num222 - 1];
                            if (!tile13.BottomSlope)
                            {
                                tile13 = Main.tile[num217 + 1, num222 - 1];
                                if (tile13.HasTile)
                                {
                                    tile13 = Main.tile[num217 + 1, num222 - 1];
                                    if (!tile13.BottomSlope)
                                    {
                                        tile13 = Main.tile[num217, num222 - 1];
                                        if (tile13.TileType != TileID.JungleGrass)
                                        {
                                            tile13 = Main.tile[num217, num222 - 1];
                                            if (tile13.TileType != TileID.BeeHive)
                                            {
                                                tile13 = Main.tile[num217, num222 - 1];
                                                if (tile13.TileType != TileID.CrispyHoneyBlock)
                                                {
                                                    goto IL_062a;
                                                }
                                            }
                                        }
                                        bool flag5 = true;
                                        for (int num223 = num217; num223 < num217 + 2; num223++)
                                        {
                                            int num224 = num222 + 1;
                                            while (num224 < num222 + 3)
                                            {
                                                tile13 = Main.tile[num223, num224];
                                                if (tile13.HasTile)
                                                {
                                                    bool[] tileCut = Main.tileCut;
                                                    tile13 = Main.tile[num223, num224];
                                                    if (tileCut[tile13.TileType])
                                                    {
                                                        tile13 = Main.tile[num223, num224];
                                                        if (tile13.TileType != TileID.BeeHive)
                                                        {
                                                            goto IL_04ae;
                                                        }
                                                    }
                                                    flag5 = false;
                                                    break;
                                                }
                                                goto IL_04ae;
                                            IL_04ae:
                                                tile13 = Main.tile[num223, num224];
                                                if (tile13.LiquidAmount <= 0)
                                                {
                                                    bool[] wallHouse = Main.wallHouse;
                                                    tile13 = Main.tile[num223, num224];
                                                    if (!wallHouse[tile13.WallType])
                                                    {
                                                        num224++;
                                                        continue;
                                                    }
                                                }
                                                flag5 = false;
                                                break;
                                            }
                                            if (!flag5)
                                            {
                                                break;
                                            }
                                        }
                                        if (flag5 && WorldGen.CountNearBlocksTypes(num217, num222, WorldGen.genRand.Next(3, 10), 1, 444) > 0)
                                        {
                                            flag5 = false;
                                        }
                                        if (flag5)
                                        {
                                            for (int num225 = num217; num225 < num217 + 2; num225++)
                                            {
                                                for (int num226 = num222 + 1; num226 < num222 + 3; num226++)
                                                {
                                                    WorldGen.KillTile(num225, num226);
                                                }
                                            }
                                            for (int num227 = num217; num227 < num217 + 2; num227++)
                                            {
                                                for (int num228 = num222 + 1; num228 < num222 + 3; num228++)
                                                {
                                                    tile13 = Main.tile[num227, num228];
                                                    tile13.HasTile = true;
                                                    tile13 = Main.tile[num227, num228];
                                                    tile13.TileType = TileID.BeeHive;
                                                    tile13 = Main.tile[num227, num228];
                                                    tile13.TileFrameX = (short)((num227 - num217) * 18);
                                                    tile13 = Main.tile[num227, num228];
                                                    tile13.TileFrameY = (short)((num228 - num222 - 1) * 18);
                                                }
                                            }
                                            continue;
                                        }
                                        goto IL_0891;
                                    }
                                }
                            }
                        }
                    }
                    goto IL_062a;
                IL_062a:
                    if (num217 < Main.maxTilesX - 1 && num222 < Main.maxTilesY - 2)
                    {
                        tile13 = Main.tile[num217 + 1, num222];
                        if (tile13.HasTile)
                        {
                            tile13 = Main.tile[num217 + 1, num222];
                            if (tile13.TileType == TileID.JungleGrass)
                            {
                                tile13 = Main.tile[num217 + 1, num222];
                                if (!tile13.BottomSlope && WorldGen.genRand.NextBool(40))
                                {
                                    bool flag6 = true;
                                    for (int num229 = num217; num229 < num217 + 2; num229++)
                                    {
                                        int num230 = num222 + 1;
                                        while (num230 < num222 + 3)
                                        {
                                            tile13 = Main.tile[num229, num230];
                                            if (tile13.HasTile)
                                            {
                                                bool[] tileCut2 = Main.tileCut;
                                                tile13 = Main.tile[num229, num230];
                                                if (tileCut2[tile13.TileType])
                                                {
                                                    tile13 = Main.tile[num229, num230];
                                                    if (tile13.TileType != TileID.BeeHive)
                                                    {
                                                        goto IL_0723;
                                                    }
                                                }
                                                flag6 = false;
                                                break;
                                            }
                                            goto IL_0723;
                                        IL_0723:
                                            tile13 = Main.tile[num229, num230];
                                            if (tile13.LiquidAmount <= 0)
                                            {
                                                bool[] wallHouse2 = Main.wallHouse;
                                                tile13 = Main.tile[num229, num230];
                                                if (!wallHouse2[tile13.WallType])
                                                {
                                                    num230++;
                                                    continue;
                                                }
                                            }
                                            flag6 = false;
                                            break;
                                        }
                                        if (!flag6)
                                        {
                                            break;
                                        }
                                    }
                                    if (flag6 && WorldGen.CountNearBlocksTypes(num217, num222, 20, 1, 444) > 0)
                                    {
                                        flag6 = false;
                                    }
                                    if (flag6)
                                    {
                                        for (int num231 = num217; num231 < num217 + 2; num231++)
                                        {
                                            for (int num232 = num222 + 1; num232 < num222 + 3; num232++)
                                            {
                                                WorldGen.KillTile(num231, num232);
                                            }
                                        }
                                        for (int num233 = num217; num233 < num217 + 2; num233++)
                                        {
                                            for (int num234 = num222 + 1; num234 < num222 + 3; num234++)
                                            {
                                                tile13 = Main.tile[num233, num234];
                                                tile13.HasTile = true;
                                                tile13 = Main.tile[num233, num234];
                                                tile13.TileType = TileID.BeeHive;
                                                tile13 = Main.tile[num233, num234];
                                                tile13.TileFrameX = (short)((num233 - num217) * 18);
                                                tile13 = Main.tile[num233, num234];
                                                tile13.TileFrameY = (short)((num234 - num222 - 1) * 18);
                                            }
                                        }
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    goto IL_0891;
                IL_0891:
                    if (WorldGen.genRand.Next(5) < 3)
                    {
                        num218 = WorldGen.genRand.Next(1, 10);
                    }
                }
                num218 = 0;
                for (int num235 = 0; num235 < Main.maxTilesY; num235++)
                {
                    if (num218 > 0)
                    {
                        tile13 = Main.tile[num217, num235];
                        if (!tile13.HasTile)
                        {
                            tile13 = Main.tile[num217, num235];
                            tile13.HasTile = true;
                            tile13 = Main.tile[num217, num235];
                            tile13.TileType = TileID.MushroomVines;
                            num218--;
                            goto IL_0922;
                        }
                    }
                    num218 = 0;
                    goto IL_0922;
                IL_0922:
                    tile13 = Main.tile[num217, num235];
                    if (tile13.HasTile)
                    {
                        tile13 = Main.tile[num217, num235];
                        if (tile13.TileType == TileID.MushroomGrass && WorldGen.genRand.NextBool(5))
                        {
                            tile13 = Main.tile[num217, num235];
                            if (!tile13.BottomSlope && WorldGen.GrowMoreVines(num217, num235) && WorldGen.genRand.Next(5) < 3)
                            {
                                num218 = WorldGen.genRand.Next(1, 10);
                            }
                        }
                    }
                }
                num218 = 0;
                for (int num236 = 0; num236 < Main.maxTilesY; num236++)
                {
                    if (num218 > 0)
                    {
                        tile13 = Main.tile[num217, num236];
                        if (!tile13.HasTile)
                        {
                            tile13 = Main.tile[num217, num236];
                            tile13.HasTile = true;
                            tile13 = Main.tile[num217, num236];
                            tile13.TileType = TileID.CorruptVines;
                            num218--;
                            goto IL_0a13;
                        }
                    }
                    num218 = 0;
                    goto IL_0a13;
                IL_0a13:
                    tile13 = Main.tile[num217, num236];
                    if (tile13.HasTile)
                    {
                        tile13 = Main.tile[num217, num236];
                        if (!tile13.BottomSlope)
                        {
                            tile13 = Main.tile[num217, num236];
                            if (tile13.TileType == TileID.CorruptGrass && WorldGen.GrowMoreVines(num217, num236) && WorldGen.genRand.Next(5) < 3)
                            {
                                num218 = WorldGen.genRand.Next(1, 10);
                            }
                        }
                    }
                }
                num218 = 0;
                for (int num237 = 0; num237 < Main.maxTilesY; num237++)
                {
                    if (num218 > 0)
                    {
                        tile13 = Main.tile[num217, num237];
                        if (!tile13.HasTile)
                        {
                            tile13 = Main.tile[num217, num237];
                            tile13.HasTile = true;
                            tile13 = Main.tile[num217, num237];
                            tile13.TileType = TileID.CrimsonVines;
                            num218--;
                            goto IL_0af7;
                        }
                    }
                    num218 = 0;
                    goto IL_0af7;
                IL_0af7:
                    tile13 = Main.tile[num217, num237];
                    if (tile13.HasTile)
                    {
                        tile13 = Main.tile[num217, num237];
                        if (!tile13.BottomSlope)
                        {
                            tile13 = Main.tile[num217, num237];
                            if (tile13.TileType == TileID.CrimsonGrass && WorldGen.GrowMoreVines(num217, num237) && WorldGen.genRand.Next(5) < 3)
                            {
                                num218 = WorldGen.genRand.Next(1, 10);
                            }
                        }
                    }
                }
                num218 = 0;
                for (int num238 = 0; num238 < Main.maxTilesY; num238++)
                {
                    if (num218 > 0)
                    {
                        tile13 = Main.tile[num217, num238];
                        if (!tile13.HasTile)
                        {
                            tile13 = Main.tile[num217, num238];
                            tile13.HasTile = true;
                            tile13 = Main.tile[num217, num238];
                            tile13.TileType = TileID.AshVines;
                            num218--;
                            goto IL_0bde;
                        }
                    }
                    num218 = 0;
                    goto IL_0bde;
                IL_0bde:
                    tile13 = Main.tile[num217, num238];
                    if (tile13.HasTile)
                    {
                        tile13 = Main.tile[num217, num238];
                        if (!tile13.BottomSlope)
                        {
                            tile13 = Main.tile[num217, num238];
                            if (tile13.TileType == TileID.AshGrass && WorldGen.GrowMoreVines(num217, num238) && WorldGen.genRand.Next(5) < 3)
                            {
                                num218 = WorldGen.genRand.Next(1, 10);
                            }
                        }
                    }
                }
            }
        }

        public static void Flowers()
        {
            int num204 = (int)((double)Main.maxTilesX * 0.004);
            for (int num205 = 0; num205 < num204; num205++)
            {
                int num206 = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                int num207 = WorldGen.genRand.Next(15, 30);
                int num208 = WorldGen.genRand.Next(15, 30);
                Tile tile12;
                if (WorldGen.remixWorldGen)
                {
                    num207 = WorldGen.genRand.Next(15, 45);
                    num208 = WorldGen.genRand.Next(15, 45);
                    int num209 = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 350);
                    if (GenVars.logX >= 0)
                    {
                        num206 = GenVars.logX;
                        num209 = GenVars.logY;
                        GenVars.logX = -1;
                    }
                    int num210 = WorldGen.genRand.NextFromList(21, 24, 27, 30, 33, 36, 39, 42);
                    for (int num211 = num206 - num207; num211 < num206 + num207; num211++)
                    {
                        for (int num212 = num209 - num208; num212 < num209 + num208; num212++)
                        {
                            tile12 = Main.tile[num211, num212];
                            if (tile12.TileType == TileID.FallenLog)
                            {
                                continue;
                            }
                            bool[] tileSolid9 = Main.tileSolid;
                            tile12 = Main.tile[num211, num212];
                            if (tileSolid9[tile12.TileType])
                            {
                                continue;
                            }
                            tile12 = Main.tile[num211, num212];
                            if (tile12.TileType == TileID.Plants)
                            {
                                tile12 = Main.tile[num211, num212];
                                tile12.TileFrameX = (short)((num210 + WorldGen.genRand.Next(3)) * 18);
                                if (!WorldGen.genRand.NextBool(3))
                                {
                                    tile12 = Main.tile[num211, num212];
                                    tile12.TileType = TileID.Plants2;
                                }
                                continue;
                            }
                            tile12 = Main.tile[num211, num212 + 1];
                            if (tile12.WallType != WallID.None)
                            {
                                continue;
                            }
                            tile12 = Main.tile[num211, num212 + 1];
                            if (tile12.TileType != TileID.Grass)
                            {
                                tile12 = Main.tile[num211, num212 + 1];
                                if (tile12.TileType != TileID.ClayBlock)
                                {
                                    tile12 = Main.tile[num211, num212 + 1];
                                    if (tile12.TileType != TileID.Stone)
                                    {
                                        bool[] ore = TileID.Sets.Ore;
                                        tile12 = Main.tile[num211, num212 + 1];
                                        if (!ore[tile12.TileType])
                                        {
                                            continue;
                                        }
                                    }
                                }
                                tile12 = Main.tile[num211, num212];
                                if (tile12.HasTile)
                                {
                                    continue;
                                }
                            }
                            tile12 = Main.tile[num211, num212];
                            if (tile12.HasTile)
                            {
                                tile12 = Main.tile[num211, num212];
                                if (tile12.TileType != TileID.SmallPiles)
                                {
                                    tile12 = Main.tile[num211, num212];
                                    if (tile12.TileType != TileID.LargePiles)
                                    {
                                        tile12 = Main.tile[num211, num212];
                                        if (tile12.TileType != TileID.LargePiles2)
                                        {
                                            tile12 = Main.tile[num211, num212];
                                            if ((tile12.TileType != TileID.Trees || !((double)num211 < (double)Main.maxTilesX * 0.48)) && !((double)num211 > (double)Main.maxTilesX * 0.52))
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                            tile12 = Main.tile[num211, num212 + 1];
                            if (tile12.TileType != TileID.ClayBlock)
                            {
                                tile12 = Main.tile[num211, num212 + 1];
                                if (tile12.TileType != TileID.Stone)
                                {
                                    bool[] ore2 = TileID.Sets.Ore;
                                    tile12 = Main.tile[num211, num212 + 1];
                                    if (!ore2[tile12.TileType])
                                    {
                                        goto IL_0432;
                                    }
                                }
                            }
                            tile12 = Main.tile[num211, num212 + 1];
                            tile12.TileType = TileID.Grass;
                            tile12 = Main.tile[num211, num212 + 2];
                            if (tile12.TileType != TileID.ClayBlock)
                            {
                                tile12 = Main.tile[num211, num212 + 2];
                                if (tile12.TileType != TileID.Stone)
                                {
                                    bool[] ore3 = TileID.Sets.Ore;
                                    tile12 = Main.tile[num211, num212 + 2];
                                    if (!ore3[tile12.TileType])
                                    {
                                        goto IL_0432;
                                    }
                                }
                            }
                            tile12 = Main.tile[num211, num212 + 2];
                            tile12.TileType = TileID.Grass;
                            goto IL_0432;
                        IL_0432:
                            WorldGen.KillTile(num211, num212);
                            if (WorldGen.genRand.NextBool(2))
                            {
                                tile12 = Main.tile[num211, num212 + 1];
                                tile12.Slope = 0;
                                tile12 = Main.tile[num211, num212 + 1];
                                tile12.IsHalfBlock = false;
                            }
                            WorldGen.PlaceTile(num211, num212, 3);
                            tile12 = Main.tile[num211, num212];
                            if (tile12.HasTile)
                            {
                                tile12 = Main.tile[num211, num212];
                                if (tile12.TileType == TileID.Plants)
                                {
                                    tile12 = Main.tile[num211, num212];
                                    tile12.TileFrameX = (short)((num210 + WorldGen.genRand.Next(3)) * 18);
                                    if (!WorldGen.genRand.NextBool(3))
                                    {
                                        tile12 = Main.tile[num211, num212];
                                        tile12.TileType = TileID.Plants2;
                                    }
                                }
                            }
                            tile12 = Main.tile[num211, num212 + 2];
                            if (tile12.TileType != TileID.ClayBlock)
                            {
                                tile12 = Main.tile[num211, num212 + 2];
                                if (tile12.TileType != TileID.Stone)
                                {
                                    bool[] ore4 = TileID.Sets.Ore;
                                    tile12 = Main.tile[num211, num212 + 2];
                                    if (!ore4[tile12.TileType])
                                    {
                                        continue;
                                    }
                                }
                            }
                            tile12 = Main.tile[num211, num212 + 2];
                            tile12.TileType = TileID.Dirt;
                        }
                    }
                }
                else
                {
                    for (int num213 = num208; (double)num213 < Main.worldSurface - (double)num208 - 1.0; num213++)
                    {
                        tile12 = Main.tile[num206, num213];
                        if (tile12.HasTile)
                        {
                            if (GenVars.logX >= 0)
                            {
                                num206 = GenVars.logX;
                                num213 = GenVars.logY;
                                GenVars.logX = -1;
                            }
                            int num214 = WorldGen.genRand.NextFromList(21, 24, 27, 30, 33, 36, 39, 42);
                            for (int num215 = num206 - num207; num215 < num206 + num207; num215++)
                            {
                                for (int num216 = num213 - num208; num216 < num213 + num208; num216++)
                                {
                                    tile12 = Main.tile[num215, num216];
                                    if (tile12.TileType == TileID.FallenLog)
                                    {
                                        continue;
                                    }
                                    bool[] tileSolid10 = Main.tileSolid;
                                    tile12 = Main.tile[num215, num216];
                                    if (tileSolid10[tile12.TileType])
                                    {
                                        continue;
                                    }
                                    tile12 = Main.tile[num215, num216];
                                    if (tile12.TileType == TileID.Plants)
                                    {
                                        tile12 = Main.tile[num215, num216];
                                        tile12.TileFrameX = (short)((num214 + WorldGen.genRand.Next(3)) * 18);
                                        if (!WorldGen.genRand.NextBool(3))
                                        {
                                            tile12 = Main.tile[num215, num216];
                                            tile12.TileType = TileID.Plants2;
                                        }
                                        continue;
                                    }
                                    tile12 = Main.tile[num215, num216 + 1];
                                    if (tile12.WallType != WallID.None)
                                    {
                                        continue;
                                    }
                                    tile12 = Main.tile[num215, num216 + 1];
                                    if (tile12.TileType != TileID.Grass)
                                    {
                                        tile12 = Main.tile[num215, num216 + 1];
                                        if (tile12.TileType != TileID.ClayBlock)
                                        {
                                            tile12 = Main.tile[num215, num216 + 1];
                                            if (tile12.TileType != TileID.Stone)
                                            {
                                                bool[] ore5 = TileID.Sets.Ore;
                                                tile12 = Main.tile[num215, num216 + 1];
                                                if (!ore5[tile12.TileType])
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                        tile12 = Main.tile[num215, num216];
                                        if (tile12.HasTile)
                                        {
                                            continue;
                                        }
                                    }
                                    tile12 = Main.tile[num215, num216];
                                    if (tile12.HasTile)
                                    {
                                        tile12 = Main.tile[num215, num216];
                                        if (tile12.TileType != TileID.SmallPiles)
                                        {
                                            tile12 = Main.tile[num215, num216];
                                            if (tile12.TileType != TileID.LargePiles)
                                            {
                                                tile12 = Main.tile[num215, num216];
                                                if (tile12.TileType != TileID.LargePiles2)
                                                {
                                                    tile12 = Main.tile[num215, num216];
                                                    if ((tile12.TileType != TileID.Trees || !((double)num215 < (double)Main.maxTilesX * 0.48)) && !((double)num215 > (double)Main.maxTilesX * 0.52))
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    tile12 = Main.tile[num215, num216 + 1];
                                    if (tile12.TileType != TileID.ClayBlock)
                                    {
                                        tile12 = Main.tile[num215, num216 + 1];
                                        if (tile12.TileType != TileID.Stone)
                                        {
                                            bool[] ore6 = TileID.Sets.Ore;
                                            tile12 = Main.tile[num215, num216 + 1];
                                            if (!ore6[tile12.TileType])
                                            {
                                                goto IL_094c;
                                            }
                                        }
                                    }
                                    tile12 = Main.tile[num215, num216 + 1];
                                    tile12.TileType = TileID.Grass;
                                    tile12 = Main.tile[num215, num216 + 2];
                                    if (tile12.TileType != TileID.ClayBlock)
                                    {
                                        tile12 = Main.tile[num215, num216 + 2];
                                        if (tile12.TileType != TileID.Stone)
                                        {
                                            bool[] ore7 = TileID.Sets.Ore;
                                            tile12 = Main.tile[num215, num216 + 2];
                                            if (!ore7[tile12.TileType])
                                            {
                                                goto IL_094c;
                                            }
                                        }
                                    }
                                    tile12 = Main.tile[num215, num216 + 2];
                                    tile12.TileType = TileID.Grass;
                                    goto IL_094c;
                                IL_094c:
                                    WorldGen.KillTile(num215, num216);
                                    if (WorldGen.genRand.NextBool(2))
                                    {
                                        tile12 = Main.tile[num215, num216 + 1];
                                        tile12.Slope = 0;
                                        tile12 = Main.tile[num215, num216 + 1];
                                        tile12.IsHalfBlock = false;
                                    }
                                    WorldGen.PlaceTile(num215, num216, 3);
                                    tile12 = Main.tile[num215, num216];
                                    if (tile12.HasTile)
                                    {
                                        tile12 = Main.tile[num215, num216];
                                        if (tile12.TileType == TileID.Plants)
                                        {
                                            tile12 = Main.tile[num215, num216];
                                            tile12.TileFrameX = (short)((num214 + WorldGen.genRand.Next(3)) * 18);
                                            if (!WorldGen.genRand.NextBool(3))
                                            {
                                                tile12 = Main.tile[num215, num216];
                                                tile12.TileType = TileID.Plants2;
                                            }
                                        }
                                    }
                                    tile12 = Main.tile[num215, num216 + 2];
                                    if (tile12.TileType != TileID.ClayBlock)
                                    {
                                        tile12 = Main.tile[num215, num216 + 2];
                                        if (tile12.TileType != TileID.Stone)
                                        {
                                            bool[] ore8 = TileID.Sets.Ore;
                                            tile12 = Main.tile[num215, num216 + 2];
                                            if (!ore8[tile12.TileType])
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    tile12 = Main.tile[num215, num216 + 2];
                                    tile12.TileType = TileID.Dirt;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        public static void Mushrooms()
        {
            int num193 = (int)((double)Main.maxTilesX * 0.002);
            for (int num194 = 0; num194 < num193; num194++)
            {
                int num195 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
                int num196 = WorldGen.genRand.Next(4, 10);
                int num197 = WorldGen.genRand.Next(15, 30);
                Tile tile11;
                for (int num201 = 1; (double)num201 < Main.worldSurface - 1.0; num201++)
                {
                    tile11 = Main.tile[num195, num201];
                    if (tile11.HasTile)
                    {
                        for (int num202 = num195 - num196; num202 < num195 + num196; num202++)
                        {
                            for (int num203 = num201 - num197; num203 < num201 + num197 && num202 >= 10 && num203 >= 0 && num202 <= Main.maxTilesX - 10 && num203 <= Main.maxTilesY - 10; num203++)
                            {
                                tile11 = Main.tile[num202, num203];
                                if (tile11.TileType != TileID.Plants)
                                {
                                    tile11 = Main.tile[num202, num203];
                                    if (tile11.TileType != TileID.CorruptPlants)
                                    {
                                        tile11 = Main.tile[num202, num203];
                                        if (tile11.TileType == TileID.CrimsonPlants)
                                        {
                                            tile11 = Main.tile[num202, num203];
                                            tile11.TileFrameX = 270;
                                        }
                                        continue;
                                    }
                                }
                                tile11 = Main.tile[num202, num203];
                                tile11.TileFrameX = 144;
                            }
                        }
                        break;
                    }
                }
            }
        }

        public static void Stalac()
        {
            for (int num19 = 20; num19 < Main.maxTilesX - 20; num19++)
            {
                for (int num20 = (int)Main.worldSurface; num20 < Main.maxTilesY - 20; num20++)
                {
                    if ((Main.tenthAnniversaryWorld || WorldGen.drunkWorldGen || WorldGen.genRand.NextBool(5)) && Main.tile[num19, num20 - 1].LiquidAmount == 0)
                    {
                        int num21 = WorldGen.genRand.Next(7);
                        int treeTileType = 0;
                        switch (num21)
                        {
                            case 0:
                                treeTileType = 583;
                                break;
                            case 1:
                                treeTileType = 584;
                                break;
                            case 2:
                                treeTileType = 585;
                                break;
                            case 3:
                                treeTileType = 586;
                                break;
                            case 4:
                                treeTileType = 587;
                                break;
                            case 5:
                                treeTileType = 588;
                                break;
                            case 6:
                                treeTileType = 589;
                                break;
                        }
                        WorldGen.TryGrowingTreeByType(treeTileType, num19, num20);
                    }
                    if (!WorldGen.oceanDepths(num19, num20) && !Main.tile[num19, num20].HasTile && WorldGen.genRand.NextBool(5))
                    {
                        if ((Main.tile[num19, num20 - 1].TileType == TileID.Stone || Main.tile[num19, num20 - 1].TileType == TileID.SnowBlock || Main.tile[num19, num20 - 1].TileType == TileID.IceBlock || Main.tile[num19, num20 - 1].TileType == TileID.Ebonstone || Main.tile[num19, num20 - 1].TileType == TileID.Crimstone || Main.tileStone[Main.tile[num19, num20 - 1].TileType] || Main.tileMoss[Main.tile[num19, num20 - 1].TileType]) && !Main.tile[num19, num20].HasTile && !Main.tile[num19, num20 + 1].HasTile)
                        {
                            //Main.tile[num19, num20 - 1].Slope = 0;
                            WorldGen.SlopeTile(num19, num20 - 1, 0);
                        }
                        if ((Main.tile[num19, num20 + 1].TileType == TileID.Stone || Main.tile[num19, num20 + 1].TileType == TileID.SnowBlock || Main.tile[num19, num20 + 1].TileType == TileID.IceBlock || Main.tile[num19, num20 + 1].TileType == TileID.Ebonstone || Main.tile[num19, num20 + 1].TileType == TileID.Crimstone || Main.tileStone[Main.tile[num19, num20 + 1].TileType] || Main.tileMoss[Main.tile[num19, num20 + 1].TileType]) && !Main.tile[num19, num20].HasTile && !Main.tile[num19, num20 - 1].HasTile)
                        {
                            //Main.tile[num19, num20 + 1].Slope = 0;
                            WorldGen.SlopeTile(num19, num20 + 1, 0);
                        }
                        WorldGen.PlaceTight(num19, num20);
                    }
                }
                for (int num22 = 5; num22 < (int)Main.worldSurface; num22++)
                {
                    if ((Main.tile[num19, num22 - 1].TileType == TileID.SnowBlock || Main.tile[num19, num22 - 1].TileType == TileID.IceBlock) && WorldGen.genRand.NextBool(5))
                    {
                        if (!Main.tile[num19, num22].HasTile && !Main.tile[num19, num22 + 1].HasTile)
                        {
                            //Main.tile[num19, num22 - 1].Slope = 0;
                            WorldGen.SlopeTile(num19, num22 - 1, 0);
                        }
                        WorldGen.PlaceTight(num19, num22);
                    }
                    if ((Main.tile[num19, num22 - 1].TileType == TileID.Ebonstone || Main.tile[num19, num22 - 1].TileType == TileID.Crimstone) && WorldGen.genRand.NextBool(5))
                    {
                        if (!Main.tile[num19, num22].HasTile && !Main.tile[num19, num22 + 1].HasTile)
                        {
                            //Main.tile[num19, num22 - 1].Slope = 0;
                            WorldGen.SlopeTile(num19, num22 - 1, 0);
                        }
                        WorldGen.PlaceTight(num19, num22);
                    }
                    if ((Main.tile[num19, num22 + 1].TileType == TileID.Ebonstone || Main.tile[num19, num22 + 1].TileType == TileID.Crimstone) && WorldGen.genRand.NextBool(5))
                    {
                        if (!Main.tile[num19, num22].HasTile && !Main.tile[num19, num22 - 1].HasTile)
                        {
                            //Main.tile[num19, num22 + 1].Slope = 0;
                            WorldGen.SlopeTile(num19, num22 + 1, 0);
                        }
                        WorldGen.PlaceTight(num19, num22);
                    }
                }
            }
        }

        public static void SettleLiquidsAgain()
        {
            Liquid.worldGenTilesIgnoreWater(ignoreSolids: true);
            Liquid.QuickWater(3);
            WorldGen.WaterCheck();
            int num144 = 0;
            Liquid.quickSettle = true;
            int num145 = 10;
            while (num144 < num145)
            {
                int num146 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                num144++;
                double num147 = 0.0;
                int num148 = num146 * 5;
                while (Liquid.numLiquid > 0)
                {
                    num148--;
                    if (num148 < 0)
                    {
                        break;
                    }
                    double num149 = (double)(num146 - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (double)num146;
                    if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num146)
                    {
                        num146 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                    }
                    if (num149 > num147)
                    {
                        num147 = num149;
                    }
                    else
                    {
                        num149 = num147;
                    }
                    Liquid.UpdateLiquid();
                }
                WorldGen.WaterCheck();
            }
            Liquid.quickSettle = false;
            Liquid.worldGenTilesIgnoreWater(ignoreSolids: false);
            Main.tileSolid[484] = false;
        }

        public static void TileCleanup()
        {
            for (int num57 = 40; num57 < Main.maxTilesX - 40; num57++)
            {
                int num58 = 40;
                while (num58 < Main.maxTilesY - 40)
                {
                    Tile tile5 = Main.tile[num57, num58];
                    if (tile5.HasTile)
                    {
                        tile5 = Main.tile[num57, num58];
                        if (tile5.TopSlope)
                        {
                            tile5 = Main.tile[num57, num58];
                            if (tile5.LeftSlope)
                            {
                                tile5 = Main.tile[num57 + 1, num58];
                                if (tile5.IsHalfBlock)
                                {
                                    goto IL_00c6;
                                }
                            }
                            tile5 = Main.tile[num57, num58];
                            if (tile5.RightSlope)
                            {
                                tile5 = Main.tile[num57 - 1, num58];
                                if (tile5.IsHalfBlock)
                                {
                                    goto IL_00c6;
                                }
                            }
                        }
                    }
                    goto IL_00f0;
                IL_00f0:
                    tile5 = Main.tile[num57, num58];
                    if (tile5.HasTile)
                    {
                        tile5 = Main.tile[num57, num58];
                        if (tile5.LiquidAmount > 0)
                        {
                            bool[] slowlyDiesInWater = TileID.Sets.SlowlyDiesInWater;
                            tile5 = Main.tile[num57, num58];
                            if (slowlyDiesInWater[tile5.TileType])
                            {
                                WorldGen.KillTile(num57, num58);
                            }
                        }
                    }
                    tile5 = Main.tile[num57, num58];
                    if (!tile5.HasTile)
                    {
                        tile5 = Main.tile[num57, num58];
                        if (tile5.LiquidAmount == 0 && !WorldGen.genRand.NextBool(3) && WorldGen.SolidTile(num57, num58 - 1))
                        {
                            int num59 = WorldGen.genRand.Next(15, 21);
                            for (int num60 = num58 - 2; num60 >= num58 - num59; num60--)
                            {
                                tile5 = Main.tile[num57, num60];
                                if (tile5.LiquidAmount >= 128)
                                {
                                    tile5 = Main.tile[num57, num60];
                                    if (tile5.LiquidType != LiquidID.Shimmer)
                                    {
                                        int num61 = 373;
                                        tile5 = Main.tile[num57, num60];
                                        if ((tile5.LiquidType == LiquidID.Lava))
                                        {
                                            num61 = 374;
                                        }
                                        else
                                        {
                                            tile5 = Main.tile[num57, num60];
                                            if ((tile5.LiquidType == LiquidID.Honey))
                                            {
                                                num61 = 375;
                                            }
                                        }
                                        int maxValue3 = num58 - num60;
                                        if (WorldGen.genRand.Next(maxValue3) <= 1)
                                        {
                                            tile5 = Main.tile[num57, num58];
                                            if (tile5.WallType == WallID.HiveUnsafe)
                                            {
                                                num61 = 375;
                                            }
                                            tile5 = Main.tile[num57, num58];
                                            tile5.TileType = (ushort)num61;
                                            tile5 = Main.tile[num57, num58];
                                            tile5.TileFrameX = 0;
                                            tile5 = Main.tile[num57, num58];
                                            tile5.TileFrameY = 0;
                                            tile5 = Main.tile[num57, num58];
                                            tile5.HasTile = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            tile5 = Main.tile[num57, num58];
                            if (!tile5.HasTile)
                            {
                                num59 = WorldGen.genRand.Next(3, 11);
                                for (int num62 = num58 + 1; num62 <= num58 + num59; num62++)
                                {
                                    tile5 = Main.tile[num57, num62];
                                    if (tile5.LiquidAmount >= 200)
                                    {
                                        tile5 = Main.tile[num57, num62];
                                        if (tile5.LiquidType != LiquidID.Shimmer)
                                        {
                                            int num63 = 373;
                                            tile5 = Main.tile[num57, num62];
                                            if ((tile5.LiquidType == LiquidID.Lava))
                                            {
                                                num63 = 374;
                                            }
                                            else
                                            {
                                                tile5 = Main.tile[num57, num62];
                                                if ((tile5.LiquidType == LiquidID.Honey))
                                                {
                                                    num63 = 375;
                                                }
                                            }
                                            int num64 = num62 - num58;
                                            if (WorldGen.genRand.Next(num64 * 3) <= 1)
                                            {
                                                tile5 = Main.tile[num57, num58];
                                                tile5.TileType = (ushort)num63;
                                                tile5 = Main.tile[num57, num58];
                                                tile5.TileFrameX = 0;
                                                tile5 = Main.tile[num57, num58];
                                                tile5.TileFrameY = 0;
                                                tile5 = Main.tile[num57, num58];
                                                tile5.HasTile = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            tile5 = Main.tile[num57, num58];
                            if (!tile5.HasTile && WorldGen.genRand.NextBool(4))
                            {
                                Tile tile6 = Main.tile[num57, num58 - 1];
                                if (TileID.Sets.Conversion.Sandstone[tile6.TileType] || TileID.Sets.Conversion.HardenedSand[tile6.TileType])
                                {
                                    tile5 = Main.tile[num57, num58];
                                    tile5.TileType = TileID.SandDrip;
                                    tile5 = Main.tile[num57, num58];
                                    tile5.TileFrameX = 0;
                                    tile5 = Main.tile[num57, num58];
                                    tile5.TileFrameY = 0;
                                    tile5 = Main.tile[num57, num58];
                                    tile5.HasTile = true;
                                }
                            }
                        }
                    }
                    tile5 = Main.tile[num57, num58];
                    if (tile5.TileType == TileID.Traps)
                    {
                        tile5 = Main.tile[num57, num58];
                        int num65 = tile5.TileFrameY / 18;
                        if (num65 <= 2 || num65 == 5)
                        {
                            int num66 = -1;
                            tile5 = Main.tile[num57, num58];
                            if (tile5.TileFrameX >= 18)
                            {
                                num66 = 1;
                            }
                            tile5 = Main.tile[num57 + num66, num58];
                            if (!tile5.IsHalfBlock)
                            {
                                tile5 = Main.tile[num57 + num66, num58];
                                if (tile5.Slope == 0)
                                {
                                    goto IL_05b9;
                                }
                            }
                            tile5 = Main.tile[num57 + num66, num58];
                            tile5.HasTile = false;
                        }
                    }
                    else
                    {
                        tile5 = Main.tile[num57, num58];
                        if (tile5.TileType == TileID.BreakableIce)
                        {
                            tile5 = Main.tile[num57, num58 + 1];
                            if (tile5.LiquidAmount == 0 && WorldGen.CanKillTile(num57, num58))
                            {
                                tile5 = Main.tile[num57, num58];
                                tile5.HasTile = false;
                            }
                        }
                    }
                    goto IL_05b9;
                IL_0601:
                    tile5 = Main.tile[num57, num58];
                    if (tile5.TileType == TileID.ShadowOrbs)
                    {
                        tile5 = Main.tile[num57, num58];
                        int num67 = tile5.TileFrameX / 18;
                        int num68 = 0;
                        int num69 = num57;
                        num68 += num67 / 2;
                        int num70;
                        if (!WorldGen.drunkWorldGen)
                        {
                            num70 = (WorldGen.crimson ? 1 : 0);
                        }
                        else
                        {
                            tile5 = Main.tile[num57, num58];
                            num70 = ((tile5.WallType == WallID.CrimstoneUnsafe) ? 1 : 0);
                        }
                        num68 = num70;
                        num67 %= 2;
                        num69 -= num67;
                        tile5 = Main.tile[num57, num58];
                        int num71 = tile5.TileFrameY / 18;
                        int num72 = 0;
                        int num73 = num58;
                        num72 += num71 / 2;
                        num71 %= 2;
                        num73 -= num71;
                        for (int num74 = 0; num74 < 2; num74++)
                        {
                            for (int num75 = 0; num75 < 2; num75++)
                            {
                                int x6 = num69 + num74;
                                int y6 = num73 + num75;
                                tile5 = Main.tile[x6, y6];
                                tile5.HasTile = true;
                                tile5 = Main.tile[x6, y6];
                                tile5.Slope = 0;
                                tile5 = Main.tile[x6, y6];
                                tile5.IsHalfBlock = false;
                                tile5 = Main.tile[x6, y6];
                                tile5.TileType = TileID.ShadowOrbs;
                                tile5 = Main.tile[x6, y6];
                                tile5.TileFrameX = (short)(num74 * 18 + 36 * num68);
                                tile5 = Main.tile[x6, y6];
                                tile5.TileFrameY = (short)(num75 * 18 + 36 * num72);
                            }
                        }
                    }
                    tile5 = Main.tile[num57, num58];
                    if (tile5.TileType == TileID.Heart)
                    {
                        tile5 = Main.tile[num57, num58];
                        int num76 = tile5.TileFrameX / 18;
                        int num77 = 0;
                        int num78 = num57;
                        num77 += num76 / 2;
                        num76 %= 2;
                        num78 -= num76;
                        tile5 = Main.tile[num57, num58];
                        int num79 = tile5.TileFrameY / 18;
                        int num80 = 0;
                        int num81 = num58;
                        num80 += num79 / 2;
                        num79 %= 2;
                        num81 -= num79;
                        for (int num82 = 0; num82 < 2; tile5 = Main.tile[num82, num58 + 2], tile5.Slope = 0, tile5 = Main.tile[num82, num58 + 2], tile5.IsHalfBlock = false, num82++)
                        {
                            for (int num83 = 0; num83 < 2; num83++)
                            {
                                int x7 = num78 + num82;
                                int y7 = num81 + num83;
                                tile5 = Main.tile[x7, y7];
                                tile5.HasTile = true;
                                tile5 = Main.tile[x7, y7];
                                tile5.Slope = 0;
                                tile5 = Main.tile[x7, y7];
                                tile5.IsHalfBlock = false;
                                tile5 = Main.tile[x7, y7];
                                tile5.TileType = TileID.Heart;
                                tile5 = Main.tile[x7, y7];
                                tile5.TileFrameX = (short)(num82 * 18 + 36 * num77);
                                tile5 = Main.tile[x7, y7];
                                tile5.TileFrameY = (short)(num83 * 18 + 36 * num80);
                            }
                            tile5 = Main.tile[num82, num58 + 2];
                            if (!tile5.HasTile)
                            {
                                tile5 = Main.tile[num82, num58 + 2];
                                tile5.HasTile = true;
                                bool[] tileSolid3 = Main.tileSolid;
                                tile5 = Main.tile[num82, num58 + 2];
                                if (tileSolid3[tile5.TileType])
                                {
                                    bool[] tileSolidTop3 = Main.tileSolidTop;
                                    tile5 = Main.tile[num82, num58 + 2];
                                    if (!tileSolidTop3[tile5.TileType])
                                    {
                                        continue;
                                    }
                                }
                                tile5 = Main.tile[num82, num58 + 2];
                                tile5.TileType = TileID.Dirt;
                            }
                        }
                    }
                    tile5 = Main.tile[num57, num58];
                    if (tile5.TileType == TileID.ManaCrystal)
                    {
                        tile5 = Main.tile[num57, num58];
                        int num84 = tile5.TileFrameX / 18;
                        int num85 = 0;
                        int num86 = num57;
                        num85 += num84 / 2;
                        num84 %= 2;
                        num86 -= num84;
                        tile5 = Main.tile[num57, num58];
                        int num87 = tile5.TileFrameY / 18;
                        int num88 = 0;
                        int num89 = num58;
                        num88 += num87 / 2;
                        num87 %= 2;
                        num89 -= num87;
                        for (int num90 = 0; num90 < 2; tile5 = Main.tile[num90, num58 + 2], tile5.Slope = 0, tile5 = Main.tile[num90, num58 + 2], tile5.IsHalfBlock = false, num90++)
                        {
                            for (int num91 = 0; num91 < 2; num91++)
                            {
                                int x8 = num86 + num90;
                                int y8 = num89 + num91;
                                tile5 = Main.tile[x8, y8];
                                tile5.HasTile = true;
                                tile5 = Main.tile[x8, y8];
                                tile5.Slope = 0;
                                tile5 = Main.tile[x8, y8];
                                tile5.IsHalfBlock = false;
                                tile5 = Main.tile[x8, y8];
                                tile5.TileType = TileID.ManaCrystal;
                                tile5 = Main.tile[x8, y8];
                                tile5.TileFrameX = (short)(num90 * 18 + 36 * num85);
                                tile5 = Main.tile[x8, y8];
                                tile5.TileFrameY = (short)(num91 * 18 + 36 * num88);
                            }
                            tile5 = Main.tile[num90, num58 + 2];
                            if (!tile5.HasTile)
                            {
                                tile5 = Main.tile[num90, num58 + 2];
                                tile5.HasTile = true;
                                bool[] tileSolid4 = Main.tileSolid;
                                tile5 = Main.tile[num90, num58 + 2];
                                if (tileSolid4[tile5.TileType])
                                {
                                    bool[] tileSolidTop4 = Main.tileSolidTop;
                                    tile5 = Main.tile[num90, num58 + 2];
                                    if (!tileSolidTop4[tile5.TileType])
                                    {
                                        continue;
                                    }
                                }
                                tile5 = Main.tile[num90, num58 + 2];
                                tile5.TileType = TileID.Dirt;
                            }
                        }
                    }
                    bool[] basicChest = TileID.Sets.BasicChest;
                    tile5 = Main.tile[num57, num58];
                    if (basicChest[tile5.TileType])
                    {
                        tile5 = Main.tile[num57, num58];
                        int num92 = tile5.TileFrameX / 18;
                        int num93 = 0;
                        ushort num94 = 21;
                        int num95 = num57;
                        int num96 = num58;
                        tile5 = Main.tile[num57, num58];
                        int num97 = num96 - tile5.TileFrameY / 18;
                        tile5 = Main.tile[num57, num58];
                        if (tile5.TileType == TileID.Containers2)
                        {
                            num94 = 467;
                        }
                        bool[] basicChest2 = TileID.Sets.BasicChest;
                        tile5 = Main.tile[num57, num58];
                        if (basicChest2[tile5.TileType])
                        {
                            tile5 = Main.tile[num57, num58];
                            num94 = tile5.TileType;
                        }
                        while (num92 >= 2)
                        {
                            num93++;
                            num92 -= 2;
                        }
                        num95 -= num92;
                        int num98 = Chest.FindChest(num95, num97);
                        if (num98 != -1)
                        {
                            switch (Main.chest[num98].item[0].type)
                            {
                                case ItemID.PiranhaGun:
                                    num93 = 23;
                                    break;
                                case ItemID.ScourgeoftheCorruptor:
                                    num93 = 24;
                                    break;
                                case ItemID.VampireKnives:
                                    num93 = 25;
                                    break;
                                case ItemID.RainbowGun:
                                    num93 = 26;
                                    break;
                                case ItemID.StaffoftheFrostHydra:
                                    num93 = 27;
                                    break;
                            }
                        }
                        for (int num99 = 0; num99 < 2; tile5 = Main.tile[num99, num58 + 2], tile5.Slope = 0, tile5 = Main.tile[num99, num58 + 2], tile5.IsHalfBlock = false, num99++)
                        {
                            for (int num100 = 0; num100 < 2; num100++)
                            {
                                int x9 = num95 + num99;
                                int y9 = num97 + num100;
                                tile5 = Main.tile[x9, y9];
                                tile5.HasTile = true;
                                tile5 = Main.tile[x9, y9];
                                tile5.Slope = 0;
                                tile5 = Main.tile[x9, y9];
                                tile5.IsHalfBlock = false;
                                tile5 = Main.tile[x9, y9];
                                tile5.TileType = num94;
                                tile5 = Main.tile[x9, y9];
                                tile5.TileFrameX = (short)(num99 * 18 + 36 * num93);
                                tile5 = Main.tile[x9, y9];
                                tile5.TileFrameY = (short)(num100 * 18);
                            }
                            tile5 = Main.tile[num99, num58 + 2];
                            if (!tile5.HasTile)
                            {
                                tile5 = Main.tile[num99, num58 + 2];
                                tile5.HasTile = true;
                                bool[] tileSolid5 = Main.tileSolid;
                                tile5 = Main.tile[num99, num58 + 2];
                                if (tileSolid5[tile5.TileType])
                                {
                                    bool[] tileSolidTop5 = Main.tileSolidTop;
                                    tile5 = Main.tile[num99, num58 + 2];
                                    if (!tileSolidTop5[tile5.TileType])
                                    {
                                        continue;
                                    }
                                }
                                tile5 = Main.tile[num99, num58 + 2];
                                tile5.TileType = TileID.Dirt;
                            }
                        }
                    }
                    tile5 = Main.tile[num57, num58];
                    if (tile5.TileType == TileID.Pots)
                    {
                        tile5 = Main.tile[num57, num58];
                        int num101 = tile5.TileFrameX / 18;
                        int num102 = 0;
                        int num103 = num57;
                        while (num101 >= 2)
                        {
                            num102++;
                            num101 -= 2;
                        }
                        num103 -= num101;
                        tile5 = Main.tile[num57, num58];
                        int num104 = tile5.TileFrameY / 18;
                        int num105 = 0;
                        int num106 = num58;
                        while (num104 >= 2)
                        {
                            num105++;
                            num104 -= 2;
                        }
                        num106 -= num104;
                        for (int num107 = 0; num107 < 2; tile5 = Main.tile[num107, num58 + 2], tile5.Slope = 0, tile5 = Main.tile[num107, num58 + 2], tile5.IsHalfBlock = false, num107++)
                        {
                            for (int num108 = 0; num108 < 2; num108++)
                            {
                                int x10 = num103 + num107;
                                int y10 = num106 + num108;
                                tile5 = Main.tile[x10, y10];
                                tile5.HasTile = true;
                                tile5 = Main.tile[x10, y10];
                                tile5.Slope = 0;
                                tile5 = Main.tile[x10, y10];
                                tile5.IsHalfBlock = false;
                                tile5 = Main.tile[x10, y10];
                                tile5.TileType = TileID.Pots;
                                tile5 = Main.tile[x10, y10];
                                tile5.TileFrameX = (short)(num107 * 18 + 36 * num102);
                                tile5 = Main.tile[x10, y10];
                                tile5.TileFrameY = (short)(num108 * 18 + 36 * num105);
                            }
                            tile5 = Main.tile[num107, num58 + 2];
                            if (!tile5.HasTile)
                            {
                                tile5 = Main.tile[num107, num58 + 2];
                                tile5.HasTile = true;
                                bool[] tileSolid6 = Main.tileSolid;
                                tile5 = Main.tile[num107, num58 + 2];
                                if (tileSolid6[tile5.TileType])
                                {
                                    bool[] tileSolidTop6 = Main.tileSolidTop;
                                    tile5 = Main.tile[num107, num58 + 2];
                                    if (!tileSolidTop6[tile5.TileType])
                                    {
                                        continue;
                                    }
                                }
                                tile5 = Main.tile[num107, num58 + 2];
                                tile5.TileType = TileID.Dirt;
                            }
                        }
                    }
                    tile5 = Main.tile[num57, num58];
                    if (tile5.TileType == TileID.DemonAltar)
                    {
                        tile5 = Main.tile[num57, num58];
                        int num109 = tile5.TileFrameX / 18;
                        int num110 = 0;
                        int num111 = num57;
                        int num112 = num58;
                        tile5 = Main.tile[num57, num58];
                        int num113 = num112 - tile5.TileFrameY / 18;
                        while (num109 >= 3)
                        {
                            num110++;
                            num109 -= 3;
                        }
                        num111 -= num109;
                        int num114;
                        if (!WorldGen.drunkWorldGen)
                        {
                            num114 = (WorldGen.crimson ? 1 : 0);
                        }
                        else
                        {
                            tile5 = Main.tile[num57, num58];
                            num114 = ((tile5.WallType == WallID.CrimstoneUnsafe) ? 1 : 0);
                        }
                        num110 = num114;
                        for (int num115 = 0; num115 < 3; num115++)
                        {
                            for (int num116 = 0; num116 < 2; num116++)
                            {
                                int x11 = num111 + num115;
                                int y11 = num113 + num116;
                                tile5 = Main.tile[x11, y11];
                                tile5.HasTile = true;
                                tile5 = Main.tile[x11, y11];
                                tile5.Slope = 0;
                                tile5 = Main.tile[x11, y11];
                                tile5.IsHalfBlock = false;
                                tile5 = Main.tile[x11, y11];
                                tile5.TileType = TileID.DemonAltar;
                                tile5 = Main.tile[x11, y11];
                                tile5.TileFrameX = (short)(num115 * 18 + 54 * num110);
                                tile5 = Main.tile[x11, y11];
                                tile5.TileFrameY = (short)(num116 * 18);
                            }
                            tile5 = Main.tile[num111 + num115, num113 + 2];
                            if (tile5.HasTile)
                            {
                                bool[] tileSolid7 = Main.tileSolid;
                                tile5 = Main.tile[num111 + num115, num113 + 2];
                                if (tileSolid7[tile5.TileType])
                                {
                                    bool[] tileSolidTop7 = Main.tileSolidTop;
                                    tile5 = Main.tile[num111 + num115, num113 + 2];
                                    if (!tileSolidTop7[tile5.TileType])
                                    {
                                        goto IL_13ca;
                                    }
                                }
                            }
                            tile5 = Main.tile[num111 + num115, num113 + 2];
                            tile5.HasTile = true;
                            bool[] platforms = TileID.Sets.Platforms;
                            tile5 = Main.tile[num111 + num115, num113 + 2];
                            if (!platforms[tile5.TileType])
                            {
                                tile5 = Main.tile[num111 + num115, num113 + 2];
                                if (tile5.TileType == TileID.RollingCactus)
                                {
                                    tile5 = Main.tile[num111 + num115, num113 + 2];
                                    tile5.TileType = TileID.HardenedSand;
                                }
                                else
                                {
                                    bool[] boulders2 = TileID.Sets.Boulders;
                                    tile5 = Main.tile[num111 + num115, num113 + 2];
                                    if (!boulders2[tile5.TileType])
                                    {
                                        bool[] tileSolid8 = Main.tileSolid;
                                        tile5 = Main.tile[num111 + num115, num113 + 2];
                                        if (tileSolid8[tile5.TileType])
                                        {
                                            bool[] tileSolidTop8 = Main.tileSolidTop;
                                            tile5 = Main.tile[num111 + num115, num113 + 2];
                                            if (!tileSolidTop8[tile5.TileType])
                                            {
                                                goto IL_13ca;
                                            }
                                        }
                                    }
                                    tile5 = Main.tile[num111 + num115, num113 + 2];
                                    tile5.TileType = TileID.Dirt;
                                }
                            }
                            goto IL_13ca;
                        IL_13ca:
                            tile5 = Main.tile[num111 + num115, num113 + 2];
                            tile5.Slope = 0;
                            tile5 = Main.tile[num111 + num115, num113 + 2];
                            tile5.IsHalfBlock = false;
                            tile5 = Main.tile[num111 + num115, num113 + 3];
                            if (tile5.TileType == TileID.Pots)
                            {
                                tile5 = Main.tile[num111 + num115, num113 + 3];
                                if (tile5.TileFrameY % 36 >= 18)
                                {
                                    tile5 = Main.tile[num111 + num115, num113 + 3];
                                    tile5.TileType = TileID.Dirt;
                                    tile5 = Main.tile[num111 + num115, num113 + 3];
                                    tile5.HasTile = false;
                                }
                            }
                        }
                        for (int num117 = 0; num117 < 3; num117++)
                        {
                            tile5 = Main.tile[num111 - 1, num113 + num117];
                            if (tile5.TileType != TileID.Pots)
                            {
                                tile5 = Main.tile[num111 - 1, num113 + num117];
                                if (tile5.TileType != TileID.Heart)
                                {
                                    tile5 = Main.tile[num111 - 1, num113 + num117];
                                    if (tile5.TileType != TileID.ManaCrystal)
                                    {
                                        goto IL_1553;
                                    }
                                }
                            }
                            tile5 = Main.tile[num111 - 1, num113 + num117];
                            if (tile5.TileFrameX % 36 < 18)
                            {
                                tile5 = Main.tile[num111 - 1, num113 + num117];
                                tile5.TileType = TileID.Dirt;
                                tile5 = Main.tile[num111 - 1, num113 + num117];
                                tile5.HasTile = false;
                            }
                            goto IL_1553;
                        IL_1553:
                            tile5 = Main.tile[num111 + 3, num113 + num117];
                            if (tile5.TileType != TileID.Pots)
                            {
                                tile5 = Main.tile[num111 + 3, num113 + num117];
                                if (tile5.TileType != TileID.Heart)
                                {
                                    tile5 = Main.tile[num111 - 1, num113 + num117];
                                    if (tile5.TileType != TileID.ManaCrystal)
                                    {
                                        continue;
                                    }
                                }
                            }
                            tile5 = Main.tile[num111 + 3, num113 + num117];
                            if (tile5.TileFrameX % 36 >= 18)
                            {
                                tile5 = Main.tile[num111 + 3, num113 + num117];
                                tile5.TileType = TileID.Dirt;
                                tile5 = Main.tile[num111 + 3, num113 + num117];
                                tile5.HasTile = false;
                            }
                        }
                    }
                    tile5 = Main.tile[num57, num58];
                    if (tile5.TileType == TileID.LihzahrdAltar)
                    {
                        tile5 = Main.tile[num57, num58 + 1];
                        if (tile5.TileType == TileID.WoodenSpikes)
                        {
                            tile5 = Main.tile[num57, num58 + 1];
                            tile5.TileType = TileID.LihzahrdBrick;
                        }
                    }
                    tile5 = Main.tile[num57, num58];
                    if (tile5.WallType == WallID.LihzahrdBrickUnsafe)
                    {
                        tile5 = Main.tile[num57, num58];
                        tile5.LiquidAmount = 0;
                    }
                    num58++;
                    continue;
                IL_00c6:
                    tile5 = Main.tile[num57, num58];
                    tile5.Slope = 0;
                    tile5 = Main.tile[num57, num58];
                    tile5.IsHalfBlock = true;
                    goto IL_00f0;
                IL_05b9:
                    tile5 = Main.tile[num57, num58];
                    if (tile5.WallType != WallID.HellstoneBrickUnsafe)
                    {
                        tile5 = Main.tile[num57, num58];
                        if (tile5.WallType != WallID.ObsidianBrickUnsafe)
                        {
                            goto IL_0601;
                        }
                    }
                    tile5 = Main.tile[num57, num58];
                    tile5.LiquidAmount = 0;
                    goto IL_0601;
                }
            }
        }

        public static void FinalCleanup()
        {
            Main.tileSolid[484] = false;
            WorldGen.FillWallHolesInArea(new Rectangle(0, 0, Main.maxTilesX, (int)Main.worldSurface));
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                int num = 0;
                while (num < Main.maxTilesY)
                {
                    Tile tile = Main.tile[i, num];
                    if (tile.HasTile && !WorldGen.SolidTile(i, num + 1))
                    {
                        tile = Main.tile[i, num];
                        if (tile.TileType != TileID.Sand)
                        {
                            tile = Main.tile[i, num];
                            if (tile.TileType != TileID.Ebonsand)
                            {
                                tile = Main.tile[i, num];
                                if (tile.TileType != TileID.Crimsand)
                                {
                                    tile = Main.tile[i, num];
                                    if (tile.TileType != TileID.Slush)
                                    {
                                        tile = Main.tile[i, num];
                                        if (tile.TileType != TileID.Silt)
                                        {
                                            goto IL_06be;
                                        }
                                    }
                                }
                            }
                        }
                        if ((double)num < Main.worldSurface + 10.0)
                        {
                            tile = Main.tile[i, num + 1];
                            if (!tile.HasTile)
                            {
                                tile = Main.tile[i, num + 1];
                                if (tile.WallType != WallID.CorruptionUnsafe4 && !WorldGen.oceanDepths(i, num))
                                {
                                    int num2 = 10;
                                    int num3 = num + 1;
                                    for (int j = num3; j < num3 + 10; j++)
                                    {
                                        tile = Main.tile[i, j];
                                        if (tile.HasTile)
                                        {
                                            tile = Main.tile[i, j];
                                            if (tile.TileType == TileID.MinecartTrack)
                                            {
                                                num2 = 0;
                                                break;
                                            }
                                        }
                                    }
                                    while (true)
                                    {
                                        tile = Main.tile[i, num3];
                                        if (tile.HasTile || num2 <= 0 || num3 >= Main.maxTilesY - 50)
                                        {
                                            break;
                                        }
                                        tile = Main.tile[i, num3 - 1];
                                        tile.Slope = 0;
                                        tile = Main.tile[i, num3 - 1];
                                        tile.IsHalfBlock = false;
                                        tile = Main.tile[i, num3];
                                        tile.HasTile = true;
                                        tile = Main.tile[i, num3];
                                        ref ushort type = ref tile.TileType;
                                        tile = Main.tile[i, num];
                                        type = tile.TileType;
                                        tile = Main.tile[i, num3];
                                        tile.Slope = 0;
                                        tile = Main.tile[i, num3];
                                        tile.IsHalfBlock = false;
                                        num3++;
                                        num2--;
                                    }
                                    if (num2 == 0)
                                    {
                                        tile = Main.tile[i, num3];
                                        if (!tile.HasTile)
                                        {
                                            tile = Main.tile[i, num];
                                            switch (tile.TileType)
                                            {
                                                case TileID.Sand:
                                                    tile = Main.tile[i, num3];
                                                    tile.TileType = TileID.HardenedSand;
                                                    tile = Main.tile[i, num3];
                                                    tile.HasTile = true;
                                                    break;
                                                case TileID.Ebonsand:
                                                    tile = Main.tile[i, num3];
                                                    tile.TileType = TileID.CorruptHardenedSand;
                                                    tile = Main.tile[i, num3];
                                                    tile.HasTile = true;
                                                    break;
                                                case TileID.Crimsand:
                                                    tile = Main.tile[i, num3];
                                                    tile.TileType = TileID.CrimsonHardenedSand;
                                                    tile = Main.tile[i, num3];
                                                    tile.HasTile = true;
                                                    break;
                                                case TileID.Slush:
                                                    tile = Main.tile[i, num3];
                                                    tile.TileType = TileID.SnowBlock;
                                                    tile = Main.tile[i, num3];
                                                    tile.HasTile = true;
                                                    break;
                                                case TileID.Silt:
                                                    tile = Main.tile[i, num3];
                                                    tile.TileType = TileID.Stone;
                                                    tile = Main.tile[i, num3];
                                                    tile.HasTile = true;
                                                    break;
                                            }
                                            goto IL_0690;
                                        }
                                    }
                                    tile = Main.tile[i, num3];
                                    if (tile.HasTile)
                                    {
                                        bool[] tileSolid = Main.tileSolid;
                                        tile = Main.tile[i, num3];
                                        if (tileSolid[tile.TileType])
                                        {
                                            bool[] tileSolidTop = Main.tileSolidTop;
                                            tile = Main.tile[i, num3];
                                            if (!tileSolidTop[tile.TileType])
                                            {
                                                tile = Main.tile[i, num3];
                                                tile.Slope = 0;
                                                tile = Main.tile[i, num3];
                                                tile.IsHalfBlock = false;
                                            }
                                        }
                                    }
                                    goto IL_0690;
                                }
                            }
                        }
                        bool[] tileSolid2 = Main.tileSolid;
                        tile = Main.tile[i, num + 1];
                        if (tileSolid2[tile.TileType])
                        {
                            bool[] tileSolidTop2 = Main.tileSolidTop;
                            tile = Main.tile[i, num + 1];
                            if (!tileSolidTop2[tile.TileType])
                            {
                                tile = Main.tile[i, num + 1];
                                if (!tile.TopSlope)
                                {
                                    tile = Main.tile[i, num + 1];
                                    if (!tile.IsHalfBlock)
                                    {
                                        goto IL_05aa;
                                    }
                                }
                                tile = Main.tile[i, num + 1];
                                tile.Slope = 0;
                                tile = Main.tile[i, num + 1];
                                tile.IsHalfBlock = false;
                                goto IL_0690;
                            }
                        }
                        goto IL_05aa;
                    }
                    goto IL_06be;
                IL_05aa:
                    tile = Main.tile[i, num];
                    switch (tile.TileType)
                    {
                        case TileID.Sand:
                            tile = Main.tile[i, num];
                            tile.TileType = TileID.HardenedSand;
                            break;
                        case TileID.Ebonsand:
                            tile = Main.tile[i, num];
                            tile.TileType = TileID.CorruptHardenedSand;
                            break;
                        case TileID.Crimsand:
                            tile = Main.tile[i, num];
                            tile.TileType = TileID.CrimsonHardenedSand;
                            break;
                        case TileID.Slush:
                            tile = Main.tile[i, num];
                            tile.TileType = TileID.SnowBlock;
                            break;
                        case TileID.Silt:
                            tile = Main.tile[i, num];
                            tile.TileType = TileID.Stone;
                            break;
                    }
                    goto IL_0690;
                IL_0753:
                    tile = Main.tile[i, num];
                    if (tile.TileType != TileID.AntlionLarva)
                    {
                        tile = Main.tile[i, num];
                        if (tile.TileType != TileID.LargePiles2)
                        {
                            tile = Main.tile[i, num];
                            if (tile.TileType != TileID.Stalactite)
                            {
                                goto IL_07bb;
                            }
                        }
                    }
                    WorldGen.TileFrame(i, num);
                    goto IL_07bb;
                IL_0825:
                    tile = Main.tile[i, num];
                    if (tile.TileType == TileID.DemonAltar)
                    {
                        WorldGen.TileFrame(i, num);
                    }
                    bool[] isATreeTrunk = TileID.Sets.IsATreeTrunk;
                    tile = Main.tile[i, num];
                    if (!isATreeTrunk[tile.TileType])
                    {
                        tile = Main.tile[i, num];
                        if (tile.TileType != TileID.PalmTree)
                        {
                            goto IL_0896;
                        }
                    }
                    WorldGen.TileFrame(i, num);
                    goto IL_0896;
                IL_0690:
                    tile = Main.tile[i, num - 1];
                    if (tile.TileType == TileID.PalmTree)
                    {
                        WorldGen.TileFrame(i, num - 1);
                    }
                    goto IL_06be;
                IL_07bb:
                    tile = Main.tile[i, num];
                    if (tile.TileType == TileID.Pots)
                    {
                        WorldGen.TileFrame(i, num);
                    }
                    tile = Main.tile[i, num];
                    if (tile.TileType != TileID.ClosedDoor)
                    {
                        tile = Main.tile[i, num];
                        if (tile.TileType != TileID.OpenDoor)
                        {
                            goto IL_0825;
                        }
                    }
                    WorldGen.TileFrame(i, num);
                    goto IL_0825;
                IL_0896:
                    tile = Main.tile[i, num];
                    if (tile.TileType == TileID.Traps)
                    {
                        tile = Main.tile[i, num];
                        tile.Slope = 0;
                        tile = Main.tile[i, num];
                        tile.IsHalfBlock = false;
                    }
                    tile = Main.tile[i, num];
                    if (tile.HasTile)
                    {
                        bool[] boulders = TileID.Sets.Boulders;
                        tile = Main.tile[i, num];
                        if (boulders[tile.TileType])
                        {
                            tile = Main.tile[i, num];
                            int num4 = tile.TileFrameX / 18;
                            int num5 = i;
                            num5 -= num4;
                            tile = Main.tile[i, num];
                            int num6 = tile.TileFrameY / 18;
                            int num7 = num;
                            num7 -= num6;
                            bool flag = false;
                            for (int k = 0; k < 2; k++)
                            {
                                Tile tile2 = Main.tile[num5 + k, num7 - 1];
                                if (tile2 != null && tile2.HasTile && tile2.TileType == TileID.DemonAltar)
                                {
                                    flag = true;
                                    break;
                                }
                                for (int l = 0; l < 2; l++)
                                {
                                    int x = num5 + k;
                                    int y = num7 + l;
                                    tile = Main.tile[x, y];
                                    tile.HasTile = true;
                                    tile = Main.tile[x, y];
                                    tile.Slope = 0;
                                    tile = Main.tile[x, y];
                                    tile.IsHalfBlock = false;
                                    tile = Main.tile[x, y];
                                    ref ushort type2 = ref tile.TileType;
                                    tile = Main.tile[i, num];
                                    type2 = tile.TileType;
                                    tile = Main.tile[x, y];
                                    tile.TileFrameX = (short)(k * 18);
                                    tile = Main.tile[x, y];
                                    tile.TileFrameY = (short)(l * 18);
                                }
                            }
                            if (flag)
                            {
                                ushort num8 = 0;
                                tile = Main.tile[i, num];
                                if (tile.TileType == TileID.RollingCactus)
                                {
                                    num8 = 397;
                                }
                                for (int m = 0; m < 2; m++)
                                {
                                    for (int n = 0; n < 2; n++)
                                    {
                                        int x2 = num5 + m;
                                        int y2 = num7 + n;
                                        tile = Main.tile[x2, y2];
                                        tile.HasTile = true;
                                        tile = Main.tile[x2, y2];
                                        tile.Slope = 0;
                                        tile = Main.tile[x2, y2];
                                        tile.IsHalfBlock = false;
                                        tile = Main.tile[x2, y2];
                                        tile.TileType = num8;
                                        tile = Main.tile[x2, y2];
                                        tile.TileFrameX = 0;
                                        tile = Main.tile[x2, y2];
                                        tile.TileFrameY = 0;
                                    }
                                }
                            }
                        }
                    }
                    tile = Main.tile[i, num];
                    if (tile.TileType == TileID.PalmTree)
                    {
                        tile = Main.tile[i, num];
                        if (tile.LiquidAmount > 0)
                        {
                            WorldGen.KillTile(i, num);
                        }
                    }
                    bool[] wallDungeon = Main.wallDungeon;
                    tile = Main.tile[i, num];
                    if (wallDungeon[tile.WallType])
                    {
                        tile = Main.tile[i, num];
                        //tile.lava/* tModPorter Suggestion: LiquidType = ... */(lava: false);
                        tile = Main.tile[i, num];
                        if (tile.HasTile)
                        {
                            tile = Main.tile[i, num];
                            if (tile.TileType == TileID.Obsidian)
                            {
                                WorldGen.KillTile(i, num);
                                tile = Main.tile[i, num];
                                //tile.lava/* tModPorter Suggestion: LiquidType = ... */(lava: false);
                                tile = Main.tile[i, num];
                                tile.LiquidAmount = byte.MaxValue;
                            }
                        }
                    }
                    tile = Main.tile[i, num];
                    if (tile.HasTile)
                    {
                        tile = Main.tile[i, num];
                        if (tile.TileType == TileID.MinecartTrack)
                        {
                            int num9 = 15;
                            int num10 = 1;
                            int num11 = num;
                            while (num - num11 < num9)
                            {
                                tile = Main.tile[i, num11];
                                tile.LiquidAmount = 0;
                                num11--;
                            }
                            for (num11 = num; num11 - num < num10; num11++)
                            {
                                tile = Main.tile[i, num11];
                                tile.LiquidAmount = 0;
                            }
                        }
                    }
                    tile = Main.tile[i, num];
                    if (tile.HasTile)
                    {
                        tile = Main.tile[i, num];
                        if (tile.TileType == TileID.GoldCoinPile)
                        {
                            tile = Main.tile[i, num + 1];
                            if (!tile.HasTile)
                            {
                                tile = Main.tile[i, num + 1];
                                tile.ClearEverything();
                                tile = Main.tile[i, num + 1];
                                tile.HasTile = true;
                                tile = Main.tile[i, num + 1];
                                tile.TileType = TileID.GoldCoinPile;
                            }
                        }
                    }
                    if (i > WorldGen.beachDistance && i < Main.maxTilesX - WorldGen.beachDistance && (double)num < Main.worldSurface)
                    {
                        tile = Main.tile[i, num];
                        if (tile.LiquidAmount > 0)
                        {
                            tile = Main.tile[i, num];
                            if (tile.LiquidAmount < byte.MaxValue)
                            {
                                tile = Main.tile[i - 1, num];
                                if (tile.LiquidAmount < byte.MaxValue)
                                {
                                    tile = Main.tile[i + 1, num];
                                    if (tile.LiquidAmount < byte.MaxValue)
                                    {
                                        tile = Main.tile[i, num + 1];
                                        if (tile.LiquidAmount < byte.MaxValue)
                                        {
                                            bool[] clouds = TileID.Sets.Clouds;
                                            tile = Main.tile[i - 1, num];
                                            if (!clouds[tile.TileType])
                                            {
                                                bool[] clouds2 = TileID.Sets.Clouds;
                                                tile = Main.tile[i + 1, num];
                                                if (!clouds2[tile.TileType])
                                                {
                                                    bool[] clouds3 = TileID.Sets.Clouds;
                                                    tile = Main.tile[i, num + 1];
                                                    if (!clouds3[tile.TileType])
                                                    {
                                                        tile = Main.tile[i, num];
                                                        tile.LiquidAmount = 0;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    num++;
                    continue;
                IL_06be:
                    tile = Main.tile[i, num];
                    if (tile.WallType != WallID.Sandstone)
                    {
                        tile = Main.tile[i, num];
                        if (tile.WallType != WallID.HardenedSand)
                        {
                            goto IL_0753;
                        }
                    }
                    tile = Main.tile[i, num];
                    if (tile.LiquidAmount > 0 && !WorldGen.remixWorldGen)
                    {
                        tile = Main.tile[i, num];
                        tile.LiquidAmount = byte.MaxValue;
                        tile = Main.tile[i, num];
                        tile.LiquidType = LiquidID.Lava;
                    }
                    goto IL_0753;
                }
            }
            int num12 = 0;
            int num13 = 3;
            num13 = WorldGen.GetWorldSize() switch
            {
                1 => 6,
                2 => 9,
                _ => 3,
            };
            if (WorldGen.tenthAnniversaryWorldGen)
            {
                num13 *= 5;
            }
            int num14 = 50;
            int minValue = num14;
            int minValue2 = num14;
            int maxValue = Main.maxTilesX - num14;
            int maxValue2 = Main.maxTilesY - 200;
            int num15 = 3000;
            while (num12 < num13)
            {
                num15--;
                if (num15 <= 0)
                {
                    break;
                }
                int x3 = WorldGen.genRand.Next(minValue, maxValue);
                int y3 = WorldGen.genRand.Next(minValue2, maxValue2);
                Tile tile3 = Main.tile[x3, y3];
                if (tile3.HasTile && tile3.TileType >= TileID.Dirt)
                {
                    bool flag2 = TileID.Sets.Dirt[tile3.TileType];
                    if (WorldGen.notTheBees)
                    {
                        flag2 = flag2 || TileID.Sets.Mud[tile3.TileType];
                    }
                    if (flag2)
                    {
                        num12++;
                        tile3.ClearTile();
                        tile3.HasTile = true;
                        tile3.TileType = TileID.DirtiestBlock;
                    }
                }
            }
            //WorldGen.ShimmerCleanUp();
            Main.tileSolid[659] = true;
            Main.tileSolid[GenVars.crackedType] = true;
            Main.tileSolid[484] = true;
            //WorldGen.gen = false;
            //Main.AnglerQuestSwap();
            //WorldGen.skipFramingDuringGen = false;
        }
        #endregion
    }
}
