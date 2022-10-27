using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using CalamityMod.NPCs.TownNPCs;

namespace CalRemix
{
	public class KillDrunkBitch : GlobalNPC // MURDER the drunk princess
	{
		public override void AI(NPC npc)
		{
			if (npc.type == ModContent.NPCType<FAP>())
			{
					npc.active = false;
			}	
		}
    }
}
