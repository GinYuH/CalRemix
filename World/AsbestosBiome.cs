using System;
using System.Collections.Generic;
using CalRemix.Items.Placeables;
using CalRemix.Tiles;
using CalRemix.Walls;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace CalRemix.World
{
    public class AsbestosBiome : MicroBiome
    {
        private struct Magma
        {
            public readonly double Pressure;

            public readonly double Resistance;

            public readonly bool IsActive;

            private Magma(double pressure, double resistance, bool active)
            {
                Pressure = pressure;
                Resistance = resistance;
                IsActive = active;
            }

            public Magma ToFlow()
            {
                return new Magma(Pressure, Resistance, active: true);
            }

            public static Magma CreateFlow(double pressure, double resistance = 0.0)
            {
                return new Magma(pressure, resistance, active: true);
            }

            public static Magma CreateEmpty(double resistance = 0.0)
            {
                return new Magma(0.0, resistance, active: false);
            }
        }

        private const int MAX_MAGMA_ITERATIONS = 300;

        private Magma[,] _sourceMagmaMap = new Magma[300, 300];

        private Magma[,] _targetMagmaMap = new Magma[300, 300];

        private static Vector2D[] _normalisedVectors = new Vector2D[9]
        {
        Vector2D.Normalize(new Vector2D(-1.0, -1.0)),
        Vector2D.Normalize(new Vector2D(-1.0, 0.0)),
        Vector2D.Normalize(new Vector2D(-1.0, 1.0)),
        Vector2D.Normalize(new Vector2D(0.0, -1.0)),
        new Vector2D(0.0, 0.0),
        Vector2D.Normalize(new Vector2D(0.0, 1.0)),
        Vector2D.Normalize(new Vector2D(1.0, -1.0)),
        Vector2D.Normalize(new Vector2D(1.0, 0.0)),
        Vector2D.Normalize(new Vector2D(1.0, 1.0))
        };

        public static bool CanPlace(Point origin, StructureMap structures)
        {
            if (WorldGen.BiomeTileCheck(origin.X, origin.Y))
            {
                return false;
            }
            return !GenBase._tiles[origin.X, origin.Y].HasTile;
        }

        public override bool Place(Point origin, StructureMap structures)
        {
            if (GenBase._tiles[origin.X, origin.Y].HasTile)
            {
                return false;
            }
            origin.X -= _sourceMagmaMap.GetLength(0) / 2;
            origin.Y -= _sourceMagmaMap.GetLength(1) / 2;
            BuildMagmaMap(origin);
            SimulatePressure(out var effectedMapArea);
            PlaceGranite(origin, effectedMapArea);
            CleanupTiles(origin, effectedMapArea);
            PlaceDecorations(origin, effectedMapArea);
            structures.AddStructure(effectedMapArea, 8);
            return true;
        }

        private void BuildMagmaMap(Point tileOrigin)
        {
            _sourceMagmaMap = new Magma[400, 400];
            _targetMagmaMap = new Magma[400, 400];
            for (int i = 0; i < _sourceMagmaMap.GetLength(0); i++)
            {
                for (int j = 0; j < _sourceMagmaMap.GetLength(1); j++)
                {
                    int i2 = i + tileOrigin.X;
                    int j2 = j + tileOrigin.Y;
                    _sourceMagmaMap[i, j] = Magma.CreateEmpty((!WorldGen.SolidTile(i2, j2)) ? 1 : 4);
                    _targetMagmaMap[i, j] = _sourceMagmaMap[i, j];
                }
            }
        }

        private void SimulatePressure(out Rectangle effectedMapArea)
        {
            int length = _sourceMagmaMap.GetLength(0);
            int length2 = _sourceMagmaMap.GetLength(1);
            int halflength1 = length / 2;
            int halflength2 = length2 / 2;
            int num3 = halflength1;
            int num4 = num3;
            int num5 = halflength2;
            int num6 = num5;
            for (int i = 0; i < 300; i++)
            {
                for (int j = num3; j <= num4; j++)
                {
                    for (int k = num5; k <= num6; k++)
                    {
                        Magma magma = _sourceMagmaMap[j, k];
                        if (!magma.IsActive)
                        {
                            continue;
                        }
                        double num7 = 0.0;
                        Vector2D zero = Vector2D.Zero;
                        for (int l = -1; l <= 1; l++)
                        {
                            for (int m = -1; m <= 1; m++)
                            {
                                if (l == 0 && m == 0)
                                {
                                    continue;
                                }
                                Vector2D vector2D = _normalisedVectors[(l + 1) * 3 + (m + 1)];
                                Magma magma2 = _sourceMagmaMap[j + l, k + m];
                                if (magma.Pressure > 0.01 && !magma2.IsActive)
                                {
                                    if (l == -1)
                                    {
                                        num3 = Utils.Clamp(j + l, 1, num3);
                                    }
                                    else
                                    {
                                        num4 = Utils.Clamp(j + l, num4, length - 2);
                                    }
                                    if (m == -1)
                                    {
                                        num5 = Utils.Clamp(k + m, 1, num5);
                                    }
                                    else
                                    {
                                        num6 = Utils.Clamp(k + m, num6, length2 - 2);
                                    }
                                    _targetMagmaMap[j + l, k + m] = magma2.ToFlow();
                                }
                                double pressure = magma2.Pressure;
                                num7 += pressure;
                                zero += pressure * vector2D;
                            }
                        }
                        num7 /= 8.0;
                        if (num7 > magma.Resistance)
                        {
                            double num8 = zero.Length() / 8.0;
                            double val = Math.Max(num7 - num8 - magma.Pressure, 0.0) + num8 + magma.Pressure * 0.875 - magma.Resistance;
                            val = Math.Max(0.0, val);
                            _targetMagmaMap[j, k] = Magma.CreateFlow(val, Math.Max(0.0, magma.Resistance - val * 0.02));
                        }
                    }
                }
                if (i < 2)
                {
                    _targetMagmaMap[halflength1, halflength2] = Magma.CreateFlow(25.0);
                }
                Utils.Swap(ref _sourceMagmaMap, ref _targetMagmaMap);
            }
            effectedMapArea = new Rectangle(num3, num5, num4 - num3 + 1, num6 - num5 + 1);
        }

        private bool ShouldUseLava(Point tileOrigin)
        {
            int length = _sourceMagmaMap.GetLength(0);
            int length2 = _sourceMagmaMap.GetLength(1);
            int num = length / 2;
            int num2 = length2 / 2;
            if (tileOrigin.Y + num2 <= GenVars.lavaLine - 30)
            {
                return false;
            }
            for (int i = -50; i < 50; i++)
            {
                for (int j = -50; j < 50; j++)
                {
                    if (GenBase._tiles[tileOrigin.X + num + i, tileOrigin.Y + num2 + j].HasTile)
                    {
                        ushort type = GenBase._tiles[tileOrigin.X + num + i, tileOrigin.Y + num2 + j].TileType;
                        if (type == 147 || (uint)(type - 161) <= 2u || type == 200)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void PlaceGranite(Point tileOrigin, Rectangle magmaMapArea)
        {
            bool flag = ShouldUseLava(tileOrigin);
            ushort type = (ushort)ModContent.TileType<AsbestosPlaced>();
            ushort wall = (ushort)ModContent.WallType<AsbestosWallPlaced>();
            for (int i = magmaMapArea.Left; i < magmaMapArea.Right; i++)
            {
                for (int j = magmaMapArea.Top; j < magmaMapArea.Bottom; j++)
                {
                    Magma magma = _sourceMagmaMap[i, j];
                    if (!magma.IsActive)
                    {
                        continue;
                    }
                    Tile tile = GenBase._tiles[tileOrigin.X + i, tileOrigin.Y + j];
                    double num = Math.Sin((double)(tileOrigin.Y + j) * 0.4) * 0.2 + 4;
                    double num2 = 0.2 + 0.5 / Math.Sqrt(Math.Max(0.0, magma.Pressure - magma.Resistance));
                    if (Math.Max(1.0 - Math.Max(0.0, num * num2), magma.Pressure / 15.0) > 0.35 + (WorldGen.SolidTile(tileOrigin.X + i, tileOrigin.Y + j) ? 0.0 : 0.5))
                    {
                        if (TileID.Sets.Ore[tile.TileType])
                        {
                            tile.ResetToType(tile.TileType);
                        }
                        else
                        {
                            tile.ResetToType(type);
                        }
                        tile.WallType = wall;
                    }
                    else if (magma.Resistance < 0.01)
                    {
                        WorldUtils.ClearTile(tileOrigin.X + i, tileOrigin.Y + j);
                        tile.WallType = wall;
                    }
                    if (tile.LiquidAmount > 0 && flag)
                    {
                        tile.LiquidType = 1;
                    }
                }
            }
        }

        private void CleanupTiles(Point tileOrigin, Rectangle magmaMapArea)
        {
            ushort wall = (ushort)ModContent.WallType<AsbestosWallPlaced>();
            List<Point16> list = new List<Point16>();
            for (int i = magmaMapArea.Left; i < magmaMapArea.Right; i++)
            {
                for (int j = magmaMapArea.Top; j < magmaMapArea.Bottom; j++)
                {
                    if (!_sourceMagmaMap[i, j].IsActive)
                    {
                        continue;
                    }
                    int tileAmt = 0;
                    int origX = i + tileOrigin.X;
                    int origY = j + tileOrigin.Y;
                    if (!WorldGen.SolidTile(origX, origY))
                    {
                        continue;
                    }
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            if (WorldGen.SolidTile(origX + k, origY + l))
                            {
                                tileAmt++;
                            }
                        }
                    }
                    if (tileAmt < 3)
                    {
                        list.Add(new Point16(origX, origY));
                    }
                }
            }
            foreach (Point16 item in list)
            {
                int x = item.X;
                int y = item.Y;
                WorldUtils.ClearTile(x, y, frameNeighbors: true);
                GenBase._tiles[x, y].WallType = wall;
            }
            list.Clear();
        }

        private void PlaceDecorations(Point tileOrigin, Rectangle magmaMapArea)
        {
            FastRandom fastRandom = new FastRandom(Main.ActiveWorldFileData.Seed).WithModifier(65440uL);
            for (int i = magmaMapArea.Left; i < magmaMapArea.Right; i++)
            {
                for (int j = magmaMapArea.Top; j < magmaMapArea.Bottom; j++)
                {
                    Magma magma = _sourceMagmaMap[i, j];
                    int num = i + tileOrigin.X;
                    int num2 = j + tileOrigin.Y;
                    if (!magma.IsActive)
                    {
                        continue;
                    }
                    WorldUtils.TileFrame(num, num2);
                    WorldGen.SquareWallFrame(num, num2);
                    FastRandom fastRandom2 = fastRandom.WithModifier(num, num2);
                    if (fastRandom2.Next(8) == 0 && GenBase._tiles[num, num2].HasTile)
                    {
                        if (!GenBase._tiles[num, num2 + 1].HasTile)
                        {
                            WorldGen.PlaceUncheckedStalactite(num, num2 + 1, fastRandom2.Next(2) == 0, fastRandom2.Next(3), spiders: false);
                        }
                        if (!GenBase._tiles[num, num2 - 1].HasTile)
                        {
                            WorldGen.PlaceUncheckedStalactite(num, num2 - 1, fastRandom2.Next(2) == 0, fastRandom2.Next(3), spiders: false);
                        }
                    }
                    if (fastRandom2.Next(2) == 0)
                    {
                        Tile.SmoothSlope(num, num2);
                    }
                }
            }
        }

        public static void GenerateHouse(int i, int j, int hauseAreaX, int hauseAreaY)
        {
            bool first = (!Main.tile[i, j].HasTile) ;
            int padding = 1;

            for (int x = 0; x < hauseAreaX; x++)
            {
                for (int y = 0; y < hauseAreaY; y++)
                {
                    Tile remp = Main.tile[i + x, j + y];
                    if ((x < padding || x > (hauseAreaX - padding - 1) || y < padding || y > (hauseAreaY - padding - 1)))
                    {
                        if (Main.rand.Next(0, 40) > 5 || (j == hauseAreaY - 1 && (x == hauseAreaX - 1 || x == 0)))
                        {
                            remp.ResetToType(TileID.WoodBlock);
                        }
                        else
                        {
                            if (Main.rand.NextBool())
                            {
                                remp.ResetToType((ushort)ModContent.TileType<AsbestosPlaced>());
                            }
                            else
                            {
                                remp.ClearEverything();
                            }
                        }
                    }
                    else
                    {
                        if (Main.rand.NextBool(10))
                        {
                            remp.ResetToType((ushort)ModContent.TileType<AsbestosPlaced>());
                        }
                        if (Main.rand.NextBool(60))
                        {
                            WorldGen.TileRunner(i + x, j + y, Main.rand.Next(4, 11), Main.rand.Next(2, 4), 51, addTile: true, TileID.Cobweb, -1.0, noYChange: false, overRide: false);
                        }
                        if (Main.rand.NextBool(40))
                        {
                        }
                        else
                        {
                            remp.WallType = Main.rand.NextBool(3) ? WallID.Planked : WallID.Wood;
                        }
                    }
                    WorldGen.TileFrame(i + x, j + y);
                    WorldGen.SquareWallFrame(i + x, j + y);
                    if (y == hauseAreaY - 1 && (x < 2 || x > hauseAreaX - 3))
                    {
                        for (int z = 0; z < 80; z++)
                        {
                            Tile blug = Main.tile[i + x, j + y + z + 1];                            
                            if (blug.HasTile)
                            {
                                break;
                            }
                            else
                            {
                                blug.ResetToType(TileID.WoodenBeam);
                                WorldGen.TileFrame(i + x, j + y + z + 1);
                            }                            
                        }
                    }
                }
            }
            if (first)
            {
                bool right = Main.rand.NextBool();
                int xPoint = right ? i : i + hauseAreaX - 1;
                WorldGen.KillTile(xPoint, j + hauseAreaY - 4, noItem: true);
                WorldGen.KillTile(xPoint, j + hauseAreaY - 3, noItem: true);
                WorldGen.KillTile(xPoint, j + hauseAreaY - 2, noItem: true);
                WorldGen.PlaceTile(xPoint, j + hauseAreaY - 4, TileID.ClosedDoor, true);

                int chestX = i + Main.rand.Next(1, hauseAreaX - 4);
                for (int k = -3; k < 3; k++)
                {
                    WorldGen.KillTile(chestX + k, j + hauseAreaY - 3, noItem: true);
                    WorldGen.KillTile(chestX + k, j + hauseAreaY - 2, noItem: true);
                    Main.tile[chestX + k, j + hauseAreaY - 1].ResetToType((ushort)ModContent.TileType<AsbestosPlaced>());
                }

                int chest = AddAsbestosChest(chestX, j + hauseAreaY - 3);
                if (chest != -1)
                {
                    for (int e = 0; e < Main.chest[chest].item.Length; e++)
                    {
                        Main.chest[chest].item[e].SetDefaults(ModContent.ItemType<Asbestos>());
                        Main.chest[chest].item[e].stack = Main.rand.Next(4, 120);
                    }
                }
            }
        }

        public static bool CanGenHouse(int i, int j, int hauseAreaX, int hauseAreaY)
        {
            for (int x = 0; x < hauseAreaX; x++)
            {
                for (int y = 0; y < hauseAreaY; y++)
                {
                    Tile remp = Main.tile[i + x, j + y];
                    if (remp.HasTile)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void GenerateAllHouses()
        {
            int maxHouses = 15;
            int houseCount = 0;
            for (int tries = 0; tries < 30; tries++)
            {
                if (houseCount > maxHouses)
                {
                    break;
                }
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    if (houseCount > maxHouses)
                    {
                        break;
                    }
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        Tile t = Main.tile[i, j];
                        if (!t.HasTile && t.WallType == ModContent.WallType<AsbestosWallPlaced>() && Main.rand.NextBool(2222))
                        {
                            int minLength = 20;
                            int maxLength = 30;
                            int hauseAreaX = Main.rand.Next(minLength,(int)(maxLength * 1.5f));
                            int hauseAreaY = Main.rand.Next(minLength, (int)MathHelper.Max(maxLength / 2, minLength + 1));

                            if (CanGenHouse(i, j, hauseAreaX, hauseAreaY))
                            {
                                GenerateHouse(i, j, hauseAreaX, hauseAreaY);
                                houseCount++;
                            }
                        }
                        if (houseCount > maxHouses)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public static int AddAsbestosChest(int i, int j)
        {
            for (int k = j; k < Main.maxTilesY - 10; k++)
            {
                if (Main.tile[i, k].HasTile && Main.tileSolid[Main.tile[i, k].TileType] && !Main.tileSolidTop[Main.tile[i, k].TileType])
                {

                    if (Main.tile[i - 1, k].TopSlope)
                    {
                        WorldGen.SlopeTile(i - 1, k, 0);
                }

                    if (Main.tile[i, k].TopSlope)
                    {
                        WorldGen.SlopeTile(i, k, 0);
                    }
                }

                int num5 = 2;
                for (int n = i - num5; n <= i + num5; n++)
                {
                    for (int num6 = k - num5; num6 <= k + num5; num6++)
                    {
                        if (Main.tile[n, num6].HasTile && (TileID.Sets.Boulders[Main.tile[n, num6].TileType] || Main.tile[n, num6].TileType == 26 || Main.tile[n, num6].TileType == 237))
                            return -1;
                    }
                }

                if (!WorldGen.SolidTile(i, k))
                    continue;

                int num7 = k;
                return WorldGen.PlaceChest(i - 1, num7 - 1, style: 15);
            }
            return -1;
        }        
    }
}