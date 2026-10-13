using CalRemix.Core.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
    public class FlashScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (!SubworldSystem.IsActive<FlashSubworld>())
            {
                return false;
            }
            return true;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (isActive)
            {
                player.ManageSpecialBiomeVisuals("CalRemix:FlashSky", isActive);
                SkyManager.Instance.Activate("CalRemix:FlashSky", player.position);
            }
        }
    }
}