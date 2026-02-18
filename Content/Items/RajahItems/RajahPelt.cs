using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems
{
    public class RajahPelt : ModItem
	{
		public override void SetStaticDefaults()
		{
            //Tooltip.SetDefault("Surpisingly durable for a pelt of fur");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 20;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Yellow;
		}
	}
}
