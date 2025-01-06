using CalRemix.Content.Items.Placeables.Trophies;
using Terraria;
using Terraria.ID;

namespace CalRemix.Content.Items.Lore
{
    public class KnowledgeOxygen : RemixLoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("The Archwitch");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = RarityHelper.Oxygen;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<OxygenTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
