using CalRemix.Content.NPCs.PandemicPanic;
using CalRemix.Core.World;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;


namespace CalRemix.Core.Scenes
{
    public class EaterScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (CalRemixWorld.eaterTimer > 0)
                return true;
            return false;
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalRemix:EaterSky", isActive);
            SkyManager.Instance.Activate("CalRemix:EaterSky", player.position);
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
    }
}