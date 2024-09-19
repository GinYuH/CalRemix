using CalRemix.Content.Items.Lore;
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

        public override string LoreText => "That's it? Some child toy's defunct battery? You've got to be kidding me.\nI've picked locks with stronger batteries. This isn't impressive.\nAt least that electronic bucket of bolts won't hamper my ambitions any longer.\nWars used to be fought with sticks and stones. To see this device as a remnant of evolution is a shame.\nDo better. The Inventors were better at their jobs than whoever constructed this pity.";

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
