using System;
using System.Security.Cryptography.X509Certificates;
using CalamityMod;
using CalRemix.Content.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class LookerBookProj : ModProjectile
    {
        // ai 0 = timer
        // ai 1 = if you had enough mana  ( 1 equals not enough mana)
        // ai 2 = time between ready and shooting.

        public bool readyToShoot = false;
        public bool shooting = false;
        public bool ending = false;
        public int frameCounter = 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 28;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[1] == 1 && Projectile.ai[0] == 2)
            {
                Projectile.velocity *= 1.2f;
                Projectile.velocity.Y -= 2.6f;
            }
            if (Projectile.ai[0] == 20)
            {
                {
                    readyToShoot = true;
                }
                if (Projectile.ai[1] == 0)
                {
                    readyToShoot = true;
                }
                else
                {
                    Projectile.velocity *= 0.8f;
                }
            }
            if (readyToShoot && Projectile.ai[1] == 0)
            {
                Projectile.ai[2]++;
                Projectile.velocity *= 0.95f;
            }
            if (Projectile.ai[2] == 10)
            {
                
                readyToShoot = true;
            }
            //animation and projectile shooting
            if (readyToShoot && Projectile.ai[1] == 0)
            {
                frameCounter++;
                if (frameCounter > 8)
                {
                    if (Projectile.frame < 4 && !ending)
                    {
                        Projectile.frame++;
                    }
                    else
                    {
                        if (ending)
                        {
                            if (Projectile.frame > 3)
                            {
                                Projectile.frame = 2;
                            }
                            if (Projectile.frame > 0)
                            {
                                Projectile.frame--;
                            }   
                        }
                    }
                    frameCounter = 0;
                }
                if (Projectile.frame == 3 && !ending)
                {
                    //FIRE!!!!!
                    for (int i = 0; i < 10; i++)
                    {
                        Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Blood, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4), 0, default, 1f);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
                            Main.rand.Next(5,16) * Projectile.Center.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.ToRadians(14)),
                            ModContent.ProjectileType<LookerBookTooth>(), Projectile.damage / 4, 1f, Main.myPlayer);
                    }
                    Projectile.ai[2] = 0;
                    ending = true;
                }
                
            }

            if (ending && Projectile.ai[2] > 80)
            {
                Projectile.Kill();
            }

            if (Projectile.ai[1] == 1)
            {
                Projectile.rotation += 0.1f;
                Projectile.velocity.Y += 0.1f;
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }

            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            if (Projectile.ai[1] == 1)
            {
                if (readyToShoot)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.velocity.Y *= 1.1f;
                }
            }
            readyToShoot = true;
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity *= -1;
            if (Projectile.ai[1] == 1)
            {
                if (readyToShoot)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.velocity.Y *= 1.3f;
                }
            }
            readyToShoot = true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.ai[1] == 1)
            {
                modifiers.FinalDamage *= 1.3f;
                modifiers.Knockback *= 2f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Blood, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4), 0, default, 1.5f);
            }
        }
    }
}