using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
	public class CausticClaw : ModItem
	{
        public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Caustic Claw");
            Item.ResearchUnlockCount = 1;
        }
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
			Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.useTime = 11; 
			Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
            Item.UseSound = SoundID.Item73;
            Item.DamageType = DamageClass.Magic;
			Item.damage = 22;
			Item.knockBack = 3f;
            Item.mana = 6;
			Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<CausticTendril>();
            Item.shootSpeed = 22f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.myPlayer == player.whoAmI)
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(45f)), type, damage / 2, knockback, player.whoAmI);
            return false;
        }
    }
}
