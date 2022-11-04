using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using System.IO;
using CalamityMod.World;
using System;
using CalamityMod.Items.Materials;
using CalRemix.Tiles;
using CalamityMod.Tiles.Astral;
using CalamityMod.Tiles.FurnitureMonolith;
using CalRemix.Items.Placeables;
using CalamityMod.Walls;
using CalamityMod;
using Microsoft.Xna.Framework;
using static CalRemix.CalRemixGlobalNPC;

namespace CalRemix
{
    public class CalRemixWorld : ModSystem
    {
        public static int lifeTiles;
        public int ShrineTimer = 1200;
        public bool GeneratedShrines = false;

        public override void PostUpdateWorld()
        {
            if (Main.hardMode)
            {
                if (ShrineTimer <= 0)
                {
                    if (!GeneratedShrines)
                    {
                        HMChest(TileID.CrystalBlock, TileID.CrystalBlock, WallID.Crystal, ModContent.ItemType<HallowEffigy>(), TileID.Pearlstone, 21);
                        HMChest(ModContent.TileType<AstralMonolith>(), ModContent.TileType<AstralMonolith>(), ModContent.WallType<AstralMonolithWall>(), ModContent.ItemType<AstralEffigy>(), ModContent.TileType<AstralStone>(), 46);

                        Color messageColor = Color.Magenta;
                        CalamityUtils.DisplayLocalizedText("Shrines appear within the newly spread infections!", messageColor);

                        GeneratedShrines = true;
                    }
                }
                else
                {
                    ShrineTimer--;
                }
            }
        }

        public override void ResetNearbyTileEffects()
        {
            lifeTiles = 0;
        }
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            // Life Ore tiles
            lifeTiles = tileCounts[TileType<LifeOreTile>()];
        }

        public static void HMChest(int block1, int block2, int wall, int loot, int anchor, int chest)
        {
            int x = Main.maxTilesX;
            int y = Main.maxTilesY;
            for (int k = 0; k < (int)(x * y * 100E-05); k++)
            {
                int tilesX = WorldGen.genRand.Next(0, x);
                int tilesY = WorldGen.genRand.Next((int)(y * 0.35f), (int)(y * 0.5f));

                if (Main.tile[tilesX, tilesY].TileType == anchor)
                {
                    UndergroundShrines.SpecialHut((ushort)block1, (ushort)block2, (ushort)wall, UndergroundShrines.UndergroundShrineType.Surface, tilesX, tilesY);
                    for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
                    {
                        Chest cheste = Main.chest[chestIndex];
                        if (cheste != null && Main.tile[cheste.x, cheste.y].TileType == TileID.Containers && Main.tile[cheste.x, cheste.y + 2].TileType == block1)
                        {
                            if (block1 == TileID.CrystalBlock)
                            {
                                Main.tile[cheste.x, cheste.y].TileType = TileID.Containers2;
                                Main.tile[cheste.x + 1, cheste.y].TileType = TileID.Containers2;
                                Main.tile[cheste.x, cheste.y + 1].TileType = TileID.Containers2;
                                Main.tile[cheste.x + 1, cheste.y + 1].TileType = TileID.Containers2;
                            }
                            if (block1 == ModContent.TileType<AstralMonolith>())
                            {
                                Main.tile[cheste.x, cheste.y].TileType = (ushort)ModContent.TileType<MonolithChest>();
                                Main.tile[cheste.x + 1, cheste.y].TileType = (ushort)ModContent.TileType<MonolithChest>();
                                Main.tile[cheste.x, cheste.y + 1].TileType = (ushort)ModContent.TileType<MonolithChest>();
                                Main.tile[cheste.x + 1, cheste.y + 1].TileType = (ushort)ModContent.TileType<MonolithChest>();
                            }
                            cheste.item[0].SetDefaults(loot);
                        }
                    }
                    break;
                }
            }
        }
    }
}