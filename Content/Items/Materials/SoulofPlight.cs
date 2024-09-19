using CalamityMod.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
	public class SoulofPlight : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Plight");
      		Tooltip.SetDefault("The artificial essence of powerful machines");
			Item.ResearchUnlockCount = 25;
			ItemID.Sets.ItemNoGravity[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
		public override void SetDefaults()
		{
            Item.rare = ItemRarityID.Lime;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
			Item.maxStack = 9999;
    	}
	}
}
