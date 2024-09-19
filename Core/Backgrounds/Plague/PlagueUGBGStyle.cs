using Terraria.ModLoader;

namespace CalRemix.Core.Backgrounds.Plague
{
    public class PlagueUGBGStyle : ModUndergroundBackgroundStyle
	{
		public override void FillTextureArray(int[] textureSlots)
        {
            for (int i = 0; i <= 3; i++)
                textureSlots[i] = BackgroundTextureLoader.GetBackgroundSlot("CalRemix/Core/Backgrounds/Plague/PlagueUG" + i.ToString());
		}
	}
}