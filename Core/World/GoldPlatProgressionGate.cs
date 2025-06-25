using System.IO;
using System.Threading.Tasks;

using CalamityMod.NPCs.DesertScourge;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace CalRemix.Core.World;

internal sealed class GoldPlatProgressionGate : ModSystem
{
    private sealed class OreSpawner : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            base.OnKill(npc);

            if (hasSpawnedOre)
            {
                return;
            }

            if (npc.type != ModContent.NPCType<DesertScourgeHead>())
            {
                return;
            }

            hasSpawnedOre = true;

            CalRemixHelper.ChatMessage(
                Language.GetTextValue("Mods.CalRemix.WorldGen.BlessedWithGoldOrPlatinumMessage"),
                new Color(50, 255, 130)
            );

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            Task.Run(
                () =>
                {
                    if (GenVars.gold == 0)
                    {
                        GenVars.gold = TileID.Gold;
                    }
                    
                    // Vanilla does it twice for some reason; I don't know.
                    for (var i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 0.00012); i++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.worldSurface, (int)Main.rockLayer), WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), GenVars.gold);
                    }

                    /*for (var i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 0.00012); i++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)GenVars.worldSurfaceLow - 20), WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), GenVars.gold);
                    }*/

                    // Warble our new ore (and also warble existing ores again
                    // lmfao).
                    // Nevermind, do it manually.
                    // WarbledOres.WarblePreHardmodeOres(clearOresNearSpawn: false);
                    
                    for (var x = 0; x < Main.maxTilesX; x++)
                    for (var y = 0; y < Main.maxTilesY; y++)
                    {
                        var tile = Main.tile[x, y];

                        if (tile.TileType is TileID.Gold or TileID.Platinum)
                        {
                            tile.TileType = WorldGen.genRand.NextBool() ? TileID.Gold : TileID.Platinum;
                        }
                    }
                }
            );
        }
    }

    private static bool hasSpawnedOre;

    public override void Load()
    {
        base.Load();

        On_WorldGen.TileRunner += DontSpawnOreDuringGen;
    }

    public override void SaveWorldData(TagCompound tag)
    {
        base.SaveWorldData(tag);

        tag["hasSpawnedOre"] = hasSpawnedOre;
    }

    public override void LoadWorldData(TagCompound tag)
    {
        base.LoadWorldData(tag);

        if (tag.TryGet("hasSpawnedOre", out bool value))
        {
            hasSpawnedOre = value;
        }
    }

    public override void NetSend(BinaryWriter writer)
    {
        base.NetSend(writer);

        writer.Write(hasSpawnedOre);
    }

    public override void NetReceive(BinaryReader reader)
    {
        base.NetReceive(reader);

        hasSpawnedOre = reader.ReadBoolean();
    }

    private static void DontSpawnOreDuringGen(On_WorldGen.orig_TileRunner orig, int i, int j, double strength, int steps, int type, bool addTile, double speedX, double speedy, bool noYChange, bool @override, int ignoreTileType)
    {
        // Don't spawn any gold or platinum ore during world gen.  We could make
        // it so this only applies to the Shinies pass?
        if (WorldGen.gen && type is TileID.Gold or TileID.Platinum)
        {
            return;
        }

        orig(i, j, strength, steps, type, addTile, speedX, speedy, noYChange, @override, ignoreTileType);
    }
}