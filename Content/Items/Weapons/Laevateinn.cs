using CalamityMod.Items;
using CalamityMod.Rarities;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons;

public class Laevateinn : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 560;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.width = 10;
        Item.height = 10;
        Item.useTime = 23;
        Item.useAnimation = 23;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.knockBack = 6;
        Item.rare = ModContent.RarityType<Turquoise>();
        Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
        Item.UseSound = SoundID.DD2_PhantomPhoenixShot;
        Item.autoReuse = true;
        Item.channel = true;
        Item.shoot = ModContent.ProjectileType<LaevateinnProj>();
        Item.shootSpeed = 0f;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (player.ownedProjectileCounts[Item.shoot] < 1)
        {
            return true;
        }
        return false;
    }
}

