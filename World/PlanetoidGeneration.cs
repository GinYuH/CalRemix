using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalamityMod;
using CalRemix.Tiles;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.Ores;

namespace CalRemix.World
{
    public class PlanetoidGeneration : ModSystem
    {
        public static void GenerateCosmiliteSlag()
        {
            CalamityMod.World.Planets.LuminitePlanet.GenerateLuminitePlanetoids(); // MORE
            int minCloud = 0;
            bool planetsexist = false;
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = (int)(Main.maxTilesY * 0.6f); j > 0; j--)
                {
                    if (Main.tile[i, j].TileType == TileType<ExodiumOre>())
                    {
                        planetsexist = true;
                        minCloud = j;
                        break;
                    }
                    if (minCloud != 0)
                        break;
                }
            }
            bool cutitNOW = false;
            for (int loo = 0; loo < 200; loo++)
            {
                if (cutitNOW)
                {
                    break;
                }

                if (planetsexist)
                {
                    for (int i = 0; i < Main.maxTilesX; i++)
                    {
                        for (int j = 0; j < minCloud; j++)
                        {
                            if (Main.rand.NextBool(75))
                            {
                                if (Main.tile[i, j].TileType == TileID.LunarOre || Main.tile[i, j].TileType == TileType<ExodiumOre>())
                                {
                                    int planetradius = Main.rand.Next(4, 7);
                                    for (int p = i - planetradius; p < i + planetradius; p++)
                                    {
                                        for (int q = j - planetradius; q < j + planetradius; q++)
                                        {
                                            int dist = (p - i) * (p - i) + (q - j) * (q - j);
                                            if (dist > planetradius * planetradius)
                                                continue;

                                            if (WorldGen.InWorld(p, q, 1) && Main.tile[p, q].HasTile)
                                            {
                                                if (Main.tile[p, q].TileType == TileID.LunarOre || Main.tile[p, q].TileType == TileType<ExodiumOre>())
                                                {
                                                    Main.tile[p, q].TileType = (ushort)TileType<CosmiliteSlagPlaced>();

                                                    WorldGen.SquareTileFrame(p, q, true);
                                                    NetMessage.SendTileSquare(-1, p, q, 1);
                                                    cutitNOW = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (Main.rand.NextBool(222))
                            {
                                int planetradius = Main.rand.Next(2, 5);
                                if (Main.tile[i, j].TileType == TileID.Dirt || Main.tile[i, j].TileType == TileID.Stone || Main.tile[i, j].TileType == TileID.Grass || TileID.Sets.Ore[Main.tile[i, j].TileType])
                                {
                                    for (int p = i - planetradius; p < i + planetradius; p++)
                                    {
                                        for (int q = j - planetradius; q < j + planetradius; q++)
                                        {
                                            int dist = (p - i) * (p - i) + (q - j) * (q - j);
                                            if (dist > planetradius * planetradius)
                                                continue;

                                            if (WorldGen.InWorld(p, q, 1) && Main.tile[p, q].HasTile)
                                                if (Main.tile[p, q].TileType == TileID.Dirt || Main.tile[p, q].TileType == TileID.Stone || Main.tile[p, q].TileType == TileID.Grass || TileID.Sets.Ore[Main.tile[p, q].TileType])
                                                {
                                                    Main.tile[p, q].TileType = (ushort)TileType<CosmiliteSlagPlaced>();

                                                    WorldGen.SquareTileFrame(p, q, true);
                                                    NetMessage.SendTileSquare(-1, p, q, 1);
                                                    cutitNOW = true;
                                                }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Color messageColor = Color.Magenta;
                    CalamityUtils.DisplayLocalizedText("Rifts materialize in the upper atmosphere...", messageColor);
                    CalRemixWorld.generatedCosmiliteSlag = true;
                    CalRemixWorld.UpdateWorldBool();
                }
            }
        }
    }
}
