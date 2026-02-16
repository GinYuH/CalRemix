using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;

namespace CalRemix.Content.Items.Placeables
{
    public class AltaroftheAblessed : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Altar of the Ablessed");
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
            Item.value = Item.buyPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.createTile = ModContent.TileType<AltaroftheAblessedPlaced>();
        }
    }
}
