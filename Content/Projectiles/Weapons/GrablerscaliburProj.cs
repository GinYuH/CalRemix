using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CalamityMod.Particles;
using Terraria.DataStructures;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class GrablerscaliburProj : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.netUpdate = true;
            Projectile.velocity = (Main.player[Projectile.owner].velocity * 0.3f) + (Vector2.Normalize(Main.MouseWorld - Main.player[Projectile.owner].Center) * 3);
            Projectile.velocity.Y -= 1.6f;
            Projectile.damage = 13;
            Projectile.knockBack = 7;
        }
        public override void AI()
        {
            if (Projectile.ai[0] % 50 == 0)
            {
                Projectile.velocity.Y += 2f;
            }
            Projectile.ai[0] += 1;
            if (Projectile.ai[0] % 8 == 0) Projectile.rotation += MathHelper.ToRadians(Main.rand.Next(1, 18));
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item111, Projectile.position);
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, Projectile.width * 2, Projectile.height * 2, DustID.Grass);
                dust.position = (dust.position + Projectile.Center) / 2f;
                if (i % 2 == 0)
                {
                    dust.velocity = (Projectile.velocity * -Main.rand.Next(20, 30) * 0.1f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(-10, 10)));
                }
                else
                {
                    dust.velocity = (Projectile.velocity * Main.rand.Next(3, 5) * 0.1f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 360)));
                }
                dust.noGravity = true;
            }
        }
    }
}