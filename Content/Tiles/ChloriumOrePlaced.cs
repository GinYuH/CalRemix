using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Tiles
{
    public class ChloriumOrePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
            Main.tileOreFinderPriority[Type] = 701; // Metal Detector value, see https://terraria.wiki.gg/wiki/Metal_Detector
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(95, 194, 239), name);

            DustType = DustID.Platinum;
            HitSound = SoundID.Tink;
            MineResist = 4f;
            MinPick = 200;
        }
    }
}
