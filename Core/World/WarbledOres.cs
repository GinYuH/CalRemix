using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Schematics;
using CalamityMod;
using CalamityMod.Tiles.Abyss;
using System;
using CalRemix.Content.Tiles;
using Terraria.DataStructures;
using Terraria.ID;
using System.Threading;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace CalRemix.Core.World
{
    public class WarbledOres : ILoadable
    {
        public static int ChlorineTileType;

        public void Load(Mod mod)
        {
            ChlorineTileType = ModContent.TileType<ChloriumOrePlaced>();
            On.CalamityMod.CalamityUtils.SpawnOre += WarbleOrespawns;
            IL_WorldGen.hardUpdateWorld += WarbleChlorophyte;
        }

        private void WarbleChlorophyte(ILContext il)
        {
            var cursor = new ILCursor(il);

            //two times, first for the mud spread and second for teh chloro spread
            for (int b = 0; b < 2; b++)
            {
                if (!cursor.TryGotoNext(MoveType.After,
                    i => i.MatchLdsflda<Main>("tile"),
                    i => i.MatchLdloc(out _),
                    i => i.MatchLdloc(out _),
                    i => i.MatchCall<Tilemap>("get_Item"),
                    i => i.MatchStloc(out _),
                    i => i.MatchLdloca(out _),
                    i => i.MatchCall<Tile>("get_type"),
                    i => i.MatchLdcI4(TileID.Chlorophyte)
                ))
                {
                    return;
                }

                cursor.EmitPop();
                cursor.EmitDelegate(() => WorldGen.genRand.NextBool() ? TileID.Chlorophyte : ChlorineTileType);
            }

            int tileTypeIndex = 0;
            ILLabel chloroSpreadBranchLabel = null;

            if (!cursor.TryGotoPrev(MoveType.After,
                    i => i.MatchLdloc(out tileTypeIndex),
                    i => i.MatchLdcI4(TileID.Chlorophyte),
                    i => i.MatchBeq(out chloroSpreadBranchLabel)
                ))
                return;

            //Chrlorine also spreads
            cursor.Emit(OpCodes.Ldloc, tileTypeIndex);
            cursor.EmitDelegate(() => ChlorineTileType);
            cursor.Emit(OpCodes.Beq_S, chloroSpreadBranchLabel);
        }

        public static void WarblePreHardmodeOres()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = Main.tile[i, j];
                    bool oreTile = true;

                    if (t.TileType == TileID.Copper || t.TileType == TileID.Tin)
                        t.TileType = WorldGen.genRand.NextBool() ? TileID.Copper : TileID.Tin;
                    else if (t.TileType == TileID.Iron || t.TileType == TileID.Lead)
                        t.TileType = WorldGen.genRand.NextBool() ? TileID.Iron : TileID.Lead;
                    else if (t.TileType == TileID.Silver || t.TileType == TileID.Tungsten)
                        t.TileType = WorldGen.genRand.NextBool() ? TileID.Silver : TileID.Tungsten;
                    else if (t.TileType == TileID.Gold || t.TileType == TileID.Platinum)
                        t.TileType = WorldGen.genRand.NextBool() ? TileID.Gold : TileID.Platinum;
                    else
                        oreTile = false;

                    //Clear ores from spawn point to delay the joke
                    if (oreTile && i > Main.spawnTileX - 200 && i < Main.spawnTileX + 200 && j < Main.spawnTileY + 200 && j > Main.spawnTileY - 100)
                        t.TileType = TileID.Stone;

                }
            }
        }

        public static void WarbleChestLoot(Chest chest)
        {
            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
            {
                if (chest.item[inventoryIndex].type == ItemID.CopperBar || chest.item[inventoryIndex].type == ItemID.TinBar)
                    chest.item[inventoryIndex].SetDefaults(WorldGen.genRand.NextBool() ? ItemID.CopperBar : ItemID.TinBar);
                else if (chest.item[inventoryIndex].type == ItemID.IronBar || chest.item[inventoryIndex].type == ItemID.LeadBar)
                    chest.item[inventoryIndex].SetDefaults(WorldGen.genRand.NextBool() ? ItemID.IronBar : ItemID.LeadBar);
                else if (chest.item[inventoryIndex].type == ItemID.SilverBar || chest.item[inventoryIndex].type == ItemID.TungstenBar)
                    chest.item[inventoryIndex].SetDefaults(WorldGen.genRand.NextBool() ? ItemID.SilverBar : ItemID.TungstenBar);
                else if (chest.item[inventoryIndex].type == ItemID.GoldBar || chest.item[inventoryIndex].type == ItemID.PlatinumBar)
                    chest.item[inventoryIndex].SetDefaults(WorldGen.genRand.NextBool() ? ItemID.GoldBar : ItemID.PlatinumBar);
            }
        }

        private void WarbleOrespawns(On.CalamityMod.CalamityUtils.orig_SpawnOre orig, int type, double frequency, float verticalStartFactor, float verticalEndFactor, int strengthMin, int strengthMax, int[] convertibleTiles)
        {
            orig(type, frequency, verticalStartFactor, verticalEndFactor, strengthMin, strengthMax, convertibleTiles);

            //Calamity spawns the alt ore second all the time so we only care about those
            if (type == TileID.Palladium || type == TileID.Orichalcum || type == TileID.Titanium)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    ThreadPool.QueueUserWorkItem(_ => WarbleHardmodeOres(verticalStartFactor, verticalEndFactor));
            }
        }

        public static void WarbleHardmodeOres(float verticalStart, float verticalEnd)
        {
            int yStart = (int)(Main.maxTilesY * verticalStart);
            int yEnd = (int)(Main.maxTilesY * verticalEnd);

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = yStart; j < yEnd; j++)
                {
                    Tile t = Main.tile[i, j];
                    bool converted = false;

                    if (t.TileType == TileID.Cobalt || t.TileType == TileID.Palladium)
                    {
                        t.TileType = WorldGen.genRand.NextBool() ? TileID.Cobalt : TileID.Palladium;
                        converted = true;
                    }
                    else if (t.TileType == TileID.Mythril || t.TileType == TileID.Orichalcum)
                    {
                        t.TileType = WorldGen.genRand.NextBool() ? TileID.Mythril : TileID.Orichalcum;
                        converted = true;
                    }
                    else if (t.TileType == TileID.Adamantite || t.TileType == TileID.Titanium)
                    {
                        t.TileType = WorldGen.genRand.NextBool() ? TileID.Adamantite : TileID.Titanium;
                        converted = true;
                    }

                    if (converted && Main.netMode == NetmodeID.Server)
                        NetMessage.SendTileSquare(-1, i, j);

                }
            }
        }

        public void Unload()
        {
            
        }
    }
}