using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Critters;
using CalRemix.Content.Tiles.Subworlds.Piggy;
using CalRemix.Content.Tiles.Subworlds.Wolf;

namespace CalRemix.Content.Items.Placeables.Subworlds.Wolf
{
    public class WolfSnow : ModItem
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
            Item.createTile = ModContent.TileType<WolfSnowPlaced>();
            Item.width = 12;
            Item.height = 12;
        }
    }
}