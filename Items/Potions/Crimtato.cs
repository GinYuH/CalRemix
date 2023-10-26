using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Potions
{
    public class Crimtato : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimtato");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 20;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item2;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(platinum: 1, gold: 4);
            Item.buffType = BuffID.WellFed3;
            Item.buffTime = 864000;
        }
    }
}