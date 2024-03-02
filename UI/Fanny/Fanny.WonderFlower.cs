using CalamityMod.Items.Accessories.Wings;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadWonderFlowerPortraits()
        {
            FannyPortrait.LoadPortrait("TalkingFlower", 11, 5);
        }

        public static readonly SoundStyle OnwardAndUpwardSound = new("CalRemix/Sounds/Fanny/OnwardAndUpward");
        public static readonly SoundStyle WonderFannyVoice = new("CalRemix/Sounds/Fanny/WonderFannyTalk");

        public static void LoadWonderFlowerMessages()
        {
            FannyMessage wonderWings = new FannyMessage("Wonder_Wings", "Onward and upward!",
                "TalkingFlower", (FannySceneMetrics m) => Main.LocalPlayer.wingTimeMax > 0, 15, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: true, maxWidth: 500)
                .SpokenByAnotherFanny(FannyUIState.WonderFlower).SetSoundOverride(OnwardAndUpwardSound);
            fannyMessages.Add(wonderWings);

            FannyMessage wonderTracers = new FannyMessage("Wonder_Tracers", "Onward and upward... and sideward!",
                "TalkingFlower", (FannySceneMetrics m) => Main.LocalPlayer.HasItem(ModContent.ItemType<TracersCelestial>()) || Main.LocalPlayer.equippedWings?.type == ModContent.ItemType<TracersCelestial>(), 15, onlyPlayOnce: true, displayOutsideInventory: true, needsToBeClickedOff: true, maxWidth: 500)
                .SpokenByAnotherFanny(FannyUIState.WonderFlower).SetSoundOverride(OnwardAndUpwardSound);
            fannyMessages.Add(wonderTracers);
        }
    }
}