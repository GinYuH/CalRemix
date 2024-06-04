using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Biomes;
using CalRemix.NPCs.Bosses.Carcinogen;
using CalamityMod;
using CalamityMod.Items.VanillaArmorChanges;

namespace CalRemix
{
    public class CalRemixWall : GlobalWall
    {
        public override void KillWall(int i, int j, int type, ref bool fail)
        {
            if (Main.LocalPlayer.InModBiome<AsbestosBiome>())
            {
                int carcChance = WearingLead(Main.LocalPlayer) ? 5 : 10;
                if (Main.rand.NextBool(carcChance))
                {
                    if (type == WallID.Wood || type == WallID.Planked)
                    {
                        if (!NPC.AnyNPCs(ModContent.NPCType<Carcinogen>()))
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                NPC.SpawnOnPlayer(Main.myPlayer, ModContent.NPCType<Carcinogen>());
                            }
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