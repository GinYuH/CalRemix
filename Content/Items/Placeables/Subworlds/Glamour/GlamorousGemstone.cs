using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles.Subworlds.Piggy;
using CalamityMod.Items.Critters;
using CalRemix.Content.Tiles.Subworlds.Glamour;
using CalRemix.Content.Items.Weapons.Stormbow;

namespace CalRemix.Content.Items.Placeables.Subworlds.Glamour
{
    public class GlamorousGemstone : ModItem
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
            Item.createTile = ModContent.TileType<GlamorousGemstonePlaced>();
            Item.width = 12;
            Item.height = 12;
        }

        public override void AddRecipes()
        {
            CreateRecipe(300).
                AddIngredient(ModContent.ItemType<BigEater>()).
                Register();
            CreateRecipe()
                .AddIngredient<GlamorousGemWall>(4)
                .AddTile(TileID.WorkBenches)
                .Register();
            CreateRecipe()
                .AddIngredient<LargeGlamorousGemWall>(4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}