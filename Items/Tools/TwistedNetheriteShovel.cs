using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalRemix.Items.Materials;

namespace CalRemix.Items.Tools
{
    public class TwistedNetheriteShovel : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 60;
            Item.damage = 2;
            Item.knockBack = 1f;
            Item.useTime = 60;
            Item.useAnimation = 60;

            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TwistedNetheriteBar>(3).
                AddRecipeGroup("Wood", 2).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
