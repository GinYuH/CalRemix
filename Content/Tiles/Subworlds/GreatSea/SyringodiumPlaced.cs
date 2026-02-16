using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.GreatSea
{
    public class SyringodiumPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(54, 133, 95));
            HitSound = SoundID.Dig;
            DustType = DustID.JungleGrass;
            Main.tileBlendAll[Type] = true;
        }
    }
}