using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.Items.Tools
{
    public class TwistedNetheritePickaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 60;
            Item.damage = 60;
            Item.knockBack = 10f;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.pick = 600;

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
