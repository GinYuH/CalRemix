using CalRemix.Gores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class ChaosMist : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Chaos Mist");
		}
		public override void SetDefaults() {
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true; 
			Projectile.hostile = false;
			Projectile.penetrate = 10;
            Projectile.timeLeft = 30;
			Projectile.light = 0.5f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            AIType = ProjectileID.Flames;
		}
        public override void AI()
		{
            if (!Main.dedServ)
            {
                for (int j = 0; j < 2; j++)
                {
                    int num357 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 1, 1, DustID.Shadowflame);
                    Dust dust = Main.dust[num357];
                    dust.scale = Main.rand.Next(60, 90) * 0.013f;
                    dust.velocity = new Vector2(Projectile.velocity.X + Main.rand.Next(-2, 3), Projectile.velocity.Y + Main.rand.Next(-2, 3));
                    dust.noGravity = true;
                }
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
				if (Projectile.Hitbox.Intersects(npc.Hitbox) && !npc.friendly && !npc.townNPC && npc.active)
				{
					npc.life = 0;
					npc.active = false;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        return;
                    }
                    Gore.NewGore(null, npc.position, Vector2.Zero, ModContent.GoreType<NoxCloud>());
                }
            }
        }
    }
}