using System;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Summon;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons
{
    public class NowhereStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 46;
            Item.damage = 78;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1f;

            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;

            Item.UseSound = SoundID.Item44 with { Pitch = -0.5f };
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<NowhereDragonLight>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[type] > 0 && player.ownedProjectileCounts[ModContent.ProjectileType<NowhereAura>()] <= 0 && !player.HasCooldown("NowhereAura"))
            {
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<NowhereAura>(), 0, 0, player.whoAmI);
                SoundEngine.PlaySound(BetterSoundID.ItemPortalGun2 with { Pitch = 0.2f });
            }
            else if (player.ownedProjectileCounts[type] <= 0)
            {
                Vector2 mouseDirection = Vector2.Normalize(Main.MouseWorld - player.Center) * Item.shootSpeed;

                float slots = player.maxMinions - player.slotsMinions;

                int p = Projectile.NewProjectile(source, Main.MouseWorld, mouseDirection.RotatedBy(MathHelper.PiOver2), ModContent.ProjectileType<NowhereDragonLight>(), damage, knockback, Main.myPlayer, 0f, 0f);
                if (Main.projectile.IndexInRange(p))
                {
                    Main.projectile[p].originalDamage = Main.projectile[p].damage = Item.damage + Item.damage * (int)(slots * 0.3f);
                    Main.projectile[p].minionSlots = slots / 2;
                    Main.projectile[p].scale = 1 + slots / 2 * 0.2f;
                }
                p = Projectile.NewProjectile(source, Main.MouseWorld, mouseDirection.RotatedBy(-MathHelper.PiOver2), ModContent.ProjectileType<NowhereDragonDark>(), damage, knockback, Main.myPlayer, 0f, 0f);
                Main.projectile[p].originalDamage = Main.projectile[p].damage = Item.damage + Item.damage * (int)(slots * 0.3f);
                Main.projectile[p].minionSlots = slots / 2;
                Main.projectile[p].scale = 1 + slots / 2 * 0.2f;
            }
            return false;
        }
    }
}
