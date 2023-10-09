using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;

namespace CalRemix.Items.Potions
{
    public class ClamChowder : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clam Chowder");
            Tooltip.SetDefault("New Ilmeris style\nMedium improvements to all stats\nIncreases life regeneration");
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
            Item.value = Item.sellPrice(gold: 2);
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTurn = true;
            Item.buffType = BuffID.WellFed3;
            Item.buffTime = 28800;
        }
    }
}
