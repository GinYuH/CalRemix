using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ObjectData;
using Terraria.ID;

namespace CalRemix.Tiles.MusicBoxes
{
    public abstract class PlacedRemixMusicBox : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }
    }
	public class AcidsighterMusicBox : PlacedRemixMusicBox
    {
		public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.AcidsighterMusicBox>());
            base.SetStaticDefaults();
        }
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.AcidsighterMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class CryoSlimeMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.CryoSlimeMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.CryoSlimeMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class DerellectMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.DerellectMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.DerellectMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class ExcavatorMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.ExcavatorMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.ExcavatorMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class LaRugaMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.LaRugaMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.LaRugaMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class PlaguedJungleMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.PlaguedJungleMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.PlaguedJungleMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class PolyphemalusMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.PolyphemalusMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.PolyphemalusMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class PolyphemalusGFBMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.PolyphemalusGFBMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.PolyphemalusGFBMusicBox>();
            base.MouseOver(i, j);
        }
    }
}