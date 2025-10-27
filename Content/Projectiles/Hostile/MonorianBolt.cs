using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
	public class MonorianBolt : ModProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
			Projectile.aiStyle = ProjAIStyleID.GemStaffBolt;
			AIType = ProjectileID.RubyBolt;
		}
		public override void AI()
		{
			/*Vector2 dustpos = Projectile.position;
			for (int dusttimer = 0; dusttimer < 3; dusttimer++)
			{
				int dusty = Dust.NewDust(Projectile.position, 1, 1, DustID.GemRuby, 0f, 0f, 100, default);
				Main.dust[dusty].noGravity = true;
				Main.dust[dusty].noLight = true;
			}
			if (Main.rand.Next(8) == 0)
            {
				int dusto = Dust.NewDust(Projectile.position, 1, 1, DustID.GemRuby, 0f, 0f, 100, default);
				Main.dust[dusto].noLight = true;
			}*/
		}
	}
}
