using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
namespace CalRemix.Content.Projectiles.Weapons
{
    public class PlushieCotton : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.02f;
            if (Projectile.velocity.Y < 0f)
                Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.02f;
            else
                Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.02f;
            Projectile.velocity.X *= 0.97f;
            if (Projectile.velocity.Y < 1f)
                Projectile.velocity.Y += 0.05f;
            else if (Projectile.velocity.Y > 1f)
                Projectile.velocity.Y = 1f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity) => false;
    }
}