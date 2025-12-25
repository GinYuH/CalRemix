using CalamityMod;
using CalamityMod.Sounds;
using CalamityMod.World;
using CalRemix.Content.NPCs.Bosses.Hypnos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
	public class LargeTeslaSphere : ModProjectile
	{
		public static Asset<Texture2D> endo;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Large Tesla Sphere");
			Main.projFrames[Projectile.type] = 6;
			if (!Main.dedServ)
			{
				endo = ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/ExoEndo02");
			}
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
				Projectile.ai[1]++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame > 5)
			{
				Projectile.frame = 0;
			}
			if (Projectile.ai[1] > 3)
				Projectile.ai[1] = 0;
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

			if (CalamityWorld.death)
			{
				Projectile.ai[2]++;
				if (Projectile.ai[2] > 120 && Projectile.ai[2] % 60 == 0)
				{
					Vector2 endoPos = Projectile.Center - Vector2.UnitY * 80;
					SoundEngine.PlaySound(CommonCalamitySounds.ExoLaserShootSound, Projectile.Center);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), endoPos, endoPos.DirectionTo(target.Center) * 20, ModContent.ProjectileType<BlueExoPulseLaser>(), Projectile.damage, 0, Main.myPlayer);
					}
				}
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

        public override void PostDraw(Color lightColor)
        {
			if (Main.zenithWorld)
			{
				Texture2D tex = endo.Value;

				Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition - 100 * Vector2.UnitY, tex.Frame(1, 4, 0, (int)Projectile.ai[1]), lightColor, 0, new Vector2(tex.Width / 2, tex.Height / 8), Projectile.scale, Projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
			}
        }
    }
}