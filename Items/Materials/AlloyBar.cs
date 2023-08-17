using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace CalRemix.Items.Materials
{
	public class AlloyBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alloy Bar");
			Item.ResearchUnlockCount = 25;
    	}
		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 50);
			Item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CopperBar).
                AddIngredient(ItemID.TinBar).
                AddIngredient(ItemID.IronBar).
                AddIngredient(ItemID.LeadBar).
                AddIngredient(ItemID.SilverBar).
                AddIngredient(ItemID.TungstenBar).
                AddIngredient(ItemID.GoldBar).
                AddIngredient(ItemID.PlatinumBar).
                AddIngredient(ItemID.DemoniteBar).
                AddIngredient(ItemID.CrimtaneBar).
                AddIngredient(ItemID.MeteoriteBar).
                AddIngredient(ItemID.HellstoneBar).
                AddIngredient(ItemID.CopperBar).
                AddIngredient(ModContent.ItemType<SeaRemains>()).
                AddIngredient(ModContent.ItemType<AerialiteBar>()).
                AddTile(TileID.Furnaces).
                Register();
        }
    }
}
