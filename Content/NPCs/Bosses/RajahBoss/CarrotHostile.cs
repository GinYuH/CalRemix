using CalRemix.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RajahBoss
{
    public class CarrotHostile : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Carrot");
		}

		public override void SetDefaults()
		{
            Projectile.CloneDefaults(ProjectileID.WoodenArrowHostile);
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
