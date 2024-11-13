using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Schematics;
using CalamityMod;
using System;
using CalRemix.Content.Items.Placeables;
using Terraria.ID;
using CalamityMod.Items.Potions;
using System.Collections.Generic;
using CalamityMod.Tiles.Astral;
using CalamityMod.Tiles.AstralDesert;
using CalamityMod.Tiles.AstralSnow;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Crags;
using Terraria.WorldBuilding;
using CalRemix.Content.Tiles;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace CalRemix.Core.World
{
    public class AncientConsoleGen : ModSystem
    {

        public static List<int> cragList = new List<int>
        {
            ModContent.TileType<BrimstoneSlag>(),
            ModContent.TileType<ScorchedRemains>(),
            ModContent.TileType<BrimstoneSlab>()
        };
        public static void GenerateConsole()
        {
            int grass = ModContent.TileType<AshenAltar>();
            bool dungeonRight = Main.dungeonX > (int)(Main.maxTilesX * 0.5f);
            bool shouldbreak = false;
            int xMin = dungeonRight ? (int)(Main.maxTilesX * 0.66f) : 0;
            int xMax = dungeonRight ? Main.maxTilesX : (int)(Main.maxTilesX * 0.33f);
            for (int i = xMin; i < xMax; i++)
            {
                if (shouldbreak)
                    break;
                for (int j = Main.UnderworldLayer; j < Main.maxTilesY; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t.TileType == grass)
                    {
                        for (int k = i; k < i + 3; k++)
                        {
                            for (int l = j; l < j + 3; l++)
                            {
                                Main.tile[k, l].Get<TileWallWireStateData>().HasTile = false;
                            }
                        }
                        WorldGen.PlaceTile(i + 1, j + 2, ModContent.TileType<Content.Tiles.AncientConsole>(), forced: true);
                        shouldbreak = true;
                        break;
                    }
                }
            }
            if (!shouldbreak)
            {
                CalRemix.instance.Logger.Error("Could not place Ancient Console!");
            }
        }
    }
}