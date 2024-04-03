using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    public class WormMeal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Worm Meal");
            Tooltip.SetDefault("'Surely my summons will eat this.. right?'");
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 70;
            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 2;
            player.GetModPlayer<CalRemixPlayer>().wormMeal = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<GulletWorm>()).
                AddIngredient(ItemID.Stinkfish, 3).
                AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
