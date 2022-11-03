using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using System.IO;
using System;
using CalamityMod.Items.Materials;
using CalRemix.Tiles;

namespace CalRemix
{
    public class CalRemixWorld : ModSystem
    {
        public static int lifeTiles;

        public override void ResetNearbyTileEffects()
        {
            lifeTiles = 0;
        }
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            // Life Ore tiles
            lifeTiles = tileCounts[TileType<LifeOreTile>()];
        }
    }
}