using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;
using CalamityMod.Items.Fishing.SunkenSeaCatches;

namespace CalRemix.Content.Items.Placeables
{
    public class GaggleStatue : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Gaggle Statue");
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
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.White;
            Item.createTile = ModContent.TileType<GaggleStatuePlaced>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<SurfClam>(), 60).
                AddIngredient(ItemID.StoneBlock, 50).
                AddTile(TileID.HeavyWorkBench).
                Register();
        }
    }
}
