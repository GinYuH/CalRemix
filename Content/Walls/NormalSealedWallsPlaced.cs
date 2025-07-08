using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class SealedStoneWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(74, 11, 70));
            DustType = DustID.PurpleCrystalShard;
        }
    }
    public class SealedDirtWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(105, 16, 99));
            DustType = DustID.PurpleCrystalShard;
        }
    }
    public class SealedWoodWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(105, 56, 93));
            DustType = DustID.PurpleCrystalShard;
        }
    }
    public class UnsafeSealedStoneWallPlaced : ModWall
    {
        public override string Texture => "CalRemix/Content/Walls/SealedStoneWallPlaced";
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(74, 11, 70));
            DustType = DustID.PurpleCrystalShard;
        }
    }
    public class UnsafeSealedDirtWallPlaced : ModWall
    {
        public override string Texture => "CalRemix/Content/Walls/SealedDirtWallPlaced";
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(105, 16, 99));
            DustType = DustID.PurpleCrystalShard;
        }
    }
}