using CalRemix.UI.Games.TrapperQuest;
using Microsoft.Xna.Framework;
using Terraria;
namespace CalRemix.UI.Games
{
    public class GameManager
    {
        public static Vector2 playingField => new Vector2(TQHandler.tileSize * TQHandler.RoomWidthDefault, TQHandler.tileSize * TQHandler.RoomHeightDefault);
        public static Vector2 ScreenOffset => new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f) - TQHandler.RoomSizeDefault * TQHandler.tileSize / 2f - CameraPosition;

        public static Vector2 CameraPosition = Vector2.Zero;
    }
}
