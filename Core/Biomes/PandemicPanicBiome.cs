using CalRemix.Content.NPCs.PandemicPanic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes
{
    // Shows setting up two basic biomes. For a more complicated example, please request.
    public class PandemicPanicBiome : ModBiome
    {
        //public override bool IsPrimaryBiome =>false; // Allows this biome to impact NPC prices

        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;

        // Populate the Bestiary Filter
        public override string BestiaryIcon => "CalRemix/Core/Biomes/PandemicPanicIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG24";
        public override Color? BackgroundColor => Color.Red;

        // Use SetStaticDefaults to assign the display name
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pandemic Panic");
        }

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            return PandemicPanic.IsActive;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.Event;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalRemix:PandemicPanic", isActive);
        }
        public override int Music => CalRemixMusic.PandemicPanic;
    }
}