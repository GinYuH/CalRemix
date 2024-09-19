using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class SaltWater : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Salt Water");
        }
		public override void SetDefaults() 
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 7;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.rotation = Main.rand.NextFloat(MathHelper.ToRadians(360));
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(6f) * (Projectile.direction > 0 ? 1 : -1);
            Timer++;
            if (Main.rand.NextBool(12))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.Smoke);
                dust.velocity = -(Projectile.velocity / 2).RotatedByRandom(MathHelper.ToRadians(22.5f));
            }
            if (Timer > 120)
                Projectile.aiStyle = ProjAIStyleID.GroundProjectile;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.Smoke);
                dust.velocity = Vector2.One.RotatedByRandom(MathHelper.ToRadians(360f)) * Main.rand.NextFloat(1f, 1.5f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 60);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }
    }
}