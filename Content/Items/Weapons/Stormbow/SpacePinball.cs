using System;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.Magic;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Weapons.Stormbow
{
    public class SpacePinball : ModItem, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item12;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;

            Item.width = 20;
            Item.height = 46;
            Item.damage = 34;
            Item.crit = 4;
            Item.useTime = 12;
            Item.useAnimation = 12;

            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ProjectileID.GreenLaser;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // big govt secret: this is actually just a really edited undines retribution. but dont tell anyone that
            // u can edit the i < whatever for extra arrows lool. lol. haha lol
            for (int i = 0; i < 1; i++)
            {
                Vector2 cursorPos = Main.MouseWorld;
                cursorPos.X = player.Center.X + (Main.MouseWorld.X - player.Center.X);
                cursorPos.Y = player.Center.Y - 800 - (100 * (i * 0.75f));
                float speedX = Main.rand.Next(-30, 61) * 0.02f;
                float speedY = Main.rand.Next(-30, 61) * 0.02f;
                speedY += 15;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-30, 31);
                cursorPos.Y += Main.rand.Next(-30, 31); 

                // if to right of player, right direct all projectiles. else, left
                if (Main.MouseWorld.X - player.Center.X > 0)
                {
                    cursorPos.X -= 200;
                    speedX += 5;
                }
                else
                {
                    cursorPos.X += 200;
                    speedX -= 5;
                }

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI, 0.0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MeteoriteBar, 30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.ManaCrystal, 2).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}