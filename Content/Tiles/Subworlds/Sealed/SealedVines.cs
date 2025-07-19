using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles.Subworlds.Sealed
{
    public class SealedVines : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileCut[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoFail[Type] = true;
            TileID.Sets.IsVine[Type] = true;
            TileID.Sets.VineThreads[Type] = true;
            TileID.Sets.ReplaceTileBreakDown[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
            AddMapEntry(new Color(168, 99, 166));
            HitSound = SoundID.Grass;
            DustType = DustID.Shadewood_Tree;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Main.instance.TilesRenderer.CrawlToTopOfVineAndAddSpecialPoint(j, i);
            return false;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = -2;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (WorldGen.loadSuccess)
            {
                Tile tileAbove = Framing.GetTileSafely(i, j - 1);
                if (!tileAbove.HasTile)
                {
                    WorldGen.KillTile(i, j);
                    return true;
                }
            }
            return true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            // GIVE VINE ROPE IF SPECIAL VINE BOOK
            if (WorldGen.genRand.NextBool() && Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].cordage)
                Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8f, j * 16 + 8f), ItemID.VineRope);

            if (Main.tile[i, j + 1] != null)
            {
                if (Main.tile[i, j + 1].HasTile)
                {
                    if (Main.tile[i, j + 1].TileType == ModContent.TileType<SealedVines>())
                    {
                        WorldGen.KillTile(i, j + 1, false, false, false);
                        if (!Main.tile[i, j + 1].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j + 1, 0f, 0, 0, 0);
                    }
                }
            }
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.tile[i, j + 1] != null)
            {
                if (!Main.tile[i, j + 1].HasTile && Main.tile[i, j + 1].TileType != (ushort)ModContent.TileType<SealedVines>())
                {
                    if (Main.tile[i, j + 1].LiquidAmount >= 128 && Main.tile[i, j + 1].LiquidType != LiquidID.Lava)
                    {
                        bool canGrowVine = false;
                        for (int k = j; k > j - 10; j--)
                        {
                            if (Main.tile[i, k].BottomSlope)
                            {
                                canGrowVine = false;
                                break;
                            }
                            if (Main.tile[i, k].HasTile && !Main.tile[i, k].BottomSlope)
                            {
                                canGrowVine = true;
                                break;
                            }
                        }
                        if (canGrowVine)
                        {
                            int vineX = i;
                            int vineY = j + 1;
                            Main.tile[vineX, vineY].TileType = (ushort)ModContent.TileType<SealedVines>();
                            Main.tile[vineX, vineY].TileFrameX = (short)(WorldGen.genRand.Next(8) * 18);
                            Main.tile[vineX, vineY].TileFrameY = (short)(4 * 18);
                            Main.tile[vineX, vineY - 1].TileFrameX = (short)(WorldGen.genRand.Next(12) * 18);
                            Main.tile[vineX, vineY - 1].TileFrameY = (short)(WorldGen.genRand.Next(4) * 18);
                            Main.tile[vineX, vineY].Get<TileWallWireStateData>().HasTile = true;
                            WorldGen.SquareTileFrame(vineX, vineY, true);
                            WorldGen.SquareTileFrame(vineX, vineY - 1, true);
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendTileSquare(-1, vineX, vineY, 3, TileChangeType.None);
                                NetMessage.SendTileSquare(-1, vineX, vineY - 1, 3, TileChangeType.None);
                            }
                        }
                    }
                }
            }
        }
    }

    public class CarnelianVines : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileCut[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoFail[Type] = true;
            TileID.Sets.IsVine[Type] = true;
            TileID.Sets.VineThreads[Type] = true;
            TileID.Sets.ReplaceTileBreakDown[Type] = true;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
            AddMapEntry(new Color(176, 26, 54));
            HitSound = SoundID.Grass;
            DustType = DustID.RedMoss;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Main.instance.TilesRenderer.CrawlToTopOfVineAndAddSpecialPoint(j, i);
            return false;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = -2;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (WorldGen.loadSuccess)
            {
                Tile tileAbove = Framing.GetTileSafely(i, j - 1);
                if (!tileAbove.HasTile)
                {
                    WorldGen.KillTile(i, j);
                    return true;
                }
            }
            return true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            // GIVE VINE ROPE IF SPECIAL VINE BOOK
            if (WorldGen.genRand.NextBool() && Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].cordage)
                Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8f, j * 16 + 8f), ItemID.VineRope);

            if (Main.tile[i, j + 1] != null)
            {
                if (Main.tile[i, j + 1].HasTile)
                {
                    if (Main.tile[i, j + 1].TileType == ModContent.TileType<CarnelianVines>())
                    {
                        WorldGen.KillTile(i, j + 1, false, false, false);
                        if (!Main.tile[i, j + 1].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j + 1, 0f, 0, 0, 0);
                    }
                }
            }
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.tile[i, j + 1] != null)
            {
                if (!Main.tile[i, j + 1].HasTile && Main.tile[i, j + 1].TileType != (ushort)ModContent.TileType<CarnelianVines>())
                {
                    if (Main.tile[i, j + 1].LiquidAmount >= 128 && Main.tile[i, j + 1].LiquidType != LiquidID.Lava)
                    {
                        bool canGrowVine = false;
                        for (int k = j; k > j - 10; j--)
                        {
                            if (Main.tile[i, k].BottomSlope)
                            {
                                canGrowVine = false;
                                break;
                            }
                            if (Main.tile[i, k].HasTile && !Main.tile[i, k].BottomSlope)
                            {
                                canGrowVine = true;
                                break;
                            }
                        }
                        if (canGrowVine)
                        {
                            int vineX = i;
                            int vineY = j + 1;
                            Main.tile[vineX, vineY].TileType = (ushort)ModContent.TileType<CarnelianVines>();
                            Main.tile[vineX, vineY].TileFrameX = (short)(WorldGen.genRand.Next(8) * 18);
                            Main.tile[vineX, vineY].TileFrameY = (short)(4 * 18);
                            Main.tile[vineX, vineY - 1].TileFrameX = (short)(WorldGen.genRand.Next(12) * 18);
                            Main.tile[vineX, vineY - 1].TileFrameY = (short)(WorldGen.genRand.Next(4) * 18);
                            Main.tile[vineX, vineY].Get<TileWallWireStateData>().HasTile = true;
                            WorldGen.SquareTileFrame(vineX, vineY, true);
                            WorldGen.SquareTileFrame(vineX, vineY - 1, true);
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendTileSquare(-1, vineX, vineY, 3, TileChangeType.None);
                                NetMessage.SendTileSquare(-1, vineX, vineY - 1, 3, TileChangeType.None);
                            }
                        }
                    }
                }
            }
        }
    }
}
