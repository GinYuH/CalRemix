using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;

namespace CalRemix.Content.Items.Placeables
{
    public class HallowEffigy : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Hallow Effigy");
            Tooltip.SetDefault("When placed down nearby players have their damage, crit, and movement speed increased by 25%,\n"+
            "their defense by 20, their DR by 8%, and their health by 20%\n"+
            "Nearby players also suffer a permanent chaos state"); 
        }

        public override void SetDefaults()
        {
            Item.width = 20;
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
            Item.createTile = ModContent.TileType<HallowEffigyPlaced>();
        }
    }
}
