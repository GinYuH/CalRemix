using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
	public class CausticTendril : ModProjectile
    {
        public ref float TurnAngle => ref Projectile.ai[0];
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Caustic Tendril");
		}
		public override void SetDefaults() 
        {
            Projectile.width = 10;
			Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 44;
            Projectile.penetrate = 1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            TurnAngle = Main.rand.NextBool() ? Main.rand.NextFloat(MathHelper.ToRadians(-2.5f), MathHelper.ToRadians(0)) : Main.rand.NextFloat(MathHelper.ToRadians(0.1f), MathHelper.ToRadians(2.6f));
        }
        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.RotatedBy(TurnAngle);
            TurnAngle += 0.00001f * (TurnAngle < MathHelper.ToRadians(0) ? -1 : 1);
            Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, (int)CalamityDusts.SulfurousSeaAcid, Scale: 2f * Projectile.scale);
            dust.noGravity = true;
        }
    }
}