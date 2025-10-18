using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Tiles;
using CalamityMod.Items.Placeables.Plates;
using CalamityMod.Items.Materials;
using Terraria.DataStructures;

namespace CalRemix.Content.Items.Placeables
{
    public class IonCube : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Ion Cube");
            // Tooltip.SetDefault("Only one may be placed");
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
            Item.createTile = ModContent.TileType<IonCubePlaced>();
            Item.width = 12;
            Item.height = 12;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<EssenceofSunlight>(), 5).
                AddIngredient(ModContent.ItemType<Cinderplate>(), 40).
                AddTile(TileID.Anvils).
                Register();
        }

        public override bool CanUseItem(Player player)
        {
            bool anyKenny = false;
            foreach (var T in TileEntity.ByID)
            {
                if (T.Value.type == ModContent.TileEntityType<IonCubeTE>())
                {
                    anyKenny = true;
                    break;
                }
            }
            return !anyKenny;
        }
    }
}