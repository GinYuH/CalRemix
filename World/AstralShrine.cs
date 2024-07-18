using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Schematics;
using CalamityMod;
using CalamityMod.Tiles.Abyss;
using System;
using CalRemix.Tiles;
using Terraria.DataStructures;
using CalRemix.Items.Placeables;
using Terraria.ID;
using CalamityMod.Items.Potions;
using Terraria.WorldBuilding;
using System.Collections.Generic;
using CalamityMod.Tiles.Astral;
using CalamityMod.Tiles.AstralDesert;
using CalamityMod.Tiles.AstralSnow;
using CalamityMod.Items.Materials;

namespace CalRemix.World
{
    public class AstralShrine : ModSystem
    {

        public static List<int> astrallist = new List<int>
        {
            ModContent.TileType<AstralStone>(),
            ModContent.TileType<AstralSandstone>(),
            ModContent.TileType<HardenedAstralSand>(),
            ModContent.TileType<CelestialRemains>(),
            ModContent.TileType<NovaeSlag>(),
            ModContent.TileType<AstralDirt>(),
            ModContent.TileType<AstralIce>(),
            ModContent.TileType<AstralSnow>(),
            ModContent.TileType<AstralGrass>(),
            ModContent.TileType<AstralClay>(),
            ModContent.TileType<AstralSand>(),
            ModContent.TileType<AstralMonolith>(),
        };
        public static void GenerateAstralShrine()
        {
            if (!Main.hardMode)
                return;
            bool shouldbreak = false;
            for (int att = 0; att < 200; att++)
            {
                if (shouldbreak)
                {
                    break;
                }
                for (int i = 200; i < Main.maxTilesX - 200; i++)
                {
                    if (shouldbreak)
                    {
                        break;
                    }
                    for (int j = (int)(Main.worldSurface); j < Main.maxTilesY - 100; j++)
                    {
                        if (shouldbreak)
                        {
                            break;
                        }
                        if (Main.rand.NextBool(2222))
                        {
                            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                            if (t != null && t.HasTile && astrallist.Contains(t.TileType))
                            {
                                bool _ = false;
                                SchematicManager.PlaceSchematic("Hallow Shrine", new Point(i, j), SchematicAnchor.CenterLeft, ref _, new Action<Chest, int, bool>(FillAstralChest));
                                shouldbreak = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static void FillAstralChest(Chest c, int Type, bool place)
        {
            c.item[0].SetDefaults(ModContent.ItemType<AstralEffigy>());
            c.item[1].SetDefaults(ModContent.ItemType<StarblightSoot>());
            c.item[1].stack = Main.rand.Next(24, 29);
            c.item[2].SetDefaults(ModContent.ItemType<CalamityMod.Items.Placeables.Furniture.AstralTorch>());
            c.item[2].stack = Main.rand.Next(100, 111);
            c.item[3].SetDefaults(ItemID.PlatinumCoin);
            c.item[3].stack = Main.rand.Next(1, 5);
            c.item[4].SetDefaults(ItemID.GreaterHealingPotion);
            c.item[4].stack = Main.rand.Next(10, 13);
            c.item[5].SetDefaults(ItemID.MagicPowerPotion);
            c.item[5].stack = Main.rand.Next(10, 13);
            c.item[6].SetDefaults(ModContent.ItemType<GravityNormalizerPotion>());
            c.item[6].stack = Main.rand.Next(10, 13);
            c.item[7].SetDefaults(ModContent.ItemType<AstralInjection>());
            c.item[7].stack = Main.rand.Next(10, 13);
        }
    }
}