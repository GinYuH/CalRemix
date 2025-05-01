using CalRemix.Core.World;
using CalRemix.UI;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes;
public class TrueStoryMusicScene : ModSceneEffect
{
    public override bool IsSceneEffectActive(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
            return player.GetModPlayer<CalRemixPlayer>().trueStory < TrueStory.MaxStoryTime && !CalRemixWorld.playerSawTrueStory.Contains(player.name);
        return false;
    }
    public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
    public override int Music => CalRemixMusic.TrueStory;
}