using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;

namespace CalRemix.Content.Items.Placeables
{
    public class CloseToYou : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 9, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.createTile = ModContent.TileType<CloseToYouPlaced>();
        }
    }
}
