using CalamityMod;
using CalamityMod.Items.Weapons.Summon;
using CalRemix.Buffs;
using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Items.Weapons;

public class MackerelStaff : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Holy Mackerel Staff");
        Item.ResearchUnlockCount = 1;
        ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
        ItemID.Sets.LockOnIgnoresCollision[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 1);
        Item.damage = 46;
        Item.DamageType = DamageClass.Summon;
        Item.mana = 10;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.knockBack = 1.2f;
        Item.UseSound = SoundID.Item21;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<HolyMackerel>();
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
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<HerringStaff>().
            AddIngredient(ItemID.CrystalSerpent).
            AddIngredient(ItemID.HallowedBar, 10).
            AddIngredient(ItemID.SoulofLight, 20).
            AddTile(TileID.MythrilAnvil).
            Register();
    }

}

