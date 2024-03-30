﻿using Terraria.ModLoader;

namespace CalRemix.Backgrounds
{
    public class SulphurousSeaBackground : ModSurfaceBackgroundStyle
    {
        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            b -= 250f;
            return BackgroundTextureLoader.GetBackgroundSlot("CalamityMod/Backgrounds/SulphurSeaSurfaceClose");
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