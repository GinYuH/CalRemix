using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Walls;

namespace CalRemix.Items.Placeables
{
    public class AsbestosWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.createWall = ModContent.WallType<AsbestosWallPlaced>();
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient<Asbestos>()
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}