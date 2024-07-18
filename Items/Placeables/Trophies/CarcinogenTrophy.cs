using Terraria;
using Terraria.ModLoader;
using CalRemix.Tiles;
using CalRemix.Tiles.Trophies;

namespace CalRemix.Items.Placeables.Trophies
{
    public class CarcinogenTrophy : ModItem
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
            Item.createTile = ModContent.TileType<CarcinogenTrophyPlaced>();
            Item.width = 12;
            Item.height = 12;
            Item.rare = 1;
        }
    }
}