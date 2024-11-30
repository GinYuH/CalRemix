using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Summon;
using CalRemix.Content.Items.Materials;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons;

public class CirnogenicStaff : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
        ItemID.Sets.LockOnIgnoresCollision[Type] = true;
    }
    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Yellow;
        Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
        Item.damage = 128;
        Item.DamageType = DamageClass.Summon;
        Item.mana = 12;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 24;
        Item.useAnimation = 24;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.knockBack = 3f;
        Item.UseSound = SoundID.Item78;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<CirnogenicSentry>();
        Item.shootSpeed = 12;
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        position = Main.MouseWorld;
        velocity = Vector2.Zero;
        int num = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
        if (Main.projectile.IndexInRange(num))
            Main.projectile[num].originalDamage = Item.damage;
        player.UpdateMaxTurrets();
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<CryogenicStaff>().
            AddIngredient<LifeAlloy>(9).
            AddIngredient<SoulofBright>(9).
            AddTile(TileID.MythrilAnvil).
            Register();
    }

}

