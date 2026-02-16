using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.RajahBoss
{
    public class RajahStomp: ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rajah Stomp");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 30;
        }
    }
}
