using CalRemix.Content.NPCs.Minibosses;
using Terraria;
using Terraria.ModLoader;


namespace CalRemix.Core.Scenes
{
	public class LaRugaMusicScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<LaRuga>()))
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.type == ModContent.NPCType<LaRuga>())
                    {
                        if (npc.ai[0] != 0)
                            return true;
                    }
                }
            }
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => CalRemixMusic.LaRuga;
    }
}