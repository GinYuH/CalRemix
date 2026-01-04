using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class Carrot : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("");
		}
		
		public override void SetDefaults()
		{
            Item.UseSound = SoundID.Item2;
            Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.maxStack = 30;
			Item.consumable = true;
			Item.width = 16;
			Item.height = 16;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.buffType = BuffID.WellFed;
			Item.buffTime = 52000;
		}

        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.NightOwl, 52000);
            return base.UseItem(player);
        }
	}
}
