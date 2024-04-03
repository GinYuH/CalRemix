using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace CalRemix.Particles
{
    public class Snowflake : Particle
    {
        public override bool SetLifetime => true;
        public override bool UseHalfTransparency => true;
        public override string Texture => "CalRemix/Particles/Snowflake";

        public float VelocityDiminish { get; set; }
        public float VanishPercentage { get; set; }
        public float SpinAmount { get; set; }

        private delegate double DoubleOutput(double input);
        private DoubleOutput velocityMethod;

        public Snowflake(Vector2 position, Vector2 velocity, int lifetime, float scale, Color color)
        {
            Position = position;
            Velocity = velocity;
            Scale = scale;
            Lifetime = lifetime;
            Color = color;

            velocityMethod = Main.rand.Next(3) switch
            {
                1 => Math.Cos,
                2 => Math.Log,
                3 => Math.Tan,
                _ => Math.Sin
            };
        }

        public override void Update()
        {
            if (Time % 20 == 0)
            {
                velocityMethod = Main.rand.Next(3) switch
                {
                    1 => Math.Cos,
                    2 => Math.Log, 
                    3 => Math.Tan,
                    _ => Math.Sin
                };
            }
            float velocityAxis = (float)velocityMethod(Time * Math.PI);
            velocityAxis *= Main.rand.NextBool() ? 1 : -1;
            Vector2 finalVelocity = new(velocityAxis);
            Velocity = Vector2.Lerp(Velocity, finalVelocity, VelocityDiminish);

            Rotation += Velocity.ToRotation() * Velocity.X > 0 ? 0.01f : -0.01f;

            Color.A =(byte)(Utils.GetLerpValue(1f, VanishPercentage, LifetimeCompletion, true)*255f);
        }
    }
}
