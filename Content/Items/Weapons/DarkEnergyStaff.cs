using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Rarities;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons;

public class DarkEnergyStaff : ModItem
{
    public override void SetDefaults()
    {
        Item.rare = ModContent.RarityType<Turquoise>();
        Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
        Item.damage = 76;
        Item.DamageType = DamageClass.Summon;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 14;
        Item.useAnimation = 14;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.mana = 10;
        Item.noMelee = true;
        Item.knockBack = 5;
        Item.UseSound = SoundID.DD2_EtherianPortalOpen;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<DarkEnergySentry>();
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        position = Main.MouseWorld;
        velocity.X = 0f;
        velocity.Y = 0f;
        int num = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<DarkEnergySentry>(), damage / 2, knockback, Main.myPlayer);
        if (Main.projectile.IndexInRange(num))
        {
            Main.projectile[num].originalDamage = Item.damage;
        }
        player.UpdateMaxTurrets();
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<SanctifiedSpark>(1).
            AddIngredient<DarkPlasma>(2).
            AddIngredient<GalacticaSingularity>(5).
            AddTile(TileID.LunarCraftingStation).
            Register();
    }

}

