using System.Collections.Generic;
using Terraria.ModLoader;
namespace CalRemix.Core;
public class HideVanillaTips : ModSystem
{
    public override void ModifyGameTipVisibility(IReadOnlyList<GameTipData> gameTips)
    {
        for (int i = 0; i <= 130; i++)
        {
            gameTips[i]?.Hide();
        }
    }
}