using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ObjectData;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Tiles.MusicBoxes
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
    public class AcidRainTier2MusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.AcidRainTier2MusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.AcidRainTier2MusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class CarcinogenMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.CarcinogenMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.CarcinogenMusicBox>();
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
    public class EmpressofLightDayMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.EmpressofLightDayMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.EmpressofLightDayMusicBox>();
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
    public class HydrogenMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.HydrogenMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.HydrogenMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class IonogenMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.IonogenMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.IonogenMusicBox>();
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
    public class PolyphemalusAltMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.PolyphemalusAltMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.PolyphemalusAltMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class TheCalamityMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.TheCalamityMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.TheCalamityMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class MenuMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.MenuMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.MenuMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class VernalPassMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.VernalPassMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.VernalPassMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class PyrogenMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.PyrogenMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.PyrogenMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class OxygenMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.OxygenMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.OxygenMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class PhytogenMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.PhytogenMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.PhytogenMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class PathogenMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.PathogenMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.PathogenMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class BaronStraitMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.BaronStraitMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.BaronStraitMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class AsbestosMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.AsbestosMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.AsbestosMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class PlasticOracleMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.PlasticOracleMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.PlasticOracleMusicBox>();
            base.MouseOver(i, j);
        }
    }
    public class ProfanedDesertMusicBox : PlacedRemixMusicBox
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ItemType<Items.Placeables.MusicBoxes.ProfanedDesertMusicBox>());
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemType<Items.Placeables.MusicBoxes.ProfanedDesertMusicBox>();
            base.MouseOver(i, j);
        }
    }
}