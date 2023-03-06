using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
	public class CoyoteVenom : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coyote Venom");
      	Tooltip.SetDefault("Try not to drink it");
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
