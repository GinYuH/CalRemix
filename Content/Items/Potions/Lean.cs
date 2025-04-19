using CalamityMod.Items.Materials;
using CalRemix.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Potions
{
    public class Lean : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Devourer of Substances");
            // Tooltip.SetDefault("A SYRUP DOES NOT FEAR CONSUMPTION");
        }


        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 50);
            Item.buffType = ModContent.BuffType<ILoveLean>();
            Item.buffTime = 3600 * 60;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2).
                AddIngredient(ItemID.Vitamins).
                AddIngredient<CosmiliteBar>().
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}