using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Core.World;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;


namespace CalRemix.Core.Scenes
{
    public class DisilphiaScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Disilphia>()))
                return true;
            return false;
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalRemix:Disilphia", isActive);
            if (isActive)
            {
                SkyManager.Instance.Activate("CalRemix:Disilphia", player.position);
            }
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
    }
}