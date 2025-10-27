using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Build.Tasks;

namespace CalRemix.Content.Projectiles.Hostile
{
	public class ArkhalisProj : ModProjectile
	{
		private bool stopeh = false;
		public override string Texture => $"Terraria/Images/Item_{ItemID.Arkhalis}";
		
		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.hostile = true;
			Projectile.aiStyle = 2;
			Projectile.alpha = 150;
			Projectile.timeLeft = 600;
			AIType = ProjectileID.ThrowingKnife;
		}
		public override void AI()
		{
			if (Projectile.velocity.Y < 0) //Less than 0 means going up
            {
				Projectile.damage = 0;
				Projectile.alpha -= 5;
            }
			else
            {
				Projectile.alpha = 0;
				Projectile.damage = Main.expertMode ? 10 : 15;
				Projectile.rotation = MathHelper.ToRadians(135);
				Projectile.velocity.X = 0;
            }
			if (Projectile.velocity.Y > 6 && !stopeh)
            {
				stopeh = true;
				Projectile.velocity.Y = 6;
            }
			if (stopeh)
            {
				Projectile.velocity.Y += 0.02f;
            }
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(stopeh);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			stopeh = reader.ReadBoolean();
		}
	}
}
