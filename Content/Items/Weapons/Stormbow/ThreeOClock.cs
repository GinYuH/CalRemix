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
    public class ThreeOClock : ModItem, ILocalizedModType
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
            Item.damage = 46;
            Item.crit = 8;
            Item.useTime = 32;
            Item.useAnimation = 32;

            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<ClockArrowForNightsEdgeBow>();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int arrows = 2;
            
            // from the sky
            for (int i = 0; i < arrows; i++)
            {
                Vector2 cursorPos = Main.MouseWorld;
                cursorPos.X = player.Center.X;
                cursorPos.Y = player.Center.Y - 800 - (100 * (i * 0.75f));
                float speedX = Main.rand.Next(-60, 91) * 0.02f;
                float speedY = Main.rand.Next(-60, 91) * 0.02f;
                speedY += 15;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-60, 61);
                cursorPos.Y += Main.rand.Next(-60, 61);

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI);
            }

            // from the fround
            for (int i = 0; i < arrows; i++)
            {
                Vector2 cursorPos = Main.MouseWorld;
                cursorPos.X = player.Center.X;
                cursorPos.Y = player.Center.Y + 800 + (100 * (i * 0.75f));
                float speedX = Main.rand.Next(-60, 91) * 0.02f;
                float speedY = Main.rand.Next(-60, 91) * 0.02f;
                speedY -= 15;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-60, 61);
                cursorPos.Y += Main.rand.Next(-60, 61);

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI);
            }

            // left
            for (int i = 0; i < arrows; i++)
            {
                Vector2 cursorPos = player.Center; // lol
                float speedX = Main.rand.Next(-60, 91) * 0.02f;
                float speedY = Main.rand.Next(-60, 91) * 0.02f;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-60, 61);
                cursorPos.Y += Main.rand.Next(-60, 61);

                cursorPos.X -= 1500;
                speedX += 15;

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI);
            }

            // right
            for (int i = 0; i < arrows; i++)
            {
                Vector2 cursorPos = player.Center; // lol
                float speedX = Main.rand.Next(-60, 91) * 0.02f;
                float speedY = Main.rand.Next(-60, 91) * 0.02f;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-60, 61);
                cursorPos.Y += Main.rand.Next(-60, 61);

                cursorPos.X += 1500;
                speedX -= 15;

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WorldFeeder>().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<Watercooler>().
                AddIngredient<Vinewrath>().
                AddIngredient<Fruminous>().
                AddIngredient<PurifiedGel>(10).
                AddTile(TileID.DemonAltar).
                Register();

            CreateRecipe().
                AddIngredient<OfVericourse>().
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<Watercooler>().
                AddIngredient<Vinewrath>().
                AddIngredient<Fruminous>().
                AddIngredient<PurifiedGel>(10).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}