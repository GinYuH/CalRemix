using CalamityMod;
using CalamityMod.Walls;
using CalRemix.Content.Walls;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.Biomes
{
    // Shows setting up two basic biomes. For a more complicated example, please request.
    public class FrozenStrongholdBiome : ModBiome
    {
        //public override bool IsPrimaryBiome =>false; // Allows this biome to impact NPC prices

        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        // Populate the Bestiary Filter
        public override string BestiaryIcon => "CalRemix/Core/Biomes/PermafrostIcon";
        public override string BackgroundPath => "Terraria/Images/MapBG4";
        public override Color? BackgroundColor => Color.Cyan;

        // Use SetStaticDefaults to assign the display name
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen Stronghold");
        }

        int Flake = ModContent.WallType<FrostflakeWallPlaced>();

        int Cryonic = ModContent.WallType<CryonicBrickWall>();

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            if (CalRemixWorld.strongholdTiles < 50)
                return false;
            Point pointo = player.Center.ToTileCoordinates();
            int searchRange = 22;
            // Only count as the biome if behind a wall
            if (!player.behindBackWall)
                return false;
            for (int i = pointo.X - searchRange; i < pointo.X + searchRange; i++)
            {
                for (int j = pointo.Y - searchRange; j < pointo.Y + searchRange; j++)
                {
                    Tile t = CalamityUtils.ParanoidTileRetrieval(i, j);
                    if (t != null)
                    {
                        // Common walls that players tend to be near
                        if (t.WallType == WallID.Wood || t.WallType == WallID.LivingWoodUnsafe || t.WallType == WallID.Planked)
                            break;

                        if (t.WallType == Flake || t.WallType == Cryonic)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override int Music => CalRemixMusic.FrozenStronghold;
    }
}