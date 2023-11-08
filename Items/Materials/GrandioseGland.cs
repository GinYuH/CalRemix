using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
	public class GrandioseGland : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grandiose Gland");
      		Tooltip.SetDefault("It's disgusting");
			Item.ResearchUnlockCount = 5;
    	}
		public override void SetDefaults()
        {
            Item.rare = ItemRarityID.LightPurple;
            Item.sellPrice(silver: 80);
            Item.maxStack = 9999;
        }
    }
}
