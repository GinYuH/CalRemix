using CalRemix.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Tiles
{
    public class ArsenicOrePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            MineResist = 1.2f;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 710;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(173, 163, 166), name);
        }
    }
}