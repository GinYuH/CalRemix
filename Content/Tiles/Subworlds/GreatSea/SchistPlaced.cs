using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.GreatSea
{
    public class SchistPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(56, 59, 56));
            HitSound = SoundID.Tink;
            DustType = TileID.Stone;
            Main.tileBlendAll[Type] = true;
        }
    }
}