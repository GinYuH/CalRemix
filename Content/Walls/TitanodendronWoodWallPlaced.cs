using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class TitanodendronWoodWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(52, 30, 15), null);
            DustType = DustID.RichMahogany;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
    public class UnsafeTitanodendronWoodWallPlaced : ModWall
    {
        public override string Texture => "CalRemix/Content/Walls/TitanodendronWoodWallPlaced";
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(52, 30, 15), null);
            DustType = DustID.RichMahogany;
        }


        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}
