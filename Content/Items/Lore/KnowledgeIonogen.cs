using CalRemix.Content.Items.Placeables.Trophies;
using Terraria;
using Terraria.ID;

namespace CalRemix.Content.Items.Lore
{
    public class KnowledgeIonogen : RemixLoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("The Archmagian");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = RarityHelper.Ionogen;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<IonogenTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
