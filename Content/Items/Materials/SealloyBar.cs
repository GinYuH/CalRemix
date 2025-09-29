using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class SealloyBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 10);
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<Mercury>(), 4).
                AddIngredient(ModContent.ItemType<SealToken>(), 4).
                AddTile(TileID.AdamantiteForge).
                Register();
        }
    }
}
