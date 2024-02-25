using CalamityMod.NPCs.NormalNPCs;
using Terraria;
using Terraria.ModLoader;


namespace CalRemix.Scenes
{
	public class CryoSlimeMusicScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<CryoSlime>()))
                return true;
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/AntarcticReinsertion");
    }
}