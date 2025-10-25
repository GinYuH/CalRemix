using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.DamageClasses;

namespace CalRemix.Content.Items.Weapons.Farming
{
    public class TwistedNetheriteScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 60;
            Item.damage = 260;
            Item.knockBack = 5f;
            Item.useTime = 12;
            Item.useAnimation = 12;

            Item.DamageType = ModContent.GetInstance<FarmingDamageClass>();
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
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
