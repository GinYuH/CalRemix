using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace CalRemix.Content.Items.Weapons
{
    public class Megaskeet : ModItem
	{
        private int shootCount = 1;
        public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Megaskeet");
            /* Tooltip.SetDefault("50% chance to not consume ammo\n" +
                "Fires streams of hard sunlight every other shot\n" +
                "Fires a homing sunskater every 17 shots, which explodes into cinder shards on death"); */ 
		}

		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ModContent.RarityType<Violet>();
			Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.useTime = 3; 
			Item.useAnimation = 3;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item40;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 129;
			Item.knockBack = 2f; 
			Item.noMelee = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 22f;
			Item.useAmmo = AmmoID.Bullet;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (shootCount % 2 == 0 || shootCount == 17)
            {
                return false;
            }
            return Main.rand.NextFloat() >= 0.50f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (shootCount > 17)
            {
                shootCount = 0;
            }
            if (shootCount % 2 == 0)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            }
            else
            {
                int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.HeatRay , damage, knockback, player.whoAmI);
                Main.projectile[proj].DamageType = DamageClass.Ranged;
                Main.projectile[proj].penetrate = -1;
            }
            if (shootCount == 17)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SkeeterBullet>(), damage, knockback, player.whoAmI);
            }
            shootCount++;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SDOMG>(1).
                AddIngredient<ClockGatlignum>(1).
                AddIngredient<YharonSoulFragment>(5).
                AddIngredient<CoreofSunlight>(17).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
