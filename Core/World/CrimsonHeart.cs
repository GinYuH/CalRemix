using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.DraedonStructures;
using CalamityMod.Tiles.SunkenSea;
using CalamityMod.Schematics;
using CalamityMod;

namespace CalRemix.Core.World
{
    public class CrimsonHeart : ModSystem
    {

        public static void GenerateCrimsonHeart()
        {
            int widthdiv2 = 16;
            int heightdiv2 = 22;
            bool gennedMeld = false;
            Vector2 meldCoords = Vector2.Zero;
            int ymin = Main.remixWorld ? (int)(Main.maxTilesY * 0.2f) : (int)(Main.maxTilesY * 0.4f);
            int ymax = Main.remixWorld ? (int)(Main.maxTilesY * 0.6f) : Main.UnderworldLayer - 100;
            for (int loop = 0; loop < 200; loop++)
            {
                if (gennedMeld)
                    break;
                for (int x = (int)(Main.maxTilesX * 0.2f); x < Main.maxTilesX * 0.8f; x++)
                {
                    if (gennedMeld)
                        break;
                    if (x > Main.maxTilesX * 0.4f && x < Main.maxTilesX * 0.6f)
                        continue;
                    for (int y = ymin; y < ymax; y++)
                    {
                        if (gennedMeld)
                            break;
                        if (Main.rand.NextBool(2222222))
                        {
                            if (widthdiv2 * 2 > Main.maxTilesX - 100 || heightdiv2 * 2 > Main.maxTilesY - 100)
                                continue;
                            if (x - widthdiv2 < 100 || y - heightdiv2 < 100)
                                continue;
                            bool canGen = true;
                            for (int m = x - 100; m < x + 100; m++)
                            {
                                if (!canGen)
                                    break;
                                for (int n = y - 100; n < y + 100; n++)
                                {
                                    if (!canGen)
                                        break;
                                    Tile t = Main.tile[m, n];
                                    if (WorldGen.InWorld(m, n, 1))
                                    {
                                        if (t.TileType == TileID.StoneSlab || t.TileType == TileType<LaboratoryPlating>() || t.TileType == TileType<LaboratoryPanels>() || t.TileType == TileType<RustedPipes>() || TileID.Sets.IsAContainer[t.TileType] || TileID.Sets.AvoidedByMeteorLanding[t.TileType] || t.TileType == TileID.LihzahrdBrick || Main.tileDungeon[t.TileType] || t.TileType == TileType<Navystone>())
                                        {
                                            canGen = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (canGen)
                            {
                                if (WorldGen.InWorld(x, y, 1))
                                {
                                    bool _ = false;
                                    SchematicManager.PlaceSchematic<Action<Chest>>("Crimson Heart", new Point(x, y), SchematicAnchor.CenterLeft, ref _);
                                    Vector2 schematicSize = new Vector2(RemixSchematics.TileMaps["Crimson Heart"].GetLength(0), RemixSchematics.TileMaps["Crimson Heart"].GetLength(1));
                                    CalamityUtils.AddProtectedStructure(new Rectangle(x, y, (int)schematicSize.X, (int)schematicSize.Y), 4);
                                    gennedMeld = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (!gennedMeld)
            {
                CalRemix.instance.Logger.Error("Crimson Heart failed to generate!");
            }
        }
    }
}