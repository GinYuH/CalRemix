using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Potions
{
    public class TitanFinger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Titan Finger");
            Tooltip.SetDefault("Contact with the nefarious anglerfish\nMajor improvements to all stats\nIncreases life regeneration");
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 26;
            Item.rare = ItemRarityID.Pink;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.value = Item.sellPrice(gold: 5);
            Item.UseSound = SoundID.Item2;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTurn = true;
            Item.buffType = BuffID.WellFed2;
            Item.buffTime = 14400;
        }
    }
}
