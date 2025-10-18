using CalamityMod.Items;
using CalRemix.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class ElementalChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.defense = 30;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<DefaultDamageClass>() += 0.25f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalBar>(20).
                AddIngredient<WaterMesh>(24).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
