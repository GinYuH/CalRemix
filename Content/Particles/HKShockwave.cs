using System;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static CalamityMod.CalamityUtils;

namespace CalRemix.Content.Particles
{
    public class HKShockwave : Particle
    {
        public override string Texture => "CalRemix/Content/Particles/HKShockwave";
        public override bool UseAdditiveBlend => true;
        public override bool SetLifetime => true;

        private float OriginalScale;
        private float FinalScale;
        private float opacity;
        private Color BaseColor;

        public HKShockwave(Vector2 position, Vector2 velocity, Color color, float originalScale, float finalScale, int lifeTime)
        {
            Position = position;
            Velocity = velocity;
            BaseColor = color;
            OriginalScale = originalScale;
            FinalScale = finalScale;
            Scale = originalScale;
            Lifetime = lifeTime;
            Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }

        public override void Update()
        {
            float pulseProgress = PiecewiseAnimation(LifetimeCompletion, new CurveSegment[] { new CurveSegment(EasingType.PolyOut, 0f, 0f, 1f, 4) });
            Scale = MathHelper.Lerp(OriginalScale, FinalScale, pulseProgress);

            opacity = (float)Math.Sin(MathHelper.PiOver2 + LifetimeCompletion * MathHelper.PiOver2);

            Color = BaseColor * opacity;
            Lighting.AddLight(Position, Color.R / 255f, Color.G / 255f, Color.B / 255f);
            Velocity *= 0.95f;
        }

        public override void CustomDraw(SpriteBatch spriteBatch)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(tex, Position - Main.screenPosition, null, Color * opacity, Rotation, tex.Size() / 2f, Scale, SpriteEffects.None, 0);
        }

        public override void CustomDraw(SpriteBatch spriteBatch, Vector2 basePosition)
        {
            Texture2D tex = ModContent.Request<Texture2D>("CalRemix/Content/Particles/HKShockwave").Value; //somehow wont work for set particles and defaults to the first particle it has (blood)
            spriteBatch.Draw(tex, basePosition - Main.screenPosition, null, Color * opacity, Rotation, tex.Size() / 2f, Scale, SpriteEffects.None, 0);
        }
    }
}
