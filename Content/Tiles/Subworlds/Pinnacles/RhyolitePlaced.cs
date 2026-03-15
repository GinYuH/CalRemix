using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.Pinnacles
{
    public class RhyolitePlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(36, 36, 36));
            HitSound = SoundID.Tink;
            DustType = DustID.Stone;
            Main.tileBlendAll[Type] = true;
        }
    }
}