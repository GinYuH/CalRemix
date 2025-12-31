using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Items.Ammo;

namespace CalRemix.Content.Items.Weapons
{
	public class TwentyTwoon : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.useTime = 22; 
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item108;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 220;
			Item.knockBack = 7.5f;
			Item.shoot = ModContent.ProjectileType<Twetwepoon>();
            Item.shootSpeed = 22f;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[Item.shoot] > 1)
                return false;
            for (int i = 1; i < 10; i++)
            {
                Vector2 pos1 = position + velocity.RotatedBy(MathHelper.Pi / 2) / velocity.Length() * 2f * i;
                Vector2 pos2 = position - velocity.RotatedBy(MathHelper.Pi / 2) / velocity.Length() * 2f * i;
                Projectile.NewProjectile(source, pos1, velocity, type, damage / 2, knockback, player.whoAmI);
                Projectile.NewProjectile(source, pos2, velocity, type, damage / 2, knockback, player.whoAmI);
            }
            Vector2 pos3 = position + velocity.RotatedBy(MathHelper.Pi / 2) / velocity.Length() * 2f;
            Vector2 pos4 = position - velocity.RotatedBy(MathHelper.Pi / 2) / velocity.Length() * 2f;
            Projectile.NewProjectile(source, pos3, velocity, type, damage / 2, knockback, player.whoAmI);
            Projectile.NewProjectile(source, pos4, velocity, type, damage / 2, knockback, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-24f, 0f);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Twentoon>().
                AddIngredient<Dualpoon>().
                AddIngredient<HornetRound>().
                AddIngredient<ExoPrism>(10).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
