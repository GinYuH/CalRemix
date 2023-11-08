using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
	public class Theswordisinsidethecore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The sword is inside the core");
      		Tooltip.SetDefault("Needless to say, it truly is inside the core");
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
                AddIngredient(ItemID.BrokenHeroSword).
                AddIngredient(ItemID.FrostCore).
                AddIngredient(ItemID.AncientBattleArmorMaterial).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
