using Terraria.ModLoader;

namespace CalRemix.Core.Backgrounds
{
    public class ProfanedDesertBackground : ModSurfaceBackgroundStyle
    {
        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            b -= 1650f;
            return BackgroundTextureLoader.GetBackgroundSlot("CalRemix/Core/Backgrounds/ProfanedDesertBG");
        }

        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            //This just fades in the background and fades out other backgrounds.
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
    }
}