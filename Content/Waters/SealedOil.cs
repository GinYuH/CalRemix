using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Waters
{
    public class SealedOil : ModWaterStyle
    {
        public override int ChooseWaterfallStyle()
        {
            return ModContent.Find<ModWaterfallStyle>("CalRemix/SealedOilWaterfall").Slot;
        }

        public override int GetSplashDust()
        {
            return DustID.Obsidian;
        }

        public override int GetDropletGore()
        {
            return ModContent.Find<ModGore>("CalRemix/GrandWaterDroplet").Type;
        }

        public override Color BiomeHairColor()
        {
            return Color.Black;
        }
    }
    public class SealedOilWaterfall : ModWaterfallStyle
    {
    }
}