using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalamityMod.Graphics.Primitives;

namespace CalRemix.Content.Projectiles.Hostile
{
	public class ArkhalisProj : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Item_{ItemID.Arkhalis}";

        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailingMode[Type] = 1;
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
        }

        public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = false;
			Projectile.hostile = true;
			Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
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
				Projectile.tileCollide = true;
				Projectile.alpha = 0;
				Projectile.rotation = MathHelper.ToRadians(135);
				Projectile.velocity.X = 0;
            }
		}

        public override bool CanHitPlayer(Player target)
        {
			return Projectile.velocity.Y > 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D tex = TextureAssets.Projectile[Type].Value;

			Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2, Projectile.scale, 0);

			if (Projectile.velocity.Y > 0)
			{
				PrimitiveRenderer.RenderTrail(Projectile.oldPos, new((float f, Vector2 v) => 4 * (1 - f), (float f, Vector2 v) => Color.LightCyan * f * 0.3f, new((float f, Vector2 v) => Projectile.Size * 0.5f)));
			}

			return false;
        }
    }
}
