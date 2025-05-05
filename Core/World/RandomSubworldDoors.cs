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
using CalRemix.Content.Tiles;

namespace CalRemix.Core.World
{
    public class RandomSubworldDoors : ModSystem
    {
        public static List<int> doorTypes = new List<int>()
        {
            ModContent.TileType<ExosphereDoor>(),
            ModContent.TileType<BaronDoor>(),
        };

        public static void GenerateRandomSubworldDoors()
        {
            for (int d = 0; d < doorTypes.Count; d++)
            {
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
                            if (Main.rand.NextBool(222222))
                            {
                                Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                                Tile next = CalamityUtils.ParanoidTileRetrieval(i + 1, j);
                                if (t != null && t.HasTile && t.IsTileSolidGround() && next != null && next.HasTile && next.IsTileSolidGround())
                                {
                                    bool emptySpace = true;
                                    for (int k = i; k < i + 2; k++)
                                    {
                                        for (int l = j - 1; l > j - 4; l--)
                                        {
                                            Tile u = CalamityUtils.ParanoidTileRetrieval(k, l);
                                            if (u == null || u.HasTile || u.LiquidAmount > 0)
                                            {
                                                emptySpace = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (emptySpace)
                                    {
                                        t.ResetToType(t.TileType);
                                        next.ResetToType(next.TileType);
                                        WorldGen.PlaceTile(i + 1, j - 1, doorTypes[d]);
                                        //Main.LocalPlayer.position = new Vector2(i, j - 3) * 16;
                                        shouldbreak = true;
                                    }
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