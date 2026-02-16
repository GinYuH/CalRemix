using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Items.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class WaterMesh : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 2);
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<FireApple>()).
                AddIngredient(ModContent.ItemType<WaterSeeds>(), 5).
                AddTile(TileID.MythrilAnvil).
                DisableDecraft().
                Register();

            CreateRecipe().
                AddIngredient(ModContent.ItemType<Astrogel>()).
                AddIngredient(ModContent.ItemType<CosmiliteSlag>(), 5).
                AddIngredient(ItemID.WaterBucket).
                AddIngredient(ItemID.Bottle, 3).
                AddTile(TileID.MythrilAnvil).
                DisableDecraft().
                Register();
        }
    }
}
