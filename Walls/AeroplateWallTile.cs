using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Walls;

public class AeroplateWallTile : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[((ModBlockType)this).Type] = true;
		AddMapEntry(new Color(235, 108, 108), null);
        DustType = DustID.Terragrim;
    }


	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = (fail ? 1 : 3);
	}
}
