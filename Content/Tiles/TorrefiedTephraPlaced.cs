using CalamityMod.Tiles.FurnitureProfaned;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles
{
    public class TorrefiedTephraPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<ProfanedCrystal>()] = true;
            Main.tileMerge[ModContent.TileType<ProfanedCrystal>()][Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(113, 90, 71));
        }
    }
}