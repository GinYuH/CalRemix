using CalRemix.Projectiles.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
	public class MackerelBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MackerelBuff");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<HolyMackerel>()] > 0)
                player.GetModPlayer<CalRemixPlayer>().mackerel = true;
            if (!player.GetModPlayer<CalRemixPlayer>().mackerel)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
                player.buffTime[buffIndex] = 18000;
        }
	}
}