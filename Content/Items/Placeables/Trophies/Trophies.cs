using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Trophies;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace CalRemix.Content.Items.Placeables.Trophies;
public abstract class RemixTrophy : ModItem
{
    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.autoReuse = true;
        Item.maxStack = 99;
        Item.consumable = true;
        Item.width = 12;
        Item.height = 12;
        Item.rare = ItemRarityID.Blue;
    }
}
public class AcidsighterTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<AcidsighterTrophyPlaced>();
    }
}
public class AstigmageddonTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<AstigmageddonTrophyPlaced>();
    }
}
public class CarcinogenTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<CarcinogenTrophyPlaced>();
    }
}
public class CataractacombTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<CataractacombTrophyPlaced>();
    }
}
public class ConjunctivirusTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<ConjunctivirusTrophyPlaced>();
    }
}
public class DerellectTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<DerellectTrophyPlaced>();
    }
}
public class ExcavatorTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<ExcavatorTrophyPlaced>();
    }
}
public class ExotrexiaTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<ExotrexiaTrophyPlaced>();
    }
}
public class HydrogenTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<HydrogenTrophyPlaced>();
    }
}
public class HypnosTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<HypnosTrophyPlaced>();
    }
}
public class IonogenTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<IonogenTrophyPlaced>();
    }
}
public class OldIonogenTrophy : RemixTrophy
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.ShimmerTransformToItem[Type] = ItemType<IonogenTrophy>();
    }
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<OldIonogenTrophyPlaced>();
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient(ItemType<IonogenTrophy>()).
            AddTile(TileID.HeavyWorkBench).
            Register();
    }
}
public class OrigenTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<OrigenTrophyPlaced>();
    }
}
public class OxygenTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<OxygenTrophyPlaced>();
    }
}
public class PathogenTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<PathogenTrophyPlaced>();
    }
}
public class PhytogenTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<PhytogenTrophyPlaced>();
    }
}
public class PyrogenTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<PyrogenTrophyPlaced>();
    }
}
public class RedTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<RedTrophyPlaced>();
    }
}
public class SepulcherTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<SepulcherTrophyPlaced>();
    }
}
public class SepulcherBodyTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<SepulcherBodyTrophyPlaced>();
    }
}
public class SepulcherHandTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<SepulcherHandTrophyPlaced>();
    }
}
public class SepulcherBodyAltTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<SepulcherBodyAltTrophyPlaced>();
    }
}
public class SepulcherTailTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<SepulcherTailTrophyPlaced>();
    }
}
public class SepulcherOrbTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<SepulcherOrbTrophyPlaced>();
    }
}
public class SoulSeekerTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<SoulSeekerTrophyPlaced>();
    }
}
public class BrimstoneHeartTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<BrimstoneHeartTrophyPlaced>();
    }
}
public class FlinstoneGangsterTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<FlinstoneGangsterTrophyPlaced>();
    }
}
public class LivyatanTrophy : RemixTrophy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.createTile = TileType<LivyatanTrophyPlaced>();
    }
}