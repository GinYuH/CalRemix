using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class AncientFlashBulletProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 0.1f / 255f);
            if (Main.rand.NextBool(3))
            {
                int num137 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, DustID.PortalBoltTrail, 0f, 0f, 0, default, 0.5f);
                Main.dust[num137].alpha = Projectile.alpha;
                Main.dust[num137].velocity *= 0f;
                Main.dust[num137].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Width() * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item93 with { Volume = 2 }, Projectile.Center);
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AncientFlash>(), (int)((double)Projectile.damage * 0.25), 0f, Projectile.owner, 0f, 0f);
            }
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PortalBoltTrail, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}