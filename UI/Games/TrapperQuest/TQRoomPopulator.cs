using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CalRemix.UI.Games.TrapperQuest
{
    public static class TQRoomPopulator
    {
        public static List<TQRoom> LoadedRooms = new List<TQRoom>();

        public static List<string> RoomData = new List<string>();

        public static TQRoom LoadRoom(string filename)
        {
            using (Stream stream = CalRemix.instance.GetFileStream(filename, true))
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string roomData = reader.ReadToEnd();
                RoomData.Add(roomData);
                return LevelEditor.ImportRoom(true, roomData, clear: false);
            }
        }
    }
}