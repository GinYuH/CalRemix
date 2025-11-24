using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes
{
    // Shows setting up two basic biomes. For a more complicated example, please request.
    public class NowhereBiome : ModBiome
    {
        //public override bool IsPrimaryBiome =>false; // Allows this biome to impact NPC prices

        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        // Populate the Bestiary Filter
        public override string BestiaryIcon => "CalRemix/Core/Biomes/NowhereIcon";
        public override string BackgroundPath => "CalRemix/Core/Backgrounds/Subworlds/NowhereBG";
        public override Color? BackgroundColor => Color.White;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<NowhereSubworld>();
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override int Music => CalRemixMusic.LaRuga;
    }
}