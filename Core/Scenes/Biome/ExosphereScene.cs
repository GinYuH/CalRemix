using CalRemix.Core.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;


namespace CalRemix.Core.Scenes
{
	public class ExoSphereScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => SubworldSystem.Current == ModContent.GetInstance<ExosphereSubworld>();
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => CalRemixMusic.Exosphere;
    }
}