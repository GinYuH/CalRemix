using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;

namespace CalRemix.Content.Items.Placeables
{
    public class ArmorPolisher : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.createTile = ModContent.TileType<ArmorPolisherPlaced>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RotPearl>())
                .AddIngredient(ModContent.ItemType<SealedStone>(), 50)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
