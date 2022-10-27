using Terraria.ModLoader;
using CalamityMod;
using Terraria;

namespace CalRemix
{
	public class CalRemixProjectile : GlobalProjectile
	{
		public bool nihilicArrow = false;
		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

		public override void AI(Projectile projectile)
        {
			Player player = Main.LocalPlayer;
			CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
			if (modPlayer.brimPortal && nihilicArrow)
				CalamityMod.CalamityUtils.HomeInOnNPC(projectile, false, 500, 10f, 1);
		}

	}
}
