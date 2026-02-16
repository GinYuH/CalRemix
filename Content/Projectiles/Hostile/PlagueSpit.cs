using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class PlagueSpit : ModProjectile
	{
        public override void SetStaticDefaults() 
        {
			// DisplayName.SetDefault("Plague Spit");
            Main.projFrames[Projectile.type] = 4;
        }
		public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.timeLeft = 480;
        }
		public override void AI()
		{
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(20))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, (int)CalamityMod.Dusts.CalamityDusts.Plague, 0f, 0f);
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
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++) 
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, (int)CalamityMod.Dusts.CalamityDusts.Plague, 0f, 0f);
                d.velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
            }

        }
    }
}