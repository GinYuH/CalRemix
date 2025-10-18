using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using static Terraria.Utils;
using static Microsoft.Xna.Framework.MathHelper;

namespace CalRemix.Content.NPCs.Bosses.Noxus
{
    public class DarkWave : ModProjectile
    {
        public int Lifetime = 60;

        public float Opacity = 1f;

        public float MinScale = 1.2f;

        public float MaxScale = 5f;

        public float MaxRadius = 2000f;

        public float RadiusExpandRateInterpolant = 0.08f;

        public ref float Radius => ref Projectile.ai[0];

        public static float DetermineScreenShakePower(float lifetimeCompletionRatio, float distanceFromPlayer) => GetLerpValue(2400f, 1000f, distanceFromPlayer, true) * (1f - lifetimeCompletionRatio) * 5.5f;

        public static Color DetermineExplosionColor() => Color.Lerp(Color.MediumSlateBlue, Color.Black, 0.1f);

        public static Texture2D ExplosionNoiseTexture => ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Neurons").Value;

        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 72;
            Projectile.height = 72;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = Lifetime;
            Projectile.scale = 0.001f;
        }

        public override void AI()
        {
            // Do screen shake effects.
            float distanceFromPlayer = Projectile.Distance(Main.LocalPlayer.Center);
            Main.LocalPlayer.Calamity().GeneralScreenShakePower = MathF.Max(Main.LocalPlayer.Calamity().GeneralScreenShakePower, DetermineScreenShakePower(1f - Projectile.timeLeft / (float)Lifetime, distanceFromPlayer));

            // Cause the wave to expand outward, along with its hitbox.
            Radius = Lerp(Radius, MaxRadius, RadiusExpandRateInterpolant);
            Projectile.scale = Lerp(MinScale, MaxScale, GetLerpValue(Lifetime, 0f, Projectile.timeLeft, true));

            if (Projectile.ai[1] != 0f)
                Projectile.Opacity = Projectile.ai[1];
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return CalamityUtils.CircularHitboxCollision(Projectile.Center, Radius * 0.4f, targetHitbox);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            DrawData explosionDrawData = new(ExplosionNoiseTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * Projectile.Opacity);

            var shader = GameShaders.Misc[$"{Mod.Name}:ShockwaveShader"];
            shader.UseColor(DetermineExplosionColor());
            shader.Shader.Parameters["screenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            shader.Shader.Parameters["explosionDistance"].SetValue(Radius * Projectile.scale * 0.5f);
            shader.Shader.Parameters["projectilePosition"].SetValue(Projectile.Center - Main.screenPosition);
            shader.Shader.Parameters["shockwaveOpacityFactor"].SetValue(Projectile.Opacity);
            shader.Apply();
            explosionDrawData.Draw(Main.spriteBatch);

            Main.spriteBatch.ExitShaderRegion();
            return false;
        }
    }
}
