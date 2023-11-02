using CalRemix.Projectiles.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Buffs
{
	public class StatueBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Animated Statue");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<AnimatedStatue>()] > 0)
                player.GetModPlayer<CalRemixPlayer>().statue = true;
            if (!player.GetModPlayer<CalRemixPlayer>().statue)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
                player.buffTime[buffIndex] = 18000;
        }
	}
}