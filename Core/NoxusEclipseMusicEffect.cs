using CalRemix.Core.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Core
{
    public class NoxusEclipseMusicEffect : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => NoxusSkySceneSystem.EclipseDarknessInterpolant >= 0.01f && player.ZoneOverworldHeight;

        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override float GetWeight(Player player) => 0.67f;

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/NoxusEclipse");
    }
}
