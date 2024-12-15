using Terraria;
using Terraria.ModLoader;
using CalRemix.Content.Tiles;
using Terraria.ID;

namespace CalRemix.Content.Items.Placeables
{
    public class DerellectTrophyInv : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Derellect Trophy");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<DerellectTrophyPlaced>();
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}