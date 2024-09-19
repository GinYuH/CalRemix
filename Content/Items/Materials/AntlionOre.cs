using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class AntlionOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antlion Ore");
			Item.ResearchUnlockCount = 25;
    	}
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 2);
            Item.maxStack = 9999;
    	}
	}
}
