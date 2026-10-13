using CalRemix.Core.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
    public class GlamourScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (!SubworldSystem.IsActive<GlamourSubworld>() && !player.Remix().glamourMonolith)
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
                player.ManageSpecialBiomeVisuals("CalRemix:GlamourSky", isActive);
                SkyManager.Instance.Activate("CalRemix:GlamourSky", player.position);
            }
        }
    }
}