using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class PrincessFlame : ModProjectile
    {
        public const int Lifetime = 60;
        public const int FadeoutTime = 25;
        public ref float Time => ref Projectile.ai[0];
        public override string Texture => "CalamityMod/Projectiles/Magic/PrinceFlameLarge"; 
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = (Projectile.height = 40);
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 11;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Wraith);
                    dust.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedByRandom(0.61000001430511475) * 2.5f;
                    dust.velocity.Y += Main.rand.NextFloat(-2.4f, 1.6f);
                    dust.velocity *= 0.4f;
                    dust.scale = Main.rand.NextFloat(1.2f, 1.7f);
                    dust.noGravity = Main.rand.NextBool();
                }
                Projectile.localAI[0] = 1f;
            }
            if (Projectile.timeLeft == 25)
                ExplodeIntoFireballs();

            bool flag = Projectile.timeLeft < 25;
            for (int j = 0; j < ((!flag) ? 1 : 2); j++)
            {
                Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin);
                dust2.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 6f);
                dust2.scale *= Main.rand.NextFloat(1.15f, 1.7f);
                dust2.noGravity = Main.rand.NextBool();
            }
            if (flag)
            {
                Projectile.frame = (int)Math.Round(MathHelper.Lerp(4f, 7f, Utils.GetLerpValue(25f, 0f, Projectile.timeLeft, clamped: true)));
                Projectile.velocity *= 0.95f;
                return;
            }
            if (Time % 8f == 7f)
            {
                for (int k = 0; k < 12; k++)
                {
                    Vector2 spinningpoint = Vector2.UnitX * -Projectile.width / 2f;
                    spinningpoint += -Vector2.UnitY.RotatedBy((float)k * (MathF.PI * 2f) / 12f) * new Vector2(8f, 16f);
                    spinningpoint = spinningpoint.RotatedBy(Projectile.rotation - MathF.PI / 2f);
                    Dust dust3 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.CopperCoin, 0f, 0f, 160);
                    dust3.scale = 1.1f;
                    dust3.noGravity = true;
                    dust3.position = Projectile.Center + spinningpoint;
                    dust3.velocity = Projectile.velocity * 0.1f;
                    dust3.velocity = (Projectile.Center - Projectile.velocity * 3f - dust3.position).SafeNormalize(Vector2.Zero) * 1.25f;
                }
            }

            Time += 1f;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathF.PI / 2f;
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter / 5 % 4;
        }
        public void ExplodeIntoFireballs()
        {
            SoundEngine.PlaySound(in SoundID.DD2_KoboldIgnite, Projectile.Center);
            if (Main.myPlayer == Projectile.owner)
            {
                int damage = (int)((float)Projectile.damage * 0.66f);
                float knockBack = Projectile.knockBack * 0.4f;
                float num = Main.rand.NextFloatDirection() * 0.31f;
                for (int i = 0; i < 6; i++)
                {
                    Vector2 vector = (MathF.PI * 2f * (float)i / 6f + num).ToRotationVector2() * 8f;
                    Vector2 position = Projectile.Center + (vector * 120f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, -vector, ModContent.ProjectileType<PrinceFlameSmall>(), damage, knockBack, Projectile.owner);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.Lerp(lightColor, Color.White, 0.8f);
            lightColor.A /= 4;
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (timeLeft > 25)
            {
                ExplodeIntoFireballs();
            }

            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin);
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3f, 8f);
                dust.position += dust.velocity.RotatedBy(1.5707963705062866) * 2f;
                dust.scale *= Main.rand.NextFloat(1.15f, 1.7f);
                dust.noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }
    }
}