using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Schematics;
using CalamityMod;
using System;
using Terraria;
using Terraria.ID;

namespace CalRemix.Core.World
{
    public class OrigenWorkshop : ModSystem
    {
        public static void GenerateOrigenWorkshop()
        {
            bool generated = false;
            for (int h = 0; h < 100; h++)
            {
                if (generated)
                    break;
                for (int i = 100; i < Main.maxTilesX - 100; i++)
                {
                    if (generated)
                        break;
                    for (int j = 22; j < Main.maxTilesY / 2f; j++)
                    {
                        if (generated)
                            break;
                        if (Main.rand.NextBool(100))
                        {
                            Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                            if (!t.HasTile)
                                continue;
                            if (t.TileType != TileID.SnowBlock)
                                continue;
                            bool enoughAir = true;
                            int blocksToCheck = 80; // lol !
                            for (int k = 1; k < blocksToCheck; k++)
                            {
                                if (Main.tile[i, j - k].HasTile || Main.tile[i, j - k].LiquidAmount > 10)
                                {
                                    enoughAir = false;
                                    break;
                                }
                            }
                            if (!enoughAir)
                            {
                                continue;
                            }

                            bool _ = false;
                            SchematicManager.PlaceSchematic<Action<Chest>>("Origen Workshop", new Point(i, j), SchematicAnchor.CenterLeft, ref _);
                            generated = true;
                        }
                    }
                }
            }
            if (!generated)
            {
                CalRemix.instance.Logger.Error("Origen workshop failed to generate!");
            }
        }
    }
}