using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class RubiconGauntletProj : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Items/Weapons/RubiconGauntlet";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 360;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override void AI()
        {
            if (Projectile.velocity.Length() >= 4f)
            {
                for (int i = 0; i < 2; i++)
                {
                    float dustyX = 0f;
                    float dustyY = 0f;
                    if (i == 1)
                    {
                        dustyX = Projectile.velocity.X * 0.5f;
                        dustyY = Projectile.velocity.Y * 0.5f;
                    }
                    int smok = Dust.NewDust(new Vector2(Projectile.position.X + 3f + dustyX, Projectile.position.Y + 3f + dustyY) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, Main.rand.NextBool() ? DustID.GemRuby : DustID.Smoke, 0f, 0f, 100, default, 0.5f);
                    Main.dust[smok].scale *= 2f + (float)Main.rand.Next(10) * 0.1f;
                    Main.dust[smok].velocity *= 0.2f;
                    Main.dust[smok].noGravity = true;
                }
            }

            // Almost instantly accelerate to very high speed
            if (Projectile.velocity.Length() < 12f)
            {
                Projectile.velocity *= 1.25f;
            }
            else if (Main.rand.NextBool())
            {
                int fieryDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemRuby : DustID.Smoke, 0f, 0f, 100, default, 0.5f);
                Main.dust[fieryDust].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[fieryDust].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[fieryDust].noGravity = true;
                Main.dust[fieryDust].position = Projectile.Center + new Vector2(0f, (float)(-(float)Projectile.height / 2)).RotatedBy((double)Projectile.rotation, default) * 1.1f;
                Main.rand.Next(2);
                fieryDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemRuby : DustID.Smoke, 0f, 0f, 100, default, 0.5f);
                Main.dust[fieryDust].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[fieryDust].noGravity = true;
                Main.dust[fieryDust].position = Projectile.Center + new Vector2(0f, (float)(-(float)Projectile.height / 2 - 6)).RotatedBy((double)Projectile.rotation, default) * 1.1f;
            }

            Projectile.ai[0] += 1f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.ai[0] > 10f || Projectile.ai[0] > 5f)
            {
                Projectile.ai[0] = 10f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X *= 0.97f;
                    if (Math.Abs(Projectile.velocity.X) < 0.01f)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y += 0.1f;
            }

            if (Projectile.Calamity().stealthStrike)
            {
                CalamityUtils.HomeInOnNPC(Projectile, true, 2000, 14f, 20);
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 220;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            SoundEngine.PlaySound(BetterSoundID.ItemExplosion, Projectile.position);
            for (int j = 0; j < 40; j++)
            {
                int boomDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemRuby : DustID.Smoke, 0f, 0f, 100, default, 1f);
                Main.dust[boomDust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[boomDust].scale = 0.5f;
                    Main.dust[boomDust].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int k = 0; k < 70; k++)
            {
                int boomDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemRuby : DustID.Smoke, 0f, 0f, 100, default, 2f);
                Main.dust[boomDust2].noGravity = true;
                Main.dust[boomDust2].velocity *= 5f;
                boomDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemRuby : DustID.Smoke, 0f, 0f, 100, default, 1f);
                Main.dust[boomDust2].velocity *= 2f;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Vector2 goreSource = Projectile.Center;
                int goreAmt = 3;
                Vector2 source = new Vector2(goreSource.X - 24f, goreSource.Y - 24f);
                for (int goreIndex = 0; goreIndex < goreAmt; goreIndex++)
                {
                    float velocityMult = 0.33f;
                    if (goreIndex < (goreAmt / 3))
                    {
                        velocityMult = 0.66f;
                    }
                    if (goreIndex >= (2 * goreAmt / 3))
                    {
                        velocityMult = 1f;
                    }
                    int type = Main.rand.Next(61, 64);
                    int smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    Gore gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y -= 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y -= 1f;
                }
            }
        }
    }
}
