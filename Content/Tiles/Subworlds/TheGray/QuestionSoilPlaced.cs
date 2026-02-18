using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.TheGray
{
    public class QuestionSoilPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(84, 84, 84));
            DustType = DustID.Stone;
            Main.tileBlendAll[Type] = true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}