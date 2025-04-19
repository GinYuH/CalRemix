using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class TetrachromancyProjectile : ModProjectile
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
            Projectile.timeLeft = 30;
            Projectile.scale = 1f;
        }
        private bool channelled = true;
        public override void AI()
        {
            Projectile.rotation += (float)((360 / 60) * Math.PI / 180);
            if (Main.player[Projectile.owner].channel && channelled)
            {
                MoveTowards(Main.MouseWorld, 40, 10);
                Projectile.timeLeft = 30;
            } else
            {
                channelled = false;
            }
        }

        private void MoveTowards(Vector2 goal, float speed, float inertia)
        {
            Vector2 moveTo = (goal - Projectile.Center).SafeNormalize(Vector2.UnitY) * speed / 1.5f;
            Projectile.velocity = (Projectile.velocity * (inertia - 1) + moveTo) / inertia;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(in SoundID.Zombie103, Projectile.Center);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 5), ModContent.ProjectileType<NystagmusProjectileBlue>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -5), ModContent.ProjectileType<NystagmusProjectileGray>(), Projectile.damage, Projectile.knockBack, Projectile.owner,1);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(5, 0), ModContent.ProjectileType<NystagmusProjectileRed>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(-5, 0), ModContent.ProjectileType<NystagmusProjectileGreen>(), Projectile.damage, Projectile.knockBack, Projectile.owner,1);
        }
    }
}