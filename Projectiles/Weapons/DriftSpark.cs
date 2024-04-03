using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Projectiles.Weapons
{
    public class DriftSpark : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_504";
        public int dust = DustID.Torch;
        public int buff = BuffID.OnFire;
        public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Spark");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            Projectile.penetrate = 2;
        }
        public override void AI()
        {
            Projectile.alpha = 255;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 3f)
            {
                int num18 = 100;
                if (Projectile.ai[0] > 20f)
                {
                    int num19 = 40;
                    float num20 = Projectile.ai[0] - 20f;
                    num18 = (int)(100f * (1f - num20 / (float)num19));
                    if (num20 >= (float)num19)
                    {
                        Projectile.Kill();
                    }
                }
                if (Projectile.ai[0] <= 10f)
                {
                    num18 = (int)Projectile.ai[0] * 10;
                }
                if (Main.rand.Next(100) < num18)
                {
                    int num21 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust, 0f, 0f, 150);
                    Main.dust[num21].position = (Main.dust[num21].position + Projectile.Center) / 2f;
                    Main.dust[num21].noGravity = true;
                    Dust dust2 = Main.dust[num21];
                    dust2.velocity *= 2f;
                    dust2 = Main.dust[num21];
                    dust2.scale *= 1.2f;
                    dust2 = Main.dust[num21];
                    dust2.velocity += Projectile.velocity;
                }
            }
            if (Projectile.ai[0] >= 20f)
            {
                Projectile.velocity.X *= 0.99f;
                Projectile.velocity.Y += 0.1f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (buff > -1)
                target.AddBuff(buff, 120);
            else if (buff == -2)
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
                target.AddBuff(BuffID.OnFire3, 120);
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
                target.AddBuff(ModContent.BuffType<KamiFlu>(), 120);
                target.AddBuff(BuffID.Frostburn2, 120);
                target.AddBuff(BuffID.Venom, 120);
            }
        }
    }
}