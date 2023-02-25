using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
	public class EssenceofBabil : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Essence of Babil");
      	Tooltip.SetDefault("The essence of lively creatures");
			SacrificeTotal = 25;
    	}
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
			Item.maxStack = 999;
    	}
	}
}
