using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Tiles
{
    public class TanzaniteGlassPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(237, 147, 215), name);
            HitSound = SoundID.Shatter;
            DustType = DustID.PinkCrystalShard;
        }
    }
}