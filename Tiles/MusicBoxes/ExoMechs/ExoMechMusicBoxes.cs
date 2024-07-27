using Terraria;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Tiles.MusicBoxes.ExoMechs
{
    public class ApingasLarryMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.ExoMechs.ApingasLarryMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.ExoMechs.ApingasLarryMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class ApingasMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.ExoMechs.ApingasMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.ExoMechs.ApingasMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class ApingasThanosMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.ExoMechs.ApingasThanosMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.ExoMechs.ApingasThanosMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class LarryMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.ExoMechs.LarryMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.ExoMechs.LarryMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class ThanosLarryMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.ExoMechs.ThanosLarryMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.ExoMechs.ThanosLarryMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class ThanosMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.ExoMechs.ThanosMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.ExoMechs.ThanosMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class XOMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.ExoMechs.XOMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.ExoMechs.XOMusicBox>();
            base.MouseOver(i, j);
        }
    }
}