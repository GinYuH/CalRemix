using CalamityMod;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons
{
    public class WindTurbineBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.knockBack = 0f;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = 56;
            Item.useTime = 56;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item71;

            Item.width = 100;
            Item.height = 100;

            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.maxStack = 999;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 10, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WulfrumMetalScrap>(4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}