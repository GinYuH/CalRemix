using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRemix.Projectiles.Weapons
{
    public class CarolBell : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 66;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.sentry = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Projectile.velocity = new Vector2(0f, (float)Math.Sin((double)(6.28318548f * Projectile.ai[1] / 300f)) * 0.5f);
            Projectile.ai[1] += 1f;
            if (Projectile.ai[0] > 0)
            {
                Projectile.ai[0]--;
            }
            if (Projectile.ai[0] <= 0)
            { 
                foreach (Projectile p in Main.projectile)
                {
                    if (!p.active || p == null)
                        continue;
                    if (p.TryGetGlobalProjectile(out CalRemixProjectile crp))
                    {
                        if (Terraria.ID.ProjectileID.Sets.IsAWhip[p.type])
                        {
                            if (!crp.whipGonged)
                            {
                                for (int j = 0; j < p.WhipPointsForCollision.Count; j++)
                                {
                                    Rectangle whipPoint = new Rectangle((int)p.WhipPointsForCollision[j].X, (int)p.WhipPointsForCollision[j].Y, 10, 10);
                                    if (whipPoint.Intersects(Projectile.getRect()))
                                    {
                                        if (Projectile.owner == Main.myPlayer)
                                        {
                                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CarolWave>(), Projectile.damage, 1f, Main.myPlayer);
                                        }
                                        SoundEngine.PlaySound(CalamityMod.Projectiles.Melee.PwnagehammerProj.UseSoundFunny, Projectile.Center);
                                        crp.whipGonged = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        public override bool? CanDamage() => false;
    }
}
