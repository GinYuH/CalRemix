using CalamityMod;
using CalRemix.Content.Walls;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes.Subworlds
{
    public class OvergrowthRainforestBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Jungle;
        public override string BestiaryIcon => "CalRemix/Core/Biomes/OvergrowthRainforestIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG9";
        public override Color? BackgroundColor => Color.Green;
        public override string MapBackground => BackgroundPath;
        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;
        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<OvergrowthRainforestSubworld>();
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
        public override int Music => MusicID.OtherworldlyJungle;
    }
    public class TitanicTrunksBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Jungle;
        public override string BestiaryIcon => "CalRemix/Core/Biomes/TitanicTrunksIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG9";
        public override Color? BackgroundColor => Color.Green;
        public override string MapBackground => BackgroundPath;
        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;
        public override bool IsBiomeActive(Player player)
        {
            Point pos = player.Center.ToTileCoordinates();
            return SubworldSystem.IsActive<OvergrowthRainforestSubworld>() && CalamityUtils.ParanoidTileRetrieval(pos.X, pos.Y).WallType == ModContent.WallType<UnsafeTitanodendronWoodWallPlaced>();
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicID.OtherworldlyJungle;
    }
    public class CanopiesBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Jungle;
        public override string BestiaryIcon => "CalRemix/Core/Biomes/CanopiesIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG9";
        public override Color? BackgroundColor => Color.Green;
        public override string MapBackground => BackgroundPath;
        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;
        public override bool IsBiomeActive(Player player)
        {
            Point pos = player.Center.ToTileCoordinates();
            return SubworldSystem.IsActive<OvergrowthRainforestSubworld>() && CalamityUtils.ParanoidTileRetrieval(pos.X, pos.Y).WallType == ModContent.WallType<UnsafeTitanodendronLeafBlockWallPlaced>();
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicID.OtherworldlyJungle;
    }
    public class BigOlBranchesBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Jungle;
        public override string BestiaryIcon => "CalRemix/Core/Biomes/BigolBranchesIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG9";
        public override Color? BackgroundColor => Color.Green;
        public override string MapBackground => BackgroundPath;
        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;
        public override bool IsBiomeActive(Player player)
        {
            Point pos = player.Center.ToTileCoordinates();
            return SubworldSystem.IsActive<OvergrowthRainforestSubworld>() && CalamityUtils.ParanoidTileRetrieval(pos.X, pos.Y).WallType <= WallID.None && Main.LocalPlayer.Center.Y / 16 > OvergrowthRainforestGeneration.treeTopLevel * Main.maxTilesY && Main.LocalPlayer.Center.Y / 16 < OvergrowthRainforestGeneration.groundLevel * Main.maxTilesY - 100;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicID.OtherworldlyJungle;
    }
    public class ForestFloorBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Jungle;
        public override string BestiaryIcon => "CalRemix/Core/Biomes/ForestFloorIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG9";
        public override Color? BackgroundColor => Color.Green;
        public override string MapBackground => BackgroundPath;
        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;
        public override bool IsBiomeActive(Player player)
        {
            Point pos = player.Center.ToTileCoordinates();
            return SubworldSystem.IsActive<OvergrowthRainforestSubworld>() && CalamityUtils.ParanoidTileRetrieval(pos.X, pos.Y).WallType <= WallID.None && !player.InModBiome<BigOlBranchesBiome>() && Main.LocalPlayer.Center.Y / 16 > OvergrowthRainforestGeneration.treeTopLevel * Main.maxTilesY;
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicID.OtherworldlyJungle;
    }
}