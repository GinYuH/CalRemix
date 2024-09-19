using CalamityMod;
using CalRemix.Content.NPCs.Bosses.Hypnos;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
	public class LargeTeslaSphere : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Large Tesla Sphere");
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 94;
			Projectile.height = 94;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 6000;
			Projectile.penetrate = -1;
			CooldownSlot = 1;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0f, 0.95f, 1.15f);
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 4)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame > 5)
			{
				Projectile.frame = 0;
			}
			Player target = Main.player[(int) Projectile.ai[0]];
			if (target != null && target.active)
			{
				Vector2 distance = target.Center - Projectile.Center;
				distance /= 50;
				Projectile.velocity = ((Projectile.velocity * 24f + distance) / 25f);
				if (Projectile.velocity.Length() < 6)
				{
					Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * 6;
				}
				//Projectile.velocity.Normalize();
				//Projectile.velocity *= 6;
			}

			
			NPC hyp = Main.npc[CalRemixNPC.hypnos];

			if (hyp != null && hyp.active && hyp.life > 0 && hyp.type == ModContent.NPCType<Hypnos>())
			{
				if (hyp.ModNPC<Hypnos>().p2 || !hyp.active)
				{
					Projectile.damage = 0;
					Projectile.velocity = Projectile.velocity * 0.9f;
					Projectile.alpha += 10;
					if (Projectile.alpha >= 255)
                    {
						Projectile.Kill();
                    }
				}
				else
				{
					Projectile.timeLeft = 600;
				}
			}
			else
            {
				Projectile.Kill();
            }
		}
	}
}