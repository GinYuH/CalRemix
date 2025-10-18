using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.Utils;

namespace CalRemix.Content.Particles
{
    public class MagicBurstParticle : Particle
    {
        public float Opacity = 1f;

        public override bool SetLifetime => true;

        public override bool UseCustomDraw => true;

        public override bool UseAdditiveBlend => true;

        public override string Texture => "CalRemix/Content/Particles/MagicBurst";

        public MagicBurstParticle(Vector2 position, Vector2 velocity, Color color, int lifetime, float scale)
        {
            Position = position;
            Velocity = velocity;
            Color = color;
            Scale = scale;
            Lifetime = lifetime;
        }

        public override void Update()
        {
            Opacity = GetLerpValue(0f, 4f, Lifetime - Time, true);
            Scale += 0.24f;
        }

        public override void CustomDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle frame = texture.Frame(1, 5, 0, (int)(LifetimeCompletion * 4.999f));
            spriteBatch.Draw(texture, Position - Main.screenPosition, frame, Color * Opacity, Rotation, frame.Size() * 0.5f, Scale * 0.8f, 0, 0f);
        }
    }
}
