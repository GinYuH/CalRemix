using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Subworlds.GreatSea;
using CalRemix.Content.Tiles.Subworlds.Pinnacles;

namespace CalRemix.Content.Items.Placeables.Subworlds.Pinnacles
{
    public class PowderedAsh : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
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
            Item.createTile = ModContent.TileType<PowderedAshPlaced>();
            Item.width = 12;
            Item.height = 12;
        }
    }
}