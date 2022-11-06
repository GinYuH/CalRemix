using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs.TownNPCs;
using CalRemix.Items.Materials;
using CalRemix.Tiles;
using Microsoft.Xna.Framework;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod;
using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.AdultEidolonWyrm;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.SulphurousSea;
using CalRemix.Items;
using CalRemix.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.Bumblebirb;

namespace CalRemix
{
	public class CalRemixGlobalNPC : GlobalNPC
	{
        public override void AI(NPC npc)
        {
            if (npc.type == ModContent.NPCType<MicrobialCluster>() && npc.catchItem == 0)
            {
                npc.catchItem = ModContent.ItemType<DisgustingSeawater>();
            }
            if (npc.type == ModContent.NPCType<FAP>()) // MURDER the drunk princess
            {
                npc.active = false;
            }
            if (npc.type == ModContent.NPCType<Bumblefuck>() && Main.LocalPlayer.ZoneDesert)
            {
                npc.localAI[1] = 0;
            }
        }
        public override void ModifyTypeName(NPC npc, ref string typeName)
        {
            if (npc.type == ModContent.NPCType<WITCH>())
            {
                typeName = "Calamity Witch";
            }
            else if (npc.type == ModContent.NPCType<BrimstoneElemental>())
            {
                typeName = "Calamity Elemental";
            }
            else if (npc.type == ModContent.NPCType<BrimstoneHeart>())
            {
                typeName = "Calamity Heart";
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
            else if (npc.type == ModContent.NPCType<Bumblefuck>())
            {
                npc.damage = 80;
                npc.lifeMax = 58500;
                npc.defense = 20;
                npc.value = Item.buyPrice(gold: 10);
            }
            else if (npc.type == ModContent.NPCType<Bumblefuck2>())
            {
                npc.damage = 60;
                npc.lifeMax = 3375;
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
            else if (npc.type == ModContent.NPCType<Bumblefuck>())
            {
                npcLoot.Add(DropHelper.PerPlayer(ModContent.ItemType<DesertFeather>(), 11, 17, 34));
            }
            else if (npc.type == ModContent.NPCType<NuclearTerror>())
            {
                npcLoot.Add(ModContent.ItemType<Microxodonta>(), 3);
            }
        }

        public override bool PreKill(NPC npc)
        {
            if (!CalamityMod.DownedBossSystem.downedRavager && npc.type == ModContent.NPCType<RavagerBody>())
            {
                CalamityUtils.SpawnOre(ModContent.TileType<LifeOreTile>(), 0.25E-05, 0.45f, 0.65f, 30, 40);

                Color messageColor = Color.Lime;
                CalamityUtils.DisplayLocalizedText("Vitality sprawls throughout the underground.", messageColor);
            }
            if (npc.type == Terraria.ID.NPCID.WallofFlesh && !Main.hardMode)
            {
                CalRemixWorld.ShrineTimer = 6000;
            }
            return true;
        }
    }
}
