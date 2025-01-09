using Microsoft.Xna.Framework;
using Terraria;
namespace CalRemix.UI.Games
{
    public class GameManager
    {
        public static readonly Vector2 playingField = new Vector2(789, 472);
        public static Vector2 ScreenOffset => new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f) - playingField / 2f;
    }
}
