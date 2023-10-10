using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;

namespace CalRemix.Items.Potions
{
    public class MeldChipIceCream : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meld Chip Ice Cream");
            Tooltip.SetDefault("Better eat it before it melds\nMajor improvements to all stats\nIncreases life regeneration");
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 26;
            Item.rare = ItemRarityID.Lime;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.value = Item.sellPrice(gold: 10);
            Item.UseSound = SoundID.Item2;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTurn = true;
            Item.buffType = BuffID.WellFed2;
            Item.buffTime = 14400;
        }
    }
}
