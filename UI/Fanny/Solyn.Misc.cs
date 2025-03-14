using CalamityMod.Items.Accessories.Wings;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadSolynMessages()
        {
            HelperMessage.New("SolynIntro1", "Oh... my head's spinning, and things suddenly seem more two-dimensional?",
                "Solyn", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Solyn).AddDelay(60);

            HelperMessage.New("SolynIntro2", "Wait a minute...",
                "Solyn", duration: 3, cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Solyn).ChainAfter(delay: 1, startTimerOnMessageSpoken: true);

            HelperMessage.New("SolynIntro3", "It seems I've been resurrected into a strange new form!",
                "Solyn", duration: 5, cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Solyn).ChainAfter(delay: 2, startTimerOnMessageSpoken: true);

            HelperMessage solynLast = HelperMessage.New("SolynIntro4", "$0, I don't know why this is happening, but I'll do my best to continue helping you in your journey!",
                "Solyn")
                .SpokenByAnotherHelper(ScreenHelpersUIState.Solyn).ChainAfter(startTimerOnMessageSpoken: true)
                .AddDynamicText(HelperMessage.GetPlayerName);

            HelperMessage.New("SolynIntro5", "There can only be one.",
                "FannyCryptid")
                .ChainAfter();
        }
    }
}