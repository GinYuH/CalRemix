using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
    public class PathogenCaltrop : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("White Blood Cell");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            Projectile.ai[1]++;
            if (Projectile.ai[0] > 5f)
            {
                Projectile.ai[0] = 5f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X *= 0.97f;
                    if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y += 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            if ((double)Projectile.velocity.Y < 0.25 && (double)Projectile.velocity.Y > 0.15)
            {
                Projectile.velocity.X *= 0.8f;
            }
            Projectile.rotation = (0f - Projectile.velocity.X) * 0.05f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            int maxSplits = 2;
            if (Main.expertMode && Projectile.ai[2] < maxSplits)
            {
                int amt = 3;
                float speed = 10;
                for (int i = 0; i < amt; i++)
                {
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(speed, speed), Type, (int)MathHelper.Max((int)(Projectile.damage * 0.5f), 60), 0f, ai2: Projectile.ai[2] + 1);
                    Main.projectile[p].scale *= 0.66f;
                    Main.projectile[p].damage = (int)(Main.projectile[p].damage * 0.66f);
                    Main.projectile[p].timeLeft = ContentSamples.ProjectilesByType[Type].timeLeft / 2;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[dust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[dust].scale = 0.5f;
                    Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawProjectileWithBackglow(Projectile, Color.Red * 0.4f, lightColor, 8);
            CalamityUtils.DrawAfterimagesCentered(Projectile, 0, lightColor, 3);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item29, Projectile.Center);
            Projectile.velocity.Y = oldVelocity.Y * -(1 + Projectile.ai[2] * 0.5f);
            Projectile.velocity.X = oldVelocity.X * 1.2f;
            return false;
        }
    }
}