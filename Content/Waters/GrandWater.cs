using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Waters
{
    public class GrandWater : ModWaterStyle
    {
        public override int ChooseWaterfallStyle()
        {
            return ModContent.Find<ModWaterfallStyle>("CalRemix/GrandWaterfall").Slot;   //get the waterfall style
        }

        public override int GetSplashDust()
        {
            return DustID.Water_GlowingMushroom;
        }

        public override int GetDropletGore()
        {
            return ModContent.Find<ModGore>("CalRemix/GrandWaterDroplet").Type;
        }

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 1f;
            b = 1f;
        }

        public override Color BiomeHairColor()
        {
            return Color.Gray; //idk i figured lime fits most
        }
    }
    public class GrandWaterfall : ModWaterfallStyle
    {
    }
}