using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadIntroMessages()
        {
            fannyMessages.Add(new FannyMessage("Intro", "Hello there! I'm Fanny the Flame, your personal guide to assist you with traversing this dangerous world. I wish you good luck on your journey and a Fan-tastic time!",
                "Idle", (FannySceneMetrics scene) => !Main.zenithWorld, displayOutsideInventory: true));

            fannyMessages.Add(new FannyMessage("GfbintroEvil", "WELCOME TO HELL!",
    "EvilIdle", (FannySceneMetrics scene) => Main.zenithWorld).SpokenByEvilFanny());

            fannyMessages.Add(new FannyMessage("Gfbintro", "This is different that its supposed to be... Oh! You made a getfixedboi world. This world presents new, unfamiliar challenges so always be on your toes.",
                "Idle", (FannySceneMetrics scene) => Main.zenithWorld));
        }
    }
}