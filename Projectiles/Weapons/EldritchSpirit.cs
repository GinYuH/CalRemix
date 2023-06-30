using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Audio;
using Terraria.ID;
namespace CalRemix.Projectiles.Weapons
{
    public class EldritchSpirit : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 90;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 60 && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(90);
            if (Main.rand.NextBool(2))
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.SpectreStaff, 0f, 0f, 100, default, 1.2f);

            if (Projectile.timeLeft < 60)
                CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 800f, 16f, 20f);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath39, Projectile.Center);
        }
    }
}
