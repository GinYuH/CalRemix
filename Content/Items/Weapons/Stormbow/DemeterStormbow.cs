using System;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.Magic;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Projectiles.Weapons;
using CalRemix.Content.Projectiles.Weapons.Stormbow;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class DemeterStormbow : ModItem, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;

            Item.width = 22;
            Item.height = 46;
            Item.damage = 84;
            Item.crit = 16;
            Item.useTime = 28;
            Item.useAnimation = 28;

            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<TerraHawk>();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 cursorPos = player.Center;
            cursorPos.Y -= 450;
            float speedX = Main.rand.Next(-60, 91) * 0.02f;
            float speedY = Main.rand.Next(-60, 91) * 0.02f;

            // arrow position noise pass
            cursorPos.X += Main.rand.Next(-60, 61);
            cursorPos.Y += Main.rand.Next(-60, 61);

            // if to right of player, right direct proj. else, left
            if (Main.MouseWorld.X - player.Center.X > 0)
            {
                cursorPos.X -= 1500;
                speedX += 30;
            }
            else
            {
                cursorPos.X += 1500;
                speedX -= 30;
            }

            int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI, 0.0f);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FourOClock>().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<RisingFire>().
                AddIngredient<GiantStormbow>().
                AddIngredient<LivingShard>(12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}