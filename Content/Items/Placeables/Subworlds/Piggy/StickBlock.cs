using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Critters;
using CalRemix.Content.Tiles.Subworlds.Piggy;

namespace CalRemix.Content.Items.Placeables.Subworlds.Piggy
{
    public class StickBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
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
            Item.createTile = ModContent.TileType<StickBlockPlaced>();
            Item.width = 12;
            Item.height = 12;
        }

        public override void AddRecipes()
        {
            CreateRecipe(300).
                AddIngredient(ModContent.ItemType<PiggyItem>()).
                AddRecipeGroup(RecipeGroupID.Wood, 300).
                AddTile(TileID.Sawmill).
                Register();

            CreateRecipe().
                AddIngredient(ModContent.ItemType<StickWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}