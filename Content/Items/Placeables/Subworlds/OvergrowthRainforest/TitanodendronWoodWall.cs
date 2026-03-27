using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Subworlds.OvergrowthRainforest;
using CalRemix.Content.Walls;

namespace CalRemix.Content.Items.Placeables.Subworlds.OvergrowthRainforest
{
    public class TitanodendronWoodWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<TitanodendronWoodWallPlaced>());
        }
        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ModContent.ItemType<TitanodendronWood>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}