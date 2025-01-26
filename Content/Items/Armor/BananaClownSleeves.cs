using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
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
            item.height = 20;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<MagicDamageClass>().Flat += 2;
        }
    }
}
