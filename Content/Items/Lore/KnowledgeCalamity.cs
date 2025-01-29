using CalRemix.Content.Items.Misc;
using Terraria;
using Terraria.ID;

namespace CalRemix.Content.Items.Lore
{
    public class KnowledgeCalamity : RemixLoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.White;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CalamitousCertificate>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
