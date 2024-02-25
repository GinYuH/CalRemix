using CalRemix.NPCs.Bosses.BossScule;
using Terraria;
using Terraria.ModLoader;


namespace CalRemix.Scenes
{
	public class CalamityScene : ModSceneEffect
	{
        public override bool IsSceneEffectActive(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<TheCalamity>()))
                return true;
            return false;
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalRemix:CalamitySky", isActive);
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
    }
}