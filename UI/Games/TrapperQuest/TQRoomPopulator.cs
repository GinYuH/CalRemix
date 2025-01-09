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

        public static void PopulateRoomEnemies(TQRoom room)
        {
            room.Entities.AddRange(Main.rand.NextFromCollection(Layouts).Population.GetClone());
        }

        public static void Populate(this TQRoom room)
        {
            PopulateRoomEnemies(room);
        }

        public static readonly List<RoomLayout> Layouts = new List<RoomLayout>()
        {
            new RoomLayout(new List<GameEntity>()
            {
				//Entities




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