using System;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class FoveanatorEnergyBomb : ModProjectile
    {

        private const int TotalXFrames = 2;
        private const int TotalYFrames = 8;
        private const int ExplodeFrames = 6;
        private const int ExplodeDamageStartFrame = 3;
        private const int FrameTimer = 6;

        public int frameX = 0;
        public int frameY = 0;

        public int CurrentFrame
        {
            get => frameX * TotalYFrames + frameY;
            set
            {
                frameX = value / TotalYFrames;
                frameY = value % TotalYFrames;
            }
        }

        private const int TimeLeft = 300;
        private const int SlowDownTime = TimeLeft / 2;
        private const int ExplosionDuration = FrameTimer * ExplodeFrames;

        private const float ExplodeDistance = 50f;

        public override void SetDefaults()
        {
            Projectile.width = 133;
            Projectile.height = 142;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = TimeLeft;
        }

        public override void AI()
        {
            // If moving, decelerate.
            if (Projectile.velocity.Length() > 0f && Projectile.timeLeft <= SlowDownTime)
                Projectile.velocity *= 0.95f;

            // Get a target and calculate distance from it.
            int target = Player.FindClosest(Projectile.Center, 1, 1);
            float distanceFromTarget = Projectile.Distance(Main.player[target].Center);

            // Explode when within a certain distance of the target.
            if (distanceFromTarget <= ExplodeDistance && Projectile.timeLeft > ExplosionDuration)
                Projectile.timeLeft = ExplosionDuration;

            bool explode = Projectile.timeLeft <= ExplosionDuration;

            // Stop immediately if explosion is triggered.
            if (explode && Projectile.velocity.Length() > 0f)
                Projectile.velocity = Vector2.Zero;

            // Reset the frame counter and frameY when explosion is triggered.
            if (Projectile.timeLeft == ExplosionDuration)
            {
                Projectile.frameCounter = 0;
                frameY = 0;
            }

            // Explosion noise when actually exploding.
            if (frameY == ExplodeDamageStartFrame && explode && Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(DeusMine.ExplodeSound, Projectile.Center);
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter % FrameTimer == 0)
            {
                CurrentFrame++;

                // Non-explosion uses the left side of the sheet, explosion uses the right side of the sheet.
                frameX = explode ? 1 : 0;

                // Kill the projectile when the explosion animation is done.
                if (explode && frameY >= ExplodeFrames)
                {
                    if (Projectile.ai[0] > 0f)
                    {
                        if (Projectile.owner == Main.myPlayer)
                        {
                            float laserSpeed = 2f;
                            int type = ModContent.ProjectileType<FoveanatorLaser>();
                            int damage = (int)Math.Round(Projectile.damage * 0.85);
                            Vector2 laserVelocity = (Main.player[target].Center - Projectile.Center).SafeNormalize(Vector2.UnitY) * laserSpeed;
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + laserVelocity.SafeNormalize(Vector2.UnitY) * 16f, laserVelocity, type, damage, 0f, Main.myPlayer);
                        }
                    }

                    Projectile.Kill();

                    return;
                }
            }

            // Explosion has brighter light.
            Color lightColor = Color.Lerp(new Color(25, 25, 128), new Color(100, 25, 128), Main.DiscoR / 255f);
            float divisor = explode ? 128f : 255f;
            Lighting.AddLight(Projectile.Center, lightColor.R / divisor, lightColor.G / divisor, lightColor.B / divisor);
        }

        public override bool PreDraw(ref Color lightColor) => false;

        public override void PostDraw(Color lightColor)
        {
            if (Projectile.frameCounter < 5)
                return;

            lightColor.R = (byte)(255 * Projectile.Opacity);
            lightColor.G = (byte)(255 * Projectile.Opacity);
            lightColor.B = (byte)(255 * Projectile.Opacity);

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() / new Vector2(TotalXFrames, TotalYFrames) * 0.5f;
            Rectangle frame = texture.Frame(TotalXFrames, TotalYFrames, frameX, frameY);
            Main.EntitySpriteDraw(texture, position, frame, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
        }

        public override bool CanHitPlayer(Player target) => Projectile.timeLeft <= ExplosionDuration && frameY >= ExplodeDamageStartFrame;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, ExplodeDistance, targetHitbox);

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.timeLeft <= ExplosionDuration)
                target.AddBuff(BuffID.Frostburn, 180);
        }
    }
}