using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using CalRemix.Items.Materials;

namespace CalRemix.Items
{
	public class Babilfish : ModItem
	{

		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Babilfish");
			// Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults() 
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
		}

		public override bool CanRightClick() 
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<EssenceofBabil>(), Main.rand.Next(5, 11));
		}
	}
}
