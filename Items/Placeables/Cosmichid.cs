using CalamityMod.Items;
using CalRemix.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Placeables
{
	public class Cosmichid : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmichid");
			SacrificeTotal = 25;
    	}
		public override void SetDefaults()
		{
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 14;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<CosmichidPlant>();
            Item.width = 12;
            Item.height = 12;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = ItemRarityID.Blue;
    	}

        public override bool ItemSpace(Player player)
        {
            if (player.GetModPlayer<CalRemixPlayer>().crystalconflict)
            {
                return true;
            }
            return false;
        }

        public override bool OnPickup(Player player)
        {
            if (player.GetModPlayer<CalRemixPlayer>().crystalconflict && player.GetModPlayer<CalRemixPlayer>().cosdam <= 0.3f)
            {
                player.GetModPlayer<CalRemixPlayer>().cosdam += 0.01f * Item.stack;
                return false;
            }
            return true;
        }
    }
}
