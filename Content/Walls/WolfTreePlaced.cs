using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls;

public class WolfTreePlaced : ModWall
{
    public override void SetStaticDefaults()
    {
        AddMapEntry(new Color(84, 91, 88), null);
        DustType = DustID.BorealWood;
    }


    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = (fail ? 1 : 3);
    }
}
