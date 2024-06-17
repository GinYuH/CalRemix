using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Biomes
{
    // Shows setting up two basic biomes. For a more complicated example, please request.
    public class AsbestosBiome : ModBiome
    {
        //public override bool IsPrimaryBiome =>false; // Allows this biome to impact NPC prices

        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        // Populate the Bestiary Filter
        public override string BestiaryIcon => "CalRemix/Biomes/AsbestosIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG41";
        public override Color? BackgroundColor => Color.Tan;

        // Use SetStaticDefaults to assign the display name
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asbestos Caves");
        }

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            return CalRemixWorld.asbestosTiles >= 50;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalRemix:Asbestos", isActive);
        }
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/FibrousWhisper");
    }
}