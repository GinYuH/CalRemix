using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Buffs
{
    public class RabbitcopterSoldier : ModBuff
	{
		public override void SetStaticDefaults()
		{
			//Description.SetDefault("Summons a Rabbitcopter Soldier to fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Hostile.RajahProjectiles.RabbitcopterSoldier>()] > 0)
			{
				modPlayer.Rabbitcopter = true;
			}
			if (!modPlayer.Rabbitcopter)
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