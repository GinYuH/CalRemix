using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Subworlds.GreatSea;
using CalamityMod.Items;
using CalRemix.Content.Tiles.Subworlds.Sealed;
using CalRemix.Content.Walls;

namespace CalRemix.Content.Items.Placeables.Subworlds.Sealed
{
    public class CarnelianRose : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<CarnelianRosePlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CarnelianiteOre>()).
                AddIngredient(ItemID.JungleRose)
                .AddTile(TileID.WorkBenches).DisableDecraft()
                .Register();
        }
    }
}