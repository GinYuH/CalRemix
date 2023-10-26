using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Sounds;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace CalRemix.Items.Weapons
{
	public class Axisdriver : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Axisdriver");
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            Tooltip.SetDefault("Shoots a burst of 3 exo bullets that tear through enemies\n" +
                "Critical hits rain elemental bolts from above\n" +
                "Right click to fire an explosive homing exo bullet\n" +
                "50% chance to not consume bullets"); 
		}

		public override void SetDefaults() 
		{
			Item.width = 116;
			Item.height = 36;
			Item.rare = ModContent.RarityType<Violet>();
			Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.useTime = 13; 
			Item.useAnimation = 39;
			Item.reuseDelay = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = CommonCalamitySounds.LargeWeaponFireSound;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 655;
			Item.knockBack = 8f; 
			Item.noMelee = true;
			Item.crit = 28;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
            Item.consumeAmmoOnLastShotOnly = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 39;
                Item.useAnimation = 39;
                Item.reuseDelay = 0;
            }
            else
            {
                Item.useTime = 13;
                Item.useAnimation = 39;
                Item.reuseDelay = 15;
            }
            return true;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.50f;
        }
        public override Vector2? HoldoutOffset() 
		{
			return new Vector2(-30f, -2f);
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;

			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) 
            {
				position += muzzleOffset;
			}
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			int projInd = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AxisExoBullet>(), damage, knockback, player.whoAmI);
			Projectile proj = Main.projectile[projInd];
            if (player.altFunctionUse == 2)
            {
				proj.ai[0] = 1;
				proj.damage = damage * 5;
            }
            else
            {
				proj.ai[0] = 0;
            }
            return false;
		}
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalBlaster>(1).
                AddIngredient<ClockGatlignum>(1).
                AddIngredient<TyrannysEnd>(1).
                AddIngredient<Infinity>(1).
                AddIngredient<Karasawa>(1).
                AddIngredient<AcesHigh>(1).
                AddIngredient<RubicoPrime>(1).
                AddIngredient<PridefulHuntersPlanarRipper>(1).
                AddIngredient<MiracleMatter>(1).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
