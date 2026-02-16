using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles.Subworlds.Piggy;

namespace CalRemix.Content.Items.Placeables.Subworlds.Piggy
{
    public class Slingshot : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 0;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<SlingshotPlaced>();
        }
    }
}
