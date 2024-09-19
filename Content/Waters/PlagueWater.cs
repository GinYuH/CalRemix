using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalRemix.Content.Waters
{
    public class PlagueWater : ModWaterStyle
	{
		public override int ChooseWaterfallStyle()
		{
			return ModContent.Find<ModWaterfallStyle>("CalRemix/PlagueWaterfall").Slot;   //get the waterfall style
		}

		public override int GetSplashDust()
		{
			return 74;	//i figured 74 fit most but i can change it again if needed
		}

		public override int GetDropletGore()
        {
            return ModContent.Find<ModGore>("CalRemix/PlagueWaterDroplet").Type;
		}

		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 1f;
			b = 1f;
		}

		public override Color BiomeHairColor()
		{
			return Color.Lime; //idk i figured lime fits most
		}
	}
}