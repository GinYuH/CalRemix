using CalRemix.Items.Lore;
using CalRemix.Items.Placeables.Trophies;
using Terraria;
using Terraria.ID;

namespace CalRemix.Items.Lore
{
    public class KnowledgeOxygen : RemixLoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("The Archwitch");
        }

        public override string LoreText => "All atoms—especially at a gaseous state such as oxygen—display an unpredictable nature akin to gods.\nNature mimics itself unabashedly from the smallest building blocks to the largest deities.\nI'm unsure of where this construct lands. If you were to ask me, this elemental disgrace is unfitting for the title of divinity.\nIf it weren't for the hundreds of bars of pressure presented by the deepest depths of the abyss, I would consider this construct formiddable.\nAlas, like all magi, their mobile graves are nothing more than fickle jokes.";

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
