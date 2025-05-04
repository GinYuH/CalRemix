using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadIntroMessages()
        {
            //Fanny (Starting Messages)
            HelperMessage.New("Intro", "Hello there! I'm Fanny the Flame, your personal guide to assist you with traversing this dangerous world. I wish you good luck on your journey and a Fan-tastic time!",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => !Main.zenithWorld).SetPriority(200);

            HelperMessage.New("GfbintroEvil", "WELCOME TO HELL!",
                "EvilFannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld).SpokenByEvilFanny(true).SetPriority(200);

            HelperMessage.New("Gfbintro", "This is different that its supposed to be... Oh! You made a getfixedboi world. This world presents new, unfamiliar challenges so always be on your toes.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.zenithWorld);

            //Crim son
            HelperMessage crimtro1 = HelperMessage.New("CrimSonIntro1", "It is Dangerous to Go Alone. Take This.", "CrimSonDefault", (ScreenHelperSceneMetrics scene) => Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().gottenCellPhone, 6, cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).InitiateConversation(40).AddEndEvent(() => Main.LocalPlayer.ConsumeItem(ItemID.CellPhone));

            HelperMessage.New("CrimSonIntro2", "Woah! Hey there buddy! How are you d-",
                "FannyIdle", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).ChainAfter(crimtro1, delay: 3, startTimerOnMessageSpoken: true);

            HelperMessage crimtro3 = HelperMessage.New("CrimSonIntro3", "Hnnk, you shut lil flame boy ueehhe.",
                "CrimSonDefault", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).ChainAfter(crimtro1);

            HelperMessage.New("CrimSonIntro4", "Well that's not nice.",
                "FannySob", HelperMessage.AlwaysShow).ChainAfter(crimtro3, delay: 3, startTimerOnMessageSpoken: true).SetHoverTextOverride("Indeed it isn't Fanny!");

            HelperMessage.New("CrimSonIntro5", "Lil flame boy 0 crim son 1 oh yea yea goochie gang",
                "CrimSonDefault", HelperMessage.AlwaysShow).ChainAfter(delay: 1, startTimerOnMessageSpoken: true).SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon).EndConversation();

            //Evil Fanny

            HelperMessage introLore = HelperMessage.New("IntroducingEvilFanny", "My friend, we've made it to Hardmode! Plenty of new opportunities have popped up and plenty of dangerous new foes now lurk about.",
                "FannyIdle", (ScreenHelperSceneMetrics scene) => Main.hardMode && !Main.zenithWorld, 8, cantBeClickedOff: true).AddDelay(5).InitiateConversation();
            HelperMessage introEvilLore = HelperMessage.New("IntroducingEvilFanny2", "'Sup",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByEvilFanny().ChainAfter(delay: 4f, startTimerOnMessageSpoken: true);
            HelperMessage introLore2 = HelperMessage.New("IntroducingEvilFanny3", "E-evil Fanny!? I thought you moved away to the Yukon!",
                "FannySob", HelperMessage.AlwaysShow, 8, cantBeClickedOff: true).ChainAfter(introLore);
            HelperMessage introEvilLore2 = HelperMessage.New("IntroducingEvilFanny4", "Yeah. Got cold.",
               "EvilFannyIdle", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByEvilFanny().ChainAfter(introEvilLore);
            HelperMessage introLore3 = HelperMessage.New("IntroducingEvilFanny5", "$0, it seems my evil counterpart, Evil Fanny, has returned! Don't trust a thing they say, and hopefully they'll leave..",
               "FannyIdle", HelperMessage.AlwaysShow, 8).AddDynamicText(HelperMessage.GetPlayerName).ChainAfter().EndConversation();
            HelperMessage introEvilLore3 = HelperMessage.New("IntroducingEvilFanny5", "\"Evil counterpart\" is a crazy way of saying you can't take the slightest bit of cricicism, just saying, bro.",
                "EvilFannyIdle", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true).SpokenByEvilFanny().ChainAfter(introEvilLore2);

            //Trapper Bulb Chan
            HelperMessage.New("TrapperBeginning1", "Hey, have you seen my precious pink flower that I've been growing for 15 years? I left her around here. She's been my best friend for years now, and I could never fathom what I'd do if I had lost h",
               "FannyIdle", (ScreenHelperSceneMetrics scene) => NPC.downedPlantBoss, cantBeClickedOff: true).InitiateConversation();

            HelperMessage fan1 = HelperMessage.New("TrapperBeginning2", "Oh.",
               "FannySob", HelperMessage.AlwaysShow, cantBeClickedOff: true, duration: 2)
                .ChainAfter();

            HelperMessage trap1 = HelperMessage.New("TrapperBeginning3", "Trapper Bulb Chan here! HAIIII!!! ^v^",
                "TrapperHappy", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(fan1, delay: 3);

            HelperMessage fan2 = HelperMessage.New("TrapperBeginning4", "Woah there! Where did you come from?",
                "FannyAwooga", HelperMessage.AlwaysShow, 7, cantBeClickedOff: true).ChainAfter(fan1, delay: 5);

            HelperMessage trap2 = HelperMessage.New("TrapperBeginning5", "UwU I'm just a friendly flower wandering around, desu!",
                "TrapperDefault", HelperMessage.AlwaysShow, 6, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(trap1);

            HelperMessage fan3 = HelperMessage.New("TrapperBeginning6", "Well, it’s nice to meet you! Any tips for our friend here?",
                "FannyNuhuh", HelperMessage.AlwaysShow, 7).ChainAfter(fan2);

            HelperMessage trap3 = HelperMessage.New("TrapperBeginning7", "Teehee, just keep blooming and shining, and remember to water your dreams, nyan!",
                "TrapperHappy", HelperMessage.AlwaysShow, 6).SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan).ChainAfter(trap2);

            HelperMessage ff = HelperMessage.New("TrapperBeginning8", "Oh come fucking on.",
                "EvilFannyDisgusted", HelperMessage.AlwaysShow).SpokenByEvilFanny().ChainAfter(fan3, startTimerOnMessageSpoken: true, delay: 10).EndConversation();

            //Bizarro Fanny
            HelperMessage.New("BizarroFannyIntro", "HIHIHI I'm FFFFanny the Flame! I willGuide you throU GH this [[The infinite cosmos di[still[ upon the nigh end of a [real[ity under[ the blossoms[ of death[",
                "BizarroFannyIdle", InAvatarUniverse, 12, cantBeClickedOff: true).SpokenByAnotherHelper(ScreenHelpersUIState.BizarroFanny);
        }
        private static bool InAvatarUniverse(ScreenHelperSceneMetrics scene)
        {
            if (CalRemixAddon.WrathInAvatarWorld != null)
                return (bool)CalRemixAddon.WrathInAvatarWorld.GetValue(null);
            return false;
        }
    }
}