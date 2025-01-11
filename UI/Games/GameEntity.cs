using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System;
using CalRemix.UI.Games.TrapperQuest;

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

        public static GameEntity SpawnFromID(int id, int x, int y, object a = null, object b = null, object c = null)
        {
            GameEntity entiry = Activator.CreateInstance(LevelEditor.types[id].GetType()) as GameEntity;
            entiry.Position = new Vector2(x, y);
            if (entiry is TQDoor door)
            {
                int roomID = (int)a;
                Vector2 pos = (Vector2)b;
                door.roomGoto = roomID;
                door.playerTP = pos;
            }
            return entiry;
        }
    }
}