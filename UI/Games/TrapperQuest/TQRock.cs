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

            if (hitbox.center.X < CollisionHitbox.Left.start.X)
                return Vector2.UnitX * (-CollisionHitbox.center.X + hitbox.center.X - hitbox.radius);
            else if (hitbox.center.X > CollisionHitbox.Right.start.X)
                return Vector2.UnitX * (CollisionHitbox.center.X + hitbox.center.X - hitbox.radius);
            else if (hitbox.center.Y < CollisionHitbox.Top.start.Y)
                return Vector2.UnitY * -(CollisionHitbox.center.Y - hitbox.center.Y + hitbox.radius);
            else if (hitbox.center.Y > CollisionHitbox.Bottom.start.Y)
                return Vector2.UnitY * (hitbox.center.Y - CollisionHitbox.center.Y + hitbox.radius);
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