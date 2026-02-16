using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.Piggy
{
    public class StickBlockPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(148, 118, 47));
            DustType = DustID.BorealWood;
            Main.tileBlendAll[Type] = true;
        }
    }
}