using CalRemix.Core.Subworlds;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes.Subworlds
{
    // Shows setting up two basic biomes. For a more complicated example, please request.
    public class PinnaclesBiome : ModBiome
    {
        //public override bool IsPrimaryBiome =>false; // Allows this biome to impact NPC prices

        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        // Populate the Bestiary Filter
        public override string BestiaryIcon => "CalRemix/Core/Biomes/AsbestosIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG41";
        public override Color? BackgroundColor => Color.Gray;

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<PinnaclesSubworld>();
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalRemix:PinnacleSky", isActive);
        }
        public override int Music => CalRemixMusic.AsbestosCaves;
    }
}