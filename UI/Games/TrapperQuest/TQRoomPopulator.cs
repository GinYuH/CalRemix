using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;

namespace CalRemix.UI.Games.TrapperQuest
{
    public static class TQRoomPopulator
    {
        public static List<GameEntity> GetClone(this List<GameEntity> source)
        {
            return source.Select(item => item.Clone())
                        .ToList();
        }

        public static void PopulateRoomEnemies(TQRoom room, int ID)
        {
            room.Entities.AddRange(Layouts[ID].Population);
            foreach (var entity in Layouts[ID].Population)
            {
                if (entity is TQRock rok)
                {
                    room.Tiles.Add((rok.tileX, rok.tileY), rok);
                }
            }
        }

        public static void Populate(this TQRoom room, int ID)
        {
            PopulateRoomEnemies(room, ID);
        }

        public static readonly List<RoomLayout> Layouts = new List<RoomLayout>()
        {
            new RoomLayout(new List<GameEntity>()
            {
				//Entities

                TQRock.NewRockTC(0, 0),
                TQRock.NewRockTC(1, 1),
                TQRock.NewRockTC(2, 2),
                TQRock.NewRockTC(3, 3),
                TQRock.NewRockTC(12, 3),
                TQRock.NewRockTC(11, 6),
                TQRock.NewRockTC(1, 3)
            }),

            new RoomLayout(new List<GameEntity>()
            {
				//Entities




			}),

            new RoomLayout(new List<GameEntity>()
            {
				//Entities.. Etc. You got me




			})
        };
    }





    //Very barebones.
    public struct RoomLayout
    {
        public List<GameEntity> Population;

        public RoomLayout(List<GameEntity> population)
        {
            Population = population;
        }
    }
}