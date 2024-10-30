using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Schematics;
using CalamityMod;
using CalamityMod.Tiles.Abyss;
using System;
using CalRemix.Content.Tiles;
using Terraria.DataStructures;
using CalRemix.Content.Items.Placeables;
using Terraria.ID;
using CalamityMod.Items.Potions;
using Terraria.WorldBuilding;
using CalamityMod.Tiles.AstralSnow;
using CalamityMod.World;
using Terraria.Utilities;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Dyes;
using CalamityMod.Items.DraedonMisc;

namespace CalRemix.Core.World
{
    public class FrozenStronghold : ModSystem
    {
        public static void GenerateFrozenStronghold()
        {
            int assIce = ModContent.TileType<AstralIce>();
            int assSnow = ModContent.TileType<AstralSnow>();
            bool shouldbreak = false;
            for (int att = 0; att < 200; att++)
            {
                if (shouldbreak)
                {
                    break;
                }
                for (int j = 200; j < (int)(Main.maxTilesY / 2f); j++)
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
                        // At attempt 199 dont bother with rng and just generate the stronghold immediately
                        Vector2 schematicSize = new Vector2(RemixSchematics.TileMaps["Frozen Stronghold"].GetLength(0), RemixSchematics.TileMaps["Frozen Stronghold"].GetLength(1) / 2);
                        if (GenVars.structures.CanPlace(new Rectangle(i, j, (int)schematicSize.X, (int)schematicSize.Y)))
                        {
                            if (Main.rand.NextBool(2222) || att == 199)
                            {
                                Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                                // Check if it's a snow biome block
                                // Astral doesn't gen on worldgen, but those blocks are here either for testing or if astral ever gets put on worldgen by some mod
                                if (t != null && t.HasTile && (t.TileType == TileID.BreakableIce || t.TileType == TileID.SnowBlock || t.TileType == TileID.IceBlock || t.TileType == assSnow || t.TileType == assIce))
                                {
                                    // Surface check
                                    bool canGen = true;
                                    // If at less than 140 gen attempts, try to check if the chosen tile is on the surface
                                    // If more than 140, ignore this
                                    if (att < 140)
                                    {
                                        // Check to see if there are 90 blocks of air above
                                        int blocksToCheck = 90;
                                        for (int m = j - 1; m > (j - blocksToCheck); m--)
                                        {
                                            Tile turd = CalamityUtils.ParanoidTileRetrieval(i, m);
                                            if (turd != null)
                                            {
                                                // If a block is found, cancel this gen attempt
                                                if (turd.HasTile)
                                                {
                                                    canGen = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    // Place the structure if the check passes
                                    if (canGen)
                                    {
                                        bool _ = false;
                                        SchematicManager.PlaceSchematic("Frozen Stronghold", new Point(i, j), SchematicAnchor.Center, ref _, new Action<Chest, int, bool>(FillStrongholdChest));
                                        CalamityUtils.AddProtectedStructure(new Rectangle(i, j, (int)schematicSize.X, (int)schematicSize.Y), 4);
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

        public static void FillStrongholdChest(Chest c, int Type, bool place)
        {
            WeightedRandom<(int, int, int)> items = new WeightedRandom<(int, int, int)>();
            items.Add((ItemID.IceBlade, 1, 2), 200);
            items.Add((ItemID.Frostbrand, 1, 2), 20);
            items.Add((ItemID.IceSickle, 1, 2), 40);
            items.Add((ItemID.IceBlock, 236, 514), 999);
            items.Add((ItemID.SnowBlock, 236, 514), 999);
            items.Add((ItemID.IceSkates, 1, 2), 120);
            items.Add((ItemID.SnowballCannon, 1, 2), 90);
            items.Add((ItemID.Snowball, 68, 236), 400);
            items.Add((ItemID.SlushBlock, 27, 85), 300);
            items.Add((ItemID.IceCream, 3, 27), 200);
            items.Add((ModContent.ItemType<DeliciousMeat>(), 6000, 9999), 300);
            items.Add((ModContent.ItemType<AbsoluteZero>(), 1, 2), 40);
            items.Add((ItemID.StaffoftheFrostHydra, 1, 2), 22);
            items.Add((ModContent.ItemType<EndothermicDye>(), 1, 2), 22);
            items.Add((ModContent.ItemType<AuricQuantumCoolingCell>(), 1, 2), 1);
            items.Add((ModContent.ItemType<FrostflakeBrick>(), 78, 213), 800);

            for (int i = 0; i < Chest.maxItems; i++)
            {
                (int, int, int) choice = items.Get();
                Item item = c.item[i];
                item.SetDefaults(choice.Item1);
                item.stack = Main.rand.Next(choice.Item2, choice.Item3);
            }
        }
    }
}