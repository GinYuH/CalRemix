using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles;
using CalamityMod.Items;

namespace CalRemix.Content.Items.Placeables
{
    public class CosmiliteSlag : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Item.ResearchUnlockCount = 100;
			DisplayName.SetDefault("Cosmilite Slag");
		}
		public override void SetDefaults() 
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 14;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<CosmiliteSlagPlaced>();
			Item.width = 12;
			Item.height = 12;
			Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
			Item.rare = ItemRarityID.Purple;
		}
	}
}