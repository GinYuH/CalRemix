using System;
using System.Security.Cryptography.X509Certificates;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Particles;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class EyesickleProj : ModProjectile
    {
        public bool returning = false;
        public bool speedySpin = false;
        public bool isPaused = false;
        public Vector2 targetPos = Vector2.Zero;
        public Vector2 speedPrePause = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            Projectile.localNPCHitCooldown = 46;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            targetPos = Main.MouseWorld;
            if (Projectile.velocity.X >= 0)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-60));
            }
            else
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(60));
            }
            
        }
        public override void AI()
        {
            //control the variables
            if (Projectile.Calamity().stealthStrike && Projectile.timeLeft == 300)
            {
                Projectile.velocity *= 1.4f;
                Projectile.localNPCHitCooldown = 16;
            }
            if (Projectile.velocity.LengthSquared() < 1 && !returning)
            {
                returning = true;
                Projectile.tileCollide = false;
            }
            if (Projectile.velocity.LengthSquared() < 7 && !speedySpin)
            {
                speedySpin = true;
                Projectile.ai[0] = 100;
            }
            if (Projectile.velocity.LengthSquared() > 7 && speedySpin)
            {
                speedySpin = false;
                Projectile.ai[0] = 100;
                Projectile.frame = 0;
            }
            if (isPaused)
            {
                Projectile.ai[1]++;
            }
            if (Projectile.ai[1] == 10)
            {
                isPaused = false;
                Projectile.ai[1] = 0;
                Projectile.velocity = speedPrePause;
            }
            // switch to spinny animation
            if (speedySpin)
            {
                Projectile.ai[0]++;
                if (Projectile.ai[0] > 10)
                {
                    if (Projectile.frame == 2)
                    {
                        Projectile.frame--;
                    }
                    else
                    {
                        Projectile.frame++;
                    }
                    Projectile.ai[0] = 0;
                }
            }
            else
            {
                Projectile.rotation += MathHelper.ToRadians(7f) * (6 - Projectile.velocity.Length());
            }
            //moove
            if (Projectile.velocity.AngleTo(targetPos) > 0)
            {
                if (!returning)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.Center.AngleTo(targetPos).ToRotationVector2() * Projectile.velocity.Length(), 0.1f); 
                }
                else
                {
                    targetPos = Main.player[Projectile.owner].Center;
                    if (Projectile.timeLeft < 200)
                    {
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.Center.AngleTo(targetPos).ToRotationVector2() * Projectile.velocity.Length(), 0.15f);
                    }
                    else
                    {
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.Center.AngleTo(targetPos).ToRotationVector2() * Projectile.velocity.Length(), 0.06f);
                    }    
                }
            }
            if (!returning && Projectile.velocity.LengthSquared() > 1)
            {
                Projectile.velocity *= 0.97f;
            }
            else
            {
                if (Projectile.velocity.LengthSquared() < 36)
                {
                    if (speedySpin)
                    {
                        Projectile.velocity *= 1.1f;
                    }
                    else
                    {
                        Projectile.velocity *= 1.01f;
                    }
                    
                }
                if (Projectile.Center.Distance(Main.player[Projectile.owner].Center) < 10f)
                {
                    Projectile.active = false;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!returning)
            {
                returning = true;
                Projectile.tileCollide = false;
            }
            SoundEngine.PlaySound(SoundID.Item49 with { Pitch = Main.rand.Next(9, 12) / 10f }, Projectile.Center);

            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }

            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!returning && !speedySpin)
            {
                Projectile.velocity *= -1;
                returning = true;
                Projectile.tileCollide = false;
            }
            //hitstop thing
            if (speedySpin && Projectile.ai[1] == 0 && !isPaused)
            {
                speedPrePause = Projectile.velocity;
                Projectile.velocity = Vector2.Zero;
                isPaused = true;
                SoundEngine.PlaySound(SoundID.Item143 with { Pitch = Main.rand.Next(80, 106) / 100f }, Projectile.Center);
                Particle spark2 = new GlowSparkParticle(Projectile.Center, new Vector2(0.1f, 0.1f).RotatedByRandom(MathHelper.ToRadians(360)), false, 12, Main.rand.NextFloat(0.05f, 0.09f), Color.LightYellow * 0.7f, new Vector2(1f, 0.1f), true);
                GeneralParticleHandler.SpawnParticle(spark2);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item49 with { Pitch = Main.rand.Next(80, 106) / 100f }, Projectile.Center);
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (speedySpin)
            {
                modifiers.FinalDamage *= 0.6f;
                modifiers.Knockback *= 0.1f;
            }
            if (Projectile.Calamity().stealthStrike)
            {
                modifiers.FinalDamage *= 0.5f;
                modifiers.ScalingArmorPenetration += 0.3f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Projectile.velocity, Mod.Find<ModGore>("EyesickleGore").Type);
        }
    }
}