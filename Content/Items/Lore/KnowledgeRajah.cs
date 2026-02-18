using CalRemix.Content.Items.Placeables.Trophies;
using CalRemix.Content.Items.RajahItems;
using Terraria;
using Terraria.ID;

namespace CalRemix.Content.Items.Lore
{
    public class KnowledgeRajah : RemixLoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Yellow;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RajahTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
