using CalRemix.Core.Subworlds;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes
{
    public class EdisBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("CalRemix/SealedOil");

        public override string BestiaryIcon => "CalRemix/Core/Biomes/SealedFieldsIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG32";
        public override Color? BackgroundColor => Color.Purple;

        public override string MapBackground => BackgroundPath;

        public override void MapBackgroundColor(ref Color color) => color = BackgroundColor.Value;

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<EdisSubworld>();
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;

        public override int Music => CalRemixMusic.Edis;
    }
}
