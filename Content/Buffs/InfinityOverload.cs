using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class InfinityOverload : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Elemental Overload");
			//Description.SetDefault("The elements lash out against your very being");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<CalRemixPlayer>().infinityOverload = true;
		}
	}
}
