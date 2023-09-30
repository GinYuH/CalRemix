using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Environment;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles
{
	public class AspidShot : ModProjectile
	{
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Infection Glob");
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
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(10))
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, 0f);
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 120);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++) 
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, 0f);
                d.velocity = new Microsoft.Xna.Framework.Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
            }

        }
    }
}