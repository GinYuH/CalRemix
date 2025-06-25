using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace CalRemix.Content.Items.Weapons
{
	public class AcesLow : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Ace's Low");
            // Tooltip.SetDefault("Fires a string of piercing spade cards when musket balls are used as ammo");
		}

		public override void SetDefaults() 
		{
			Item.width = 116;
			Item.height = 36;
			Item.rare = ItemRarityID.Green;
			Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.useTime = 12; 
			Item.useAnimation = 36;
			Item.reuseDelay = 36;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 25;
			Item.knockBack = 2f; 
			Item.noMelee = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
            Item.consumeAmmoOnLastShotOnly = true;
        }
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet)
			{
				type = ModContent.ProjectileType<AcesLowCard>();
			}
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage / 2, knockback);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("IronBar").
                AddIngredient<SeaRemains>(2).
                AddIngredient(ItemID.Sapphire,3).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
