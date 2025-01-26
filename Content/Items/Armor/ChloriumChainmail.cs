using CalamityMod.Items;
using CalRemix.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class ChloriumChainmail : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("24% increased damage\n2% increased critical strike chance");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            item.height = 20;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 21;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<DefaultDamageClass>() += 0.24f;
            player.GetCritChance<DefaultDamageClass>() += 0.2f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ChloriumBar>(24).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
