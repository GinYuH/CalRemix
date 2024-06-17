using CalamityMod.Items.Accessories.Wings;
using ReLogic.OS;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadTrapperBulbChan()
        {
            HelperMessage.New("TrapperTest", "Trapper Bulb Chan \n HAIIII ^v^ SUGOIIII DESUNE!!!", "TrapperDefault", persistsThroughSaves:false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

        }


    }
}