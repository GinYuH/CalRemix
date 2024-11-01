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
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Fishing;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Ammo;
using CalamityMod.Items.Placeables.Furniture.Monoliths;

namespace CalRemix.Core.World
{
    public class FrozenStronghold : ModSystem
    {
        public static void GenerateFrozenStronghold()
        {
            int assIce = ModContent.TileType<AstralIce>();
            int assSnow = ModContent.TileType<AstralSnow>();
            bool shouldbreak = false;
            int snowBottom = GenVars.snowTop;
            for (int att = 0; att < 100000; att++)
            {
                if (shouldbreak)
                {
                    break;
                }
                int i = WorldGen.genRand.Next(GenVars.snowOriginLeft - 200, GenVars.snowOriginRight + 200);
                int j = WorldGen.genRand.Next(GenVars.snowTop - 250, snowBottom);
                
                // At attempt 199 dont bother with rng and just generate the stronghold immediately
                Vector2 schematicSize = new Vector2(RemixSchematics.TileMaps["Frozen Stronghold"].GetLength(0), RemixSchematics.TileMaps["Frozen Stronghold"].GetLength(1));
                //This generates insanely early so there shoooooouldnt be any structures to avoid...?
                //if (GenVars.structures.CanPlace(new Rectangle(i, j, (int)schematicSize.X, (int)schematicSize.Y)))
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    // Check if it's a snow biome block
                    // Astral doesn't gen on worldgen, but those blocks are here either for testing or if astral ever gets put on worldgen by some mod
                    if (t != null && t.HasTile && (t.TileType == TileID.BreakableIce || t.TileType == TileID.SnowBlock || t.TileType == TileID.IceBlock || t.TileType == assSnow || t.TileType == assIce))
                    {
                        // Surface check
                        bool canGen = true;
                        // If at less than 140 gen attempts, try to check if the chosen tile is on the surface
                        // If more than 5, ignore this
                        if (att < 50000)
                        {
                            // Check to see if there are 90 blocks of air above
                            int blocksToCheck = WorldGen.GetWorldSize() == 1 ? 30 : 70;
                            if (att > 30000)
                                blocksToCheck = (int)(blocksToCheck * 0.5f);
                            if (att > 40000)
                                blocksToCheck = (int)(blocksToCheck * 0.5f);
                            for (int m = j - 1; m > (j - blocksToCheck); m--)
                            {
                                Tile turd = CalamityUtils.ParanoidTileRetrieval(i, m);
                                if (turd != null)
                                {
                                    // If a block is found, cancel this gen attempt
                                    if (turd.IsTileSolid())
                                    {
                                        canGen = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    canGen = false;
                                }
                            }
                        }
                        // Place the structure if the check passes
                        if (canGen)
                        {
                            bool _ = false;
                            int newj = j + 30;
                            SchematicManager.PlaceSchematic("Frozen Stronghold", new Point(i, newj), SchematicAnchor.BottomCenter, ref _, new Action<Chest, int, bool>(FillStrongholdChest));
                            CalamityUtils.AddProtectedStructure(new Rectangle(i - (int)(schematicSize.X / 2), newj - (int)schematicSize.Y, (int)schematicSize.X, (int)schematicSize.Y), 4);
                            shouldbreak = true;
                            break;
                        }
                    }
                }
            }
        }

        public static void FillStrongholdChest(Chest c, int Type, bool place)
        {
            CalculateLoot(ref c, place);
        }

