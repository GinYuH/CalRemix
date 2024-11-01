using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Walls;
using Terraria;

namespace CalRemix.Content.Items.Placeables
{
    public class FrostflakeWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Frostflake Brick Wall");
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.White;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createWall = ModContent.WallType<FrostflakeWallFriendlyPlaced>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ModContent.ItemType<FrostflakeBrick>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}