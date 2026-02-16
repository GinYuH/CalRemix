using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Subworlds.Piggy;
using CalamityMod.Items.Critters;

namespace CalRemix.Content.Items.Placeables.Subworlds.Piggy
{
    public class StrawBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
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
            Item.createTile = ModContent.TileType<StrawBlockPlaced>();
            Item.width = 12;
            Item.height = 12;
        }

        public override void AddRecipes()
        {
            CreateRecipe(300).
                AddIngredient(ModContent.ItemType<PiggyItem>()).
                AddIngredient(ItemID.Hay, 300).
                Register();

            CreateRecipe().
                AddIngredient(ModContent.ItemType<StrawWall>(), 4).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}