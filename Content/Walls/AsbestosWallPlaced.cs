using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls;

public class AsbestosWallPlaced : ModWall
{
    public override void SetStaticDefaults()
    {
        AddMapEntry(new Color(161, 155, 111), null);
        DustType = DustID.Dirt;
    }


    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = (fail ? 1 : 3);
    }
}
