using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class RedstonePillar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Redstone Pillar");
            Main.projFrames[Projectile.type] = 32;
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 100;
            Projectile.timeLeft = 300;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 2, 0, 0);
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 22; k++)
            {
                Dust dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width / 2, Projectile.height / 2, (int)CalamityDusts.Brimstone, Scale: 0.5f + Main.rand.NextFloat())];
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
            }
        }
    }
}