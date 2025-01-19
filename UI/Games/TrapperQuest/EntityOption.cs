using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using static CalRemix.UI.Games.TrapperQuest.TQHandler;

namespace CalRemix.UI.Games.TrapperQuest
{
    /// <summary>
    /// Options for a seleceted entity in the instance editor
    /// </summary>
    public abstract class EntityOption
    {
        /// <summary>
        /// The name of the option
        /// </summary>
        public virtual string Name => "";

        /// <summary>
        /// The color of the option
        /// </summary>
        public virtual Color Color => Color.Green;

        /// <summary>
        /// The entity being edited, set in the level editor
        /// </summary>
        public GameEntity BoundEntity = null;

        /// <summary>
        /// Make something happen when clicking this button
        /// </summary>
        public virtual void ClickAction() { }

        /// <summary>
        /// Make something happen when scrolling up
        /// </summary>
        public virtual void ScrollUp() { }

        /// <summary>
        /// Make something happen when scrolling down
        /// </summary>
        public virtual void ScrollDown() { }

        /// <summary>
        /// Set to true if the bound entity is what you want
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValidEntity() => false;

        /// <summary>
        /// Draws a rectangle by default
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="position"></param>
        /// <param name="rect"></param>
        public virtual void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color);
        }
    }

    /// <summary>
    /// Changes what room a door will send you to
    /// </summary>
    public class DoorRoomOption : EntityOption
    {
        public override string Name => "RoomGoto";

        public override bool IsValidEntity() => BoundEntity is TQDoor;

        public override void ScrollDown()
        {
            if (BoundEntity is not TQDoor door)
                return;
            int roomID = door.roomGoto + 1;
            if (roomID >= -1)
                door.roomGoto = roomID;
        }

        public override void ScrollUp()
        {
            if (BoundEntity is not TQDoor door)
                return;
            int roomID = door.roomGoto - 1;
            if (roomID >= -1)
                door.roomGoto = roomID;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            if (BoundEntity is not TQDoor door)
                return;
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, door.roomGoto.ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }

    /// <summary>
    /// Changes what horizontal tile the door will send you to
    /// </summary>
    public class DoorXOption : EntityOption
    {
        public override string Name => "SpawnX";

        public override bool IsValidEntity() => BoundEntity is TQDoor;

        public override void ScrollDown()
        {
            if (BoundEntity is not TQDoor door)
                return;
            Vector2 tileCords = ConvertToTileCords(door.playerTP);
            float size = tileCords.X + 1;
            if (size >= 0)
                door.playerTP.X = ConvertToScreenCords((int)size, 0).X;
        }

        public override void ScrollUp()
        {
            if (BoundEntity is not TQDoor door)
                return;
            Vector2 tileCords = ConvertToTileCords(door.playerTP);
            float size = tileCords.X - 1;
            if (size >= 0)
                door.playerTP.X = ConvertToScreenCords((int)size, 0).X;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            if (BoundEntity is not TQDoor door)
                return;
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, (door.playerTP.X / 64 - 0.5f).ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }

    /// <summary>
    /// Changes what vertical tile the door will send you to
    /// </summary>
    public class DoorYOption : EntityOption
    {
        public override string Name => "SpawnY";

        public override bool IsValidEntity() => BoundEntity is TQDoor;

        public override void ScrollDown()
        {
            if (BoundEntity is not TQDoor door)
                return;
            Vector2 tileCords = ConvertToTileCords(door.playerTP);
            float size = tileCords.Y + 1;
            if (size >= 0)
                door.playerTP.Y = ConvertToScreenCords(0, (int)size).Y;
        }

        public override void ScrollUp()
        {
            if (BoundEntity is not TQDoor door)
                return;
            Vector2 tileCords = ConvertToTileCords(door.playerTP);
            float size = tileCords.Y - 1;
            if (size >= 0)
                door.playerTP.Y = ConvertToScreenCords(0, (int)size).Y;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            if (BoundEntity is not TQDoor door)
                return;
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, (door.playerTP.Y / 64 - 0.5f).ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }

    /// <summary>
    /// Enables/disables fading when using the door. Enabled by default.
    /// </summary>
    public class FadeOption : EntityOption
    {
        public override string Name => "Fade";

        public override bool IsValidEntity() => BoundEntity is TQDoor;

        public override Color Color => (BoundEntity is TQDoor door) ? door.fade ? Microsoft.Xna.Framework.Color.Lime : Microsoft.Xna.Framework.Color.Red : Microsoft.Xna.Framework.Color.White;

        public override void ClickAction()
        {
            if (BoundEntity is not TQDoor door)
                return;
            door.fade = !door.fade;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            if (BoundEntity is not TQDoor door)
                return;
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, door.fade.ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }

    /// <summary>
    /// Changes the frame of the tile
    /// </summary>
    public class FrameOption : EntityOption
    {
        public override string Name => "Frame";

        public override bool IsValidEntity() => BoundEntity is TQFloor floor && floor.SheetType == 1;

        public override void ScrollDown()
        {
            if (BoundEntity is not TQFloor door)
                return;
            int size = door.frame + 1;
            if (size >= 0 && size <= 5)
                door.frame = size;
        }

        public override void ScrollUp()
        {
            if (BoundEntity is not TQFloor door)
                return;
            int size = door.frame - 1;
            if (size >= 0 && size <= 5)
                door.frame = size;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            if (BoundEntity is not TQFloor door)
                return;
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, door.frame.ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }
}