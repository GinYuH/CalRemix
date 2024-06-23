using CalRemix.World;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace CalRemix.EmoteBubbles
{
    public class HypnosEmote : ModEmoteBubble
	{
		public override void SetStaticDefaults()
		{
			AddToCategory(EmoteID.Category.Dangers);
		}

		public override bool IsUnlocked()
		{
			return RemixDowned.downedHypnos;
		}
	}
}