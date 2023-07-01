using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using System.IO;
using CalamityMod.World;
using System;
using CalRemix.Tiles;
using CalamityMod.Tiles.Astral;
using CalamityMod.Tiles.AstralDesert;
using CalamityMod.Tiles.AstralSnow;
using CalamityMod.Tiles.FurnitureMonolith;
using CalRemix.Items.Placeables;
using CalamityMod.Walls;
using CalamityMod;
using Microsoft.Xna.Framework;

namespace CalRemix
{
    public class CalRemixWorld : ModSystem
    {
        public static int lifeTiles;
        public static int ShrineTimer = -20;
        public static bool downedDerellect = false;
        public static bool downedExcavator = false;

        public static bool guideHasExisted = false;
        public static bool deusDeadInSnow = false;

        public static int transmogrifyingItem = -1;
        public static int transmogrifyingItemAmt = 0;
        public static int transmogrifyTimeLeft = 0;

        public static void UpdateWorldBool()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void OnWorldLoad()
        {
            downedDerellect = false;
            downedExcavator = false;
            guideHasExisted = false;
            deusDeadInSnow = false;

            transmogrifyingItem = -1;
            transmogrifyingItemAmt = 0;
            transmogrifyTimeLeft = 0;
    }
        public override void OnWorldUnload()
        {
            downedDerellect = false;
            downedExcavator = false;
            guideHasExisted = false;
            deusDeadInSnow = false;

            transmogrifyingItem = -1;
            transmogrifyingItemAmt = 0;
            transmogrifyTimeLeft = 0;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag["downedDerellect"] = downedDerellect;
            tag["downedExcavator"] = downedExcavator;
            tag["guideHasExisted"] = guideHasExisted;
            tag["deusDeadInSnow"] = deusDeadInSnow;

            tag["transmogrifyingItem"] = transmogrifyingItem;
            tag["transmogrifyingItemAmt"] = transmogrifyingItemAmt;
            tag["transmogrifyTimeLeft"] = transmogrifyTimeLeft;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedDerellect = tag.Get<bool>("downedDerellect");
            downedExcavator = tag.Get<bool>("downedExcavator");
            guideHasExisted = tag.Get<bool>("guideHasExisted");
            deusDeadInSnow = tag.Get<bool>("deusDeadInSnow");

            transmogrifyingItem = tag.Get<int>("transmogrifyingItem");
            transmogrifyingItem = tag.Get<int>("transmogrifyingItemAmt");
            transmogrifyTimeLeft = tag.Get<int>("transmogrifyTimeLeft");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(downedDerellect);
            writer.Write(downedExcavator);
            writer.Write(guideHasExisted);
            writer.Write(deusDeadInSnow);

            writer.Write(transmogrifyingItem);
            writer.Write(transmogrifyingItemAmt);
            writer.Write(transmogrifyTimeLeft);
        }

        public override void NetReceive(BinaryReader reader)
        {
            downedDerellect = reader.ReadBoolean();
            downedExcavator = reader.ReadBoolean();
            guideHasExisted = reader.ReadBoolean();
            deusDeadInSnow = reader.ReadBoolean();

            transmogrifyingItem = reader.ReadInt32();
            transmogrifyingItemAmt = reader.ReadInt32();
            transmogrifyTimeLeft = reader.ReadInt32();
        }

        List<int> hallowlist = new List<int>
        {
            TileID.Pearlstone,
            TileID.HallowedIce,
            TileID.HallowHardenedSand,
            TileID.HallowSandstone
        };
        List<int> astrallist = new List<int>
        {
            ModContent.TileType<AstralStone>(),
            ModContent.TileType<AstralSandstone>(),
            ModContent.TileType<HardenedAstralSand>(),
            ModContent.TileType<CelestialRemains>(),
            ModContent.TileType<NovaeSlag>(),
            ModContent.TileType<AstralDirt>(),
            ModContent.TileType<AstralIce>(),
            ModContent.TileType<AstralSnow>(),
        };
        public override void PostUpdateWorld()
        {
            CalamityMod.World.CalamityWorld.spawnedCirrus = false;
            if (NPC.AnyNPCs(NPCID.Guide)) guideHasExisted = true;
            if (ShrineTimer == 0)
            {
                HMChest(TileID.CrystalBlock, TileID.CrystalBlock, WallID.Crystal, ModContent.ItemType<HallowEffigy>(), hallowlist, 21);
                HMChest(ModContent.TileType<AstralMonolith>(), ModContent.TileType<AstralMonolith>(), ModContent.WallType<AstralMonolithWall>(), ModContent.ItemType<AstralEffigy>(), astrallist, 46);

                Color messageColor = Color.Magenta;
                CalamityUtils.DisplayLocalizedText("Shrines appear within the newly spread infections!", messageColor);
            }
            if (ShrineTimer > -20)
            {
                ShrineTimer--;
            }
            if (transmogrifyTimeLeft > 0) transmogrifyTimeLeft--;
            if (transmogrifyTimeLeft > 200) transmogrifyTimeLeft = 200;
        }

        public override void ResetNearbyTileEffects()
        {
            lifeTiles = 0;
        }
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            // Life Ore tiles
            lifeTiles = tileCounts[TileType<LifeOreTile>()];
        }

        public static void HMChest(int block1, int block2, int wall, int loot, List<int> anchor, int chest)
        {
            int x = Main.maxTilesX;
            int y = Main.maxTilesY;
            for (int k = 0; k < (int)(x * y * 100E-05); k++)
            {
                int tilesX = WorldGen.genRand.Next(0, x);
                int tilesY = WorldGen.genRand.Next((int)(y * 0.35f), (int)(y * 0.8f));

                if (anchor.Contains(Main.tile[tilesX, tilesY].TileType))
                {
                    UndergroundShrines.SpecialHut((ushort)block1, (ushort)Main.tile[tilesX, tilesY].TileType, (ushort)wall, UndergroundShrines.UndergroundShrineType.Surface, tilesX, tilesY);
                    for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
                    {
                        Chest cheste = Main.chest[chestIndex];
                        if (cheste != null && Main.tile[cheste.x, cheste.y].TileType == TileID.Containers && Main.tile[cheste.x, cheste.y + 2].TileType == block1)
                        {
                            if (block1 == TileID.CrystalBlock)
                            {
                                Main.tile[cheste.x, cheste.y].TileType = TileID.Containers2;
                                Main.tile[cheste.x + 1, cheste.y].TileType = TileID.Containers2;
                                Main.tile[cheste.x, cheste.y + 1].TileType = TileID.Containers2;
                                Main.tile[cheste.x + 1, cheste.y + 1].TileType = TileID.Containers2;
                            }
                            if (block1 == ModContent.TileType<AstralMonolith>())
                            {
                                Main.tile[cheste.x, cheste.y].TileType = (ushort)ModContent.TileType<MonolithChest>();
                                Main.tile[cheste.x + 1, cheste.y].TileType = (ushort)ModContent.TileType<MonolithChest>();
                                Main.tile[cheste.x, cheste.y + 1].TileType = (ushort)ModContent.TileType<MonolithChest>();
                                Main.tile[cheste.x + 1, cheste.y + 1].TileType = (ushort)ModContent.TileType<MonolithChest>();
                            }
                            cheste.item[0].SetDefaults(loot);
                        }
                    }
                    break;
                }
            }
        }
    }
}