using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class Mercury : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.maxStack = 9999;
        }
    }
}
