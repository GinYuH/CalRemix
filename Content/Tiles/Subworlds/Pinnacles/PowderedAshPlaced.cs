using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.Pinnacles
{
    public class PowderedAshPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(201, 201, 201));
            HitSound = SoundID.Dig;
            DustType = DustID.Slush;
            Main.tileBlendAll[Type] = true;
        }
    }
}