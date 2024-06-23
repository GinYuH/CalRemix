using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Walls;

public class BanishedPlatingWallPlaced : ModWall
{
    public override void SetStaticDefaults()
    {
        AddMapEntry(new Color(64, 122, 128), null);
        DustType = DustID.Dirt;
    }


    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = (fail ? 1 : 3);
    }
}
