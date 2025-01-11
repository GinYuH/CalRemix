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
            return; // TODO: FIX THIS, MANDATORY
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
                GameEntity.SpawnFromID(1,32,416),
                GameEntity.SpawnFromID(1,32,352),
                GameEntity.SpawnFromID(1,32,288),
                GameEntity.SpawnFromID(1,32,224),
                GameEntity.SpawnFromID(1,32,160),
                GameEntity.SpawnFromID(1,32,96),
                GameEntity.SpawnFromID(1,32,32),
                GameEntity.SpawnFromID(1,96,32),
                GameEntity.SpawnFromID(1,160,32),
                GameEntity.SpawnFromID(1,224,32),
                GameEntity.SpawnFromID(1,288,32),
                GameEntity.SpawnFromID(1,352,32),
                GameEntity.SpawnFromID(1,416,32),
                GameEntity.SpawnFromID(1,480,32),
                GameEntity.SpawnFromID(1,544,32),
                GameEntity.SpawnFromID(1,608,32),
                GameEntity.SpawnFromID(1,672,32),
                GameEntity.SpawnFromID(1,736,32),
                GameEntity.SpawnFromID(1,800,32),
                GameEntity.SpawnFromID(1,800,96),
                GameEntity.SpawnFromID(1,800,160),
                GameEntity.SpawnFromID(1,800,288),
                GameEntity.SpawnFromID(1,800,352),
                GameEntity.SpawnFromID(1,800,416),
                GameEntity.SpawnFromID(1,736,416),
                GameEntity.SpawnFromID(1,672,416),
                GameEntity.SpawnFromID(1,608,416),
                GameEntity.SpawnFromID(1,544,416),
                GameEntity.SpawnFromID(1,480,416),
                GameEntity.SpawnFromID(1,416,416),
                GameEntity.SpawnFromID(1,352,416),
                GameEntity.SpawnFromID(1,288,416),
                GameEntity.SpawnFromID(1,224,416),
                GameEntity.SpawnFromID(1,160,416),
                GameEntity.SpawnFromID(1,96,416),
                GameEntity.SpawnFromID(2,800,224, 1, new Vector2(96,224))
            }),

            new RoomLayout(new List<GameEntity>()
            {
                GameEntity.SpawnFromID(2,32,224,0, new Vector2(772, 224)),
                GameEntity.SpawnFromID(1,32,160),
                GameEntity.SpawnFromID(1,32,96),
                GameEntity.SpawnFromID(1,32,32),
                GameEntity.SpawnFromID(1,32,288),
                GameEntity.SpawnFromID(1,32,352),
                GameEntity.SpawnFromID(1,32,416),
                GameEntity.SpawnFromID(1,544,288),
                GameEntity.SpawnFromID(1,544,352),
                GameEntity.SpawnFromID(1,608,352),
                GameEntity.SpawnFromID(1,608,288),
                GameEntity.SpawnFromID(1,672,352),
                GameEntity.SpawnFromID(1,672,288),
                GameEntity.SpawnFromID(1,672,224),
                GameEntity.SpawnFromID(1,672,160),
                GameEntity.SpawnFromID(1,608,160),
                GameEntity.SpawnFromID(1,544,160),
                GameEntity.SpawnFromID(1,544,96),
                GameEntity.SpawnFromID(1,544,32),
                GameEntity.SpawnFromID(1,608,32),
                GameEntity.SpawnFromID(1,608,96),
                GameEntity.SpawnFromID(1,672,96),
                GameEntity.SpawnFromID(1,672,32),
                GameEntity.SpawnFromID(1,736,32),
                GameEntity.SpawnFromID(1,800,32),
                GameEntity.SpawnFromID(1,800,96),
                GameEntity.SpawnFromID(1,736,96),
                GameEntity.SpawnFromID(1,736,160),
                GameEntity.SpawnFromID(1,736,224),
                GameEntity.SpawnFromID(1,736,288),
                GameEntity.SpawnFromID(1,736,352),
                GameEntity.SpawnFromID(1,736,416),
                GameEntity.SpawnFromID(1,800,352),
                GameEntity.SpawnFromID(1,800,288),
                GameEntity.SpawnFromID(1,800,224),
                GameEntity.SpawnFromID(1,800,160),
                GameEntity.SpawnFromID(1,800,416),
                GameEntity.SpawnFromID(1,672,416),
                GameEntity.SpawnFromID(1,608,416),
                GameEntity.SpawnFromID(1,544,416),
                GameEntity.SpawnFromID(2,160,96,2, new Vector2(544, 224)),
            }),

            new RoomLayout(new List<GameEntity>()
            {
				GameEntity.SpawnFromID(1,32,160),
                GameEntity.SpawnFromID(1,32,96),
                GameEntity.SpawnFromID(1,32,32),
                GameEntity.SpawnFromID(1,32,288),
                GameEntity.SpawnFromID(1,32,352),
                GameEntity.SpawnFromID(1,32,416),
                GameEntity.SpawnFromID(1,32,224),
                GameEntity.SpawnFromID(1,96,160),
                GameEntity.SpawnFromID(1,96,96),
                GameEntity.SpawnFromID(1,96,32),
                GameEntity.SpawnFromID(1,96,288),
                GameEntity.SpawnFromID(1,96,352),
                GameEntity.SpawnFromID(1,96,416),
                GameEntity.SpawnFromID(1,160,416),
                GameEntity.SpawnFromID(1,224,416),
                GameEntity.SpawnFromID(1,288,416),
                GameEntity.SpawnFromID(1,352,416),
                GameEntity.SpawnFromID(1,416,416),
                GameEntity.SpawnFromID(1,416,352),
                GameEntity.SpawnFromID(1,480,352),
                GameEntity.SpawnFromID(1,480,288),
                GameEntity.SpawnFromID(1,544,288),
                GameEntity.SpawnFromID(1,544,224),
                GameEntity.SpawnFromID(1,544,160),
                GameEntity.SpawnFromID(1,544,96),
                GameEntity.SpawnFromID(1,544,32),
                GameEntity.SpawnFromID(1,480,32),
                GameEntity.SpawnFromID(1,416,32),
                GameEntity.SpawnFromID(1,352,32),
                GameEntity.SpawnFromID(1,288,32),
                GameEntity.SpawnFromID(1,224,32),
                GameEntity.SpawnFromID(1,160,32),
                GameEntity.SpawnFromID(1,480,96),
                GameEntity.SpawnFromID(1,608,96),
                GameEntity.SpawnFromID(1,608,32),
                GameEntity.SpawnFromID(1,608,160),
                GameEntity.SpawnFromID(1,608,224),
                GameEntity.SpawnFromID(1,672,224),
                GameEntity.SpawnFromID(1,672,160),
                GameEntity.SpawnFromID(1,672,96),
                GameEntity.SpawnFromID(1,672,32),
                GameEntity.SpawnFromID(1,736,32),
                GameEntity.SpawnFromID(1,736,96),
                GameEntity.SpawnFromID(1,736,160),
                GameEntity.SpawnFromID(1,736,224),
                GameEntity.SpawnFromID(1,800,224),
                GameEntity.SpawnFromID(1,800,160),
                GameEntity.SpawnFromID(1,800,96),
                GameEntity.SpawnFromID(1,800,32),
                GameEntity.SpawnFromID(1,800,288),
                GameEntity.SpawnFromID(1,736,288),
                GameEntity.SpawnFromID(1,608,288),
                GameEntity.SpawnFromID(1,608,352),
                GameEntity.SpawnFromID(1,608,416),
                GameEntity.SpawnFromID(1,672,416),
                GameEntity.SpawnFromID(1,672,352),
                GameEntity.SpawnFromID(1,672,288),
                GameEntity.SpawnFromID(1,736,416),
                GameEntity.SpawnFromID(1,800,352),
                GameEntity.SpawnFromID(1,736,352),
                GameEntity.SpawnFromID(1,800,416),
                GameEntity.SpawnFromID(1,544,352),
                GameEntity.SpawnFromID(1,480,416),
                GameEntity.SpawnFromID(1,544,416),
                GameEntity.SpawnFromID(2,96,224, 1, new Vector2(480, 224)),
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