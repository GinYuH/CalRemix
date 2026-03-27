using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class TitanodendronLeafBlockWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(20, 54, 14), null);
            DustType = DustID.Grass;
        }


        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
    public class UnsafeTitanodendronLeafBlockWallPlaced : ModWall
    {
        public override string Texture => "CalRemix/Content/Walls/TitanodendronLeafBlockWallPlaced";
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(20, 54, 14), null);
            DustType = DustID.Grass;
        }


        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}
