using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class FriendlyCell1 : ModProjectile
    {
        public override string Texture => "CalRemix/Content/Projectiles/Hostile/PathogenCell1";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Friendly Blood Cell");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Projectile.ai[1] == 0 ? ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/PathogenCell1").Value : ModContent.Request<Texture2D>("CalRemix/Content/Projectiles/Hostile/PathogenCell2").Value;
            CalamityUtils.DrawAfterimagesCentered(Projectile, 0, lightColor, 5, tex);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
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