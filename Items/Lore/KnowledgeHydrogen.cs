using CalRemix.Items.Lore;
using CalRemix.Items.Placeables.Trophies;
using Terraria;
using Terraria.ID;

namespace CalRemix.Items.Lore
{
    public class KnowledgeHydrogen : RemixLoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("The Archdiviner");
        }

        public override string LoreText => "I suppose Ivy is no stranger to secrets.\nWith this construct's shoddy design, I'm sure this was made far before her disappearance.\nIt's compelling to see how her magical endeavors have evolved over the centuries.\nControlled explosives are a force seldom seen on the battlefields.\nHowever, I find the utility of such tool more compelling.\nAs for why she kept it locked within the Sunken Seas is beyond me.\nI'm sure Amidas wouldn't be too keen on learning his precious homeland's gone.\nI would check up on him, if I were you. Losing your roots demoralizes a warrior beyond belief.\nI should know this, of course.";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = RarityHelper.Hydrogen;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<HydrogenTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
