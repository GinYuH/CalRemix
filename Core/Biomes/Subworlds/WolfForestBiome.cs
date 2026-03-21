using CalRemix.Core.Subworlds;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes.Subworlds
{
    public class WolfForestBiome : ModBiome
    {
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;
        public override string BestiaryIcon => "CalRemix/Core/Biomes/BaronStraitIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG42";
        public override Color? BackgroundColor => Color.SlateGray;
        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<WolfForestSubworld>();
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override int Music => CalRemixMusic.BaronStrait;
    }
}