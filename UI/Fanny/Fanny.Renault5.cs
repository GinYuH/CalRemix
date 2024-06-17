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
        public static readonly SoundStyle VroomVroom = new SoundStyle("CalRemix/Sounds/Fanny/Renault") with { PlayOnlyIfFocused = false};
        public static HelperMessage RenaultAd;

        public static void LoadRenault5()
        {
            RenaultAd = HelperMessage.New("Renault5Advertisment", "Ah-gee, you sure said it Fanny! I couldn’t agree more! *VROOM VROOM*", "Renault5", duration: 60 , cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Renault5).NeedsActivation().AddDelay(0.4f);

            OnMessageEnd += AgreeWithFanny;
        }

        private static void AgreeWithFanny(HelperMessage message)
        {
            if (message.DesiredSpeaker == ScreenHelpersUIState.FannyTheFire && !message.HasAnyEndEvents)
            {
                RenaultAd.ActivateMessage();
            }

        }

        public static void RenaultAdvertisment(ScreenHelper helper)
        {
            try
            {
                Platform.Get<IPathService>().OpenURL("https://www.renault.co.uk/electric-vehicles/r5-e-tech-electric.html");
                if (helper.Speaking)
                {
                    helper.UsedMessage.TimeLeft -= 60 * 20;
                    if (helper.UsedMessage.TimeLeft <= 30)
                        helper.UsedMessage.TimeLeft = 30;
                }
            }
            catch
            {
                Console.WriteLine("Failed to open link?!");
            }

        }
    }
}