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
        public override string LoreText => "Oh, the Archseer, my biggest blister.\nHold on a moment. Where is she? I could've swore Draedon entrapped the correct personnel.\nIt's too late to reprimand this error. Even then, I see no similarities in them.\nPerhaps a new set of ocular lens will benefit the mad scientist.\nI admire the strength Cinder flaunted throughout her onslaught.\nTruly, words fail to capture my intrigue, no matter the quality.\nBeyond her physical prowess, she posed a threat to the inner workings of my adversaries.\nIn the face of war, I see no need to spare mind to love. Permafrost, on the other hand, may say otherwise.\nHowever, my army does not tolerate rubber-willed traitors.\nWe were able to quell the wildfire that is Cinder without Permafrost's help. A chain's only as strong as its weakest link.\nMy esteemed engineer handled the threat well. Whether it was with excessive force is past my judgment.\nI wish the collapse of their eclectic found family. Even love has its faults.";
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
