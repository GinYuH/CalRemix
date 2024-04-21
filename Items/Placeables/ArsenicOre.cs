using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Tiles;
using CalamityMod.Items;
using CalamityMod.Rarities;

namespace CalRemix.Items.Placeables
{
    public class ArsenicOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            DisplayName.SetDefault("Arsenic Ore");
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
            Item.createTile = ModContent.TileType<ArsenicOrePlaced>();
            Item.width = 12;
            Item.height = 12;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ModContent.RarityType<DarkOrange>();
        }
    }
}