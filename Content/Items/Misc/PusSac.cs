using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Misc
{
    public class PusSac : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Quest;
            Item.maxStack = 9999;
        }
    }
}
