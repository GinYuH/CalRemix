using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;

namespace CalRemix.Content.Items.Placeables
{
    public class BarPlanter : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<BarPlanterTile>());
            Item.value = 0;
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ClayPot).
                AddIngredient(ItemID.Fertilizer, 15).
                AddIngredient(ItemID.StoneBlock, 15).
                AddRecipeGroup(RecipeGroupID.IronBar, 5).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
