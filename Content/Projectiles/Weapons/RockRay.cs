using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class RockRay : EndoBeam
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        private static bool LaserTileCollision = false;
        private const int timeToExist = 70;
        private const float maximumLength = 4000f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rock Ray");
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
        }
        public override bool PreAI()
        {
            Projectile body = Main.projectile[(int)Projectile.ai[1]];
            if (body.type != ModContent.ProjectileType<RockBullet>() || !body.active)
                Projectile.Kill();

            if (Main.projectile[(int)Projectile.ai[1]].active)
            {
                Projectile.Center = Main.projectile[(int)Projectile.ai[1]].Center;
            }

            // How fat the laser is
            float laserSize = 10f;

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] >= timeToExist)
            {
                Projectile.Kill();
                return false;
            }

            // Causes the effect where the laser appears to expand/contract at the beginning and end of its life
            Projectile.scale = (float)Math.Sin((double)(Projectile.localAI[0] * MathHelper.Pi / timeToExist)) * 10f * laserSize;
            if (Projectile.scale > laserSize)
            {
                Projectile.scale = laserSize;
            }

            // The heart of the "sweeping rotation" part of the laser
            // Basically converts the velocity to a rotation, increments some value to that rotation,
            // and then converts the rotation to a velocity
            float velocityAsRotation = Projectile.velocity.ToRotation();
            velocityAsRotation += Projectile.ai[0];
            Projectile.rotation = velocityAsRotation - MathHelper.PiOver2;
            Projectile.velocity = velocityAsRotation.ToRotationVector2();

            Vector2 samplingPoint = Projectile.Center;

            float[] samples = new float[8];

            float determinedLength = 0f;
            if (LaserTileCollision)
            {
                Collision.LaserScan(samplingPoint, Projectile.velocity, Projectile.width * Projectile.scale, maximumLength, samples);
                for (int i = 0; i < samples.Length; i++)
                {
                    determinedLength += samples[i];
                }
                determinedLength /= 3f;
            }
            else
            {
                determinedLength = maximumLength;
            }

            float lerpDelta = 0.5f;
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], determinedLength, lerpDelta);

            DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], (float)Projectile.width * Projectile.scale, DelegateMethods.CastLight);
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.velocity == Vector2.Zero)
            {
                return false;
            }
            Texture2D laserTailTexture = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayStart").Value;
            Texture2D laserBodyTexture = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayMid").Value;
            Texture2D laserHeadTexture = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Lasers/UltimaRayEnd").Value;
            float laserLength = Projectile.localAI[1];
            Color drawColor = Color.Red;
            Projectile body = Main.projectile[(int)Projectile.ai[1]];

            // Laser tail logic

            Main.EntitySpriteDraw(laserTailTexture, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation, laserTailTexture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

            // Laser body logic

            laserLength -= (float)(laserTailTexture.Height / 2 + laserHeadTexture.Height) * Projectile.scale;
            Vector2 centerDelta = Projectile.Center;
            centerDelta += Projectile.velocity * Projectile.scale * (float)laserTailTexture.Height / 2f;
            if (laserLength > 0f)
            {
                float laserLengthDelta = 0f;
                Rectangle sourceRectangle = new Rectangle(0, 16 * (Projectile.timeLeft / 3 % 5), laserBodyTexture.Width, 16);
                while (laserLengthDelta + 1f < laserLength)
                {
                    if (laserLength - laserLengthDelta < (float)sourceRectangle.Height)
                    {
                        sourceRectangle.Height = (int)(laserLength - laserLengthDelta);
                    }
                    Main.EntitySpriteDraw(laserBodyTexture, centerDelta - Main.screenPosition, new Rectangle?(sourceRectangle), drawColor, Projectile.rotation, new Vector2((float)(sourceRectangle.Width / 2), 0f), Projectile.scale, SpriteEffects.None, 0);
                    laserLengthDelta += (float)sourceRectangle.Height * Projectile.scale;
                    centerDelta += Projectile.velocity * (float)sourceRectangle.Height * Projectile.scale;
                    sourceRectangle.Y += 16;
                    if (sourceRectangle.Y + sourceRectangle.Height > laserBodyTexture.Height)
                    {
                        sourceRectangle.Y = 0;
                    }
                }
            }

            // Laser head logic

            Main.EntitySpriteDraw(laserHeadTexture, centerDelta - Main.screenPosition, null, drawColor, Projectile.rotation, laserHeadTexture.Frame(1, 1, 0, 0).Top(), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 600);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 600);
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 600);
        }
    }
}