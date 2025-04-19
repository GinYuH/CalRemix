using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.Materials;

namespace CalRemix.Content.Items.Potions
{
    public class NotFabsolVodka : ModItem
    {
        public override string Texture => "CalamityMod/Items/Potions/Alcohol/FabsolsVodka";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fluorescent Vodka");
            // Tooltip.SetDefault("Boosts all damage stats by 8% but lowers defense by 10%\nIncreases immune time after being struck");
        }


        public override void SetDefaults()
        {
            Item.CloneDefaults(ModContent.ItemType<FabsolsVodka>());
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Ale, 1).
                AddIngredient(ItemID.UnicornHorn, 1).
                AddIngredient<BloodOrb>(40).
                AddTile(TileID.AlchemyTable).
                Register();
            CreateRecipe().
                AddIngredient(ItemID.Ale, 1).
                AddIngredient(ItemID.PixieDust, 10).
                AddIngredient(ItemID.CrystalShard, 5).
                AddIngredient(ItemID.UnicornHorn, 1).
                AddTile(TileID.Kegs).
                Register();
        }
    }
}