using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class Meth : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ashes of the Sunken Sea");
            // Tooltip.SetDefault("These fine crystals feel very powerful");
        }


        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 20;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item2;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 50);
            Item.buffType = ModContent.BuffType<Buffs.MethHigh>();
            Item.buffTime = 6000;
        }
    }
}