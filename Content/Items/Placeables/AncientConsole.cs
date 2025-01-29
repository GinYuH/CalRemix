using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Rarities;
using CalamityMod.Items.Placeables.FurnitureAshen;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.DraedonStructures;

namespace CalRemix.Content.Items.Placeables
{
    public class AncientConsole : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Ancient Console");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 0, 0);
            Item.rare = ModContent.RarityType<Violet>();
            Item.createTile = ModContent.TileType<Content.Tiles.AncientConsole>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<AgedLaboratoryConsoleItem>()).
                AddIngredient(ModContent.ItemType<AuricBar>(), 20).
                AddIngredient(ModContent.ItemType<AshenSlab>(), 100).
                AddTile(ModContent.TileType<AshenAltar>()).
                Register();
        }
    }
}
