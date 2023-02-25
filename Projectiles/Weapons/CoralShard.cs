using Terraria;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Ranged;

namespace CalRemix.Projectiles.Weapons
{
	public class CoralShard : ModProjectile
	{
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Dead Coral Shard");
		}
		public override void SetDefaults() 
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<SmallCoral>());
            Projectile.timeLeft = 30;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
            AIType = ModContent.ProjectileType<SmallCoral>();
        }
	}
}