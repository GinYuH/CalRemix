using CalRemix.Content.Items.Placeables.Trophies;
using Terraria;
using Terraria.ID;

namespace CalRemix.Content.Items.Lore
{
    public class KnowledgeCarcinogen : RemixLoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("The Archwizard");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = RarityHelper.Carcinogen;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CarcinogenTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
