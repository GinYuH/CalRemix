using System;
using CalamityMod;
using CalRemix.Content.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class ShadeBolt : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 5;
            Projectile.timeLeft = 360;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
        }

        public override void AI()
        {
            float ballSize = 30;
            switch (Projectile.ai[0])
            {
                case 0:
                    VoidMetaball.SpawnParticle(Projectile.Center + Main.rand.NextVector2Circular(8, 8), Vector2.Zero, ballSize);
                    break;
                case 1:
                    VoidMetaballBlue.SpawnParticle(Projectile.Center + Main.rand.NextVector2Circular(8, 8), Vector2.Zero, ballSize);
                    break;
                case 2:
                    VoidMetaballGreen.SpawnParticle(Projectile.Center + Main.rand.NextVector2Circular(8, 8), Vector2.Zero, ballSize);
                    break;
                case 3:
                    VoidMetaballYellow.SpawnParticle(Projectile.Center + Main.rand.NextVector2Circular(8, 8), Vector2.Zero, ballSize);
                    break;
            }
            if (Projectile.ai[1] == 0)
                CalamityUtils.HomeInOnNPC(Projectile, true, 2000, 12f, 20);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                float ballSize = Main.rand.NextFloat(10, 20);
                Vector2 speed = Main.rand.NextVector2Circular(10, 10);
                switch (Projectile.ai[0])
                {
                    case 0:
                        VoidMetaball.SpawnParticle(Projectile.Center + Main.rand.NextVector2Circular(8, 8), speed, ballSize);
                        break;
                    case 1:
                        VoidMetaballBlue.SpawnParticle(Projectile.Center + Main.rand.NextVector2Circular(8, 8), speed, ballSize);
                        break;
                    case 2:
                        VoidMetaballGreen.SpawnParticle(Projectile.Center + Main.rand.NextVector2Circular(8, 8), speed, ballSize);
                        break;
                    case 3:
                        VoidMetaballYellow.SpawnParticle(Projectile.Center + Main.rand.NextVector2Circular(8, 8), speed, ballSize);
                        break;
                }
            }
        }
    }
}
