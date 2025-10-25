using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
	public class Twentoon : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ModContent.RarityType<DarkBlue>();
			Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.useTime = 20; 
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item108;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 200;
			Item.knockBack = 7.5f;
			Item.shoot = ModContent.ProjectileType<Twepoon>();
			Item.shootSpeed = 20f;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (player.ownedProjectileCounts[Item.shoot] > 1)
				return false;
			for (int i = 1; i < 9; i++)
            {
                Vector2 pos1 = position + velocity.RotatedBy(MathHelper.Pi / 2) / velocity.Length() * 6.5f * i;
                Vector2 pos2 = position - velocity.RotatedBy(MathHelper.Pi / 2) / velocity.Length() * 6.5f * i;
                Projectile.NewProjectile(source, pos1, velocity, type, (int)(damage / 2.5), knockback, player.whoAmI);
                Projectile.NewProjectile(source, pos2, velocity, type, (int)(damage / 2.5), knockback, player.whoAmI);
            }
            Vector2 v = velocity;
            v.Normalize();
            Vector2 pos3 = position + (velocity.RotatedBy(MathHelper.Pi / 2) / velocity.Length() * 2f) + v * 16f;
            Vector2 pos4 = position - (velocity.RotatedBy(MathHelper.Pi / 2) / velocity.Length() * 2f) + v * 16f;
            Projectile.NewProjectile(source, pos3, velocity, type, (int)(damage / 2.5), knockback, player.whoAmI);
            Projectile.NewProjectile(source, pos4, velocity, type, (int)(damage / 2.5), knockback, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16f, -16f);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Triploon>().
                AddIngredient<UelibloomBar>(10).
                AddIngredient(ItemID.MusketBall, 20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
