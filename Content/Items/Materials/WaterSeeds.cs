using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class WaterSeeds : ModItem
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
            CreateRecipe(50).
                AddIngredient(ModContent.ItemType<AbnormalSample>()).
                AddIngredient(ItemID.DaybloomSeeds).
                AddIngredient(ItemID.BlinkrootSeeds).
                AddIngredient(ItemID.ShiverthornSeeds).
                AddIngredient(ItemID.WaterleafSeeds).
                AddIngredient(ItemID.MoonglowSeeds).
                AddIngredient(ItemID.DeathweedSeeds).
                AddIngredient(ItemID.FireblossomSeeds).
                AddTile(TileID.MythrilAnvil).
                DisableDecraft().
                Register();
        }
    }
}
