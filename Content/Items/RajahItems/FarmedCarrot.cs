using CalRemix.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.RajahItems
{
    public class FarmedCarrot : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Carrot");
		}

		public override void SetDefaults()
		{
			Projectile.width = 10; 
			Projectile.height = 10; 
			Projectile.aiStyle = 1;   
			Projectile.friendly = true; 
			Projectile.hostile = false;  
			Projectile.penetrate = -1;  
			Projectile.timeLeft = 600;  
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			AIType = ProjectileID.WoodenArrowFriendly;           
		}

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];

            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            if (!thorium.TryFind("HealerDamage", out DamageClass healer))
                return;

            if (Main.rand.Next(100) <= player.GetCritChance(healer))
            {
                modifiers.SetCrit();
            }
        }

        public override void OnKill(int timeleft)
        {
            for (int num468 = 0; num468 < 20; num468++)
            {
                int num469 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, ModContent.DustType<CarrotDust>(), -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100);
                Main.dust[num469].velocity *= 2f;
            }
        }
    }
}
