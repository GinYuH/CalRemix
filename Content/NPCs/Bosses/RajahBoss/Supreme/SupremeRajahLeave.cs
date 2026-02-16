using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RajahBoss.Supreme
{
    public class SupremeRajahLeave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rajah Rabbit; Champion of the innocent");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.width = 130;
            Projectile.height = 220;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 900;
        }
        public override void AI()
        {
            if (++Projectile.frameCounter > 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.velocity.X *= 0.00f;
            Projectile.velocity.Y -= .1f;
        }
    }
}