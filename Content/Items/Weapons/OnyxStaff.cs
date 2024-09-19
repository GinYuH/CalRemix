using CalamityMod.Items.Weapons.Magic;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons;

public class OnyxStaff : ModItem
{

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Onyx Staff");
        Tooltip.SetDefault("Casts volatile onyx fragments that explode on contact with tiles");
        Item.staff[Type] = true;
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.damage = 25;
        Item.DamageType = DamageClass.Magic;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.mana = 3;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.knockBack = 0;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(silver:80);
        Item.UseSound = SoundID.Item69;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<OnyxFragment>();
        Item.shootSpeed = 14f;

    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(7)).RotatedByRandom(MathHelper.ToRadians(15)), type, damage, knockback);
        Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-7)).RotatedByRandom(MathHelper.ToRadians(15)), type, damage, knockback);
        return true;
    }
    public override bool CanUseItem(Player player)
    {
        return player.ownedProjectileCounts[ModContent.ProjectileType<ButterflyMinion>()] < 3;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<AquamarineStaff>().
            AddIngredient(ItemID.AncientBattleArmorMaterial, 2).
            AddIngredient(ItemID.Obsidian,22).
            AddTile(TileID.Anvils).
            Register();
    }

}

