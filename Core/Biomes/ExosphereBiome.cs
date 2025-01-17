using CalRemix.Core.Subworlds;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes
{
    public class ExosphereBiome : ModBiome
    {
        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalamityMod/SulphuricWater");
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("CalRemix/SulphurousSeaBackground");
 
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
        public override string BestiaryIcon => "CalRemix/Core/Biomes/ExosphereIcon";
        public override string BackgroundPath => "CalRemix/Core/Backgrounds/ExosphereBG";
        public override string MapBackground => "CalRemix/Core/Backgrounds/ExosphereBG";

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<ExosphereSubworld>();
        }

        public override void SpecialVisuals(Player player, bool isActive)
        {
            string biomeName = "CalRemix:Exosphere";
            if (SkyManager.Instance[biomeName] != null && isActive != SkyManager.Instance[biomeName].IsActive())
            {
                if (isActive)
                {
                    SkyManager.Instance.Activate(biomeName);
                }
                else
                {
                    SkyManager.Instance.Deactivate(biomeName);
                }
            }
        }
    }
}
