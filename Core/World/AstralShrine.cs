using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;
using System;
using CalRemix.Content.Items.Placeables;
using Terraria.ID;
using CalamityMod.Items.Potions;
using System.Collections.Generic;
using CalamityMod.Tiles.Astral;
using CalamityMod.Tiles.AstralDesert;
using CalamityMod.Tiles.AstralSnow;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.FurnitureMonolith;

namespace CalRemix.Core.World
{
    public class AstralShrine : ModSystem
    {

        public static List<int> astrallist = new List<int>
        {
            ModContent.TileType<AstralStone>(),
            ModContent.TileType<AstralSandstone>(),
            ModContent.TileType<HardenedAstralSand>(),
            ModContent.TileType<CelestialRemains>(),
            ModContent.TileType<NovaeSlag>(),
            ModContent.TileType<AstralDirt>(),
            ModContent.TileType<AstralIce>(),
            ModContent.TileType<AstralSnow>(),
            ModContent.TileType<AstralGrass>(),
            ModContent.TileType<AstralClay>(),
            ModContent.TileType<AstralSand>(),
            ModContent.TileType<AstralMonolith>(),
        };
        public static void GenerateAstralShrine()
        {
            int atts = 0;
            while (atts < 100000)
            {
                int minX = 200;
                int maxX = Main.maxTilesX - 200;
                int minY = (int)Main.worldSurface - 30;
                int maxY = Main.maxTilesY - 100;

                Point p = new Point(WorldGen.genRand.Next(minX, maxX), WorldGen.genRand.Next(minY, maxY));

                Tile t = Framing.GetTileSafely(p);

                if (t.HasTile && astrallist.Contains(t.TileType))
                {
                    CalRemixHelper.PlaceSchematic("Astral Shrine", new Point(p.X, p.Y));
                    break;
                }
                else
                {
                    atts++;
                }
            }
        }
    }
}