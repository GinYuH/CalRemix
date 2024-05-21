using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Biomes;
using CalRemix.NPCs.Bosses.Carcinogen;

namespace CalRemix
{
    public class CalRemixWall : GlobalWall
    {
        public override void KillWall(int i, int j, int type, ref bool fail)
        {
            if (Main.LocalPlayer.InModBiome<AsbestosBiome>())
            {
                if (Main.rand.NextBool(22))
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
    }
}