using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class LifeThorn : ModProjectile
	{
        public override string Texture => "CalamityMod/Projectiles/Melee/ThornBase";
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Reaver Thorn");
		}
		public override void SetDefaults() 
        {
            Projectile.CloneDefaults(ProjectileID.VilethornBase);
            Projectile.friendly = false;
			Projectile.hostile = true;
            Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Default;
            Projectile.aiStyle = 4;
        }
	}
}