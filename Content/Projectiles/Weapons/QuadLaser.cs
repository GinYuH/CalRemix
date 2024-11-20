using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class QuadLaser : ModProjectile
	{
        public override string Texture => "Terraria/Images/Projectile_100";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.DeathLaser);
            AIType = ProjectileID.DeathLaser;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 1200);
            target.AddBuff(BuffID.CursedInferno, 1200);
            target.AddBuff(BuffID.Frostburn, 1200);
            target.AddBuff(BuffID.ShadowFlame, 1200);
        }
    }
}