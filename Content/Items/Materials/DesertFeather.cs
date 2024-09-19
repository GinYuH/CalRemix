using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
	public class DesertFeather : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Desert Feather");
			Item.ResearchUnlockCount = 25;
    	}
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 1);
			Item.maxStack = 9999;
    	}
	}
}
