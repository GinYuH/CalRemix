using CalRemix.Core.World;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
	public class VernalPassMusicScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => CalRemixWorld.vernalTiles >= 50;
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
        public override int Music => CalRemixMusic.VernalPass;
    }
}