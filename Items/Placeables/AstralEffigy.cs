using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Tiles;

namespace CalRemix.Items.Placeables
{
    public class AstralEffigy : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Astral Effigy");
            // Tooltip.SetDefault("When placed down nearby players gain immunity to space's low gravity");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 9, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.createTile = ModContent.TileType<AstralEffigyPlaced>();
        }
    }
}
