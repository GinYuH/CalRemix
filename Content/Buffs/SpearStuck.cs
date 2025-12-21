using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class SpearStuck : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Speared");
			//Description.SetDefault("There's a spear stuck in you. Ouch.");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();

            player.GetModPlayer<CalRemixPlayer>().Spear = true;
        }

		public override void Update(NPC npc, ref int buffIndex)
		{
            CalRemixNPC GNPC = npc.GetGlobalNPC<CalRemixNPC>();

            GNPC.Spear = true;
        }
	}
}
