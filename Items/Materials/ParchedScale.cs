using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items
{
	public class ParchedScale : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Parched Scale");
      Tooltip.SetDefault("Large scale of an apex predator");
			SacrificeTotal = 25;
    }
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
    }
	}
}
