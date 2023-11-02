using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class AngelsThesis : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Angel's Thesis");
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
        ItemID.Sets.LockOnIgnoresCollision[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 2, silver: 40);
        Item.damage = 37;
        Item.DamageType = DamageClass.Summon;
        Item.mana = 10;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 18;
        Item.useAnimation = 18;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.knockBack = 1;
        Item.UseSound = SoundID.Item113;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<AnimatedStatue>();
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        position = Main.MouseWorld;
        velocity = Vector2.Zero;
        int num = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, Main.myPlayer);
        if (Main.projectile.IndexInRange(num))
            Main.projectile[num].originalDamage = Item.damage;
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient(ItemID.JungleRose).
            AddIngredient(ItemID.DarkShard).
            AddIngredient(ItemID.LightShard).
            AddTile(TileID.Anvils).
            Register();
    }

}

