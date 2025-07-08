using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class DarnwoodWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(51, 19, 24), null);
            DustType = DustID.Mud;
            Main.wallHouse[Type] = true;
        }


        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}