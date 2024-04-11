using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Walls;

public class BaronsandWallPlaced : ModWall
{
    public override void SetStaticDefaults()
    {
        AddMapEntry(new Color(87, 82, 49), null);
        DustType = DustID.Dirt;
    }


    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = (fail ? 1 : 3);
    }
}
