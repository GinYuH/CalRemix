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
using CalamityMod.Items.Placeables.Plates;

namespace CalRemix.Items.Placeables.Plates
{
	public class Bloodplate : ModItem
    {
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Aeroplate>();
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<BloodplateTile>();
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
                .AddIngredient<BloodOrb>()
                .AddTile(TileID.Hellforge)
                .Register();
            CreateRecipe(3)
                .AddIngredient(ItemID.Obsidian, 3)
                .AddIngredient<BloodSample>()
                .AddTile(TileID.Hellforge)
                .Register();
            CreateRecipe(100)
                .AddIngredient(ItemID.Obsidian, 70)
                .AddIngredient<BloodyVein>()
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}