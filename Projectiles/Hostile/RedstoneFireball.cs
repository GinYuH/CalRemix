using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Hostile
{
    public class RedstoneFireball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Redstone Fireball");
            Main.projFrames[Projectile.type] = 32;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.timeLeft = 300;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, TorchID.Red);
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(20))
                {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, (int)CalamityMod.Dusts.CalamityDusts.Brimstone, 0f, 0f);
                    d.noGravity = true;
                    d.velocity = d.position.DirectionFrom(Projectile.Center) * 4;
                }
            }
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
                Dust dust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityDusts.Brimstone, Scale: 2f + Main.rand.NextFloat())];
                dust.noGravity = true;
                dust.velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
            }
        }
    }
}