using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class TQRock : TrapperTile, ICollidable, IDrawable, ICreative
    {
        public string Name => "Rock";

        public int ID => 1;
        public string Texture => "CalRemix/UI/Games/TrapperQuest/Rock";
        
        RectangleHitbox CollisionHitbox => new RectangleHitbox(Position - Vector2.One * 32, Vector2.One * 64);

        public float SimulationDistance => 32f;

        public static TQRock NewRockTC(int x, int y)
        {
            return NewRockTC(new Vector2(x, y));
        }

        public static TQRock NewRockTC(Vector2 position)
        {
            TQRock newRok = new TQRock();
            newRok.tileX = (int)position.X;
            newRok.tileY = (int)position.Y;
            newRok.Position = TQHandler.ConvertToScreenCords(position);
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

            if (Collision.CheckAABBvAABBCollision(CollisionHitbox.position, CollisionHitbox.dimensions, hitbox.center - hitbox.radius * Vector2.One, new Vector2(2 * hitbox.radius)))
            {
                Vector2 newpos = Vector2.Zero;
                Vector2 collider = hitbox.center;
                Vector2 rock = CollisionHitbox.center;
                float colliderSize = hitbox.radius;
                Vector2 dir = rock.DirectionTo(collider);
                newpos.X = rock.X - dir.X * dist - colliderSize;
                newpos.Y = rock.Y - dir.Y * dist - colliderSize;
                return newpos;
            }
            return Vector2.Zero;
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