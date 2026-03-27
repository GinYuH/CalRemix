using CalRemix.Content.Items.Placeables.Subworlds.TheGray;
using CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Placeables.Subworlds.OvergrowthRainforest
{
    public class TitanodendronLeafBlock : ModItem
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
            Item.createTile = ModContent.TileType<TitanodendronLeafBlockPlaced>();
            Item.width = 12;
            Item.height = 12;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TitanodendronLeafBlockWall>(), 4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}