using CalRemix.Content.Items.Misc;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
	public class GeneratorMusicScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => player.GetModPlayer<GeneratorPlayer>().generating && player.GetModPlayer<GeneratorPlayer>().music;
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music => CalRemixMusic.Origen;
    }
}