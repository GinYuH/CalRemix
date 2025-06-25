using CalRemix.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class CrabLeaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Leaves of Crabulon");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item2;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 4, silver: 20);
            Item.buffType = ModContent.BuffType<Leaves>();
            Item.buffTime = 936000;
        }
    }
}