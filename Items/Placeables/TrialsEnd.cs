using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Tiles;

namespace CalRemix.Items.Placeables
{
    public class TrialsEnd : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Trial's End");
            Tooltip.SetDefault("Once more resurrected and defeated, your adversaries may never find the peace they most desire");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<DoGPainting>();
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}