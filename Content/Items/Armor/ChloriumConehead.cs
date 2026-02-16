using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ChloriumConehead : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("16% increased stormbow damage and critical strike chance\n20% increaesd stormbow shot speed");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 17;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<StormbowDamageClass>() += 0.16f;
            player.GetCritChance<StormbowDamageClass>() += 0.16f;
            player.GetAttackSpeed<StormbowDamageClass>() += 0.20f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ChloriumChainmail>() && legs.type == ModContent.ItemType<ChloriumGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+12 flat stormbow damage";
            player.GetDamage<StormbowDamageClass>().Flat += 12;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ChloriumBar>(12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
