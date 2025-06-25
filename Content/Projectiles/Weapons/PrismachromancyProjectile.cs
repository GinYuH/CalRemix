using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class PrismachromancyProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            base.Projectile.height = 26;
            base.Projectile.friendly = true;
            base.Projectile.penetrate = 1;
            base.Projectile.tileCollide = true;
            base.Projectile.ownerHitCheck = true;
            base.Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 10;
        }
        private bool channelled = true;

        private int lifespan = 0;

        private int MaxShards = 64;
        public override void AI()
        {
            Projectile.rotation += (float)((360 / 60) * Math.PI / 180);
            if (Main.player[Projectile.owner].channel && channelled)
            {
                MoveTowards(Main.MouseWorld, 50, 5);
                Projectile.timeLeft = 10;
            }
            else
            {
                channelled = false;
            }
            lifespan++;
            if (2 + (int)Math.Ceiling((double)(Math.Pow(lifespan, 1.5) / 122)) >= MaxShards) {
                Projectile.Kill();
            }
        }

        private void MoveTowards(Vector2 goal, float speed, float inertia)
        {
            Vector2 moveTo = (goal - Projectile.Center).SafeNormalize(Vector2.UnitY) * speed / 1.5f;
            Projectile.velocity = (Projectile.velocity * (inertia - 1) + moveTo) / inertia;
        }

        public override void OnKill(int timeLeft)
        {
            
            
            int projCount = 2 + (int)Math.Ceiling((double)(Math.Pow(lifespan,1.5) / 122));
            if (projCount > 10)
            {
                SoundEngine.PlaySound(in SoundID.Item163, Projectile.Center);
            } else
            {
                SoundEngine.PlaySound(in SoundID.Item101, Projectile.Center);
            }
            for (int i = 0; i < projCount; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 10).RotatedBy(Math.PI / 180 * i * (360 / projCount)), ProjectileID.FairyQueenMagicItemShot, Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
          
        }
    }
}