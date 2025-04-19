using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
	public class SaltBooklet : ModItem
	{
        public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Salt Booklet");
            Item.ResearchUnlockCount = 1;
        }
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
			Item.rare = ItemRarityID.White;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.useTime = 30; 
			Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item39;
            Item.DamageType = DamageClass.Magic;
			Item.damage = 10;
			Item.knockBack = 5.5f;
            Item.mana = 1;
			Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<SaltCube>();
            Item.shootSpeed = 8.5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Wood, 10).
                AddIngredient(ItemID.StoneBlock, 5).
                AddCondition(Condition.NearWater).
                Register();
        }
    }
}
