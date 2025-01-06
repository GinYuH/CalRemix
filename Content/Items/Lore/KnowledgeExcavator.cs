using CalRemix.Content.Items.Placeables.Trophies;
using Terraria;
using Terraria.ID;

namespace CalRemix.Content.Items.Lore
{
    public class KnowledgeExcavator : RemixLoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ExcavatorTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
