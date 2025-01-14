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
        public static readonly List<Vector2> Walls = new List<Vector2>() { Vector2.UnitX, -Vector2.UnitX, Vector2.UnitY, -Vector2.UnitY };

        public List<GameEntity> Entities = new List<GameEntity>(); //A list of entities spawned on entry of the room.

        public Dictionary<(int, int), TQRock> Tiles = new Dictionary<(int, int), TQRock>();

        public int id;

        public Vector2 spawnPos = Vector2.Zero;

        public Vector2 RoomSize = Vector2.Zero;


        public TQRoom(int X, int Y, int ID, Vector2 roomSize = default)
        {
            new TQRoom(new Vector2(X, Y), ID, roomSize);
        }

        public TQRoom(Vector2 spawnPos, int ID, Vector2 roomSize = default)
        {
            Entities = new List<GameEntity>();
            Tiles = new Dictionary<(int, int), TQRock>();
            id = ID;
            this.spawnPos = spawnPos;
            if (roomSize == default)
                RoomSize = TQHandler.RoomSizeDefault;
            else
                RoomSize = roomSize;
        }
    }
}