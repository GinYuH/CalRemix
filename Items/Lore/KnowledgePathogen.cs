using CalRemix.Items.Lore;
using CalRemix.Items.Placeables.Trophies;
using Terraria;
using Terraria.ID;

namespace CalRemix.Items.Lore
{
    public class KnowledgePathogen : RemixLoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("The Archmagus");
        }

        public override string LoreText => "Oh, Mona. Look where your bag of tricks have taken you.\nI'd say it's a shame to see you at the hands of my enemy, but I hold no sympathy.\nAs they say, the enemy of my enemy is my friend. Well done.\nEven as a construct, she hasn't changed since the day she left.\nI remember her ambitions of godhood. She idolized the men above through her daily tasks and work alike.\nMona was a nuisance to my plans and my troops. Out of the two hundred and seventy days she served under my wing, she was only docile for nine of them.\nIdolaters such as her are unwelcomed in my ranks.";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = RarityHelper.Pathogen;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PathogenTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
