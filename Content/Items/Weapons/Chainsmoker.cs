using CalamityMod;
using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class Chainsmoker : ModItem
    {
        public int FireRate = 8;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chainsmoker");
            Tooltip.SetDefault("Sprays enemies with smoke\nReleasing the weapon after a short period causes it to shoot a fireball\nThe fireball creates explosions when touching smoked enemies");
        }
        public override void SetDefaults()
        {
            Item.width = 114;
            Item.height = 58;
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = FireRate;
            Item.useAnimation = FireRate;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = RarityHelper.Carcinogen;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ChainsmokerHoldout>();
            Item.shootSpeed = 10f;
            Item.channel = true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void HoldItem(Player player) => player.Calamity().mouseWorldListener = true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile holdout = Projectile.NewProjectileDirect(source, position, Vector2.Zero, ModContent.ProjectileType<ChainsmokerHoldout>(), damage, knockback, player.whoAmI);

            // We set the rotation to the direction to the mouse so the first frame doesn't appear bugged out.
            holdout.velocity = (player.Calamity().mouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);

            return false;
        }
    }
}
