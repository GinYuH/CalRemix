using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalRemix.Content.Tiles;
using CalRemix.Content.Items.Placeables.Plates;

namespace CalRemix.Content.Items.Placeables
{
    public class OrigenDoor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Door of Origen");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<OrigenDoorPlaced>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<Curseplate>(), 999).
                AddTile(TileID.Anvils)
                .Register();
        }
    }
}
