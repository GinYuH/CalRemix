using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items.Weapons.Melee;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Rarities;
using CalamityMod.Items;

namespace CalRemix.Content.Items.Weapons
{
	public class Rox : ModItem
	{
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.useTime = 8; 
			Item.useAnimation = 8;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 300;
			Item.knockBack = 6.5f; 
            Item.shoot = ModContent.ProjectileType<Rox1>();
            Item.shootSpeed = 10;
            Item.crit = 16;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
                Item.noUseGraphic = true;
                return player.Remix().roxCooldown <= 0 && player.ownedProjectileCounts[ModContent.ProjectileType<RoxProj>()] < 1;
            }
            else
            {
                Item.noMelee = false;
                Item.noUseGraphic = false;
                return player.ownedProjectileCounts[ModContent.ProjectileType<RoxProj>()] < 1;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, Vector2.UnitY * 20f, ModContent.ProjectileType<RoxProj>(), damage * 5, 13);
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.PiOver4), type, damage / 10, knockback);
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Roxcalibur>().
                AddIngredient(ItemID.Extractinator).
                AddIngredient(ItemID.FragmentSolar).
                AddIngredient(ItemID.GoldenKey).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
