using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems
{
    public class RajahTrophy : ModItem
	{
        public static int type;

        public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 2000;
			Item.rare = ItemRarityID.Blue;
			Item.createTile = Mod.Find<ModTile>("RajahTrophy").Type;
		}
	}
}