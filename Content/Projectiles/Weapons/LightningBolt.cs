using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class LightningBolt : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void AI()
        {
            for (int i = 0; i < 6; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SpectreStaff, Projectile.velocity.X, Projectile.velocity.Y, Alpha: 255, Scale: Main.rand.NextFloat(1, 2));
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 80; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.SpectreStaff, new Vector2(Projectile.velocity.X, Projectile.velocity.Y), Alpha: 255, Scale: Main.rand.NextFloat(2, 4));
                dust.noGravity = true;
                dust.position += Main.rand.NextVector2Square(-5, 5);
                dust.velocity = Projectile.Center.DirectionTo(dust.position) * Main.rand.Next(3, 24);
            }
            SoundEngine.PlaySound(SoundID.Thunder, Projectile.Center);
            for (int i = 0; i < 3; i++)
            {
                int lightningDamage = (int)(Projectile.damage * 5);
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
    }
}