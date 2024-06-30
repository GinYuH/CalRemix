using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadMiracleBoyMessages()
        {
            HelperMessage.New("MiracleTest", "Wow, it's absolutely freezing out here! You're shivering, but really, it's no problem. Since you're so cold, what do you say I warm you up? Mmm?!", "MiracleBoyIdle")
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

        }


    }
}