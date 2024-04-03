using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
    public class UnholyBloodCells : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unholy Blood Cells");
      		Tooltip.SetDefault("Just seeing it makes you want to puke...");
			Item.ResearchUnlockCount = 25;
    	}
		public override void SetDefaults()
        {
            Item.rare = ItemRarityID.LightPurple;
            Item.sellPrice(silver: 20);
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DepthCells>(5).
                AddIngredient<UnholyCore>(5).
                AddIngredient<BloodOrb>(20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
