using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CalRemix.Content.Items.Weapons
{
public class Heterochromia : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Heterochromia");
        Tooltip.SetDefault("90% chance to not consume ammo\n'I am looking respectfully.'");

        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

    }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 56;
        Item.value = 100000;
        Item.rare = ItemRarityID.Yellow;
        Item.damage = 30;
        Item.useAnimation = 84;
        Item.useTime = 2;
        Item.useLimitPerAnimation = 7 * 4;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.consumable = false;
        Item.autoReuse = true;
        Item.DamageType = DamageClass.Ranged;
        Item.noMelee = true;
        Item.knockBack = 6.5f;
        Item.shoot = ProjectileID.WoodenArrowFriendly;
        Item.shootSpeed = 30f;
        Item.UseSound = SoundID.Item102;
        Item.useAmmo = AmmoID.Arrow;
    }

    private int shotCount = 0;
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        int arrowType = Main.rand.Next(0, 4);
        List<int> arrows = new List<int>() { ProjectileID.FrostburnArrow, ProjectileID.CursedArrow, ProjectileID.ShadowFlameArrow, ProjectileID.FireArrow };
        List<float> spread = new List<float> { -45 / 3, -45 / 6, -45 / 12, 0, 45 / 12, 45 / 6, 45 / 3 };
        velocity *= (Main.rand.Next(100, 151) / 100f);
        Projectile.NewProjectile(source, position, velocity.RotatedBy((spread[shotCount % 7] * (Math.PI / 180))), arrows[shotCount % 4], damage, knockback, player.whoAmI);
        shotCount++;
        return false;
    }

    public override bool CanConsumeAmmo(Item ammo, Player player)
    {
        return Main.rand.NextFloat() >= 0.90f;
    }
}
}