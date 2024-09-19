using CalRemix.Content.Buffs;
using CalRemix.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class Ecstacy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ecstacy");
            Tooltip.SetDefault("Significantly boosts elemental strength");
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
            Item.rare = ItemRarityID.Purple;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item2;
            Item.value = Item.sellPrice(0, 0, 70);
            Item.buffType = ModContent.BuffType<ElementalAffinity>();
            Item.buffTime = 60 * 60 * 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient<ElementalBar>().
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}