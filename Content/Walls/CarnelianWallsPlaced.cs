using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Walls
{
    public class CarnelianStoneWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(82, 11, 14));
            DustType = DustID.GemRuby;
        }
    }
    public class CarnelianDirtWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(128, 17, 23));
            DustType = DustID.GemRuby;
        }
    }
    public class CarnelianWoodWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(97, 29, 33));
            DustType = DustID.GemRuby;
        }
    }
    public class UnsafeCarnelianStoneWallPlaced : ModWall
    {
        public override string Texture => "CalRemix/Content/Walls/CarnelianStoneWallPlaced";
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(82, 11, 14));
            DustType = DustID.GemRuby;
        }
    }
    public class UnsafeCarnelianDirtWallPlaced : ModWall
    {
        public override string Texture => "CalRemix/Content/Walls/CarnelianDirtWallPlaced";
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(128, 17, 23));
            DustType = DustID.GemRuby;
        }
    }
}