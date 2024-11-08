using CalamityMod.Rarities;
using CalRemix.Content.Items.Lore;
using CalRemix.Content.Items.Placeables.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Lore
{
    public class KnowledgePyrogen : RemixLoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("The Archseer");
        }

        public override string LoreText => "Burn baby burn";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PyrogenTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
