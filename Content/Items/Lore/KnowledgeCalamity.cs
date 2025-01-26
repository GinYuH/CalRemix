using CalRemix.Content.Items.Misc;
using Terraria;
using Terraria.ID;

namespace CalRemix.Content.Items.Lore
{
    public class KnowledgeCalamity : RemixLoreItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
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
