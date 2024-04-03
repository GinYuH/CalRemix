using CalRemix.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
namespace CalRemix.Items

{
    public class GrarbleObliterator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grarble Obliterator");
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.knockBack = 1f;
            Item.useTime = 6;
            Item.useAnimation = 25;
            Item.pick = 50;
            Item.axe = 30 / 5;

            Item.DamageType = DamageClass.Melee;
            Item.width = 36;
            Item.height = 18;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item23;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GrarbleObliteratorProj>();
            Item.shootSpeed = 40f;
		}

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnyGoldBar", 3).
                AddIngredient(ItemID.Granite, 5).
                AddIngredient(ItemID.Marble ,5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
