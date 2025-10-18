using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Core.Backgrounds;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
    public class VoidScene : ModSceneEffect
    {
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<VoidBackground>();

        public override bool IsSceneEffectActive(Player player)
        {
             if (VoidBoss.VoidIDX != -1)
             {
                if (Main.npc[VoidBoss.VoidIDX].ModNPC != null)
                {
                    if (Main.npc[VoidBoss.VoidIDX].ModNPC is VoidBoss v)
                    {
                        if (v.CurrentPhase != VoidBoss.PhaseType.SpawnAnimation)
                            return true;
                    }
                }
             }
            if (Filters.Scene["CalRemix:VoidColors"].IsActive())
                Filters.Scene["CalRemix:VoidColors"].Deactivate();
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (isActive)
            {
                if (!Filters.Scene["CalRemix:VoidColors"].IsActive())
                {
                    Filters.Scene.Activate("CalRemix:VoidColors", player.position);
                }
            }
        }
    }
}