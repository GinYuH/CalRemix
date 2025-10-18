using Terraria;
using Terraria.ModLoader;
using CalamityMod;
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
                GenerateDoorRandom(doorTypes[d]);
            }
        }

        public static void GenerateDoorRandom(int type)
        {
            bool shouldbreak = false;
            int boundX = 100;
            int boundY = 40;
            int worldSize = (int)(0.01f * Main.maxTilesX * Main.maxTilesY);
            for (int att = 0; att < 200; att++)
            {
                if (shouldbreak)
                {
                    break;
                }
                for (int i = boundX; i < Main.maxTilesX - boundX; i++)
                {
                    if (shouldbreak)
                    {
                        break;
                    }
                    for (int j = boundY; j < Main.maxTilesY - boundY; j++)
                    {
                        if (shouldbreak)
                        {
                            break;
                        }
                        if (Main.rand.NextBool(worldSize))
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
                                        if (u == null || u.HasTile)
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
                                    WorldGen.PlaceTile(i + 1, j - 1, type);
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