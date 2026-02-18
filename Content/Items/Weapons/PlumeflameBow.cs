using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalRemix.Content.Projectiles.Weapons;

namespace CalRemix.Content.Items.Weapons
{
    public class PlumeflameBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Coats wooden arrows in magma\nMagmatic arrows are trailed by extremely hot volcanic mist");
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 70;
            Item.damage = 132;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.UseSound = BetterSoundID.ItemBeesKnees;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MagmaticArrow>();
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 2; ++index)
            {
                float SpeedX = velocity.X + Main.rand.Next(-25, 26) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-25, 26) * 0.05f;

                if (CalamityUtils.CheckWoodenAmmo(type, player))
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<MagmaticArrow>(), damage, knockback, player.whoAmI);
                else
                {
                    int proj = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
                    Main.projectile[proj].noDropItem = true;
                }
            }
            return false;
        }
    }
}
