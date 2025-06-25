using System;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Sounds;
using CalRemix.Content.DamageClasses;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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

            Item.width = 20;
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
    public class TerraHawk : ModProjectile
    {

        private const int TimeLeft = 180;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = TimeLeft;
            Projectile.aiStyle = -1;
            Projectile.width = 80;
            Projectile.height = 36;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= Main.projFrames[Type])
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Type])
            {
                Projectile.frame = 0;
            }

            Projectile.localAI[1] += 1;
            if (Projectile.localAI[1] >= 10)
            {
                int projToShoot = ProjectileID.TerraBeam;
                SoundStyle soundToPlay = SoundID.Item9;

                int projectile = Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_FromThis(), Projectile.Center, new Vector2(0, 15), projToShoot, Projectile.damage, 0, Projectile.owner);
                SoundEngine.PlaySound(soundToPlay, Projectile.Center);
                Projectile.localAI[1] = 0;
            }

            int dust = Dust.NewDust(Projectile.oldPosition + Projectile.oldVelocity, Projectile.width, Projectile.height, DustID.Terra, 0f, 0f, 100, default, 1.25f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;
            Main.dust[dust].noLightEmittence = true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CommonCalamitySounds.ExoDeathSound, Projectile.Center);
            for (int i = 4; i < 31; i++)
            {
                float projOldX = Projectile.oldVelocity.X * (30f / i);
                float projOldY = Projectile.oldVelocity.Y * (30f / i);
                int dust = Dust.NewDust(new Vector2(Projectile.oldPosition.X - projOldX, Projectile.oldPosition.Y - projOldY), 8, 8, DustID.Terra, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.8f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLightEmittence = true;

                dust = Dust.NewDust(new Vector2(Projectile.oldPosition.X - projOldX, Projectile.oldPosition.Y - projOldY), 8, 8, DustID.Terra, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.4f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.1f;
                Main.dust[dust].noLightEmittence = true;
            }
        }
    }
}