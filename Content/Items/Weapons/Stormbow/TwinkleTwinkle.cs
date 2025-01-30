using System;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
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
    public class TwinkleTwinkle : ModItem, ILocalizedModType
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

            Item.width = 20;
            Item.height = 46;
            Item.damage = 54;
            Item.crit = 12;
            Item.useTime = 520;
            Item.useAnimation = 520;

            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.shoot = ModContent.ProjectileType<TwinkleStar>();
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
                speedY += 2;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-30, 31);
                cursorPos.Y += Main.rand.Next(-30, 31); 

                // if to right of player, right direct all projectiles. else, left
                if (Main.MouseWorld.X - player.Center.X > 0)
                {
                    cursorPos.X -= 200;
                    speedX += 2;
                }
                else
                {
                    cursorPos.X += 200;
                    speedX -= 2;
                }

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI, 0.0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Lumenyl>(30).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient<RuinousSoul>(30).
                AddIngredient<ExodiumCluster>(30).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}