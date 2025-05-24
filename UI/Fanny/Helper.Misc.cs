using CalamityMod.Items.Accessories.Wings;
using ReLogic.OS;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        #region Wonder Flower
        public static readonly SoundStyle OnwardAndUpwardSound = new("CalRemix/Assets/Sounds/Helpers/OnwardAndUpward");
        public static readonly SoundStyle WonderFannyVoice = new("CalRemix/Assets/Sounds/Helpers/WonderFannyTalk");

        public static void LoadWonderFlowerMessages()
        {
            HelperMessage.New("Wonder_Wings", "Onward and upward!",
                "TalkingFlower", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.wingTimeMax > 0 && !Main.LocalPlayer.ZoneUnderworldHeight, 15, maxWidth: 500)
                .SpokenByAnotherHelper(ScreenHelpersUIState.WonderFlower).SetSoundOverride(OnwardAndUpwardSound);

            HelperMessage.New("Wonder_Tracers", "Onward and upward... and sideward!",
                "TalkingFlower", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ModContent.ItemType<TracersCelestial>()) || Main.LocalPlayer.equippedWings?.type == ModContent.ItemType<TracersCelestial>(), 15, maxWidth: 500)
                .SpokenByAnotherHelper(ScreenHelpersUIState.WonderFlower).SetSoundOverride(OnwardAndUpwardSound);
        }
        #endregion

        #region Renault 5
        public static readonly SoundStyle VroomVroom = new SoundStyle("CalRemix/Assets/Sounds/Helpers/Renault") with { PlayOnlyIfFocused = false };
        public static HelperMessage RenaultAd;

        public static void LoadRenault5()
        {
            RenaultAd = HelperMessage.New("Renault5Advertisment", "Ah-gee, you sure said it Fanny! I couldn’t agree more! *VROOM VROOM*", "Renault5", duration: 60, cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Renault5).NeedsActivation().AddDelay(0.4f);

            OnMessageEnd += AgreeWithFanny;
        }

        private static void AgreeWithFanny(HelperMessage message)
        {
            if (message.DesiredSpeaker == ScreenHelpersUIState.FannyTheFire && !message.HasAnyEndEvents && Main.rand.NextBool(50)) // 
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
        #endregion
    }
}