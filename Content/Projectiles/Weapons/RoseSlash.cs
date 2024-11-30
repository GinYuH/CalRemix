using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class RoseSlash : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Melee/ExobeamSlash";
        public override void SetDefaults()
        {
            Projectile.width = 128;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
            Projectile.Opacity = 1f;
            Projectile.timeLeft = 35;
            Projectile.MaxUpdates = 2;
            Projectile.scale = 0.75f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 12;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.Opacity = (float)Projectile.timeLeft / 35f;
            if (Projectile.timeLeft == 34)
                GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(Projectile.Center, new Vector2(0.1f, 0.1f).RotatedByRandom(100.0), affectedByGravity: false, 12, Main.rand.NextFloat(0.03f, 0.05f), Main.rand.NextBool() ? Color.Pink : Color.LightPink, new Vector2(2f, 0.5f), quickShrink: true));
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Projectile.RotatingHitboxCollision(targetHitbox.TopLeft(), targetHitbox.Size());
        public override Color? GetAlpha(Color lightColor) => Color.Lerp((Projectile.ai[2] == 0f) ? Color.LightPink : Color.Pink, (Projectile.ai[2] == 0f) ? Color.HotPink : Color.DarkMagenta, (float)Projectile.identity / 7f % 1f) * Projectile.Opacity;
        public override bool PreDraw(ref Color lightColor) => false;
    }
}