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

                TQRock.NewRock(GameManager.playingField),
                TQRock.NewRock(GameManager.playingField - new Vector2(100, 100)),
                TQRock.NewRock(GameManager.playingField - new Vector2(100, 200)),
                TQRock.NewRock(GameManager.playingField - new Vector2(200, 100)),
                TQRock.NewRock(GameManager.playingField - new Vector2(200, 200)),
                TQRock.NewRock(GameManager.playingField - new Vector2(100, 0))


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