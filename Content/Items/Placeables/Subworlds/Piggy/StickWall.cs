using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Walls;

namespace CalRemix.Content.Items.Placeables.Subworlds.Piggy
{
    public class StickWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.createWall = ModContent.WallType<StickWallPlaced>();
            Item.useStyle = ItemUseStyleID.Swing;
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
                .AddIngredient<StickBlock>()
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}