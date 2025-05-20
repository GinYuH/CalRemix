using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.Items.Placeables
{
    public class FrigidChest : ModItem
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
            Item.rare = ItemRarityID.LightRed;
            Item.createTile = ModContent.TileType<FrigidChestPlaced>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CryonicBar>(12).
                AddRecipeGroup(RecipeGroupID.IronBar, 6).
                AddTile(TileID.Anvils).
                DisableDecraft().
                Register();
        }
    }
}
