using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadEvilFannyIntro()
        {

            HelperMessage introLore = HelperMessage.New("IntroducingEvilFanny", "My friend, we've made it to Hardmode! Plenty of new opportunities have popped up and plenty of dangerous new foes now lurk about.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.hardMode && !Main.zenithWorld, 8, cantBeClickedOff: true).AddDelay(5);

            HelperMessage introEvilLore = HelperMessage.New("IntroducingEvilFanny2", "'Sup",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true)
                .SpokenByEvilFanny().ChainAfter(delay : 4f, startTimerOnMessageSpoken: true);


            HelperMessage introLore2 = HelperMessage.New("IntroducingEvilFanny3", "E-evil Fanny!? I thought you moved away to the Yukon!",
                "FannySob", HelperMessage.AlwaysShow, 8, cantBeClickedOff: true)
                .ChainAfter(introLore);

            HelperMessage introEvilLore2 = HelperMessage.New("IntroducingEvilFanny4", "Yeah. Got cold.",
               "EvilFannyIdle", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true)
               .SpokenByEvilFanny().ChainAfter(introEvilLore);

            HelperMessage introLore3 = HelperMessage.New("IntroducingEvilFanny5", "$0, it seems my evil counterpart, Evil Fanny, has returned! Don't trust a thing they say, and hopefully they'll leave..",
               "FannyIdle", HelperMessage.AlwaysShow, 8)
               .AddDynamicText(HelperMessage.GetPlayerName).ChainAfter();
        }
    }
}
