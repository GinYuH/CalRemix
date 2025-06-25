using CalamityMod.Items.Materials;
using CalRemix.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class AlloyBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 25;
    	}
		public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<AlloyBarPlant>());
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 50);
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
                AddIngredient(ModContent.ItemType<WulfrumMetalScrap>()).
                AddIngredient(ModContent.ItemType<AntlionBar>()).
                AddIngredient(ModContent.ItemType<AerialiteBar>()).
                AddTile(TileID.Furnaces).
                Register();
        }
    }
}
