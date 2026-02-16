using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalRemix.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class TwistedNetheriteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 25;
    	}
		public override void SetDefaults()
		{
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.maxStack = Item.CommonMaxStack;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TwistingNether>().
                AddIngredient<CosmiliteSlag>(3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
