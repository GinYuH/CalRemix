using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadMiracleBoyMessages()
        {
            HelperMessage.New("MiracleSnow", "Wow, it's absolutely freezing out here! You're shivering, but really, it's no problem. Since you're so cold, what do you say I warm you up? Mmm?!", 
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.ZoneSnow)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

            HelperMessage.New("MiracleScourge", "This video contains content from UMG, who has blocked it from display on this website or application",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.rand.NextBool(3600) && Main.curMusic > MusicID.Count)
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy).AddEndEvent(() => Main.musicVolume = 0);

            HelperMessage.New("Miracle1", "You don't understand! I'm only one! 1 is young!",
                "MiracleBoyIdle", (ScreenHelperSceneMetrics metrics) => Main.LocalPlayer.HasItem(ItemID.AlphabetStatue1))
                .SpokenByAnotherHelper(ScreenHelpersUIState.MiracleBoy);

        }
    }
}