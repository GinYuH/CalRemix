using CalamityMod.Graphics.Primitives;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Sounds;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class TrueMonorianGemProj : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Projectiles/Weapons/MonorianGemProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 1;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 2;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 2 == 0 && Projectile.ai[0] > 1)
                GeneralParticleHandler.SpawnParticle(new HealingPlus(Projectile.position, 1f, Projectile.velocity.SafeNormalize(Vector2.UnitY) * 2, Color.LightPink, Color.DeepPink, 20));

            if (Projectile.ai[0] % 5 == 0)
            {
                SoundEngine.PlaySound(CommonCalamitySounds.LightningSound, Projectile.Center);
                int lightningDamage = (int)(Projectile.damage);
                Vector2 lightningSpawnPosition = Projectile.Center - Vector2.UnitY.RotatedByRandom(0.2f) * 1000f;
                Vector2 lightningShootVelocity = (Projectile.Center - lightningSpawnPosition + Projectile.velocity * 7.5f).SafeNormalize(Vector2.UnitY) * 15f;
                int lightning = Projectile.NewProjectile(Projectile.GetSource_FromThis(), lightningSpawnPosition, lightningShootVelocity, ModContent.ProjectileType<StormfrontLightning>(), lightningDamage, 0f, Projectile.owner);
                if (Main.projectile.IndexInRange(lightning))
                {
                    Main.projectile[lightning].CritChance = Projectile.CritChance;
                    Main.projectile[lightning].ai[0] = lightningShootVelocity.ToRotation();
                    Main.projectile[lightning].ai[1] = Main.rand.Next(100);
                    Main.projectile[lightning].DamageType = DamageClass.Magic;
                    Main.projectile[lightning].tileCollide = false;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                GeneralParticleHandler.SpawnParticle(new HealingPlus(Projectile.Center, Main.rand.NextFloat(0.5f, 0.8f), Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(4, 6), Color.LightPink * 0.6f, Color.DeepPink * 0.6f, 20));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new((float f, Vector2 v) => 8 * (1 - f), (float f, Vector2 v) => Color.Cyan * (1 - f)));
            return true;
        }
    }
}