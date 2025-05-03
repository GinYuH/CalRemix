﻿using CalamityMod;
using CalRemix.Content.NPCs.Bosses.Carcinogen;
using CalRemix.Core.Biomes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix
{
    public class CalRemixWall : GlobalWall
    {
        public override void SetStaticDefaults()
        {
            //TileID.Sets.HousingWalls[WallType<CryonicBrickWall>()] = false;
            //Main.wallHouse[WallType<CryonicBrickWall>()] = false;
        }
        public override void KillWall(int i, int j, int type, ref bool fail)
        {
            if (Main.LocalPlayer.InModBiome<AsbestosBiome>())
            {
                int carcChance = WearingLead(Main.LocalPlayer) ? 5 : 10;
                if (Main.rand.NextBool(carcChance))
                {
                    if (type == WallID.Wood || type == WallID.Planked)
                    {
                        if (!NPC.AnyNPCs(NPCType<Carcinogen>()))
                        {
                            CalRemixHelper.SpawnNPCOnPlayer(Main.myPlayer, NPCType<Carcinogen>());
                        }
                    }
                }
            }
        }

        public static bool WearingLead(Player player)
        {
            if (ItemID.LeadHelmet == player.armor[0].type && ItemID.LeadChainmail == player.armor[1].type && ItemID.LeadGreaves == player.armor[2].type)
            {
                return true;
            }
            return false;
        }
    }
}