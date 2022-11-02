using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs.TownNPCs;
using CalRemix.Items.Materials;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod;
using CalamityMod.NPCs.AdultEidolonWyrm;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.SupremeCalamitas;

namespace CalRemix
{
	public class CalRemixGlobalNPC : GlobalNPC
	{
        public override void AI(NPC npc)
        {
            if (npc.type == ModContent.NPCType<FAP>()) // MURDER the drunk princess
            {
                npc.active = false;
            }
        }
        public override void ModifyTypeName(NPC npc, ref string typeName)
        {
            if (npc.type == ModContent.NPCType<WITCH>())
            {
                typeName = "Calamity Witch";
            }
        }
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == ModContent.NPCType<BrimstoneElemental>())
            {
                npc.GivenName = "Calamity Elemental";
            }
            else if (npc.type == ModContent.NPCType<BrimstoneHeart>())
            {
                npc.GivenName = "Calamity Heart";
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == ModContent.NPCType<DesertScourgeHead>())
            {
                npcLoot.Add(DropHelper.PerPlayer(ModContent.ItemType<ParchedScale>(), 1, 25, 30));
            }
            else if (npc.type == ModContent.NPCType<AdultEidolonWyrmHead>())
            {
                npcLoot.Add(DropHelper.PerPlayer(ModContent.ItemType<SubnauticalPlate>(), 1, 22, 34));
            }
        }
    }
}
