using CalamityMod.Items.Accessories.Wings;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperMessageManager : ModSystem
    {
        public static readonly SoundStyle OnwardAndUpwardSound = new("CalRemix/Sounds/Fanny/OnwardAndUpward");
        public static readonly SoundStyle WonderFannyVoice = new("CalRemix/Sounds/Fanny/WonderFannyTalk");

        public static void LoadWonderFlowerMessages()
        {
            HelperMessage wonderWings = new HelperMessage("Wonder_Wings", "Onward and upward!",
                "TalkingFlower", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.wingTimeMax > 0, 15, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: true, maxWidth: 500)
                .SpokenByAnotherHelper(ScreenHelpersUIState.WonderFlower).SetSoundOverride(OnwardAndUpwardSound);
            screenHelperMessages.Add(wonderWings);

            HelperMessage wonderTracers = new HelperMessage("Wonder_Tracers", "Onward and upward... and sideward!",
                "TalkingFlower", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ModContent.ItemType<TracersCelestial>()) || Main.LocalPlayer.equippedWings?.type == ModContent.ItemType<TracersCelestial>(), 15, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: true, maxWidth: 500)
                .SpokenByAnotherHelper(ScreenHelpersUIState.WonderFlower).SetSoundOverride(OnwardAndUpwardSound);
            screenHelperMessages.Add(wonderTracers);
        }
    }
}