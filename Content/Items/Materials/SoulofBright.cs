using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
	public class SoulofBright : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 5;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
		public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            base.Item.value = Item.buyPrice(0, 4, 50);
            Item.maxStack = Item.CommonMaxStack;
        }
    }
}
