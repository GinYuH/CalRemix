using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons
{
    public class PinesPenetrator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pine's Penetrator");
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ChristmasTreeSword);
        }
    }
}
