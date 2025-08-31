using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;

namespace CalRemix.Content.Items.Placeables
{
    public class Mincer : ModItem
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
            Item.value = Item.buyPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.White;
            Item.createTile = ModContent.TileType<MincerPlaced>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 2)
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddTile(TileID.Sawmill)
                .Register();
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 2)
                .AddIngredient(ModContent.ItemType<SealedStone>(), 50)
                .AddTile(TileID.Sawmill)
                .Register();
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.IronBar, 2)
                .AddIngredient(ModContent.ItemType<CarnelianStone>(), 50)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}
