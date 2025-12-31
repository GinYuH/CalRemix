using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
namespace CalRemix.Content.Projectiles.Weapons
{
    public class CarolWave : ModProjectile
    {
        public override string Texture => "CalamityMod/Particles/HollowCircleSoftEdge"; 
        public const int Lifetime = 300;
        public float LifetimeCompletion => 1f - Projectile.timeLeft / (float)Lifetime;
        public override void SetDefaults()
        {
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = Lifetime;
            Projectile.DamageType = DamageClass.Summon; 
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            Projectile.Opacity = 1f - (float)Math.Pow(LifetimeCompletion, 1.56);
            Projectile.scale = MathHelper.Lerp(0.5f, 12f, LifetimeCompletion);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return CalamityUtils.CircularHitboxCollision(Projectile.Center, Projectile.scale * 48f, targetHitbox);
        }
        public override bool? CanHitNPC(NPC target) => !target.CountsAsACritter && !target.friendly && target.chaseable;

        public override bool OnTileCollide(Vector2 oldVelocity) => false;
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.LightBlue * Projectile.Opacity * 0.67f;
        }
    }
}
