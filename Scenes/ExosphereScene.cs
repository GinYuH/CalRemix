using CalRemix.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;


namespace CalRemix.Scenes
{
	public class ExoSphereScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => SubworldSystem.Current == ModContent.GetInstance<ExosphereSubworld>();
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CanAnybodyHearMe");
    }
}