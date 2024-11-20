using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using CalamityMod.Particles;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class SkyFire : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public Player Owner => Main.player[Projectile.owner];
        public ref float Time => ref Projectile.ai[1];
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 35;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 8;
        }
        public override void AI()
        {
            Time += 1f;
            if (Time >= 6f)
            {
                Projectile.scale = 1.5f * Utils.GetLerpValue(6f, 30f, Time, clamped: true);
                float rotationSpeed = MathHelper.ToRadians(3f);
                Color color = Color.Lerp(Color.DodgerBlue, Color.LightSkyBlue, 0.5f + 0.3f * MathF.Sin(Main.GlobalTimeWrappedHourly * 5f));
                GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.5f, color, 12, Projectile.scale * Main.rand.NextFloat(0.6f, 1.2f), 1f, rotationSpeed, glowing: true, 0f, required: true));
                if (Main.rand.NextBool(8))
                {
                    Color color2 = Color.Lerp(color, Color.White, 0.2f);
                    GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.5f, color2, 8, Projectile.scale * Main.rand.NextFloat(0.4f, 0.7f), 0.5f, rotationSpeed, glowing: true, 0.005f));
                }
                Lighting.AddLight(Projectile.Center, color.ToVector3() * Projectile.scale * 0.2f);
            }
            CalamityUtils.HomeInOnNPC(Projectile, ignoreTiles: false, 240f, 18f, 24f);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) =>  CalamityUtils.CircularHitboxCollision(Projectile.Center, 44f * Projectile.scale * 0.5f, targetHitbox);
    }
}