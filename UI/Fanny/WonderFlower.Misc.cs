using CalamityMod.Items.Accessories.Wings;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static readonly SoundStyle OnwardAndUpwardSound = new("CalRemix/Assets/Sounds/Helpers/OnwardAndUpward");
        public static readonly SoundStyle WonderFannyVoice = new("CalRemix/Assets/Sounds/Helpers/WonderFannyTalk");

        public static void LoadWonderFlowerMessages()
        {
            HelperMessage.New("Wonder_Wings", "Onward and upward!",
                "TalkingFlower", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.wingTimeMax > 0 && !Main.LocalPlayer.ZoneUnderworldHeight, 15, maxWidth: 500)
                .SpokenByAnotherHelper(ScreenHelpersUIState.WonderFlower).SetSoundOverride(OnwardAndUpwardSound);

            HelperMessage.New("Wonder_Tracers", "Onward and upward... and sideward!",
                "TalkingFlower", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ModContent.ItemType<MoonWalkers>()) || Main.LocalPlayer.equippedWings?.type == ModContent.ItemType<MoonWalkers>(), 15, maxWidth: 500)
                .SpokenByAnotherHelper(ScreenHelpersUIState.WonderFlower).SetSoundOverride(OnwardAndUpwardSound);
        }
    }
}