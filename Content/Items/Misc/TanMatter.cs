using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Misc
{
    public class TanMatter : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Quest;
            Item.maxStack = 9999;
        }
    }
}
