using CalamityMod.Rarities;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items
{
    public class LabRoach : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lab Roach");
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
            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.value = Terraria.Item.sellPrice(copper: 6);
            Item.bait = 20;
            Item.makeNPC = (short)ModContent.NPCType<NPCs.LabRoach>();
        }
    }
}