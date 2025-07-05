using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls;

public class StrawWallPlaced : ModWall
{
    public override void SetStaticDefaults()
    {
        Main.wallHouse[Type] = true;
        AddMapEntry(new Color(50, 120, 61), null);
        DustType = DustID.Hay;
        HitSound = SoundID.Grass;
    }


    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = (fail ? 1 : 3);
    }
}
