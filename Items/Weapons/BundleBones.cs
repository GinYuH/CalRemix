using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Projectiles.Weapons;

namespace CalRemix.Items.Weapons
{
	public class BundleBones : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 25;
            DisplayName.SetDefault("Bundle o' Bones");
		}
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(copper: 60);
            Item.useTime = 30; 
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
			Item.damage = 24;
			Item.knockBack = 2f; 
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.shoot = ModContent.ProjectileType<BoneBundle>();
            Item.shootSpeed = 8;
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
