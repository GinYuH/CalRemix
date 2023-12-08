using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalRemix.Tiles;
using CalamityMod.Items;
using CalRemix.Tiles.Plates;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Pets;
using CalRemix.Items.Materials;

namespace CalRemix.Items.Placeables.Plates
{
	public class Aeroplate : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Bloodplate>();
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<AeroplateTile>();
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