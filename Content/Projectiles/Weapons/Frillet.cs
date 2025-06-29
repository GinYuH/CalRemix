using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Weapons
{
    public class Frillet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 240;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.direction = Projectile.velocity.X.DirectionalSign();

            Projectile.ai[0]++;
            if (Projectile.ai[0] > 30)
            {
                Projectile.velocity *= 0.98f;
            }
            if (Projectile.timeLeft > 60)
            {
                Projectile.alpha = (int)MathHelper.Max(Projectile.alpha - 50, 0);
            }
            else
            {
                Projectile.alpha = (int)MathHelper.Max(Projectile.alpha + 20, 0);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DarkCelestial);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 scale = Vector2.One;
            if (Projectile.ai[0] > 30)
            {
                scale = Vector2.Lerp(scale, new Vector2(2, 0.5f), CalamityUtils.SineInEasing(Utils.GetLerpValue(60, 180, Projectile.ai[0], true), 1));
            }
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation + MathHelper.PiOver2, tex.Size() / 2, scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 300);
        }
    }
}