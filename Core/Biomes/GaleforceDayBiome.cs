using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes
{
    // Shows setting up two basic biomes. For a more complicated example, please request.
    public class GaleforceDayBiome : ModBiome
    {
        //public override bool IsPrimaryBiome =>false; // Allows this biome to impact NPC prices

        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        // Populate the Bestiary Filter
        public override string BestiaryIcon => "CalRemix/Core/Biomes/GaleforceIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG33";
        public override Color? BackgroundColor => Color.Cyan;

        // Use SetStaticDefaults to assign the display name
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Galeforce Day");
        }

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            return CalRemixWorld.oxydayTime > 0 && player.position.Y < Main.worldSurface * 16;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.Event;

        public override int Music => CalRemixMusic.GaleforceDay;
    }
}