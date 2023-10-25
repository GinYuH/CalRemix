using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items
{
    public class MinnowsPrimeItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minnows Prime");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Purple;
            Item.bait = 1;
            Item.makeNPC = (short)ModContent.NPCType<NPCs.MinnowsPrime>();
        }
    }
}