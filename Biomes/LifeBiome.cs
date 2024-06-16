using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Biomes
{
    // Shows setting up two basic biomes. For a more complicated example, please request.
    public class LifeBiome : ModBiome
	{
		//public override bool IsPrimaryBiome =>false; // Allows this biome to impact NPC prices

		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

		// Populate the Bestiary Filter
		public override string BestiaryIcon => "CalRemix/Biomes/LifeIcon";
		public override string BackgroundPath => "Terraria/Images/MapBG2";
		public override Color? BackgroundColor => Color.Lime;
		//public override string Name => "Life Heart";

		// Use SetStaticDefaults to assign the display name
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Life Heart"); //Name subject to change
		}

		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player)
		{
			return CalRemixWorld.lifeTiles >= 50;
		}

		public override SceneEffectPriority Priority => SceneEffectPriority.None;
        public override int Music => -1;
    }
}