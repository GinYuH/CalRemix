using CalamityMod;
using CalRemix.Content.NPCs.Bosses.Pyrogen;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Core.Backgrounds;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using static CalRemix.Content.NPCs.Bosses.Pyrogen.Pyrogen;

namespace CalRemix.Core.Scenes
{
    public class VoidScene : ModSceneEffect
    {
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<VoidBackground>();

        public override bool IsSceneEffectActive(Player player)
        {
             if (NPC.AnyNPCs(ModContent.NPCType<VoidBoss>()))
             {
                return true;
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