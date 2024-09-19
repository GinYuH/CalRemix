using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
	public class SaltWaterBolt : ModItem
	{
        public override void SetStaticDefaults() 
		{
            DisplayName.SetDefault("Salt Water Bolt");
            Item.ResearchUnlockCount = 1;
        }
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
			Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.useTime = 30; 
			Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item21;
            Item.DamageType = DamageClass.Magic;
			Item.damage = 21;
			Item.knockBack = 5.5f;
            Item.mana = 12;
			Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<SaltWater>();
            Item.shootSpeed = 7.5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SaltBooklet>().
                AddIngredient(ItemID.WaterBolt).
                AddIngredient(ItemID.WaterBucket).
                Register();
        }
    }
}
