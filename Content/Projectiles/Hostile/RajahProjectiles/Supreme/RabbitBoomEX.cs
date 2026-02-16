using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile.RajahProjectiles.Supreme
{
    public class RabbitBoomEX : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bunnysplosion");     
            Main.projFrames[Projectile.type] = 5;     
        }

        public override void SetDefaults()
        {
            Projectile.width = 176;
            Projectile.height = 176;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 5)
                {
                    Projectile.Kill();
                }
            }
            Projectile.velocity.X *= 0.00f;
            Projectile.velocity.Y *= 0.00f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Color.White.R, Color.White.G, Color.White.B, 120);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 300);
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.timeLeft = 0;
        }
    }
}
