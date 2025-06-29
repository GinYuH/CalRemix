using CalamityMod;
using CalRemix.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class BouncingMushroom : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.aiStyle = 14;
            Projectile.penetrate = 5;
            Projectile.DamageType = ModContent.GetInstance<StormbowDamageClass>();
            Projectile.timeLeft = 400;
        }
        public override void AI()
        {
            if (Main.rand.NextBool(25))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Sandnado, Projectile.velocity.X, Projectile.velocity.Y);
            }
        }
    }
}