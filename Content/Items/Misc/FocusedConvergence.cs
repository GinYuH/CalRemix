using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Misc
{
    public class FocusedConvergence : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(" ");
        }
        public override void UpdateInventory(Player player)
        {
            Item.stack = 0;
            Item.active = false;
        }
        public override void PostUpdate()
        {
            Item.active = false;
        }
    }
}