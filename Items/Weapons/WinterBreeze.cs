using Terraria.DataStructures;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;

namespace CalRemix.Items.Weapons
{
    public class WinterBreeze : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Winter Breeze");
            Tooltip.SetDefault("Fires several additional icicle arrows that shatter on impact");
        }
        public override void SetDefaults()
        {
            Item.width = 72;
            Item.height = 36;
            Item.damage = 120;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IcicleArrowProj>();
            Item.shootSpeed = 16f;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 5; i++)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-10, 11) * 0.25f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-10, 11) * 0.25f;
                int index = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<IcicleArrowProj>(), damage, knockback, player.whoAmI);
                Main.projectile[index].noDropItem = true;
            }

            return true;
        }
    }
}
