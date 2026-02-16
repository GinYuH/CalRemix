using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles;
using CalRemix.Content.Items.Accessories;

namespace CalRemix.Content.Items.Placeables
{
    public class AstralPearlBlock : ModItem
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
            Item.createTile = ModContent.TileType<AstralPearlBlockPlaced>();
            Item.width = 12;
            Item.height = 12;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<AstralPearl>(), 9).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}