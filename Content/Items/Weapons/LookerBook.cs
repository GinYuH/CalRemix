using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;

namespace CalRemix.Content.Items.Weapons
{
	public class LookerBook : RogueWeapon
	{
        public override void SetDefaults() 
		{
            Item.width = 22;
			Item.height = 26;
            Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.useTime = 30; 
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 28;
			Item.knockBack = 4.5f;
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<LookerBookProj>();
            Item.shootSpeed = 6;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.statMana < 40)
            {
                int num = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 1);
                return false;
            }
            player.statMana -= 40;
            player.manaRegenDelay = (int)player.maxRegenDelay;
            return true;
        }
    }
}
