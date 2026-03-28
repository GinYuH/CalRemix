using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest
{
    public class TitanodendronWoodPlaced : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(new Color(99, 58, 30));
            DustType = DustID.RichMahogany;
            Main.tileBlendAll[Type] = true;
        }

       /* public override void RandomUpdate(int i, int j)
        {
            Tile up = Main.tile[i, j - 1];
            Tile down = Main.tile[i, j + 1];
            Tile left = Main.tile[i - 1, j];
            Tile right = Main.tile[i + 1, j];
            int mos = ModContent.TileType<MossyTitanodendronWoodPlaced>();
            if (WorldGen.genRand.NextBool(3) && (up.TileType == mos || down.TileType == mos || left.TileType == mos || right.TileType == mos))
            {
                WorldGen.SpreadGrass(i, j, Type, mos, false);
            }
        }*/
    }
}