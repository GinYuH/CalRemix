using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class TQDoor : TrapperTile, IInteractable, IDrawable, ICreative
    {
        public string Name => "Door";

        public int ID => 2;
        public string Texture => "CalRemix/UI/Games/TrapperQuest/Rock";

        public int roomGoto = -1;

        public Vector2 playerTP = Vector2.Zero;

        public float CollisionCircleRadius => 32;

        public float SimulationDistance => 32f;

        public bool Interact(GameEntity entity)
        {
            if (entity is TrapperPlayer player)
            {
                player.RoomImIn = new TQRoom(playerTP, roomGoto);
                player.RoomImIn.Entities.Add(player);
                player.Position = playerTP;
                TQRoomPopulator.PopulateRoomEnemies(player.RoomImIn, roomGoto);
            }
            return false;
        }

        public static TQDoor NewDoorTC(int x, int y, int roomID, Vector2 goToPos)
        {
            return NewDoorTC(new Vector2(x, y), roomID, goToPos);
        }

        public static TQDoor NewDoorTC(Vector2 position, int roomID, Vector2 goToPos)
        {
            TQDoor newdoor = new TQDoor();
            newdoor.tileX = (int)position.X;
            newdoor.tileY = (int)position.Y;
            newdoor.Position = TQHandler.ConvertToScreenCords(position);
            newdoor.roomGoto = roomID;
            newdoor.playerTP = goToPos;
            return newdoor;
        }

        public int Layer => 1;

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D Rok = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 drawPosition = Position + offset;

            Main.EntitySpriteDraw(Rok, drawPosition, null, Color.Black, 0f, Rok.Size() / 2f, 1f, 0, 0);

        }
    }
}