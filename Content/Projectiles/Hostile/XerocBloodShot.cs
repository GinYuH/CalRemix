using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class XerocBloodShot : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blood Shot");
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2 - 0.2f;
            Player p = Main.player[(int)Projectile.ai[0]];
            if (p == null || !p.active || p.dead)
                return;
            Projectile.ai[1]++;
            int timeBeforeHome = 30;
            if (Projectile.ai[1] > timeBeforeHome && Projectile.ai[2] == 0)
            {
                if (Projectile.ai[1] > 120)
                    Projectile.tileCollide = true;
                float scaleFactor = Projectile.velocity.Length();
                float inertia = 10f;
                Vector2 speed = Projectile.DirectionTo(p.Center).SafeNormalize(Vector2.UnitY) * scaleFactor;
                Projectile.velocity = (Projectile.velocity * inertia + speed) / (inertia + 1f);
                Projectile.velocity.Normalize();
                Projectile.velocity *= scaleFactor;
            }
            int ct = CalamityUtils.CountProjectiles(Type);
            int dustAmt = (int)MathHelper.Max(1, (int)MathHelper.Lerp(4, 1, ct / 22f));
            for (int i = 0; i < dustAmt; i++)
            Dust.NewDust(Projectile.position, 22, 22, DustID.Blood, Scale: Main.rand.NextFloat(1, 4));
            Lighting.AddLight(Projectile.Center, TorchID.Red);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[dust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[dust].scale = 0.5f;
                    Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
        }
    }
}