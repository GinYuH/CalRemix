using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles;
using CalamityMod.Items;

namespace CalRemix.Content.Items.Placeables
{
    public class BeetleHead : ModItem
    {
        public override string Texture => "CalRemix/Content/Tiles/BeetleHead";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beetle Juice-Chan Head");
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 14;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<BeetleHeadPlaced>();
            Item.width = 12;
            Item.height = 12;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
        }
    }
}