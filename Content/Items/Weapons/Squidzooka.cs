using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod.Items;
using CalRemix.Content.Projectiles;
using CalamityMod;
using CalRemix.Content.Projectiles.Weapons;
using CalamityMod.Rarities;

namespace CalRemix.Content.Items.Weapons
{
    public class Squidzooka : ModItem
    {
        public static int FireRate = 34;

        public override void SetDefaults()
        {
            Item.width = 107;
            Item.height = 58;
            Item.damage = 315;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = FireRate;
            Item.useAnimation = FireRate;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SquidzookaHoldout>();
            Item.shootSpeed = 24f;
            Item.channel = true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void HoldItem(Player player) => player.Calamity().mouseWorldListener = true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile holdout = Projectile.NewProjectileDirect(source, position, Vector2.Zero, ModContent.ProjectileType<SquidzookaHoldout>(), damage, knockback, player.whoAmI);

            // We set the rotation to the direction to the mouse so the first frame doesn't appear bugged out.
            holdout.velocity = (player.Calamity().mouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);

            return false;
        }
    }
}
