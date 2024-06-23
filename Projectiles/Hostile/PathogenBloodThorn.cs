using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
    public class PathogenBloodThorn : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Thorn");
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            int dustType = 5;
            float speed = 1f;
            int dustAmt = 30;
            int dustAmt1 = 30;
            int dustAmt2 = 2;
            int dustAmt3 = 2;
            int fadeInTime = 180;
            int fadeOutTime = fadeInTime + 60;
            int timeToDie = fadeOutTime + 5;
            int frameAmount = 6;
            bool appear = Projectile.ai[0] < (float)fadeInTime;
            bool fade = Projectile.ai[0] >= (float)fadeOutTime;
            bool die = Projectile.ai[0] >= (float)timeToDie;
            Projectile.ai[0] += 1f;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity *= 0;
                Projectile.frame = Main.rand.Next(frameAmount);
                for (int i = 0; i < dustAmt; i++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24f, 24f), dustType, Projectile.velocity * speed * MathHelper.Lerp(0.2f, 0.7f, Main.rand.NextFloat()));
                    dust.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
                    dust.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
                }
                for (int j = 0; j < dustAmt1; j++)
                {
                    Dust dust2 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24f, 24f), dustType, Main.rand.NextVector2Circular(2f, 2f) + Projectile.velocity * speed * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
                    dust2.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
                    dust2.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
                    dust2.fadeIn = 1f;
                }
                {
                    SoundEngine.PlaySound(in SoundID.Item60, Projectile.Center);
                }
            }
            if (appear)
            {
                Projectile.Opacity += 0.02f;
                Projectile.scale = Projectile.Opacity * Projectile.ai[1] * 3;
                for (int k = 0; k < dustAmt2; k++)
                {
                    Dust dust3 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), dustType, Projectile.velocity * speed * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
                    dust3.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
                    dust3.velocity *= 0.5f;
                    dust3.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
                }
            }
            if (fade)
            {
                Projectile.Opacity -= 0.2f;
                for (int l = 0; l < dustAmt3; l++)
                {
                    Dust dust4 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), dustType, Projectile.velocity * speed * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
                    dust4.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
                    dust4.velocity *= 0.5f;
                    dust4.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
                }
            }
            if (die)
            {
                Projectile.Kill();
            }
            Lighting.AddLight(Projectile.Center, new Vector3(0.5f, 0.1f, 0.1f) * Projectile.scale);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[dust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[dust].scale = 0.5f;
                    Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value198 = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle value126 = value198.Frame(1, 6, 0, Projectile.frame);
            Vector2 origin11 = new Vector2(16f, value126.Height / 2);
            Color alpha14 = Projectile.GetAlpha(lightColor);
            Vector2 scale16 = new Vector2(Projectile.scale);
            float lerpValue4 = Utils.GetLerpValue(190f, 180f, Projectile.ai[0], clamped: true);
            scale16.Y *= lerpValue4;
            Vector4 vector125 = lightColor.ToVector4();
            Vector4 vector126 = new Color(67, 17, 17).ToVector4();
            vector126 *= vector125;

            Vector2 pos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            if (Projectile.scale >= 3 * Projectile.ai[1])
            {
                for (int i = 0; i < 10; i++)
                {
                    Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 4;
                    Main.spriteBatch.Draw(value198, pos + vector2, value126, Color.Red * Projectile.Opacity, Projectile.rotation, origin11, scale16, SpriteEffects.None, 0f);
                }
            }
            Main.EntitySpriteDraw(value198, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), value126, Color.DarkRed, Projectile.rotation, origin11, scale16, SpriteEffects.None);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float nothing = 0;
            // 90 is the height of the spike
            // 22 is width of the spike
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.position, Projectile.position + Projectile.rotation.ToRotationVector2() * Projectile.scale * Projectile.ai[1] * 90, 22f, ref nothing);
        }

        public override bool? CanDamage()
        {
            return Projectile.scale >= 3 * Projectile.ai[1] && Projectile.ai[0] < 190;
        }
    }
}