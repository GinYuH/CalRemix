﻿using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class MercuryRocketFalling : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Projectiles/Hostile/MercuryRocket";
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
        }
        public override void AI()
        {
            if (Main.rand.NextBool())
            {
                GeneralParticleHandler.SpawnParticle(new TimedSmokeParticle(Projectile.Center, -Projectile.velocity.SafeNormalize(Vector2.UnitY).RotatedByRandom(MathHelper.PiOver2 * 0.5f) * Main.rand.NextFloat(3f, 5f), new Color(20, 20, 20), new Color(40, 40, 40), Main.rand.NextFloat(2f, 4f), 0.8f, 40, 0.02f));
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[1]++;
            if (Projectile.ai[1] > Projectile.ai[0])
            {
                if (Projectile.velocity.Y < 60)
                Projectile.velocity.Y += 3;
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile p = Projectile;
            Main.LocalPlayer.Calamity().GeneralScreenShakePower += 4;
            p.position = p.Center;
            p.width = p.height = 800;
            p.position.X = p.position.X - (float)(p.width / 2);
            p.position.Y = p.position.Y - (float)(p.height / 2);
            p.maxPenetrate = -1;
            p.penetrate = -1;
            p.Damage();
            SoundEngine.PlaySound(MercuryRocket.RockyExplosion with { PitchVariance = 0.3f, Pitch = -0.3f, Volume = 3f }, p.Center);
            for (int i = 0; i < 20; i++)
                GeneralParticleHandler.SpawnParticle(new CustomPulse(p.Center, Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(28f, 48f), Color.Orange, "CalamityMod/Particles/Light", Vector2.One, 0, Main.rand.NextFloat(4f, 10f), 0, 40));
            for (int i = 0; i < 100; i++)
                GeneralParticleHandler.SpawnParticle(new TimedSmokeParticle(p.Center, Main.rand.NextVector2Circular(80, 80), Color.Black, new Color(20, 20, 20), Main.rand.NextFloat(2f, 4f), 0.8f, 60, 0.02f));
            GeneralParticleHandler.SpawnParticle(new StrongBloom(p.Center, Vector2.Zero, Color.Orange, 6f, 10));
            GeneralParticleHandler.SpawnParticle(new PulseRing(p.Center, Vector2.Zero, Color.Gray * 0.4f, 0.4f, 10f, 10));
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            // Only collide with tiles if past the player's vertical position
            return Projectile.position.Y > Main.player[(int)Projectile.ai[2]].Bottom.Y;
        }
    }
}