using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
	public class DeadeyeRevolver : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
		}
        public override void SetDefaults() 
		{
			Item.width = 32;
			Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
			Item.useTime = 40;
            Item.useAnimation = 40;
            Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 11;
			Item.knockBack = 5f;
			Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<DeadeyeRevolverMeatball>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			position += velocity;
			for (int u = 0; u < 3; u++)
			{
                Vector2 specilVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15)) * (1 + (Main.rand.Next(-5,6) * 0.05f));
                Projectile.NewProjectile(source, position, specilVelocity, type, damage, knockback, player.whoAmI);
            }  
            return false;
        }
    }
}