using CalamityMod;
using CalamityMod.Tiles;
using CalRemix.Content.Tiles;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
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
            // DisplayName.SetDefault("Frozen Stronghold");
        }

        int Flake = ModContent.TileType<FrostflakeBrickPlaced>();

        int Cryonic = ModContent.TileType<CryonicBrick>();

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            if (CalRemixWorld.strongholdTiles < 50)
                return false;
            Point pointo = player.Center.ToTileCoordinates();

            // Search within a 100 tile vertical distance centered on the player for stronghold blocks
            // If at least one block is above the player and one block below, this should be the biome
            int searchRange = 50;
            bool tileAbove = false;
            bool tileBelow = false;
            for (int j = pointo.Y - searchRange; j < pointo.Y + searchRange; j++)
            {
                Tile t = CalamityUtils.ParanoidTileRetrieval(pointo.X, j);
                if (t.TileType == Flake || t.TileType == Cryonic)
                {
                    if (j < pointo.Y)
                        tileAbove = true;
                    if (j > pointo.Y)
                        tileBelow = true;

                    // The above tile should have been gotten by now, so only below tile needs to be verified before breaking
                    if (tileBelow)
                        break;
                }
            }
            if (tileAbove && tileBelow)
            {
                return true;
            }
            return false;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override int Music => CalRemixMusic.FrozenStronghold;
    }
}