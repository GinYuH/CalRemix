using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Misc
{
    public class IOU : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("IOU an Originator");
            Tooltip.SetDefault("Gone Fishin'! I'll get you the Originator once I have Fishstick's head on a pike!\n- Kushim");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(silver: 2);
            Item.rare = RarityHelper.Origen;
        }
    }
}