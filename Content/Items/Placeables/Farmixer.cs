using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using CalRemix.Content.Items.Materials;
using CalamityMod.Items.Potions.Food;

namespace CalRemix.Content.Items.Placeables
{
    public class Farmixer : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<FarmixerPlaced>());
            Item.value = 0;
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Furnace).
                AddIngredient<DubiousPlating>(10).
                AddTile(TileID.WorkBenches).
                Register();

            Recipe.Create(ItemID.Blinkroot, 10).
                AddIngredient<Cosmichid>().
                AddTile<FarmixerPlaced>().
                Register();

            Recipe.Create(ItemID.Daybloom, 10).
                AddIngredient<Cosmichid>().
                AddTile<FarmixerPlaced>().
                Register();

            Recipe.Create(ItemID.Moonglow, 10).
                AddIngredient<Cosmichid>().
                AddTile<FarmixerPlaced>().
                Register();

            Recipe.Create(ItemID.Deathweed, 10).
                AddIngredient<Cosmichid>().
                AddTile<FarmixerPlaced>().
                Register();

            Recipe.Create(ItemID.Waterleaf, 10).
                AddIngredient<Cosmichid>().
                AddTile<FarmixerPlaced>().
                Register();

            Recipe.Create(ItemID.Fireblossom, 10).
                AddIngredient<Cosmichid>().
                AddTile<FarmixerPlaced>().
                Register();

            Recipe.Create(ItemID.Shiverthorn, 10).
                AddIngredient<Cosmichid>().
                AddTile<FarmixerPlaced>().
                Register();

            Recipe.Create(ModContent.ItemType<DeliciousMeat>(), 20).
                AddIngredient<AlloyBar>().
                AddTile<FarmixerPlaced>().
                Register();

            Recipe.Create(ModContent.ItemType<DeliciousMeat>(), 150).
                AddIngredient<EssentialEssenceBar>().
                AddTile<FarmixerPlaced>().
                Register();

            Recipe.Create(ModContent.ItemType<DeliciousMeat>(), 40).
                AddIngredient<ElementalBar>().
                AddTile<FarmixerPlaced>().
                Register();

            Recipe.Create(ModContent.ItemType<DeliciousMeat>(), 80).
                AddIngredient<HauntedBar>().
                AddTile<FarmixerPlaced>().
                Register();

            Recipe.Create(ModContent.ItemType<DeliciousMeat>(), 350).
                AddIngredient<YharimBar>().
                AddTile<FarmixerPlaced>().
                Register();
        }
    }
}
