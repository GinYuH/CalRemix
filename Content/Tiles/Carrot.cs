using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalRemix.Content.Tiles
{
    public class Carrot : ModTile
	{
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileLighted[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.addTile(Type);
            DustType = ModContent.DustType<CarrotDust>();
            HitSound = SoundID.Grass;
            RegisterItemDrop(ModContent.ItemType<Items.Potions.Carrot>());
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 10;
        }
    }

    public class CarrotDust : ModDust
    {
        public override void SetStaticDefaults()
        {
            Main.dust[Type].noGravity = false;
        }
        public override bool MidUpdate(Dust dust)
        {
            dust.rotation += dust.velocity.X / 3f;
            return false;
        }
    }
}