using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Trophies;
using Terraria.ID;

namespace CalRemix.Content.Items.Placeables.Trophies
{
    public class OrigenTrophy : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<OrigenTrophyPlaced>();
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.Blue;
        }
    }
}