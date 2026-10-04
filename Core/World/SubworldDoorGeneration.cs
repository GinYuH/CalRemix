using CalamityMod;
using CalRemix.Content.Tiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.World
{
    public class SubworldDoorGeneration : ModSystem
    {
        public static Dictionary<SubworldType, (string, string, Color)> subworldDoorData = new()
        {
            { SubworldType.Ant, ("AntSubworld", "Ant", Color.DarkGray) },
            { SubworldType.Bridge, ("BridgeofLostHopeSubworld", "Bridge", Color.Firebrick) },
            { SubworldType.Glamour, ("GlamourSubworld", "Glamour", Color.HotPink) },
            { SubworldType.GreatSea, ("GreatSeaSubworld", "GreatSea", Color.DeepSkyBlue) },
            { SubworldType.Horizon, ("HorizonSubworld", "Horizon", Color.Tan) },
            { SubworldType.Nightline, ("NightlineSubworld", "Nightline", Color.DarkBlue) },
            { SubworldType.Nowhere, ("NowhereSubworld", "Nowhere", Color.White) },
            { SubworldType.Jungle, ("OvergrowthRainforestSubworld", "OvergrowthJungle", Color.ForestGreen) },
            { SubworldType.Overworld, ("", "Overworld", Color.LawnGreen) },
            { SubworldType.Pinnacles, ("PinnaclesSubworld", "Pinnacles", Color.Gray) },
            { SubworldType.Savanna, ("SavannaSubworld", "Savanna", Color.IndianRed) },
            { SubworldType.Screaming, ("ScreamingSubworld", "ScreamingFace", Color.DimGray) },
            { SubworldType.Sealed, ("SealedSubworld", "Sealed", Color.Purple) },
            { SubworldType.Gray, ("TheGraySubworld", "TheGray", Color.Black) },
            { SubworldType.Virisite, ("SingularPointSubworld", "Virisite", Color.LightSeaGreen) },
            { SubworldType.Wolf, ("WolfForestSubworld", "Wolf", Color.LightBlue) }
        };

        public static bool GenerateDoor(SubworldType type, int x, int y)
        {
            Tile t = CalRemixHelper.ParanoidTileRetrieval(x, y);
            Tile next = CalRemixHelper.ParanoidTileRetrieval(x + 1, y);
            if (t != null && t.HasTile && t.IsTileSolidGround() && next != null && next.HasTile && next.IsTileSolidGround())
            {
                bool emptySpace = true;
                for (int k = x; k < x + 2; k++)
                {
                    for (int l = y - 1; l > y - 4; l--)
                    {
                        Tile u = CalRemixHelper.ParanoidTileRetrieval(k, l);
                        if (u == null || u.HasTile)
                        {
                            emptySpace = false;
                            break;
                        }
                    }
                }
                if (emptySpace)
                {
                    WorldGen.SlopeTile(x, y);
                    WorldGen.SlopeTile(x + 1, y);
                    WorldGen.PlaceTile(x + 1, y - 1, ModContent.TileType<SubworldDoorPlaced>(), true);
                    PlaceSubworldDoor(x + 1, y - 1, type);
                    return true;
                }
            }
            return false;
        }

        public static void GenerateDoorRandom(SubworldType type)
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
                            Tile t = CalRemixHelper.ParanoidTileRetrieval(i, j);
                            Tile next = CalRemixHelper.ParanoidTileRetrieval(i + 1, j);
                            if (t != null && t.HasTile && t.IsTileSolidGround() && next != null && next.HasTile && next.IsTileSolidGround())
                            {
                                bool emptySpace = true;
                                for (int k = i; k < i + 2; k++)
                                {
                                    for (int l = j - 1; l > j - 4; l--)
                                    {
                                        Tile u = CalRemixHelper.ParanoidTileRetrieval(k, l);
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
                                    WorldGen.PlaceTile(i + 1, j - 1, ModContent.TileType<SubworldDoorPlaced>());
                                    PlaceSubworldDoor(i + 1, j - 1, type);
                                    shouldbreak = true;
                                }
                                break;
                            }
                        }
                    }
                }                
            }
        }

        public static void PlaceSubworldDoor(int i, int j, SubworldType key)
        {
            TileEntity.PlaceEntityNet(i - 1, j - 2, ModContent.TileEntityType<SubworldDoorTE>());
            if (TileEntity.ByPosition.TryGetValue(new Point16(i - 1, j - 2), out TileEntity tE))
            {
                if (tE is SubworldDoorTE subDoor)
                {
                    (string, string, Color) data = subworldDoorData[key];
                    subDoor.boundSubworldName = "CalRemix/" + data.Item1;
                    subDoor.texture = "CalRemix/UI/SubworldMap/" + data.Item2;
                    subDoor.doorColor = data.Item3;
                }
            }
            CalRemixHelper.AddProtectedStructure(new Rectangle(i - 1, j - 2, 2, 4));
        }
    }
}