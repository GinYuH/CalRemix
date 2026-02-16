using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Rarities;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons;

public class PizzaWheel : RogueWeapon
{
    public override void SetDefaults()
    {
        Item.damage = 300;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.knockBack = 0.5f;
        Item.rare = ModContent.RarityType<DarkBlue>();
        Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
        Item.UseSound = SoundID.DD2_MonkStaffSwing;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<PizzaWheelProj>();
        Item.shootSpeed = 12f;
        Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (player.Calamity().StealthStrikeAvailable())
        {
            Projectile p = Main.projectile[Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PizzaWheelStealth>(), damage, knockback, player.whoAmI)];
            p.Calamity().stealthStrike = true;
            return false;
        }
        else 
            return true;
    }

}

