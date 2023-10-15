
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Backgrounds.Plague
{
	public class PlagueDesertBG : ModSurfaceBackgroundStyle
    {
		// Use this to keep far Backgrounds like the mountains.
		public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			for (int i = 0; i < fades.Length; i++)
			{
				if (i == Slot)
				{
					fades[i] += transitionSpeed;
					if (fades[i] > 1f)
					{
						fades[i] = 1f;
					}
				}
				else
				{
					fades[i] -= transitionSpeed;
					if (fades[i] < 0f)
					{
						fades[i] = 0f;
					}
				}
			}
        }

        public override int ChooseFarTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot("CalRemix/Backgrounds/Plague/PlagueDesertFar");
        }

        public override int ChooseMiddleTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot("CalRemix/Backgrounds/Plague/PlagueDesertMiddle");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return BackgroundTextureLoader.GetBackgroundSlot("CalRemix/Backgrounds/Plague/PlagueDesertClose");
        }
	}
}