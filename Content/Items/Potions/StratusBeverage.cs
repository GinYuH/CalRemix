using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Rarities;
using CalRemix.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class StratusBeverage : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stratus Beverage");
            // Tooltip.SetDefault("May make you feel spacey\nCauses all projectile shooting weapons to fire two extra homing stars\nDecreases weapon damage by 33%");
        }


        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item2;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.value = Item.buyPrice(gold: 10);
            Item.buffType = ModContent.BuffType<StratusBeverageBuff>();
            Item.buffTime = 60 * 60 * 5;

        }

        public override void AddRecipes()
        {
            CreateRecipe(5).
                AddIngredient(ItemID.BottledWater, 5).
                AddIngredient(ModContent.ItemType<Lumenyl>(), 2).
                AddIngredient(ModContent.ItemType<RuinousSoul>(), 1).
                AddIngredient(ModContent.ItemType<ExodiumCluster>(), 1).
                AddTile(TileID.AlchemyTable).
                Register();
            CreateRecipe(5).
                AddIngredient(ItemID.BottledWater, 5).
                AddIngredient(ModContent.ItemType<RuinousSoul>(), 1).
                AddIngredient(ModContent.ItemType<UnholyCore>(), 1).
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}