using System.Security.Policy;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Systems;
using CalamityMod.World;
using CalRemix.Subworlds;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Biomes
{
    public class FannyBiome : ModBiome
    {
        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalamityMod/SulphuricWater");
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("CalRemix/SulphurousSeaBackground");

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
        public override string BestiaryIcon => "CalamityMod/BiomeManagers/SulphurousSeaIcon";
        public override string BackgroundPath => "CalamityMod/Backgrounds/MapBackgrounds/SulphurBG";
        public override string MapBackground => "CalamityMod/Backgrounds/MapBackgrounds/SulphurBG";

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<FannySubworld>();
        }

        public override void SpecialVisuals(Player player, bool isActive)
        {
            string biomeName = "CalRemix:Fanny";
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
