using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles;

namespace CalRemix.Content.Items.Placeables
{
    public class MeldGunk : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            DisplayName.SetDefault("Meld Gunk");
            Tooltip.SetDefault("Friendship is strong, Meld Gunk is stronger.\nNo seriously, this block is completely unbreakable! Place with caution!");
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
            Item.createTile = ModContent.TileType<MeldGunkPlaced>();
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.White;
        }
    }
}