using CalRemix.NPCs.Bosses.BossScule;
using CalRemix.NPCs.PandemicPanic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;


namespace CalRemix.Scenes
{
    public class PandemicPanicScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (PandemicPanic.IsActive)
                return true;
            return false;
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalRemix:PandemicPanic", isActive);
            SkyManager.Instance.Activate("CalRemix:PandemicPanic", player.position);
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
    }
}