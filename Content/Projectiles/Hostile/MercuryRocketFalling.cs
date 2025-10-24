using CalamityMod;
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
            if (Main.rand.NextBool(3))
            {
                GeneralParticleHandler.SpawnParticle(new SmallSmokeParticle(Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(3f, 5f), new Color(20, 20, 20), new Color(40, 40, 40), Main.rand.NextFloat(1f, 1.4f), 0.8f, 0.02f));
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
            MercuryRocket.MercuryExplosion(Projectile, 2f);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            // Only collide with tiles if past the player's vertical position
            return Projectile.position.Y > Main.player[(int)Projectile.ai[2]].Bottom.Y;
        }
    }
}