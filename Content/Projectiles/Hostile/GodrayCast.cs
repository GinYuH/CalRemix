using System;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class GodrayCast : ModProjectile, ILocalizedModType
    {
        public static float LaserLength => 50f;
        public static float LaserLengthChangeRate => 4f;

        public static float WaveTheta => 0.01f;
        public static int WaveTwistFrames => 29;
        private ref float WaveFrameState => ref Projectile.ai[0];

        public override string Texture => "CalamityMod/Projectiles/LaserProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 3;
            Projectile.timeLeft = 280;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            // Very rapidly fade into existence.
            if (Projectile.alpha > 0)
                Projectile.alpha -= 25;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            float waveSign = WaveFrameState < 0f ? -1f : 1f;

            // Initialize waving. Setting ai[0] to a number between -1 and 1 tells it which way to wave.
            // Exactly 0 is a coinflip.
            if (Math.Abs(WaveFrameState) < 1f)
            {
                float dirToUse = WaveFrameState == 0f ? (Main.rand.NextBool() ? -1f : 1f) : waveSign;
                waveSign = -dirToUse;
                WaveFrameState = dirToUse * WaveTwistFrames * 0.5f;

                // Backfill old rotations to prevent visual glitches.
                float iterRotation = Projectile.velocity.ToRotation();
                for (int i = 0; i < Projectile.oldRot.Length; ++i)
                {
                    Projectile.oldRot[i] = iterRotation;
                    iterRotation += waveSign * WaveTheta;
                }
            }
            // Switch waving directions as necessary.
            else if (Math.Abs(WaveFrameState) > WaveTwistFrames)
                WaveFrameState = -waveSign;
            else
                WaveFrameState += waveSign;

            // Apply a constant, rapid wave to the laser's motion.
            Projectile.velocity = Projectile.velocity.RotatedBy(waveSign * WaveTheta);
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Emit light.
            Lighting.AddLight(Projectile.Center, 0.87f, 0.65f, 0.1725f);

            // Laser length shenanigans. If the laser is still growing, increase localAI 0 to indicate it is getting longer.
            if (Projectile.ai[1] == 0f)
            {
                Projectile.localAI[0] += 10f; // LaserLengthChangeRate;

                // Cap it at max length.
                if (Projectile.localAI[0] > LaserLength)
                    Projectile.localAI[0] = LaserLength;
            }

            // Otherwise it's shrinking. Once it reaches zero length it dies for good.
            else
            {
                Projectile.localAI[0] -= LaserLengthChangeRate;
                if (Projectile.localAI[0] <= 0f)
                    Projectile.Kill();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);

        public override Color? GetAlpha(Color lightColor) => new Color(222, 166, 44, 0);

        public override bool PreDraw(ref Color lightColor) => Projectile.DrawBeam(LaserLength, 2f, lightColor, curve: true);

        public override void OnKill(int timeLeft)
        {
            int dustID = 269;
            int dustAmt = 4;
            Vector2 dustPos = Projectile.Center - Projectile.velocity / 2f;
            Vector2 dustVel = Projectile.velocity / 4f;
            for (int i = 0; i < dustAmt; ++i)
            {
                Dust d = Dust.NewDustDirect(dustPos, 0, 0, dustID, 0f, 0f, Scale: 2.5f);
                d.velocity += dustVel;
                d.velocity *= Main.rand.NextFloat(0.4f, 1f);
                d.noGravity = true;
            }
        }
    }
}
