using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class RoyalRabbit : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//Description.SetDefault("Summons a Royal Rabbit to fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Hostile.RajahProjectiles.Supreme.RoyalRabbit>()] > 0)
			{
				modPlayer.RabbitcopterR = true;
			}
			if (!modPlayer.RabbitcopterR)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}