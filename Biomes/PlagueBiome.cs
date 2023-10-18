using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace CalRemix.Biomes
{
	// Shows setting up two basic biomes. For a more complicated example, please request.
	public class PlagueBiome : ModBiome
	{
		//public override bool IsPrimaryBiome =>false; // Allows this biome to impact NPC prices

		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("CalRemix/PlagueBgStyle");
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("CalRemix/PlagueUGBGStyle");

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/PlaguedJungle");

        // Populate the Bestiary Filter
        public override string BestiaryIcon => "CalRemix/Biomes/LifeIcon";
		public override string BackgroundPath => "CalRemix/Biomes/LifeMap";
		public override Color? BackgroundColor => Color.DarkGreen;
		//public override string Name => "Life Heart";

		// Use SetStaticDefaults to assign the display name
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plagued Jungle"); //Name subject to change
		}

		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player)
        {
            player.GetModPlayer<CalRemixPlayer>().ZonePlague = CalRemixWorld.PlagueTiles > 50;
            if (player.GetModPlayer<CalRemixPlayer>().ZonePlague)
                player.GetModPlayer<CalRemixPlayer>().ZonePlagueDesert = CalRemixWorld.PlagueDesertTiles > 400;
            else
                player.GetModPlayer<CalRemixPlayer>().ZonePlagueDesert = CalRemixWorld.PlagueDesertTiles > 50;
			return player.GetModPlayer<CalRemixPlayer>().ZonePlague;
		}

		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            bool usePlagueBiome = (player.GetModPlayer<CalRemixPlayer>().ZonePlague || player.GetModPlayer<CalRemixPlayer>().ZonePlagueDesert);
            player.ManageSpecialBiomeVisuals("CalRemix:PlagueBiome", usePlagueBiome); 
			ReLogic.Content.Asset<Texture2D> newrain = ModContent.Request<Texture2D>("CalRemix/Waters/PlagueRain");
            ReLogic.Content.Asset<Texture2D> ograin = ModContent.Request<Texture2D>("CalRemix/Waters/RainOriginal");
            if (Main.bloodMoon)
            { TextureAssets.Rain = ograin; }
            else if (Main.raining && player.GetModPlayer<CalRemixPlayer>().ZonePlague)
            { TextureAssets.Rain = newrain; }
            else
            { TextureAssets.Rain = ograin; }
        }
        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/PlagueWater");
    }
}