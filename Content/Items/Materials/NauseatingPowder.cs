using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class NauseatingPowder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 5);
            Item.maxStack = 9999;
        }


        public override void AddRecipes()
        {
            CreateRecipe(5)
                .AddIngredient(ModContent.ItemType<MonorianGemShards>(), 5)
                .AddIngredient(ModContent.ItemType<NeonDust>(), 5)
                .AddIngredient(ModContent.ItemType<ActivePlumestone>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
