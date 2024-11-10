using CalRemix.Core.World;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;


namespace CalRemix.Core.Scenes
{
    public class StoryScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return CalRemixWorld.trueStory < CalRemixWorld.maxStoryTime;
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (isActive)
                SkyManager.Instance.Activate("CalRemix:TrueStory", player.position);
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
    }
}