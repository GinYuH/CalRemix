using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;

namespace CalRemix.Content.Items.Placeables
{
    public class Stonecutter : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Stonecutter");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.createTile = ModContent.TileType<StonecutterPlaced>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.StoneBlock, 3996).AddTile(TileID.HeavyWorkBench).Register();
        }
    }
}
