using static Terraria.ModLoader.ModContent;
using CalamityMod.Items.Accessories;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    public class Aeroluminescence : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            GetModItem(ItemType<AeroStone>()).UpdateAccessory(player, hideVisual);
            player.hasMagiluminescence = true;
            Lighting.AddLight(player.Center, new Vector3(0, 0.4f, 0.6f));
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AeroStone>().
                AddIngredient(ItemID.Magiluminescence, 1).
                AddTile(TileID.TinkerersWorkbench).
            Register();
        }
    }
}
