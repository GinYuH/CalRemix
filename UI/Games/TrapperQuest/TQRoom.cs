using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class TQRoom
    {
        public int MapX;
        public int MapY;

        public static readonly List<Vector2> Walls = new List<Vector2>() { Vector2.UnitX, -Vector2.UnitX, Vector2.UnitY, -Vector2.UnitY };

        public List<GameEntity> Entities; //A list of entities spawned on entry of the room.

        public TQRoom(int X, int Y)
        {
            Entities = new List<GameEntity>();

            //Place the room on the map
            MapX = X;
            MapY = Y;
        }
    }
}