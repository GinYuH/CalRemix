using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class BananaClownSleeves : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("2 magic damage");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 22;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<MagicDamageClass>().Flat += 2;
        }
    }
}
