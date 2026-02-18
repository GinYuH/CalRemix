using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Rarities;

namespace CalRemix.Content.Items.Materials
{
    public class AccidatedReactiveEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 25;
            ItemID.Sets.ItemNoGravity[Type] = true;
    	}
		public override void SetDefaults()
		{
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = Item.sellPrice(gold: 5);
            Item.maxStack = 9999;
    	}
	}
}