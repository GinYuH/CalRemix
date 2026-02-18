using CalRemix.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile.RajahProjectiles.Supreme
{
    public class CarrowSplit : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Carrow");
		}

		public override void SetDefaults()
		{
            Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 10; 
			Projectile.height = 10; 
			Projectile.aiStyle = ProjAIStyleID.Arrow;   
			Projectile.friendly = true; 
			Projectile.hostile = false;  
			Projectile.penetrate = -1;  
			Projectile.timeLeft = 600;  
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.noDropItem = true;
        }

        public override void OnKill(int timeleft)
        {
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, ModContent.DustType<CarrotDust>(), -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100);
                Main.dust[num469].velocity *= 2f;
            }
        }
    }
}
