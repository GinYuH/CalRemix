using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Critters
{
    public class TheBealed : ModItem
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
            Item.rare = ItemRarityID.Cyan;
            Item.value = Terraria.Item.sellPrice(silver: 1);
            Item.bait = 40;
            Item.makeNPC = (short)ModContent.NPCType<NPCs.Subworlds.Sealed.TheBealed>();
        }
    }
}