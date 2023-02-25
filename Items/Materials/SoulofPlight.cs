using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
	public class SoulofPlight : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Plight");
      	Tooltip.SetDefault("The artificial essence of powerful machines");
			SacrificeTotal = 25;
    	}
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Lime;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
			Item.maxStack = 999;
    	}
	}
}
