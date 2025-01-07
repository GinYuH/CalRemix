using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles;

namespace CalRemix.Content.Items.Placeables
{
    public class ChloriumOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<ChloriumOrePlaced>());
            Item.width = 16;
            Item.height = 16;
            Item.value = Item.sellPrice(silver: 15, copper: 50);
            Item.rare = ItemRarityID.Lime;
        }
    }
}