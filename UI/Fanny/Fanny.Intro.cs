using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperMessageManager : ModSystem
    {
        public static void LoadIntroMessages()
        {
            screenHelperMessages.Add(new HelperMessage("Intro", "Hello there! I'm Fanny the Flame, your personal guide to assist you with traversing this dangerous world. I wish you good luck on your journey and a Fan-tastic time!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld, displayOutsideInventory: true));

            screenHelperMessages.Add(new HelperMessage("GfbintroEvil", "WELCOME TO HELL!",
    "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld).SpokenByEvilFanny());

            screenHelperMessages.Add(new HelperMessage("Gfbintro", "This is different that its supposed to be... Oh! You made a getfixedboi world. This world presents new, unfamiliar challenges so always be on your toes.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld));
        }
    }
}