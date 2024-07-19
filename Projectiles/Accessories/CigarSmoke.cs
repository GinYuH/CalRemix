using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;

namespace CalRemix.Projectiles.Accessories
{
    public class CigarSmoke : ModProjectile
    {
        public override string Texture => "CalamityMod/Particles/MediumSmoke";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cigar Smoke");
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = -1;
            Projectile.scale = 0.2f;
            Projectile.alpha = 255;
            Projectile.penetrate = 3;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.frame = Main.rand.Next(0, 4);
                Projectile.ai[0] = Main.rand.Next(1, 3);
            }
            Projectile.velocity *= 0.94f;
            Projectile.rotation += 0.01f * (Projectile.ai[0] == 2 ? 1 : -1);
            if (Projectile.scale < 1f)
            {
                Projectile.scale += 0.05f;
            }
            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha += 22;
            }
            else if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 22;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(BuffID.Poisoned, 300);
            target.AddBuff(ModContent.BuffType<WitherDebuff>(), 300);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Rectangle sourceRectangle = new(0, texture.Height / 3 * Projectile.frame, texture.Width, texture.Height / 3);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Color.Black * 0.6f;
            Main.spriteBatch.Draw(texture, position, sourceRectangle, drawColor * Projectile.Opacity, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            //Main.spriteBatch.ExitShaderRegion();
            return false;
        }
    }
}