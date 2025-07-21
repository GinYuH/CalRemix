using CalRemix.Content.Tiles.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRemix.Content.Items.Placeables.Banners
{
    public abstract class BaseBanner : ModItem
    {
        public virtual int TileType => 0;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.createTile = TileType;
            Item.placeStyle = 0;
        }
    }

    public class CuboidCurseBanner : BaseBanner
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CuboidCurseBannerAlt>();
        }
        public override int TileType => ModContent.TileType<CuboidCurseBannerPlaced>();
    }

    public class CuboidCurseBannerAlt : BaseBanner
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CuboidCurseBanner>();
        }
        public override int TileType => ModContent.TileType<CuboidCurseBannerAltPlaced>();
    }

    public class EvilAnimatronicBanner : BaseBanner
    {
        public override int TileType => ModContent.TileType<EvilAnimatronicBannerPlaced>();
    }

    public class GlitchBanner : BaseBanner
    {
        public override int TileType => ModContent.TileType<GlitchBannerPlaced>();
    }

    public class RodenttmodBanner : BaseBanner
    {
        public override int TileType => ModContent.TileType<RodenttmodBannerPlaced>();
    }

    public class TallManBanner : BaseBanner
    {
        public override int TileType => ModContent.TileType<TallManBannerPlaced>();
    }

    public class WaterloggedEffigyBanner : BaseBanner
    {
        public override int TileType => ModContent.TileType<WaterloggedEffigyBannerPlaced>();
    }

    public class WalkingBirdBanner : BaseBanner
    {
        public override int TileType => ModContent.TileType<WalkingBirdBannerPlaced>();
    }
}