﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class MikadoSwing : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 148;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>();
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player Owner = Main.player[Projectile.owner];
            
            if (++Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;

            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item15, Projectile.Center);
                Projectile.soundDelay = 24;
            }

            if (Main.myPlayer == Projectile.owner)
            {
                if (!Owner.CantUseHoldout())
                {
                    float scaleFactor6 = 1f;

                    if (Owner.ActiveItem().shoot == Projectile.type)
                        scaleFactor6 = Owner.ActiveItem().shootSpeed * Projectile.scale;

                    Vector2 slashDirection = Main.MouseWorld - Owner.RotatedRelativePoint(Owner.MountedCenter, true);
                    slashDirection.Normalize();
                    if (slashDirection.HasNaNs())
                        slashDirection = Vector2.UnitX * (float)Owner.direction;

                    slashDirection *= scaleFactor6;
                    if (slashDirection.X != Projectile.velocity.X || slashDirection.Y != Projectile.velocity.Y)
                        Projectile.netUpdate = true;

                    Projectile.velocity = slashDirection;
                }
                else
                    Projectile.Kill();
            }

            Vector2 dustSpawn = Projectile.Center + Projectile.velocity * 3f;
            Lighting.AddLight(dustSpawn, 0.1f, 0.1f, 0);

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(dustSpawn - Projectile.Size * 0.5f, Projectile.width, Projectile.height, DustID.Enchanted_Gold, Projectile.velocity.X, Projectile.velocity.Y, 100, Color.White, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].position -= Projectile.velocity;
            }

            Projectile.position = Owner.RotatedRelativePoint(Owner.MountedCenter, true) - Projectile.Size * 0.5f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
                Projectile.rotation += MathHelper.Pi;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(1, 8, 0, Projectile.frame), Color.Yellow, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 16), Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            return false;
        }
    }
}
