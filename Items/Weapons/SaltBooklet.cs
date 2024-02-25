using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Projectiles.Weapons;
using CalamityMod.Items.Placeables;

namespace CalRemix.Items.Weapons
{
	public class SaltBooklet : ModItem
	{
        public override void SetStaticDefaults() 
		{
            DisplayName.SetDefault("Salt Booklet");
            Item.ResearchUnlockCount = 1;
        }
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
			Item.rare = ItemRarityID.White;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.useTime = 30; 
			Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item39;
            Item.DamageType = DamageClass.Magic;
			Item.damage = 10;
			Item.knockBack = 5.5f;
            Item.mana = 12;
			Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<SaltCube>();
            Item.shootSpeed = 7.5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Wood, 10).
                AddIngredient<SulphurousSand>(5).
                AddCondition(Condition.NearWater).
                Register();
        }
    }
}
