using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
    public class ConquestFragment : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Conquest Fragment");
			Item.ResearchUnlockCount = 25;
    	}
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Orange;
            Item.value = 0;
			Item.maxStack = 9999;
    	}
	}
}
