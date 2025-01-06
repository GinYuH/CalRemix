using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
	public class GeneratorMusicScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => player.GetModPlayer<CalRemixPlayer>().generatingGen && player.GetModPlayer<CalRemixPlayer>().genMusic;
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music => CalRemixMusic.Generator;
    }
}