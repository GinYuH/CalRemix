using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Audio;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.ID;
namespace CalRemix.Projectiles.Weapons
{
    public class DraconicFireball : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Boss/FlareBomb";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 90;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 60 && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
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

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(90);
            if (Main.rand.NextBool(20))
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityMod.Dusts.CalamityDusts.BlueCosmilite, 0f, 0f, 100, default, 0.8f);

            if (Projectile.timeLeft < 60)
                CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 1800f, 25f, 20f);
        }

        public override void Kill(int timeLeft)
        {
                float num461 = 3f;
                num461 *= 0.7f;
                Projectile.ai[0] += 4f;
                int num462 = 0;
                while ((float)num462 < num461)
                {
                    float num463 = (float)Main.rand.Next(-15, 16);
                    float num464 = (float)Main.rand.Next(-15, 16);
                    float num465 = (float)Main.rand.Next(9, 27);
                    float num466 = (float)Math.Sqrt((double)(num463 * num463 + num464 * num464));
                    num466 = num465 / num466;
                    num463 *= num466;
                    num464 *= num466;
                    int num467 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, 0f, 0f, 100, default, 2.5f);
                    Dust dust = Main.dust[num467];
                    dust.noGravity = true;
                    dust.position.X = Projectile.Center.X;
                    dust.position.Y = Projectile.Center.Y;
                    dust.position.X += (float)Main.rand.Next(-4, 5);
                    dust.position.Y += (float)Main.rand.Next(-4, 5);
                    dust.velocity.X = num463;
                    dust.velocity.Y = num464;
                    num462++;
                }
            SoundEngine.PlaySound(CalamityMod.Projectiles.Boss.HolyBlast.ImpactSound, Projectile.Center);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 180);
        }
    }
}
