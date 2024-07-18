using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class IonogenMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (Main.netMode != NetmodeID.Server)
                ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = 1;
            Item.vanity = true;
        }
    }
}