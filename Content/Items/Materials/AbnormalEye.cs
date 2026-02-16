using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class AbnormalEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(silver: 90);
            Item.maxStack = 9999;
        }
    }
}
