using CalamityMod.Items;
using CalRemix.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class ChloriumGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("12% increased critical strike chance\n22% increased movement speed");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<DefaultDamageClass>() += 0.12f;
            player.moveSpeed += 0.22f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ChloriumBar>(18).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
