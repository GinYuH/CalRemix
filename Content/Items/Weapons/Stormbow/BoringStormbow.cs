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
    public class BoringStormbow : ModItem, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.NPCHit1;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;

            item.width = 20;
            Item.height = 46;
            Item.damage = 57;
            Item.crit = 87;
            Item.useTime = 22;
            Item.useAnimation = 22;

            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ProjectileID.DD2OgreSpit;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // big govt secret: this is actually just a really edited undines retribution. but dont tell anyone that
            // u can edit the i < whatever for extra arrows lool. lol. haha lol
            for (int i = 0; i < 6; i++)
            {
                Vector2 cursorPos = Main.MouseWorld;
                cursorPos.X = player.Center.X;
                cursorPos.Y = player.Center.Y - 800 - (100 * (i * 0.75f));
                float speedX = Main.rand.Next(-90, 121) * 0.02f;
                float speedY = Main.rand.Next(-90, 121) * 0.02f;
                speedY += 15;

                // arrow position noise pass
                cursorPos.X += Main.rand.Next(-90, 91);
                cursorPos.Y += Main.rand.Next(-90, 91); 

                int projectile = Projectile.NewProjectile(source, cursorPos.X, cursorPos.Y, speedX, speedY, type, damage, knockback, player.whoAmI, 0.0f);
            }
            return false;
        }
    }
}