using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Tools
{
    public class Quadnoculars : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Quadnoculars");
        }

        public override void SetDefaults()
        {
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
        }
        public override void HoldItem(Player player)
        {
            player.scope = true;
        }
    }
}
