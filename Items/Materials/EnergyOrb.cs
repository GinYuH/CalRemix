using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Items.Materials
{
    public class EnergyOrb : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            DisplayName.SetDefault("Energy Orb");
            Tooltip.SetDefault("This is literally useless for now");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 9999;
        }
    }
}
