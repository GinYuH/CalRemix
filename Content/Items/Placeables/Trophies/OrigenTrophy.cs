using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Trophies;

namespace CalRemix.Content.Items.Placeables.Trophies
{
    public class OrigenTrophy : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<OrigenTrophyPlaced>();
            Item.width = 12;
            Item.height = 12;
            Item.rare = 1;
        }
    }
}