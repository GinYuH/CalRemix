using CalamityMod.Items;
using CalRemix.Content.Items.Placeables.Subworlds.Sealed;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class CarnelianWoodLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityWhiteBuyPrice;
            Item.rare = ItemRarityID.White;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<DefaultDamageClass>() += 0.02f;
            player.moveSpeed += 0.12f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CarnelianWood>(18).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
