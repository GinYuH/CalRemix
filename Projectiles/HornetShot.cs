using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class HornetShot : ModProjectile
	{
        public override string Texture => "Terraria/Images/Projectile_242";
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("22 Hornet Round");
		}
		public override void SetDefaults() 
        {
            Projectile.CloneDefaults(ProjectileID.BulletHighVelocity);
            Projectile.penetrate = 22;
            AIType = ProjectileID.BulletHighVelocity;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
            NPC npc = Projectile.Center.MinionHoming(1750f, Main.player[Projectile.owner], false);
            if (npc != null && npc != target)
            {
                Projectile.velocity = npc.velocity + Projectile.DirectionTo(npc.Center) * 22f;
            }
        }
    }
}