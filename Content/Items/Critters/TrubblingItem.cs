using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Critters
{
    public class TrubblingItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
            Item.value = Terraria.Item.sellPrice(silver: 6);
            Item.bait = 10;
            Item.makeNPC = (short)ModContent.NPCType<NPCs.Trubbling>();
        }
    }
}