using CalRemix.Core.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
    public class SavannaScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (!SubworldSystem.IsActive<SavannaSubworld>())
            {
                return false;
            }
            return true;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (isActive)
            {
                Filters.Scene.Activate("CalRemix:Savanna", player.position);
            }
            player.ManageSpecialBiomeVisuals("CalRemix:Savanna", isActive);
        }
    }
}