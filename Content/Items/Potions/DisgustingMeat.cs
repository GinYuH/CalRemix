using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRemix.Content.Items.Potions
{
    public class DisgustingMeat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Disgusting Meat");
            Tooltip.SetDefault("Mmm... a delicacy, to be sure!\nBest served cold");
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(copper: 32);
        }
    }
}
