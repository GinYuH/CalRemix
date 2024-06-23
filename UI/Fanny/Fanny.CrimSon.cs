using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadCrimSon()
        {
            HelperMessage.New("CrimSonTest", "Woah, I guess I’m something of a dimension hopper myself!", "CrimSonDefault")
                .SpokenByAnotherHelper(ScreenHelpersUIState.CrimSon);

        }


    }
}