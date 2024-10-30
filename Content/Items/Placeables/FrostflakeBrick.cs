using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles;
using Terraria;

namespace CalRemix.Content.Items.Placeables
{
    public class FrostflakeBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Frostflake Brick");
            Item.ResearchUnlockCount = 100;
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
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<FrostflakeBrickPlaced>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<FrostflakeWall>(), 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}