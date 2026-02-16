using System;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.NPCs.TownNPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class FoveanatorLaser : ModProjectile
    {
        private const int FadeOutTime = 30;

        private const float Acceleration = 1.05f;
        private const float MaxVelocity = 24f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
            ProjectileID.Sets.TrailCacheLength[Type] = 2;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.Opacity = 0f;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
                Projectile.frame = 0;

            if (Projectile.velocity.Length() < MaxVelocity)
            {
                Projectile.velocity *= Acceleration;
                if (Projectile.velocity.Length() > MaxVelocity)
                {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= MaxVelocity;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Color lightColor = Color.Lerp(new Color(25, 25, 128), new Color(100, 25, 128), Main.DiscoR / 255f);
            Lighting.AddLight(Projectile.Center, lightColor.R / 255f, lightColor.G / 255f, lightColor.B / 255f);

            if (Projectile.timeLeft < FadeOutTime)
            {
                Projectile.Opacity -= 1f / FadeOutTime;
            }
            else if (Projectile.Opacity < 1f)
            {
                Projectile.Opacity += 0.25f;
                if (Projectile.Opacity > 1f)
                    Projectile.Opacity = 1f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor.R = (byte)(255 * Projectile.Opacity);
            lightColor.G = (byte)(255 * Projectile.Opacity);
            lightColor.B = (byte)(255 * Projectile.Opacity);
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor, 1);
            return false;
        }

        public override bool CanHitPlayer(Player target) => Projectile.timeLeft >= FadeOutTime;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.timeLeft >= FadeOutTime)
                target.AddBuff(BuffID.Frostburn, 90);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return true;
        }
    }
}