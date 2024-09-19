using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalRemix.Content.Tiles;
using System.Threading;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.DraedonStructures;
using CalamityMod.World;
using CalamityMod.Tiles.SunkenSea;

namespace CalRemix.Core.World
{
    public class MeldStrain : ModSystem
    {

        public static void GenerateCavernStrain()
        {
            int widthdiv2 = 16;
            int heightdiv2 = 22;
            bool gennedMeld = false;
            Vector2 meldCoords = Vector2.Zero;
            int ymin = Main.remixWorld ? (int)(Main.maxTilesY * 0.4f) : (int)(Main.maxTilesY * 0.6f);
            int ymax = Main.remixWorld ? (int)(Main.maxTilesY * 0.6f) : Main.UnderworldLayer - 100;
            for (int loop = 0; loop < 200; loop++)
            {
                if (gennedMeld)
                    break;
                for (int x = (int)(Main.maxTilesX * 0.2f); x < Main.maxTilesX * 0.8f; x++)
                {
                    if (gennedMeld)
                        break;
                    if (x > Main.maxTilesX * 0.4f && x < Main.maxTilesX * 0.6f)
                        continue;
                    for (int y = ymin; y < ymax; y++)
                    {
                        if (gennedMeld)
                            break;
                        if (Main.rand.NextBool(2222222))
                        {
                            if (widthdiv2 * 2 > Main.maxTilesX - 100 || heightdiv2 * 2 > Main.maxTilesY - 100)
                                continue;
                            if (x - widthdiv2 < 100 || y - heightdiv2 < 100)
                                continue;
                            bool canGen = true;
                            for (int m = x - 100; m < x + 100; m++)
                            {
                                if (!canGen)
                                    break;
                                for (int n = y - 100; n < y + 100; n++)
                                {
                                    if (!canGen)
                                        break;
                                    Tile t = Main.tile[m, n];
                                    if (WorldGen.InWorld(m, n, 1))
                                    {
                                        if (t.TileType == TileID.StoneSlab || t.TileType == TileType<LaboratoryPlating>() || t.TileType == TileType<LaboratoryPanels>() || t.TileType == TileType<RustedPipes>() || TileID.Sets.IsAContainer[t.TileType] || TileID.Sets.AvoidedByMeteorLanding[t.TileType] || t.TileType == TileID.LihzahrdBrick || Main.tileDungeon[t.TileType] || t.TileType == TileType<Navystone>() || t.TileType == TileID.JungleGrass)
                                        {
                                            canGen = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (canGen)
                            {
                                if (WorldGen.InWorld(x, y, 1))
                                {
                                    PlaceMeldHeart(x, y, 16, 22);
                                    meldCoords = new Vector2(x, y);
                                    gennedMeld = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (meldCoords != Vector2.Zero)
            {
                ThreadPool.QueueUserWorkItem(_ => AstralMeld(meldCoords));
                //AstralMeld(meldCoords);
            }

        }

        public static void AstralMeld(Vector2 meldCoords)
        {
            int planetradius = 56;
            for (int p = (int)meldCoords.X - planetradius; p < (int)meldCoords.X + planetradius; p++)
            {
                for (int q = (int)meldCoords.Y - planetradius; q < (int)meldCoords.Y + planetradius; q++)
                {
                    int dist = (p - (int)meldCoords.X) * (p - (int)meldCoords.X) + (q - (int)meldCoords.Y) * (q - (int)meldCoords.Y);
                    if (dist > planetradius * planetradius)
                        continue;

                    if (WorldGen.InWorld(p, q, 1) && Main.tile[p, q].HasTile)
                    {
                        AstralBiome.ConvertToAstral(p, q, true);
                    }
                }
            }
        }

        public static void PlaceMeldHeart(int x, int y, int width, int height)
        {
            // cut off at the top octagon
            for (int i = x - width; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    if (WorldGen.InWorld(i, j, 1) && Math.Abs(i - x) + Math.Abs(j - y) < Math.Sqrt(width * width + height * height))
                    {
                        if (WorldGen.CheckTileBreakability(i, j) == 0)
                        {
                            if (Main.tile[i, j].HasTile)
                            {
                                Main.tile[i, j].TileType = (ushort)TileType<MeldGunkPlaced>();
                                Main.tile[i, j].Get<TileWallWireStateData>().Slope = SlopeType.Solid;
                                Main.tile[i, j].Get<TileWallWireStateData>().IsHalfBlock = false;
                                Main.tile[i, j].ClearBlockPaintAndCoating();
                                Main.tile[i, j].LiquidAmount = 0;
                                WorldGen.SquareTileFrame(i, j, true);
                                NetMessage.SendTileSquare(-1, i, j, 1);
                            }
                            else
                            {
                                WorldGen.PlaceTile(i, j, TileType<MeldGunkPlaced>(), true, true);
                                WorldGen.SquareTileFrame(i, j, true);
                                NetMessage.SendTileSquare(-1, i, j, 1);
                            }
                        }
                    }
                }
            }
            RightTriangleGen(TileType<MeldGunkPlaced>(), x - width, y, (int)(width * 0.7f) * 2, (int)(height * 1.0f));
            RightTriangleGen(TileType<MeldGunkPlaced>(), x, y + (int)(height * 0.22f), (int)(width * 0.5f) * 2, (int)(height * 0.7f));
        }

        // at the moment this only supports a /| angled triangle
        public static void RightTriangleGen(int blockType, int x, int y, int width, int height)
        {
            float slope = -(float)height / width;
            float b = y - slope * x;
            for (int i = x; i < x + width; i++)
            {
                for (int j = y - height; j < y; j++)
                {
                    if (j >= slope * i + b)
                    {
                        if (WorldGen.CheckTileBreakability(i, j) == 0)
                        {
                            if (Main.tile[i, j].HasTile)
                            {
                                Main.tile[i, j].TileType = (ushort)blockType;
                                Main.tile[i, j].Get<TileWallWireStateData>().Slope = SlopeType.Solid;
                                Main.tile[i, j].Get<TileWallWireStateData>().IsHalfBlock = false;
                                Main.tile[i, j].ClearBlockPaintAndCoating();
                                Main.tile[i, j].LiquidAmount = 0;
                                WorldGen.SquareTileFrame(i, j, true);
                                NetMessage.SendTileSquare(-1, i, j, 1);
                            }
                            else
                            {
                                WorldGen.PlaceTile(i, j, blockType, true, true);
                                WorldGen.SquareTileFrame(i, j, true);
                                NetMessage.SendTileSquare(-1, i, j, 1);
                            }
                        }
                    }
                }
            }
        }
    }
}