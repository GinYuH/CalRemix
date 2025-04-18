using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
	public class OrnateCloth : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ornate Cloth");
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
            CreateRecipe(10).
                AddIngredient(ItemID.Silk, 10).
                AddIngredient<DesertFeather>(6).
                AddIngredient<SolarVeil>(3).
                AddIngredient<EffulgentFeather>().
                AddTile(TileID.Loom).
                Register();
        }
    }
}
