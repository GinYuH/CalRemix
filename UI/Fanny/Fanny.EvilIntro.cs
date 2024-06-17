using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperMessageManager : ModSystem
    {
        public static void LoadEvilFannyIntro()
        {

            HelperMessage introLore = new HelperMessage("IntroducingEvilFanny", "My friend, we've made it to Hardmode! Plenty of new opportunities have popped up and plenty of dangerous new foes now lurk about.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.hardMode, 8, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true).AddDelay(5);
            screenHelperMessages.Add(introLore);

            HelperMessage introEvilLore = new HelperMessage("IntroducingEvilFanny2", "'Sup",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(4f).SpokenByEvilFanny();

            introLore.AddStartEvent(() => introEvilLore.ActivateMessage());

            screenHelperMessages.Add(introEvilLore);

            HelperMessage introLore2 = new HelperMessage("IntroducingEvilFanny3", "E-evil Fanny!? I thought you moved away to the Yukon!",
                "FannySob", HelperMessage.AlwaysShow, 8, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation();
            introLore.AddEndEvent(() => introLore2.ActivateMessage());

            screenHelperMessages.Add(introLore2);

            HelperMessage introEvilLore2 = new HelperMessage("IntroducingEvilFanny4", "Yeah. Got cold.",
               "EvilFannyIdle", HelperMessage.AlwaysShow, 5, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
               .NeedsActivation().SpokenByEvilFanny();

            introEvilLore.AddEndEvent(() => introEvilLore2.ActivateMessage());

            screenHelperMessages.Add(introEvilLore2);

            HelperMessage introLore3 = new HelperMessage("IntroducingEvilFanny5", "$0, it seems my evil counterpart, Evil Fanny, has returned! Don't trust a thing they say, and hopefully they'll leave..",
               "FannyIdle", HelperMessage.AlwaysShow, 8, onlyPlayOnce: true, displayOutsideInventory: true)
               .NeedsActivation().AddDynamicText(HelperMessage.GetPlayerName);

            introLore2.AddEndEvent(() => introLore3.ActivateMessage());

            screenHelperMessages.Add(introLore3);
        }
    }
}
