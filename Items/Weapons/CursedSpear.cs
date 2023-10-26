using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Projectiles.Weapons;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Items.Weapons
{
	public class CursedSpear : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Cursed Spear");
		}
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 2, silver: 40);
            Item.useTime = 19; 
			Item.useAnimation = 19;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
            Item.UseSound = IchorSpear.ThrowSound;
			Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
			Item.damage = 76;
			Item.knockBack = 6f; 
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<CursedSpearProj>();
            Item.shootSpeed = 20;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            if (calamityPlayer.StealthStrikeAvailable())
            {
                int num = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (num.WithinBounds(1000))
                {
                    Main.projectile[num].Calamity().stealthStrike = true;
                }

                return false;
            }
            else
                return true;
        }
    }
}
