using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.Piggy
{
    public class StrawBlockPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(215, 242, 126));
            HitSound = SoundID.Grass;
            DustType = DustID.Hay;
            Main.tileBlendAll[Type] = true;
        }
    }
}