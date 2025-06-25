﻿using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using CalamityMod;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class RedExoPulseLaser : ModProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Boss/ArtemisLaser";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Blue Exo Pulse Laser");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.Calamity().DealsDefenseDamage = true;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.hostile = true;
			Projectile.timeLeft = 480;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			CooldownSlot = ImmunityCooldownID.Bosses;
			Projectile.alpha = 255;
		}
		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 8)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame > 3)
				Projectile.frame = 0;

			Projectile.alpha -= 30;

			Lighting.AddLight(Projectile.Center, 0.6f, 0f, 0f);

			if (Projectile.velocity.X < 0f)
			{
				Projectile.spriteDirection = -1;
				Projectile.rotation = (float)Math.Atan2((double)-(double)Projectile.velocity.Y, (double)-(double)Projectile.velocity.X);
			}
			else
			{
				Projectile.spriteDirection = 1;
				Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			}
			if (Projectile.timeLeft <= 60)
			{
				Projectile.alpha += 10;
			}
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = new Color(255 * Projectile.Opacity, 255 * Projectile.Opacity, 255 * Projectile.Opacity);
			Vector2 drawOffset = Projectile.velocity.SafeNormalize(Vector2.Zero) * -30f;
			Projectile.Center += drawOffset;
			CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
			Projectile.Center -= drawOffset;
			return false;
		}
	}
}
