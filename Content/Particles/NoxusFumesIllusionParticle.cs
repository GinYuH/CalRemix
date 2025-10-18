using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Content.NPCs.Bosses.Noxus;
using Terraria;
using Terraria.ModLoader;
using static System.MathF;
using static Terraria.Utils;

namespace CalRemix.Content.Particles
{
    public class NoxusFumesIllusionParticle : Particle
    {
        public int Direction;

        public float Opacity = 1f;

        public override int FrameVariants => 3;

        public override bool SetLifetime => true;

        public override bool UseCustomDraw => true;

        public override bool UseAdditiveBlend => false;

        public override string Texture => "CalRemix/Content/Particles/NoxusFumesYharimIllusion";

        public NoxusFumesIllusionParticle(Vector2 position, Vector2 velocity, Color color, int lifetime, float scale)
        {
            Position = position;
            Velocity = velocity;
            Color = color;
            Scale = scale;
            Lifetime = lifetime;
            Variant = Main.rand.Next(FrameVariants);

            // Very rarely cause manifestations of La Ruga.
            // In time you will know the tragic extent of their failings in time you will know the tragic extent of their failings in time you will know the tragic extent of their failings in time you will know the tragic extent of their failings
            // In time you will know the tragic extent of their failings in time you will know the tragic extent of their failings in time you will know the tragic extent of their failings in time you will know the tragic extent of their failings
            // In time you will know the tragic extent of their failings in time you will know the tragic extent of their failings in time you will know the tragic extent of their failings in time you will know the tragic extent of their failings
            // In time you will know the tragic extent of their failings in time you will know the tragic extent of their failings in time you will know the tragic extent of their failings in time you will know the tragic extent of their failings
            // In time you will know the tragic extent of their failings in time you will know the tragic extent of their failings in time you will know the tragic extent of their failings in time you will know the tragic extent of their failings
            // In time you will know the tragic extent of their failings in time you will know the tragic extent of their failings
            if (Main.rand.NextBool(1000))
                Variant = 3;

            Direction = Main.rand.NextFromList(-1, 1);
        }

        public override void Update()
        {
            if (Time <= 6f)
                Rotation = Velocity.X * 0.12f;

            Opacity = GetLerpValue(0f, 25f, Time, true) * GetLerpValue(Lifetime, 32f, Time, true) * 0.8f;
            Velocity *= 0.91f;
        }

        public override void CustomDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            if (Variant == 1)
                texture = ModContent.Request<Texture2D>("CalRemix/Content/Particles/NoxusFumesCalamitasIllusion").Value;
            if (Variant == 2)
                texture = ModContent.Request<Texture2D>("CalRemix/Content/Particles/NoxusFumesDraedonIllusion").Value;
            if (Variant == 3)
                texture = ModContent.Request<Texture2D>("CalRemix/Content/Particles/LaRugaIllusion").Value;

            Vector2 scale = new(Cbrt(Sin(Main.GlobalTimeWrappedHourly * 6.2f + Direction + Variant)) * 0.07f + Scale, Scale);

            for (int i = 0; i < (NPC.AnyNPCs(ModContent.NPCType<EntropicGod>()) ? 1 : 2); i++)
                spriteBatch.Draw(texture, Position - Main.screenPosition, null, Color * Opacity, Rotation, texture.Size() * 0.5f, scale, Direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
    }
}
