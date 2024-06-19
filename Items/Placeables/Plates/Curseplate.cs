using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Tiles.Plates;
using CalRemix.Items.Materials;

namespace CalRemix.Items.Placeables.Plates
{
    public class Curseplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Bloodplate>();
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<CurseplateTile>();
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.sellPrice(0, 0, 3, 0);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            CreateRecipe(33)
                .AddIngredient(ItemID.Obsidian, 33)
                .AddIngredient(ItemID.Nazar)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}