        public static void CalculateLoot(ref Chest c, bool first)
        {
            WeightedRandom<(int, int, int)> items = new WeightedRandom<(int, int, int)>();
            // Blocks
            items.Add((ItemID.IceBlock, 236, 514), 999);
            items.Add((ItemID.SnowBlock, 236, 514), 999);
            items.Add((ItemID.SlushBlock, 27, 85), 300);
            items.Add((ModContent.ItemType<FrostflakeBrick>(), 78, 213), 800);

            // Misc prehm
            items.Add((ItemID.Snowball, 68, 236), 400);
            items.Add((ItemID.IceCream, 3, 27), 200);
            items.Add((ItemID.FlinxFur, 3, 7), 90);
            items.Add((ItemID.FrostDaggerfish, 62, 87), 100);
            items.Add((ModContent.ItemType<FrostBolt>(), 1, 2), 200);
            items.Add((ModContent.ItemType<IcicleStaff>(), 1, 2), 200);
            items.Add((ModContent.ItemType<FrostBlossomStaff>(), 1, 2), 200);

            // Ice Chest
            items.Add((ItemID.IceBlade, 1, 2), 200);
            items.Add((ItemID.IceSkates, 1, 2), 120);
            items.Add((ItemID.SnowballCannon, 1, 2), 90);
            items.Add((ItemID.IceBoomerang, 1, 2), 200);
            items.Add((ItemID.FlurryBoots, 1, 2), 90);
            items.Add((ItemID.IceMirror, 1, 2), 160);
            items.Add((ItemID.IceMachine, 1, 2), 90);
            items.Add((ItemID.BlizzardinaBalloon, 1, 2), 90);

            // Hardmode drops
            items.Add((ItemID.Frostbrand, 1, 2), 40);
            items.Add((ItemID.IceSickle, 1, 2), 40);
            items.Add((ItemID.SnowballLauncher, 1, 2), 40);
            items.Add((ItemID.FrostStaff, 1, 2), 40);
            items.Add((ItemID.FrozenTurtleShell, 1, 2), 40);
            items.Add((ItemID.FrozenKey, 1, 2), 40);
            items.Add((ItemID.IceBow, 1, 2), 60);
            items.Add((ItemID.IceFeather, 1, 2), 60);
            items.Add((ModContent.ItemType<AncientIceChunk>(), 1, 2), 40);
            items.Add((ModContent.ItemType<FrostBarrier>(), 1, 2), 120);
            items.Add((ModContent.ItemType<CryonicOre>(), 1, 5), 200);
            items.Add((ModContent.ItemType<EssenceofEleum>(), 1, 5), 200);
            items.Add((ModContent.ItemType<FishofEleum>(), 2, 6), 100);

            // Cryogen
            items.Add((ModContent.ItemType<Avalanche>(), 1, 2), 60);
            items.Add((ModContent.ItemType<SnowstormStaff>(), 1, 2), 60);
            items.Add((ModContent.ItemType<Icebreaker>(), 1, 2), 60);
            items.Add((ModContent.ItemType<HoarfrostBow>(), 1, 2), 60);
            items.Add((ModContent.ItemType<GlacialEmbrace>(), 1, 2), 10);
            items.Add((ModContent.ItemType<CryoStone>(), 1, 2), 60);
            items.Add((ModContent.ItemType<FrostFlare>(), 1, 2), 60);

            // Permafrost
            items.Add((ModContent.ItemType<DeliciousMeat>(), 6000, 9999), 300);
            items.Add((ModContent.ItemType<FrostbiteBlaster>(), 1, 2), 60);
            items.Add((ModContent.ItemType<IcicleTrident>(), 1, 2), 60);
            items.Add((ModContent.ItemType<IceStar>(), 40, 168), 60);
            items.Add((ModContent.ItemType<PermafrostsConcoction>(), 1, 2), 60);
            items.Add((ModContent.ItemType<FrigidMonolith>(), 1, 2), 100);

            // Mech Permafrost
            items.Add((ModContent.ItemType<FrostyFlare>(), 1, 2), 50);
            items.Add((ModContent.ItemType<ArcticBearPaw>(), 1, 2), 50);
            items.Add((ModContent.ItemType<Cryophobia>(), 1, 2), 50);
            items.Add((ModContent.ItemType<CryogenicStaff>(), 1, 2), 50);

            // Festive Permafrost
            items.Add((ModContent.ItemType<AbsoluteZero>(), 1, 2), 40);
            items.Add((ModContent.ItemType<EternalBlizzard>(), 1, 2), 40);
            items.Add((ModContent.ItemType<WintersFury>(), 1, 2), 40);
            items.Add((ModContent.ItemType<HailstormBullet>(), 40, 81), 40);
            items.Add((ModContent.ItemType<IcicleArrow>(), 40, 81), 40);

            // Cryonic
            items.Add((ModContent.ItemType<KelvinCatalyst>(), 1, 2), 50);
            items.Add((ModContent.ItemType<FrostcrushValari>(), 1, 2), 50);

            // There is nowhere else to put him
            items.Add((ItemID.StaffoftheFrostHydra, 1, 2), 22);

            // Misc gm
            items.Add((ModContent.ItemType<EndothermicDye>(), 1, 3), 22);
            items.Add((ModContent.ItemType<Hypothermia>(), 1, 2), 22);
            items.Add((ModContent.ItemType<EndoHydraStaff>(), 1, 2), 22);
            items.Add((ModContent.ItemType<EndothermicEnergy>(), 4, 8), 22);
            items.Add((ModContent.ItemType<AuricQuantumCoolingCell>(), 1, 2), 1);
            items.Add((ModContent.ItemType<Endogenesis>(), 1, 2), 0.1f);

            for (int i = 0; i < Chest.maxItems; i++)
            {
                (int, int, int) choice = items.Get();
                if (first)
                    choice = items.Get();
                Item item = c.item[i];
                item.SetDefaults(choice.Item1);
                item.stack = Main.rand.Next(choice.Item2, choice.Item3);
            }
        }
    }
}