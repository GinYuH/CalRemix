using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Placeables.Relics
{
    public class DraedonRelic : ModItem
    {
        public override void SetDefaults()
        {
            // Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Relics.DraedonRelicPlaced>(), 0);

            Item.width = 30;
            Item.height = 40;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Master;
            Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
            Item.value = Item.buyPrice(0, 5);
        }
    }
}
