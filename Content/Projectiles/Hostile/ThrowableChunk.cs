using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Projectiles.Hostile
{
    public class ThrowableChunk : ModProjectile
    {
        public ref float Count => ref Projectile.ai[0];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 124;
            Projectile.height = 124;
            Projectile.hostile = true;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation += (Projectile.whoAmI % 2 == 0).ToDirectionInt() * 0.02f;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20 * (Projectile.ai[1] == 2 ? 2 : 0); i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone);
            }
            SoundEngine.PlaySound(BetterSoundID.ItemMeteorImpact, Projectile.Center);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centered = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Main.EntitySpriteDraw(texture, centered, texture.Frame(1, 3, 0, (int)Projectile.ai[1]), Projectile.GetAlpha(lightColor), 0, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
