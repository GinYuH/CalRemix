using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using Terraria;
using Terraria.ModLoader;


namespace CalRemix.Core.Scenes
{
	public class AcidRainT2MusicScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (!CalamityPlayer.areThereAnyDamnBosses && AcidRainEvent.AcidRainEventIsOngoing && DownedBossSystem.downedAquaticScourge && !DownedBossSystem.downedPolterghast)
                return true;
            return false;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => CalRemixMusic.AcidRainTier2;
    }
}