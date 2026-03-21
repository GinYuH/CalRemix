using CalRemix.Core.Backgrounds.Plague;
using CalRemix.Core.Subworlds;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes.Subworlds
{
    public class WolfForestBiome : ModBiome
    {
        public override string BestiaryIcon => "CalRemix/Core/Biomes/BaronStraitIcon";

        public override string BackgroundPath => "CalRemix/Core/Backgrounds/Subworlds/WolfForest_Map";

        public override bool IsBiomeActive(Player player)
        {
            return SubworldSystem.IsActive<WolfForestSubworld>();
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override int Music => MusicID.OtherworldlySnow;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalRemix:WolfSky", isActive);
        }
    }
}