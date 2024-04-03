using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items.Materials;

namespace CalRemix.Items.Weapons
{
    public class Triploon : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ItemRarityID.Lime;
			Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.useTime = 20; 
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item10;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 70;
			Item.knockBack = 7.5f;
			Item.shoot = ProjectileID.Harpoon;
			Item.shootSpeed = 20f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, 0f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
            Vector2 pos1 = position + Vector2.One.RotatedBy(MathHelper.Pi / 2) * 12f;
            Vector2 pos2 = position - Vector2.One.RotatedBy(MathHelper.Pi / 2) * 12f;
            Projectile.NewProjectile(source, position, velocity, type, damage / 2, knockback, player.whoAmI);
            Projectile.NewProjectile(source, pos1, velocity, type, damage / 2, knockback, player.whoAmI);
            Projectile.NewProjectile(source, pos2, velocity, type, damage / 2, knockback, player.whoAmI);
            return false;
		}
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Dualpoon>().
                AddIngredient(ItemID.Harpoon, 2).
                AddIngredient<DepthCells>(15).
                AddIngredient<ScoriaBar>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
