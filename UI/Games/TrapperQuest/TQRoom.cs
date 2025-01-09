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

        public int id;

        public Vector2 spawnPos = Vector2.Zero;


        public TQRoom(int X, int Y, int ID)
        {
            new TQRoom(new Vector2(X, Y), ID);
        }

        public TQRoom(Vector2 spawnPos, int ID)
        {
            Entities = new List<GameEntity>();
            id = ID;
            this.spawnPos = spawnPos;
        }
    }
}