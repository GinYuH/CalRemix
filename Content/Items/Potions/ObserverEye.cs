using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class ObserverEye : ModItem
    {
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
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 1);
            Item.buffType = BuffID.WellFed3;
            Item.buffTime = 864000;
        }
    }
}