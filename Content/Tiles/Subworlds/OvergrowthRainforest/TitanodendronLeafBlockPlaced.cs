using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest
{
    public class TitanodendronLeafBlockPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(69, 155, 64));
            DustType = DustID.Grass;
            HitSound = SoundID.Grass;
            Main.tileBlendAll[Type] = true;
        }
    }
}