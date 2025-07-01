using CalamityMod.Rarities;
using CalRemix.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class PurpleShavedIceEdible : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 26;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.value = Item.sellPrice(gold: 1);
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTurn = true;
            Item.buffType = BuffID.WellFed3;
            Item.buffTime = 28800;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PurpleShavedIce>(600).
                Register();
        }
    }
}
