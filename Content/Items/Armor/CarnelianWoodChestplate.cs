using CalamityMod.Items;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class CarnelianWoodChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityWhiteBuyPrice;
            Item.rare = ItemRarityID.White;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<DefaultDamageClass>() += 0.1f;
            player.GetCritChance<DefaultDamageClass>() += 0.05f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CarnelianWood>(24).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
