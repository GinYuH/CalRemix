using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class AspidShotFriendly : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Projectiles/Hostile/AspidShot";

        public static SoundStyle PopExplosion = new SoundStyle("CalRemix/Assets/Sounds/PopExplosion");

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Infection Glob");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 480;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(10))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, 0f);
                }
            }
            if (Projectile.ai[0] == 0)
            {
                if (Projectile.velocity.Y > 12)
                {
                    Projectile.velocity.Y = 12;
                }
                else
                {
                    Projectile.velocity.Y += 0.12f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 1)
            {
                Projectile.position = Projectile.Center;
                Projectile.width = Projectile.height = 120;
                Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
                Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
                Projectile.maxPenetrate = -1;
                Projectile.penetrate = -1;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
                Projectile.Damage();
                SoundEngine.PlaySound(PopExplosion, Projectile.position);
                for (int j = 0; j < 40; j++)
                {
                    int boomDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.OrangeTorch : DustID.SteampunkSteam, 0f, 0f, 100, default, 1f);
                    Main.dust[boomDust].velocity *= 3f;
                    if (Main.rand.NextBool())
                    {
                        Main.dust[boomDust].scale = 0.5f;
                        Main.dust[boomDust].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int k = 0; k < 70; k++)
                {
                    int boomDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.OrangeTorch : DustID.SteampunkSteam, 0f, 0f, 100, default, 2f);
                    Main.dust[boomDust2].noGravity = true;
                    Main.dust[boomDust2].velocity *= 5f;
                    boomDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.OrangeTorch : DustID.SteampunkSteam, 0f, 0f, 100, default, 1f);
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
}