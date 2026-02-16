using CalamityMod.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using CalamityMod.Items;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class RedWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(190, 15f, 3f);
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;
            player.moveSpeed += 0.2f;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1.1f;
            maxAscentMultiplier = 2.25f;
            constantAscend = 0.15f;
        }
    }
}
