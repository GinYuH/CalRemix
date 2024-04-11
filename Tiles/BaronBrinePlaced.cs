using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRemix.Tiles
{
    public class BaronBrinePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(168, 252, 255), name);
            HitSound = MeldGunkPlaced.gunkHit;
            DustType = DustID.BlueCrystalShard;
        }
    }
}