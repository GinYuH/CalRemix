using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class FlameFrostArrow : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Flaming Ice Arrow");
		}
		public override void SetDefaults() 
        {
            Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
            AIType = ProjectileID.FrostburnArrow;
		}
		public override void AI()
		{
            if (Projectile.timeLeft % 12 == 0)
                Dust.NewDust(Projectile.Center + Projectile.velocity, 1, 1, DustID.IceTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            if (Projectile.timeLeft % 12 == 6)
                Dust.NewDust(Projectile.Center + Projectile.velocity, 1, 1, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire3, 180);
            target.AddBuff(BuffID.Frostburn2, 180);
        }
    }
}