using CalRemix.Items.Lore;
using CalRemix.Items.Placeables.Trophies;
using Terraria;
using Terraria.ID;

namespace CalRemix.Items.Lore
{
    public class KnowledgePhytogen : RemixLoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("The Archdruid");
        }

        public override string LoreText => "What an enigmatic construct. Is that truly Silva?\nHow did she escape from the bottom of the abyss? That is rather concerning.\nI wonder if I could harness that magic in the event of my own death.\nEven after my body has long expired, my soul remains in a construct of my own.\nWhat would you call it? Aurigen? Oncogen?\nDo not say Protogen.";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Lime;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PhytogenTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
