using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ObjectData;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (Main.gamePaused || !Main.instance.IsActive || Lighting.UpdateEveryFrame && !Main.rand.NextBool(4))
                return;
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 36 && tile.TileFrameY % 36 == 0 && (int)Main.timeForVisualEffects % 7 == 0 && Main.rand.NextBool(3))
            {
                int goreType = Main.rand.Next(570, 573);
                Vector2 position = new(i * 16 + 8, j * 16 - 8);
                Vector2 velocity = new(Main.WindForVisuals * 2f, -0.5f);
                velocity.X *= 1f + Main.rand.NextFloat(-0.5f, 0.5f);
                velocity.Y *= 1f + Main.rand.NextFloat(-0.5f, 0.5f);
                if (goreType == 572)
                    position.X -= 8f;
                if (goreType == 571)
                    position.X -= 4f;
                Gore.NewGore(new EntitySource_TileUpdate(i, j), position, velocity, goreType, 0.8f);
            }
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
    public class ARMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.ARMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.ARMusicBox>();
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
    public class EolMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.EolMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.EolMusicBox>();
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
    public class HypnosMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.HypnosMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.HypnosMusicBox>();
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