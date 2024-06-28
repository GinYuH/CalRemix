using Terraria.ModLoader;

namespace CalRemix.UI
{
    public partial class ScreenHelperManager : ModSystem
    {
        public static void LoadTrapperBulbChan()
        {
            HelperMessage.New("TrapperTest", "Trapper Bulb Chan \n HAIIII ^v^ SUGOIIII DESUNE!!!", "TrapperDefault", persistsThroughSaves:false)
                .SpokenByAnotherHelper(ScreenHelpersUIState.TrapperBulbChan);

        }


    }
}