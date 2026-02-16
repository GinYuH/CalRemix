using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Audio;

namespace CalRemix.Content.Tiles.Trophies;
public abstract class PlacedRemixTrophy : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileLavaDeath[Type] = true;
        TileID.Sets.FramesOnKillWall[Type] = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
        TileObjectData.addTile(Type);
    }
}
public class AcidsighterTrophyPlaced : PlacedRemixTrophy;
public class AstigmageddonTrophyPlaced : PlacedRemixTrophy;
public class CarcinogenTrophyPlaced : PlacedRemixTrophy;
public class CataractacombTrophyPlaced : PlacedRemixTrophy;
public class ConjunctivirusTrophyPlaced : PlacedRemixTrophy;
public class DerellectTrophyPlaced : PlacedRemixTrophy;
public class ExcavatorTrophyPlaced : PlacedRemixTrophy;
public class ExotrexiaTrophyPlaced : PlacedRemixTrophy;
public class HydrogenTrophyPlaced : PlacedRemixTrophy;
public class HypnosTrophyPlaced : PlacedRemixTrophy;
public class IonogenTrophyPlaced : PlacedRemixTrophy;
public class OldIonogenTrophyPlaced : PlacedRemixTrophy;
public class OrigenTrophyPlaced : PlacedRemixTrophy;
public class OxygenTrophyPlaced : PlacedRemixTrophy;
public class PathogenTrophyPlaced : PlacedRemixTrophy;
public class PhytogenTrophyPlaced : PlacedRemixTrophy
{
    public static readonly SoundStyle EatSound = new("CalRemix/Assets/Sounds/TrophyEat");
    #region famine 
    /*
    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        Tile tile = Main.tile[i, j];
        if (tile.TileFrameY >= 55) yield return new Item(ItemID.WeaponRack);
        else yield return new Item(ModContent.ItemType<PhytogenTrophy>());
    }


    public override bool RightClick(int i, int j)
    {
        Tile tile = Main.tile[i, j];


        short frameAdjustment = (short)(54);
        if (tile.TileFrameY <= 54)
        {
            ToggleTile(i, j);
            SoundEngine.PlaySound(EatSound, new Vector2(i * 16, j * 16));
            Player player = Main.player[Main.myPlayer];
            player.AddBuff(BuffID.WellFed2, 36000);
            return true;
        }
        return false;
    }

    public void ToggleTile(int i, int j)
    {
        Tile tile = Main.tile[i, j];
        int topX = i - tile.TileFrameX % 54 / 18;
        int topY = j - tile.TileFrameY % 54 / 18;
        for (int x = topX; x < topX + 3; x++)
        {
            for (int y = topY; y < topY + 2; y++)
            {
                Main.tile[x, y].TileFrameY += 54;
            }
        }

        if (Main.netMode != NetmodeID.SinglePlayer)
        {
            NetMessage.SendTileSquare(-1, topX, topY, 3, 2);
        }
    }
   */
    #endregion
}
public class PyrogenTrophyPlaced : PlacedRemixTrophy;
public class RedTrophyPlaced : PlacedRemixTrophy;
public class SepulcherTrophyPlaced : PlacedRemixTrophy;
public class SepulcherBodyTrophyPlaced : PlacedRemixTrophy;
public class SepulcherHandTrophyPlaced : PlacedRemixTrophy;
public class SepulcherBodyAltTrophyPlaced : PlacedRemixTrophy;
public class SepulcherTailTrophyPlaced : PlacedRemixTrophy;
public class SepulcherOrbTrophyPlaced : PlacedRemixTrophy
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.addTile(Type);
    }
}
public class BrimstoneHeartTrophyPlaced : PlacedRemixTrophy
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
        TileObjectData.newTile.Width = 2;
        TileObjectData.newTile.Height = 2;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
        TileObjectData.addTile(Type);
    }
}
public class SoulSeekerTrophyPlaced : PlacedRemixTrophy
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
        TileObjectData.newTile.Width = 2;
        TileObjectData.newTile.Height = 2;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
        TileObjectData.addTile(Type);
    }
}
public class FlinstoneGangsterTrophyPlaced : PlacedRemixTrophy
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
        TileObjectData.newTile.Width = 4;
        TileObjectData.newTile.Height = 4;
        TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
        TileObjectData.addTile(Type);
    }
}
public class LivyatanTrophyPlaced : PlacedRemixTrophy;