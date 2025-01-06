using CalamityMod.Rarities;
using CalRemix.Content.Items.Placeables.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Lore
{
    public class KnowledgeClone : RemixLoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("The Archseer");
        }

        //public override string LoreText => "It seems as though Draedon's work on this reprehensible effigy was less incomplete than I thought.\nIt's despicable enough that the creature was operating on stolen magic, but to think it's fully aware of its dismal fate? I dread to think of what he had in mind for it at its fullest potential.\nNo doubt, it feared being imprisoned again, and used the abandoned construct as a shelter from a world it didn't yet understand.\nPlease, show it kindness- its existence is already painful enough. Do not repeat the same mistakes I made with the true witch.";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
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
