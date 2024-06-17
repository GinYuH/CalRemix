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
        public static void LoadMiracleBoyMessages()
        {
            HelperMessage.New("MiracleTest", "Wow, it's absolutely freezing out here! You're shivering, but really, it's no problem. Since you're so cold, what do you say I warm you up? Mmm?!", "MiracleBoyIdle", persistsThroughSaves:false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

        }


    }
}