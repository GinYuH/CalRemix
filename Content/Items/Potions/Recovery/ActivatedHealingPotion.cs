using CalamityMod.Rarities;
using CalRemix.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Potions;


namespace CalRemix.Content.Items.Potions.Recovery
{
    public class ActivatedHealingPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 32;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.healLife = 600;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.potion = true;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(5).
                AddIngredient<OmegaHealingPotion>(10).
                AddIngredient<AccidatedReactiveEssence>(5).
                AddIngredient<BloodredReactiveEssence>(5).
                AddIngredient<NocticReactiveEssence>(5).
                AddIngredient<WaterfreezeReactiveEssence>(5).
                AddTile(TileID.Bottles).
                Register()
                .DisableDecraft();
        }
    }
}