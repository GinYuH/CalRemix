using CalRemix.Core.Subworlds;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes
{
    // Shows setting up two basic biomes. For a more complicated example, please request.
    public class GrandSeaBiome : ModBiome
    {
        //public override bool IsPrimaryBiome =>false; // Allows this biome to impact NPC prices

        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;
        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/GrandWater");

        // Populate the Bestiary Filter
        public override string BestiaryIcon => "CalRemix/Core/Biomes/AsbestosIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG11";
        public override Color? BackgroundColor => Color.Cyan;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        // Use SetStaticDefaults to assign the display name
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Asbestos Caves");
        }

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<GrandSeaSubworld>() && player.position.Y < GrandSeaGeneration.caveBottom * Main.maxTilesY * 16;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

        public override int Music => CalRemixMusic.BaronStrait;
    }
}