using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;

namespace CalRemix.Content.Projectiles.Weapons
{
	public class DeadeyeRevolverMeatball : ModProjectile
	{
        public bool hasHit = false;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults() 
        {
            Projectile.width = Projectile.height = 16;
            Projectile.extraUpdates = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 200;
			Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;

            if (Main.rand.NextBool(14))
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Blood, 0, 0, 0, default, 1f);

            if (Projectile.timeLeft == 199)
            {
                for (int i = 0; i < 3; i++)
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X / 8, Projectile.velocity.Y / 8, 0, default, 2f);
            }
            if (Projectile.timeLeft == 180)
            {
                //turn into sprite variant
                Projectile.frame += Main.rand.Next(1, 4);
            }
            
            if (hasHit)
            {
                Projectile.rotation += MathHelper.ToRadians(5);
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity.X *= 0.995f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasHit)
            {
                SoundEngine.PlaySound(SoundID.NPCHit9, Projectile.Center);
                Projectile.velocity.X /= 20f;
                Projectile.velocity.Y = -5;
                hasHit = true;
            }
            
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCHit9, Projectile.Center);
            Projectile.velocity.X /= 3;
            Projectile.velocity.Y = -2;
            hasHit = true;
            Projectile.penetrate -= 1;
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasHit)
            {
                modifiers.SetCrit();
                modifiers.Knockback *= 1.5f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Blood, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4), 0, default, 1.5f);
            }
            if (hasHit)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);
            } 
        }
    }
}