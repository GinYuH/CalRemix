using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace CalRemix.Core.Scenes
{
	public class EmpressMusicScene : ModSceneEffect
	{
        public override bool IsSceneEffectActive(Player player)
        {
            if (NPC.AnyNPCs(NPCID.HallowBoss))
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.type == NPCID.HallowBoss && npc.AI_120_HallowBoss_IsGenuinelyEnraged())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music => CalRemixMusic.EmpressofLightDay;
    }
}