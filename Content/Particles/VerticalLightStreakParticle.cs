using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static System.MathF;
using static Terraria.Utils;
using static Microsoft.Xna.Framework.MathHelper;
using static CalRemix.CalRemixHelper;

namespace CalRemix.Content.Particles
{
    public class VerticalLightStreakParticle : Particle
    {
        public float Opacity = 1f;

        public Vector2 ScaleVector;

        public override bool SetLifetime => true;

        public override bool UseCustomDraw => true;

        public override bool UseAdditiveBlend => true;

        public override string Texture => "CalRemix/Content/Particles/VerticalLightStreak";

        public VerticalLightStreakParticle(Vector2 position, Vector2 velocity, Color color, int lifetime, Vector2 scale)
        {
            Position = position;
            Velocity = velocity;
            Color = color;
            ScaleVector = scale;
            Lifetime = lifetime;
        }

        public override void Update()
        {
            Opacity = GetLerpValue(0f, 4f, Time, true) * GetLerpValue(0f, 4f, Lifetime - Time, true);
        }

        public override void CustomDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, Color * Opacity, Rotation, texture.Size() * 0.5f, ScaleVector, 0, 0f);
        }
    }
}
