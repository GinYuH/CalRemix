using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
	public class CrocodileScale : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crocodile Scale");
			Item.ResearchUnlockCount = 25;
    	}
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(silver: 90);
			Item.maxStack = 9999;
    	}
	}
}
