using CalRemix.Items.Lore;
using CalRemix.Items.Placeables.Trophies;
using Terraria;
using Terraria.ID;

namespace CalRemix.Items.Lore
{
    public class KnowledgeCarcinogen : RemixLoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("The Archwizard");
        }

        public override string LoreText => "I've heard countless tales and adages about the joys of gambling. They're lying to you.\nThere's nothing left to gain when you lose it all. I'm unsure why Carcinoma thought differently.\nHowever, it is beyond astonishing to see the archwizard reduce himself to a wheezing mass.\nHis glamour decayed to scraps. Carcinoma was more of a hinderance than any form of help by the time I kicked him out of my forces.";

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
