using CalRemix.Core.Backgrounds;
using CalRemix.Core.Subworlds;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes
{
    public class SealedFieldsBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/SealedOil");

        public override string BestiaryIcon => "CalRemix/Core/Biomes/PrimordialCavesIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG32";
        public override Color? BackgroundColor => Color.Purple;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<SealedSubworld>() && CalRemixWorld.sealedTiles > 200 && (CalRemixWorld.barrenTiles + CalRemixWorld.carnelianTiles + CalRemixWorld.darnwoodTiles + CalRemixWorld.badTiles + CalRemixWorld.turnipTiles + CalRemixWorld.plumestoneTiles) < 50;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;

        public override int Music => CalRemixMusic.AsbestosCaves;
    }
    public class CarnelianForestBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/SealedOil");

        public override string BestiaryIcon => "CalRemix/Core/Biomes/PrimordialCavesIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG32";
        public override Color? BackgroundColor => Color.Red;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<SealedSubworld>() && CalRemixWorld.carnelianTiles > 100;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override int Music => CalRemixMusic.AsbestosCaves;
    }
    public class DarnwoodSwampBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/SealedOil");

        public override string BestiaryIcon => "CalRemix/Core/Biomes/PrimordialCavesIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG32";
        public override Color? BackgroundColor => Color.RosyBrown;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<SealedSubworld>() && CalRemixWorld.darnwoodTiles > 100 && player.position.X > (int)(Main.maxTilesX * SealedGeneration.seaDist) * 16 && player.position.X < (int)(Main.maxTilesX - Main.maxTilesX * SealedGeneration.seaDist) * 16;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override int Music => CalRemixMusic.AsbestosCaves;
    }
    public class UnsealedSeaBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/SealedOil");

        public override string BestiaryIcon => "CalRemix/Core/Biomes/PrimordialCavesIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG32";
        public override Color? BackgroundColor => Color.Black;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<SealedSubworld>() && (player.position.X < (int)(Main.maxTilesX * SealedGeneration.seaDist) * 16 || player.position.X > (int)(Main.maxTilesX - Main.maxTilesX * SealedGeneration.seaDist) * 16);
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override int Music => CalRemixMusic.AsbestosCaves;
    }
    public class BarrensBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/SealedOil");

        public override string BestiaryIcon => "CalRemix/Core/Biomes/PrimordialCavesIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG32";
        public override Color? BackgroundColor => Color.Black;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<SealedSubworld>() && CalRemixWorld.barrenTiles > 100;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

        public override int Music => CalRemixMusic.AsbestosCaves;
    }
    public class BadlandsBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/SealedOil");

        public override string BestiaryIcon => "CalRemix/Core/Biomes/PrimordialCavesIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG32";
        public override Color? BackgroundColor => Color.Purple;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<SealedSubworld>() && CalRemixWorld.badTiles > 100;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override int Music => CalRemixMusic.AsbestosCaves;
    }
    public class TurnipBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/SealedOil");

        public override string BestiaryIcon => "CalRemix/Core/Biomes/PrimordialCavesIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG32";
        public override Color? BackgroundColor => Color.MediumPurple;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<SealedSubworld>() && CalRemixWorld.turnipTiles > 100;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override int Music => CalRemixMusic.AsbestosCaves;
    }
    public class VoidForestBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/SealedOil");

        public override string BestiaryIcon => "CalRemix/Core/Biomes/PrimordialCavesIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG32";
        public override Color? BackgroundColor => Color.Magenta;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<SealedSubworld>() && CalRemixWorld.voidTiles > 100;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override int Music => CalRemixMusic.AsbestosCaves;
    }
    public class VolcanicFieldBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/SealedOil");

        public override string BestiaryIcon => "CalRemix/Core/Biomes/PrimordialCavesIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG32";
        public override Color? BackgroundColor => Color.IndianRed;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<SealedSubworld>() && CalRemixWorld.plumestoneTiles > 100;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override int Music => CalRemixMusic.AsbestosCaves;
    }
}