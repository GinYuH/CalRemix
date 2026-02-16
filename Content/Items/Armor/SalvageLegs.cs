using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SalvageLegs : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.defense = 30;
        }

        public override void UpdateEquip(Player player)
        {
            player.Calamity().rogueVelocity += 0.09f;
            player.moveSpeed += 0.05f;
        }
    }
}
