using CalRemix.Core.World;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace CalRemix.Content.EmoteBubbles
{
    public class HypnosEmote : ModEmoteBubble
	{
		public override void SetStaticDefaults()
		{
			AddToCategory(EmoteID.Category.Dangers);
		}
		public override bool IsUnlocked() => RemixDowned.downedHypnos;
	}
}