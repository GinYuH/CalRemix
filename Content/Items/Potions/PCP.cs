using CalRemix.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class PCP : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pixies' Capricious Power");
            Tooltip.SetDefault("This dust feels too erratic");
        }


        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
            Item.value = Item.sellPrice(0, 0, 70);
            Item.buffType = ModContent.BuffType<Fairied>();
            Item.buffTime = 60 * 30;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PixieDust, 3).
                AddTile(TileID.AlchemyTable).
                AddCondition(Condition.NearWater).
                AddCondition(Condition.InHallow).
                Register();
        }
    }
}