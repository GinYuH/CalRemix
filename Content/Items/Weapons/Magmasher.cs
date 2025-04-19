using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Rarities;

namespace CalRemix.Content.Items.Weapons
{
    public class Magmasher : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Throws out hammers that explode on enemy hits\nStealth strikes cause consecutive explosions to increase in size and damage");
        }
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 60;
            Item.damage = 460;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 45;
            Item.knockBack = 12f;
            Item.UseSound = SoundID.Item1;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.shoot = ModContent.ProjectileType<MagmasherHammer>();
            Item.shootSpeed = 16f;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
