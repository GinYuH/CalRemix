using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;
using System;
using CalRemix.Content.Items.Placeables;
using Terraria.ID;
using CalamityMod.Items.Potions;
using Terraria.DataStructures;

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
                                CalRemixHelper.PlaceSchematic("Hallow Shrine", new Point(i, j), CalRemixHelper.SchematicAnchorType.BottomLeft);
                                shouldbreak = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}