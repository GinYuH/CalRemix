using CalamityMod.Items;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Rarities;
using CalamityMod;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SalvageMask : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.defense = 30;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SalvageSuit>() && legs.type == ModContent.ItemType<SalvageLegs>();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<ThrowingDamageClass>() += 22;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = CalRemixHelper.LocalText("Items.SalvageMask.SetBonus").Value;
            player.GetModPlayer<CalRemixPlayer>().salvageSuit = true;
            player.Calamity().rogueStealthMax += 1.15f;
            player.GetDamage<ThrowingDamageClass>() += 0.87f;
            player.GetCritChance<ThrowingDamageClass>() += 28;
            player.Calamity().wearingRogueArmor = true;
        }
    }
}
