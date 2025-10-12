using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.GreatSea
{
    public class ChertPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(22, 23, 22)); // not intentional 22's
            HitSound = SoundID.Tink;
            DustType = DustID.Stone;
            Main.tileBlendAll[Type] = true;
        }
    }
}