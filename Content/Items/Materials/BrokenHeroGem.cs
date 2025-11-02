using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Materials
{
    public class BrokenHeroGem : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 2);
            Item.maxStack = 9999;
        }
    }
}
