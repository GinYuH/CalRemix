using CalamityMod.Items;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Rarities;
using CalamityMod.Sounds;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons;

public class PhreaticChanneler : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Phreatic Channeler");
        Tooltip.SetDefault("Summons molten tools which swiftly slice apart enemies");
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
        ItemID.Sets.LockOnIgnoresCollision[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.rare = ModContent.RarityType<Turquoise>();
        Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
        Item.damage = 113;
        Item.DamageType = DamageClass.Summon;
        Item.mana = 10;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.knockBack = 3f;
        Item.UseSound = CommonCalamitySounds.SwiftSliceSound;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<MoltenTools>();
        Item.shootSpeed = 12;
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        position = Main.MouseWorld;
        velocity = Vector2.Zero;
        int num = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
        if (Main.projectile.IndexInRange(num))
            Main.projectile[num].originalDamage = Item.damage;
        return false;
    }
}

