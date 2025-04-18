using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Content.Items.Weapons
{
	public class IchorDagger: RogueWeapon
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            // DisplayName.SetDefault("Ichor Dagger");
		}
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 7, silver: 20);
            Item.useTime = 15; 
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
            Item.UseSound = CursedDagger.ThrowSound;
			Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
			Item.damage = 34;
			Item.knockBack = 4.5f; 
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<IchorDaggerProj>();
            Item.shootSpeed = 12;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            if (calamityPlayer.StealthStrikeAvailable())
            {
                int num = Projectile.NewProjectile(source, position, velocity * 1.5f, type, damage, knockback, player.whoAmI);
                if (num.WithinBounds(1000))
                    Main.projectile[num].Calamity().stealthStrike = true;
                int num2 = Projectile.NewProjectile(source, position, (velocity * Main.rand.NextFloat(1f, 1.25f)).RotatedByRandom(MathHelper.ToRadians(45f)), type, damage, knockback, player.whoAmI);
                if (num2.WithinBounds(1000))
                    Main.projectile[num2].Calamity().stealthStrike = true;
                int num3 = Projectile.NewProjectile(source, position, (velocity * Main.rand.NextFloat(1f, 1.25f)).RotatedByRandom(MathHelper.ToRadians(45f)), type, damage, knockback, player.whoAmI);
                if (num3.WithinBounds(1000))
                    Main.projectile[num3].Calamity().stealthStrike = true;
                return false;
            }
            else
                return true;
        }
    }
}
