using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
	public class CoreofBabil : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Core of Babil");
			SacrificeTotal = 25;
    	}
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
			Item.maxStack = 999;
    	}
        public override void AddRecipes()
        {
            CreateRecipe(3).
                AddIngredient<EssenceofBabil>(1).
                AddIngredient(ItemID.HallowedBar).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
