using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles;
using CalRemix.Core.World;

namespace CalRemix.Content.Items.Placeables
{
    public class Asbestos : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            // DisplayName.SetDefault("Asbestos Block");
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
            Item.createTile = ModContent.TileType<AsbestosPlaced>();
            Item.width = 12;
            Item.height = 12;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ClayBlock, 20).
                AddIngredient(ItemID.Cobweb).
                AddTile(TileID.Anvils).
                AddCondition(new Condition("On worlds generated without CalRemix 1.3+", () => !CalRemixWorld.postGenUpdate)).
                DisableDecraft().
                Register();

            CreateRecipe().
                AddIngredient<AsbestosWall>().
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}