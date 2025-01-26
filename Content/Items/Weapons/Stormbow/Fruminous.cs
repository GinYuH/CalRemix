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
    public class Fruminous : ModItem, ILocalizedModType
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

            item.width = 20;
            Item.height = 46;
            Item.damage = 57;
            Item.crit = 16;
            Item.useTime = 28;
            Item.useAnimation = 28;

            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileID.Hellwing;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // big govt secret: this is actually just a really edited undines retribution. but dont tell anyone that
            // u can edit the i < whatever for extra arrows lool. lol. haha lol
            for (int i = 0; i < 5; i++)
            {
                Vector2 cursorPos = player.Center; // lol
                float speedX = Main.rand.Next(-60, 91) * 0.02f;
                float speedY = Main.rand.Next(-60, 91) * 0.02f;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-60, 61);
                cursorPos.Y += Main.rand.Next(-60, 61);

                int ai1 = -10;

                // if to right of player, right direct all projectiles. else, left
                if (Main.MouseWorld.X - player.Center.X > 0)
                {
                    cursorPos.X -= 1500;
                    speedX += 50;
                    ai1 = 10;
                }
                else
                {
                    cursorPos.X += 1500;
                    speedX -= 50;
                }

                int batOffset = Main.rand.Next(-3, 3);

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI, ai1, batOffset);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HellwingBow, 2).
                AddIngredient(ItemID.Cobweb, 15).
                AddIngredient(ItemID.HellstoneBar, 20).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}