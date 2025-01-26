using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PhytogenMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (Main.netMode != NetmodeID.Server)
                ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            item.height = 20;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}