using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content
{
	public class DefaultJump : ExtraJump
	{
		public override Position GetDefaultPosition() => BeforeBottleJumps;

		public override float GetDurationMultiplier(Player player) 
		{
			return 0.25f;
		}
		public override void OnStarted(Player player, ref bool playSound)
		{
			player.GetModPlayer<CalRemixPlayer>().remixJumpCount++;
            playSound = false;
        }
	}
}
