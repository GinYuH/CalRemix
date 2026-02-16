using CalamityMod.Items.Accessories.Vanity;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Weapons.Melee;
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
            if (message.DesiredSpeaker == ScreenHelpersUIState.FannyTheFire && !message.HasAnyEndEvents && Main.rand.NextBool(50))
            {
                RenaultAd.ActivateMessage();
            }

        }

        public static void RenaultAdvertisment(ScreenHelper helper)
        {
            try
            {
                // https://web.archive.org/web/20250328211711/https://www.renault.co.uk/electric-vehicles/r5-e-tech-electric.html just in case
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

        #region Solyn the Star
        public static void LoadSolynMessages()
        {
            HelperMessage.New("SolynIntro1", "Oh... my head's spinning, and things suddenly seem more two-dimensional?",
                "Solyn", HelperMessage.AlwaysShow, 5, cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Solyn).AddDelay(60);

            HelperMessage.New("SolynIntro2", "Wait a minute...",
                "Solyn", duration: 3, cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Solyn).ChainAfter(delay: 1, startTimerOnMessageSpoken: true);

            HelperMessage.New("SolynIntro3", "It seems I've been resurrected into a strange new form!",
                "Solyn", duration: 5, cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.Solyn).ChainAfter(delay: 2, startTimerOnMessageSpoken: true);

            HelperMessage solynLast = HelperMessage.New("SolynIntro4", "$0, I don't know why this is happening, but I'll do my best to continue helping you in your journey!",
                "Solyn")
                .SpokenByAnotherHelper(ScreenHelpersUIState.Solyn).ChainAfter(startTimerOnMessageSpoken: true)
                .AddDynamicText(HelperMessage.GetPlayerName);

            HelperMessage.New("SolynIntro5", "There can only be one.",
                "FannyCryptid")
                .ChainAfter();
        }
        #endregion

        #region The Pink Flame
        public static void LoadPinkFlameMessage()
        {
            HelperMessage.New("ThePinkFlame2", "I'd tell you a tip, but I feel like you've already.... REDDIT! ",
                "ThePinkFlame", (ScreenHelperSceneMetrics m) => Main.LocalPlayer.HasItem(ModContent.ItemType<HapuFruit>()) && Main.hardMode, 15, maxWidth: 500)
                .SpokenByAnotherHelper(ScreenHelpersUIState.ThePinkFlame);
        }
        #endregion

        #region Crim Father
        public static void LoadCrimFatherMessages()
        {
            HelperMessage cf1 = HelperMessage.New("CrimFather1", "Hey, I know my son can be... a lot to deal with. He's always chasing some trend, he's got a super short attention span, and he can be... a bit picky - so it means a lot to me that you're looking after him for me and my wife. " +
                "You're a good lad - I reckon the world would be a better place if there were more kind souls like you around.",
                "CrimFather", HelperMessage.AlwaysShow, 11, cantBeClickedOff: true)
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimFather).AddDelay(60);

            HelperMessage.New("CrimFather2", "We shouldn't be much longer - we're going to pick up a surprise for a little guy. Just keep an eye on him for a bit more, and we'll take him off your hands again, yeah? Again, thank you so much. God bless you.",
                "CrimFather")
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimFather).ChainAfter(cf1, delay: 1, startTimerOnMessageSpoken: true);
        }
        #endregion

        #region Example Helper
        // This method loads all dialogue relating to the ExampleHelper helper
        public static void LoadExampleHelperMessage()
        {
            // Helper messages are all registered using the HelperMessage.New method
            // The key is mostly worthless in terms of actually appearing, and is solely used for the game to recognize different messages and mark them as read or not. Usually you can just put a humerous name.
            // The next parameter is the actual message. This does not need to be manually localized as the helper system's loading automatically registers any non-localized dialogue then pulls from it.
            // The third parameter is the portrait used. This is the sprite's name with the "Helper" in front subtracted.
            // The fourth parameter is the condition for the message to appear. In this case, the message has a 1 in 600 chance to appear any frame while the Example Mod is enabled OR the player has the Seashine Sword.
            // There are other parameters but they should be self explanatory
            // SpokenByAnotherHelper will allow you to make the message spoken by a helper other than Fanny. In this case, the helper is our ExampleHelper.
            HelperMessage.New("ExampleHelperDialogue", "This is an example message.",
                "ExampleHelper", (ScreenHelperSceneMetrics m) => (Main.LocalPlayer.HasItem(ModContent.ItemType<SeashineSword>()) || CalRemixAddon.ExampleMod != null) && Main.rand.NextBool(600))
                .SpokenByAnotherHelper(ScreenHelpersUIState.ExampleHelper);
        }
        #endregion
    }
}