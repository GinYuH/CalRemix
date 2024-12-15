using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Plates;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Placeables.Plates
{
    public class Aeroplate : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Curseplate>();
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<AeroplateTile>();
            Item.useStyle = ItemUseStyleID.Swing;
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
            CreateRecipe(3)
                .AddIngredient(ItemID.Obsidian, 3)
                .AddIngredient<EssenceofBabil>()
                .AddTile(TileID.Hellforge)
                .Register();

            CreateRecipe()
                .AddIngredient<AeroplateWall>(4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}