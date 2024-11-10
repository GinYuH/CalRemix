using CalamityMod.Particles;
using CalRemix.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class Chainsmog : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Smog");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.timeLeft = 80;
            Projectile.tileCollide = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 2 == 0)
            {
                SmallSmokeParticle SandCloud = new SmallSmokeParticle(Projectile.Center, (-Projectile.velocity * 0.2f).RotatedByRandom(0.2f), Color.Black, Color.Gray, Projectile.scale, MathHelper.Lerp(180, 44, Projectile.ai[0] / 120), Main.rand.NextFloat(0.03f, -0.03f));
                GeneralParticleHandler.SpawnParticle(SandCloud);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ShreadedLungs>(), 300);
        }
    }
}