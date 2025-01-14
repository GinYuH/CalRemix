using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace CalRemix.UI.Games.TrapperQuest
{
    public static class TQRoomPopulator
    {
        public static List<TQRoom> LoadedRooms = new List<TQRoom>();

        public static TQRoom LoadRoom(string filename)
        {
            using (Stream stream = CalRemix.instance.GetFileStream(filename, true))
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string roomData = reader.ReadToEnd();
                return LevelEditor.ImportRoom(true, roomData);
            }
        }
    }
}