using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalRemix.UI.Games
{
    public struct CircleHitbox
    {
        public float radius;
        public Vector2 center;
        public Vector2 prevCenter;

        public CircleHitbox(Vector2 _center, float _radius)
        {
            radius = _radius;
            center = _center;
            prevCenter = center;
        }

        public CircleHitbox(Vector2 _center, Vector2 _prevCenter, float _radius)
        {
            radius = _radius;
            center = _center;
            prevCenter = _prevCenter;
        }

        public Line trajectoryLine => new Line(center, prevCenter);
    }

    public struct RectangleHitbox
    {
        public Vector2 position;
        public Vector2 center => position - dimensions / 2f;

        public Vector2 dimensions;

        public Line Top => new Line(position, position + Vector2.UnitX * dimensions.X);
        public Line Bottom => new Line(position + Vector2.UnitY * dimensions.Y, position + Vector2.UnitY * dimensions.Y + Vector2.UnitX * dimensions.X);
        public Line Left => new Line(position, position + Vector2.UnitY * dimensions.Y);
        public Line Right => new Line(position + Vector2.UnitX * dimensions.X, position + Vector2.UnitY * dimensions.Y + Vector2.UnitX * dimensions.X);


        public RectangleHitbox(Vector2 _position, Vector2 _dimensions)
        {
            position = _position;
            dimensions = _dimensions;
        }

        public bool Contains(Vector2 point)
        {
            if (position.X <= point.X && point.X < position.X + dimensions.X && position.Y <= point.Y)
            {
                return point.Y < position.Y + dimensions.Y;
            }

            return false;
        }
    }

    public struct Line
    {
        public Vector2 position;
        public Vector2 size;

        public Vector2 start => position;
        public Vector2 finish => position + size;

        public Line(Vector2 _position, Vector2 _size)
        {
            position = _position;
            size = _size;
        }

        //I copied this from the internet teehee
        public bool IsIntersecting(Line line2)
        {
            Vector2 a = start;
            Vector2 b = finish;
            Vector2 c = line2.start;
            Vector2 d = line2.finish;

            float denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
            float numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
            float numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

            // Detect coincident lines (has a problem, read below)
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            float r = numerator1 / denominator;
            float s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }
    }

    /// <summary>
    /// Any entity that can prevent movement
    /// The hitbox should be convex. If you need concave shapes , use more convex objects
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// This should represent the longest distance a point can be from the center of this object and still collide with it.
        /// E.G, a round rock of radius 1 would return 1. A square tile would return the value of one half its diagonal, since the distance between the center and the corner of a square is the longest distance
        /// </summary>
        public float SimulationDistance
        {
            get;
        }

        /// <summary>
        /// Returns the displacement the collision between this object and an entity may lead to.
        /// </summary>
        /// <param name="entityHitbox">The potentially colliding entity' hitbox.</param>
        /// <returns></returns>
        public Vector2 MovementCheck(CircleHitbox entityHitbox);


        /// <summary>
        /// If the collision of this object is currently on. If the entity just never collides, please simply don't implement this interface uwu
        /// </summary>
        public bool CanCollide
        {
            get => true;
        }

        /// <summary>
        /// What happens on a collision with an entity. If you want to hurt them on contact though, it would be wiser to implement an IDamageDealer interface to the object.
        /// </summary>
        /// <param name="collider">What entity collided with this object</param>
        public void OnCollide(GameEntity collider) { }

    }

    /// <summary>
    /// Represents an entity that can collide with collidable objects.
    /// </summary>
    public interface IColliding
    {
        /// <summary>
        /// Gets the hitbox of the colliding entity
        /// </summary>
        public CircleHitbox CollisionHitbox
        {
            get;
        }

        /// <summary>
        /// If the collision of this object is currently on. If the entity just never collides, please simply don't implement this interface uwu
        /// </summary>
        public bool CanCollide
        {
            get => true;
        }

        /// <summary>
        /// What happens on a collision. Return true if this entity dies on collision
        /// If the collider is null, it means the entity collided with the out of bounds
        /// </summary>
        public bool OnCollide(GameEntity collider) => false;
    }

    /// <summary>
    /// Represents an object the player can interact with
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// How big the radius of collision is for this interactable.
        /// </summary>
        public float CollisionCircleRadius
        {
            get;
        }

        /// <summary>
        /// Can the player interact with this?
        /// Useful if you want certain items to only be pickable up with a keybind.
        /// </summary>
        public bool CanBeInteractedWith => true;

        /// <summary>
        /// What happens when the item gets interacted with
        /// Return true to kill the interactable after the itneraction
        /// </summary>
        /// <param name="player">The player that interacted with the entity</param>
        public bool Interact(GameEntity player);
    }

    /// <summary>
    /// Represents an entity that can be drawn
    /// </summary>
    public interface IDrawable
    {

        /// <summary>
        /// The layer this should be drawn at. 
        /// Higher number = gets drawn above everything else on a lower layer.
        /// </summary>
        public int Layer => 0;

        public void Draw(SpriteBatch spriteBatch, Vector2 offset);
    }
}