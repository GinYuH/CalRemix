using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
    public class GreenDemonHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Demon Head");
      		Tooltip.SetDefault("\n\n\n\n\n\n\n\n");
			Item.ResearchUnlockCount = 25;
    	}
		public override void SetDefaults()
        {
            Item.rare = ItemRarityID.LightPurple;
            Item.sellPrice(silver: 20);
            Item.maxStack = 9999;
        }
    }
}
