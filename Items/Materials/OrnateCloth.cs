using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
	public class OrnateCloth : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ornate Cloth");
			Item.ResearchUnlockCount = 5;
        }
		public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.sellPrice(gold: 50);
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Silk, 10).
                AddIngredient<SolarVeil>(10).
                AddIngredient<EffulgentFeather>(5).
                AddTile(TileID.Loom).
                Register();
        }
    }
}
