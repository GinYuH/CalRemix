using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class CryoKeyMold : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cryo Key Mold");
            Tooltip.SetDefault("Used for crafting a Cryo Key");
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.maxStack = 9999;
            item.width = 20;
            item.height = 20;
        }
    }
}
