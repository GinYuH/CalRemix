using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class FannyManager : ModSystem
    {
        public static void LoadEvilFannyIntro()
        {

            FannyMessage introLore = new FannyMessage("IntroducingEvilFanny", "My friend, we've made it to Hardmode! Plenty of new opportunities have popped up and plenty of dangerous new foes now lurk about.",
                "Idle", (FannySceneMetrics scene) => Main.hardMode, 8, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true).AddDelay(5);
            fannyMessages.Add(introLore);

            FannyMessage introEvilLore = new FannyMessage("IntroducingEvilFanny2", "'Sup",
                "EvilIdle", FannyMessage.AlwaysShow, 6, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation(4f).SpokenByEvilFanny();

            introLore.AddStartEvent(() => introEvilLore.ActivateMessage());

            fannyMessages.Add(introEvilLore);

            FannyMessage introLore2 = new FannyMessage("IntroducingEvilFanny3", "E-evil Fanny!? I thought you moved away to the Yukon!",
                "Sob", FannyMessage.AlwaysShow, 8, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
                .NeedsActivation();
            introLore.AddEndEvent(() => introLore2.ActivateMessage());

            fannyMessages.Add(introLore2);

            FannyMessage introEvilLore2 = new FannyMessage("IntroducingEvilFanny4", "Yeah. Got cold.",
               "EvilIdle", FannyMessage.AlwaysShow, 5, needsToBeClickedOff: false, onlyPlayOnce: true, displayOutsideInventory: true)
               .NeedsActivation().SpokenByEvilFanny();

            introEvilLore.AddEndEvent(() => introEvilLore2.ActivateMessage());

            fannyMessages.Add(introEvilLore2);

            FannyMessage introLore3 = new FannyMessage("IntroducingEvilFanny5", "$0, it seems my evil counterpart, Evil Fanny, has returned! Don't trust a thing they say, and hopefully they'll leave..",
               "Idle", FannyMessage.AlwaysShow, 8, onlyPlayOnce: true, displayOutsideInventory: true)
               .NeedsActivation().AddDynamicText(FannyMessage.GetPlayerName);

            introLore2.AddEndEvent(() => introLore3.ActivateMessage());

            fannyMessages.Add(introLore3);
        }
    }
}
