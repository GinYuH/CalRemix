using CalamityMod.Rarities;
using CalRemix.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Potions
{
    public class EntropicFrond : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entropic Frond");
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
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = Item.buyPrice(gold: 50);
            Item.buffType = ModContent.BuffType<EntropicallyFed>();
            Item.buffTime = 600;
        }
    }
}