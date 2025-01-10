using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using System.Collections.Generic;
using CalRemix.UI.Games.Boi.BaseClasses;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class TQRock : GameEntity, ICollidable, IDrawable
    {
        public string Texture => "CalRemix/UI/Games/TrapperQuest/Rock";

        //Those should be made into Icolliding
        //List<int> push = new List<int>() { ModContent.ProjectileType<Brimhita>(), ModContent.ProjectileType<Anahita>(), ModContent.ProjectileType<Spider>(), ModContent.ProjectileType<Terror>() };

        RectangleHitbox CollisionHitbox => new RectangleHitbox(Position - Vector2.One * 32, Vector2.One * 64);

        public float SimulationDistance => 32f;

        public int tileX = 0;
        public int tileY = 0;

        public static TQRock NewRockTC(int x, int y)
        {
            return NewRockTC(new Vector2(x, y));
        }

        public static TQRock NewRockTC(Vector2 position)
        {
            TQRock newRok = new TQRock();
            newRok.Position = TQHandler.ConvertToScreenCords(position);
            newRok.tileX = (int)position.X;
            newRok.tileY = (int)position.Y;
            return newRok;
        }

        public Vector2 MovementCheck(CircleHitbox hitbox)
        {
            int dist = 32;

            bool collisionOccured;

            float distanceBetweenCenters = (hitbox.center - Position).Length();

            //dist is the closest you can be to the rocks center without being inside of it
            if (distanceBetweenCenters < (dist + hitbox.radius))
                collisionOccured = true;

            //Do some complicated math if you're not sure of the collision
            else
            {
                var c1c2Vect = (hitbox.center - Position).SafeNormalize(Vector2.Zero);
                var outerPoint = hitbox.center + hitbox.radius * c1c2Vect;

                collisionOccured = CollisionHitbox.Contains(outerPoint);
            }

            //Don't move the entity if no collision occured lol
            if (!collisionOccured)
                return Vector2.Zero;

            //Check from which side of the box did the hitbox get into it.
            Vector2 pushbackNormal;
            float pushbackLength;

            if (CollisionHitbox.Bottom.IsIntersecting(hitbox.trajectoryLine))
            {
                pushbackNormal = Vector2.UnitY;
                pushbackLength = (Position.Y + dist) - (hitbox.center.Y - hitbox.radius);
            }
            else if (CollisionHitbox.Left.IsIntersecting(hitbox.trajectoryLine))
            {
                pushbackNormal = Vector2.UnitX;
                pushbackLength = (Position.X - dist) - (hitbox.center.X + hitbox.radius);
            }
            else if (CollisionHitbox.Right.IsIntersecting(hitbox.trajectoryLine))
            {
                pushbackNormal = Vector2.UnitX;
                pushbackLength = (Position.X + dist) - (hitbox.center.X - hitbox.radius);
            }
            else
            {
                pushbackNormal = Vector2.UnitY;
                pushbackLength = (Position.Y - dist) - (hitbox.center.Y + hitbox.radius);
            }

            return pushbackNormal * pushbackLength;
        }

        public int Layer => 2;

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D Rok = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 drawPosition = Position + offset;

            Main.EntitySpriteDraw(Rok, drawPosition, null, Color.White, 0f, Rok.Size() / 2f, 1f, 0, 0);

        }
    }
}