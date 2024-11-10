using CalamityMod.Items;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class PineappleStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 52;
            Item.damage = 31;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0.5f;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = RarityHelper.Phytogen;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PineappleSentry>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                int p = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 30f);
                if (Main.projectile.IndexInRange(p))
                    Main.projectile[p].originalDamage = Item.damage;

                player.UpdateMaxTurrets();
                //projectile.ai[1] is attack cooldown.  Setting it here prevents immediate attacks
            }
            return false;
        }
    }
}
