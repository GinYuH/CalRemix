using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;

namespace CalRemix.UI.Games
{
    public class GameEntity
    {
        public Vector2 OldPosition;
        public Vector2 Position;
        public Vector2 Velocity;
        public virtual void OnSpawn() { }
        public virtual void Update() { }

        public Vector2 ScreenPosition()
        {
            return Position + GameManager.ScreenOffset;
        }

        public GameEntity Clone()
        {
            GameEntity clone = new GameEntity();
            clone.Position = Position;
            clone.Velocity = Velocity;
            clone.OldPosition = OldPosition;

            return clone;
        }
    }
}