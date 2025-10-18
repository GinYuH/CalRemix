using CalamityMod.Items;
using CalRemix.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class ElementalLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.14f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalBar>(14).
                AddIngredient<WaterMesh>(18).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
