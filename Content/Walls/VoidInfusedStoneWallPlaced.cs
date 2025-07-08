using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class VoidInfusedStoneWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(155, 0, 155), null);
            DustType = DustID.ArgonMoss;
        }


        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}