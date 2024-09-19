using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons;

public class Nystagmus : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Nystagmus");
        Tooltip.SetDefault("Conjures a spiral of homing eyes\n"+"'I am in spain without the A'");

        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

    }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 56;
        Item.value = 100000;
        Item.rare = 8;
        Item.damage = 20;
        Item.useAnimation = 36;
        Item.useTime = 1;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.consumable = false;
        Item.autoReuse = true;
        Item.channel = true;
        Item.DamageType = DamageClass.Magic;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.knockBack = 8f;
        Item.shoot = ModContent.ProjectileType<NystagmusProjectileBlue>();
        Item.shootSpeed = 60f;
        Item.mana = 36;
        Item.ArmorPenetration = 50;
    }

    private int shotnum = 0;
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        List<int> list = new List<int>() { ModContent.ProjectileType<NystagmusProjectileRed>(), ModContent.ProjectileType<NystagmusProjectileBlue>(), ModContent.ProjectileType<NystagmusProjectileGreen>(), ModContent.ProjectileType<NystagmusProjectileGray>() };
        shotnum++;
        Projectile.NewProjectile(source, position, 7.5f*new Vector2(0,5).SafeNormalize(Vector2.One).RotatedBy(Math.PI / 180 * ((shotnum % 36) * 10)), list[shotnum % 4], (int)(damage * 1.5f), knockback, player.whoAmI, 0);
        return false;
    }
}