using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using System.Collections.Generic;
using CalRemix.Content.Tiles;
using Microsoft.Xna.Framework;

namespace CalRemix.Core.World
{
    public class SubworldDoorGeneration : ModSystem
    {
        public static bool GenerateDoor(SubworldDoorPlaced.SubworldType type, int x, int y)
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
                    t.ResetToType(t.TileType);
                    next.ResetToType(next.TileType);
                    WorldGen.PlaceTile(x + 1, y - 1, ModContent.TileType<SubworldDoorPlaced>(), true);
                    SubworldDoorPlaced.PlaceSubworldDoor(x + 1, y - 1, type);
                    return true;
                }
            }
            return false;
        }

        public static void GenerateDoorRandom(SubworldDoorPlaced.SubworldType type)
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
                                    SubworldDoorPlaced.PlaceSubworldDoor(i + 1, j - 1, type);
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