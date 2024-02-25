using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items;
using CalRemix.Projectiles.Weapons;

namespace CalRemix.Items.Weapons
{
	public class AcidBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Acid Bow");
            Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 10;
			Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.Rarity1BuyPrice;
            Item.useTime = 29; 
			Item.useAnimation = 29;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 12;
			Item.knockBack = 3f; 
			Item.noMelee = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 17f;
			Item.useAmmo = AmmoID.Arrow;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) 
        {
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<SulphurArrow>();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                double angle = Main.rand.NextBool() ? -1 : 1;
                Projectile.NewProjectile(source, position, velocity, type, damage * 2 / 3, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(11.25f) * angle).RotatedByRandom(MathHelper.ToRadians(11.25f)), type, damage * 2 / 3, knockback, player.whoAmI);
            }
            return false;
        }
    }
}
