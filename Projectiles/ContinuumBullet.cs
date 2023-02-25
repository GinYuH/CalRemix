using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class ContinuumBullet : ModProjectile
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Continuum Bullet");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults() 
        {
            Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.penetrate = 10;
			Projectile.timeLeft = 1200; //was 6660 but projectiles that last over 100 seconds are just ASKING to cause severe lag
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            Projectile.extraUpdates = 10;
            AIType = ProjectileID.Bullet;
		}
		public override void AI()
		{
            int loopcount = 0;
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (loopcount != 6) // very jank but simple code
            {
                if (Projectile.position.X > 1000)
                {
                    Projectile.position.X = -1000;
                    loopcount = +1;
                }
                if (Projectile.position.X < -1000 && loopcount != 6)
                {
                    Projectile.position.X = 1000;
                    loopcount = +1;
                }
                if (Projectile.position.Y > 1000) 
                {
                    Projectile.position.Y = -1000;
                    loopcount = +1;
                }
                if (Projectile.position.Y < -1000 && loopcount != 6)
                {
                    Projectile.position.Y = 1000;
                    loopcount = +1;
                }
            }
        }
		public override bool PreDraw(ref Color lightColor) 
        {
            lightColor = new Color(255 * Projectile.Opacity, 255 * Projectile.Opacity, 255 * Projectile.Opacity);
            Vector2 drawOffset = Projectile.velocity.SafeNormalize(Vector2.Zero) * -30f;
            Projectile.Center += drawOffset;
            Projectile.Center -= drawOffset;
            return false;
        }
    }
}