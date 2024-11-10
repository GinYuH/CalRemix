using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Schematics;
using CalamityMod;
using System;
using CalRemix.Content.Items.Placeables;
using Terraria.ID;
using CalamityMod.Items.Potions;

namespace CalRemix.Core.World
{
    public class HallowShrine : ModSystem
    {
        public static void GenerateHallowShrine()
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
                    for (int j = 200; j < Main.maxTilesY - 200; j++)
                    {
                        if (shouldbreak)
                        {
                            break;
                        }
                        if (Main.rand.NextBool(2222))
                        {
                            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                            if (t != null && t.HasTile && (t.TileType == TileID.Pearlstone || t.TileType == TileID.HallowSandstone || t.TileType == TileID.HallowedIce))
                            {
                                bool _ = false;
                                SchematicManager.PlaceSchematic("Hallow Shrine", new Point(i, j), SchematicAnchor.CenterLeft, ref _, new Action<Chest, int, bool>(FillHallowChest));
                                shouldbreak = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static void FillHallowChest(Chest c, int Type, bool place)
        {
            c.item[0].SetDefaults(ModContent.ItemType<HallowEffigy>());
            c.item[1].SetDefaults(ItemID.SoulofLight);
            c.item[1].stack = Main.rand.Next(24, 29);
            c.item[2].SetDefaults(ItemID.HallowedTorch);
            c.item[2].stack = Main.rand.Next(100, 111);
            c.item[3].SetDefaults(ItemID.PlatinumCoin);
            c.item[3].stack = Main.rand.Next(1, 5);
            c.item[4].SetDefaults(ItemID.GreaterHealingPotion);
            c.item[4].stack = Main.rand.Next(10, 13);
            c.item[5].SetDefaults(ItemID.ShinePotion);
            c.item[5].stack = Main.rand.Next(10, 13);
            c.item[6].SetDefaults(ModContent.ItemType<SoaringPotion>());
            c.item[6].stack = Main.rand.Next(10, 13);
            c.item[7].SetDefaults(ModContent.ItemType<PhotosynthesisPotion>());
            c.item[7].stack = Main.rand.Next(10, 13);
        }
    }
}