using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items
{
    public class GulletWorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gullet Worm");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Orange;
            Item.bait = 120;
            Item.value = Item.sellPrice(gold: 10);
            Item.makeNPC = (short)ModContent.NPCType<NPCs.GulletWorm>();
        }
    }
}