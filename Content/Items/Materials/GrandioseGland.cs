using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
	public class GrandioseGland : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Grandiose Gland");
      		// Tooltip.SetDefault("It's disgusting");
			Item.ResearchUnlockCount = 5;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
		public override void SetDefaults()
        {
            Item.rare = ItemRarityID.LightPurple;
            Item.sellPrice(silver: 80);
            Item.maxStack = 9999;
        }
    }
}
