using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.World;
using CalRemix.Core.Backgrounds.Plague;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Tiles;
using CalRemix.Content.Tiles.PlaguedJungle;
using System;
using SubworldLibrary;
using CalRemix.Core.Subworlds;
using CalRemix.Content.NPCs.Bosses.Carcinogen;

namespace CalRemix.Core.World
{
    public class PlagueGeneration : ModSystem
    {
        public static void GeneratePlague()
        {
            bool gennedplague = false;
            int plagueX = 0;
            int plagueY = 0;
            int plagueY2 = 0;
            if (CalamityWorld.JungleLabCenter != Vector2.Zero)
            {
                PlaguedSpray.Convert((int)(CalamityWorld.JungleLabCenter.X / 16), (int)(CalamityWorld.JungleLabCenter.Y / 16), 111 * (WorldGen.GetWorldSize() + 1));
                plagueX = (int)(CalamityWorld.JungleLabCenter.X / 16);
                plagueY = (int)(CalamityWorld.JungleLabCenter.Y / 16);
                gennedplague = true;
            }
            if (!gennedplague)
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        if (Main.tile[i, j].TileType == TileID.Mud && Main.rand.NextBool(2222))
                        {
                            PlaguedSpray.Convert(i, j, 111 * (WorldGen.GetWorldSize() + 1));
                            plagueX = i;
                            plagueY = j;
                            gennedplague = true;
                            break;
                        }
                        if (gennedplague)
                        {
                            break;
                        }
                    }
                    if (gennedplague)
                    {
                        break;
                    }
                }
            }
            if (gennedplague)
            {
                for (int j = 100; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[plagueX, j].HasTile && !Main.tile[plagueX, j - 1].HasTile)
                    {
                        if (Main.tile[plagueX, j].TileType == TileID.Mud || Main.tile[plagueX, j].TileType == TileID.JungleGrass)
                        {
                            PlaguedSpray.Convert(plagueX, j, 55 * (WorldGen.GetWorldSize() + 1));
                            plagueY2 = j;
                            break;
                        }
                    }
                }
                for (int i = plagueX - 15 * (WorldGen.GetWorldSize() + 1); i < 15 * (WorldGen.GetWorldSize() + 1) + plagueX; i++)
                {
                    for (int j = plagueY2; j < plagueY; j++)
                    {
                        PlaguedSpray.Convert(i, j, 2);
                    }
                }
                // this is AWFUL
                for (int a = 0; a < Main.maxTilesX; a++)
                {
                    for (int b = 0; b < Main.maxTilesY; b++)
                    {
                        if (Main.tile[a, b].TileType == TileID.Larva && Main.tile[a, b].WallType == ModContent.WallType<PlaguedHiveWall>())
                        {
                            CalRemix.instance.Logger.Info("LARVA SPOTTED. Attempting to place Plagued Larva");
                            bool amongUs = false;
                            for(int c = 0; c <= 2; c++) // weird hive gen can create slight variations in where the larva generates, this is needed
                            {
                                WorldGen.PlaceTile(a + c, b + 1, ModContent.TileType<PlaguedLarva>(), forced: true);
                                if (Main.tile[a + c, b + 1].TileType == ModContent.TileType<PlaguedLarva>())
                                {
                                    NetMessage.SendTileSquare(-1, a + c, b + 1, 4);
                                    amongUs = true;
                                    break;
                                }
                                if (amongUs) break;
                            }
                        }
                    }
                }
                CalRemixWorld.generatedPlague = true;
            }
        }
    }
}