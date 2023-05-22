using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
	public class ParchedScale : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Parched Scale");
      	// Tooltip.SetDefault("Large scale of an apex predator");
			Item.ResearchUnlockCount = 25;
    	}
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
			Item.maxStack = 999;
    	}
	}
}
