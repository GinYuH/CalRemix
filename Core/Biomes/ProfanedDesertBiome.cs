using CalRemix.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes
{
    // Shows setting up two basic biomes. For a more complicated example, please request.
    public class ProfanedDesertBiome : ModBiome
    {
        //public override bool IsPrimaryBiome =>false; // Allows this biome to impact NPC prices
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("CalRemix/ProfanedDesertBackground");

        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        // Populate the Bestiary Filter
        public override string BestiaryIcon => "CalRemix/Core/Biomes/ProfanedDesertIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG39";
        public override Color? BackgroundColor => Color.Firebrick;

        // Use SetStaticDefaults to assign the display name
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Profaned Desert");
        }

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            return ProfanedDesert.scorchedWorld;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override int Music => CalRemixMusic.BaronStrait;
    }
}