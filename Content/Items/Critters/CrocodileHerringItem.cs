using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Critters
{
    public class CrocodileHerringItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crocodile Herring");
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
            Item.rare = ItemRarityID.Lime;
            Item.value = Terraria.Item.sellPrice(gold: 6);
            Item.bait = 20;
            Item.makeNPC = (short)ModContent.NPCType<NPCs.CrocodileHerring>();
        }
    }
}