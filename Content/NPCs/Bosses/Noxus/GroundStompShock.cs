using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Core.Graphics;
using Terraria;
using Terraria.ModLoader;
using static System.MathF;
using static Terraria.Utils;
using static Microsoft.Xna.Framework.MathHelper;
using static CalRemix.CalRemixHelper;

namespace CalRemix.Content.NPCs.Bosses.Noxus
{
    public class GroundStompShock : ModProjectile, IDrawsOverTiles
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 2;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 27;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Projectile.Opacity = GetLerpValue(0f, 15f, Projectile.timeLeft, true);
            Projectile.scale = Projectile.Opacity * 1.8f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.SetBlendState(BlendState.Additive);

            Vector2 drawPosition = Projectile.Center - Main.screenPosition + Vector2.UnitY * 24f;
            Texture2D zap = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/Trails/ZapTrail").Value;
            Texture2D backglowTexture = ModContent.Request<Texture2D>("CalamityMod/Skies/XerocLight").Value;

            // Draw a purple backglow.
            spriteBatch.Draw(backglowTexture, drawPosition, null, Color.BlueViolet * Projectile.Opacity, 0f, backglowTexture.Size() * 0.5f, Projectile.scale * 0.56f, 0, 0f);
            spriteBatch.Draw(backglowTexture, drawPosition, null, Color.SlateBlue * Projectile.Opacity * 0.67f, 0f, backglowTexture.Size() * 0.5f, Projectile.scale * 0.72f, 0, 0f);
            spriteBatch.ResetBlendState();

            // Draw strong bluish pink lightning zaps above the ground.
            ulong lightningSeed = (ulong)Projectile.identity * 772496uL;
            for (int i = 0; i < 6; i++)
            {
                Vector2 lightningScale = new Vector2(1f, Projectile.scale) * Lerp(0.3f, 0.5f, RandomFloat(ref lightningSeed)) * 1.9f;
                float lightningRotation = Lerp(-1.04f, 1.04f, i / 4f + RandomFloat(ref lightningSeed) * 0.1f) + PiOver2;
                Color lightningColor = Color.Lerp(Color.SlateBlue, Color.Fuchsia, RandomFloat(ref lightningSeed) * -0.22f) * Projectile.Opacity;
                lightningColor.A = 0;

                spriteBatch.Draw(zap, drawPosition, null, lightningColor, lightningRotation, zap.Size() * Vector2.UnitY * 0.5f, lightningScale, 0, 0f);
                spriteBatch.Draw(zap, drawPosition, null, lightningColor * 0.3f, lightningRotation, zap.Size() * Vector2.UnitY * 0.5f, lightningScale * new Vector2(1f, 1.1f), 0, 0f);
            }
        }

        public override bool ShouldUpdatePosition() => false;

        public override bool? CanDamage() => false;
    }
}
