using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Projectiles.Weapons;
using CalamityMod.Items;

namespace CalRemix.Items.Weapons
{
	public class RazorTeeth : ModItem
	{
		public override void SetStaticDefaults() 
		{
            Item.ResearchUnlockCount = 1;
            DisplayName.SetDefault("Razor Teeth");
		}
		public override void SetDefaults() 
		{
            Item.width = 1;
			Item.height = 1;
            Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.Rarity2BuyPrice;
            Item.useTime = 17; 
			Item.useAnimation = 17;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.damage = 20;
			Item.knockBack = 2f; 
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<RazorTooth>();
            Item.shootSpeed = 12;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            if (calamityPlayer.StealthStrikeAvailable())
            {
                for (int i = -1; i <= 1; i++)
                {
                    int num = Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(11.25f * i)), type, damage, knockback, player.whoAmI);
                    if (num.WithinBounds(Main.maxProjectiles))
                        Main.projectile[num].Calamity().stealthStrike = true;
                }
                return false;
            }
            else
                return true;
        }
    }
}
