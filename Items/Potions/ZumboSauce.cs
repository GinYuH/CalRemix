using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;

namespace CalRemix.Items.Potions
{
    public class ZumboSauce : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zumbo Sauce");
            Item.ResearchUnlockCount = 5;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 26;
            Item.rare = ItemRarityID.LightPurple;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTurn = true;
            Item.buffType = BuffID.WellFed3;
            Item.buffTime = 108000;
        }
    }
}
