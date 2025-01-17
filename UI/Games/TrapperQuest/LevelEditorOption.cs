using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using static CalRemix.UI.Games.TrapperQuest.TQHandler;

namespace CalRemix.UI.Games.TrapperQuest
{
    /// <summary>
    /// For usage in the Level Editor's tool window
    /// Used for various tools such as saving/loading levels and editing room data
    /// </summary>
    public abstract class LevelEditorOption
    {
        /// <summary>
        /// The name of the tool
        /// </summary>
        public virtual string Name => "";

        /// <summary>
        /// The color of the tool
        /// </summary>
        public virtual Color Color => Color.Green;

        /// <summary>
        /// Make something happen when clicking this tool button
        /// </summary>
        public virtual void ClickAction() { }

        /// <summary>
        /// Make something happen when scrolling upwards with your mouse
        /// </summary>
        public virtual void ScrollUp() { }

        /// <summary>
        /// Make something happen when scrolling downwards with your mouse
        /// </summary>
        public virtual void ScrollDown() { }

        /// <summary>
        /// Overrides how the button is drawn. Defaults to drawing au unnamed rectangle
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
    /// Changes the horizontal size of the current room
    /// Cannot go lower than 13
    /// </summary>
    public class SizeXOption : LevelEditorOption
    {
        public override string Name => "SizeX";

        public override void ScrollDown()
        {
            float size = player.RoomImIn.RoomSize.X + 1;
            if (size >= RoomWidthDefault)
                player.RoomImIn.RoomSize.X = size;
        }

        public override void ScrollUp()
        {
            float size = player.RoomImIn.RoomSize.X - 1;
            if (size >= RoomWidthDefault)
                player.RoomImIn.RoomSize.X = size;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, player.RoomImIn.RoomSize.X.ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }

    /// <summary>
    /// Changes the vertical size of the current room
    /// Cannot go lower than 7
    /// </summary>
    public class SizeYOption : LevelEditorOption
    {
        public override string Name => "SizeY";

        public override void ScrollDown()
        {
            float size = player.RoomImIn.RoomSize.Y + 1;
            if (size >= RoomHeightDefault)
                player.RoomImIn.RoomSize.Y = size;
        }

        public override void ScrollUp()
        {
            float size = player.RoomImIn.RoomSize.Y - 1;
            if (size >= RoomHeightDefault)
                player.RoomImIn.RoomSize.Y = size;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, player.RoomImIn.RoomSize.Y.ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }

    /// <summary>
    /// Changes the ID of the current room
    /// </summary>
    public class RoomIDOption : LevelEditorOption
    {
        public override string Name => "RoomID";

        public override void ScrollDown()
        {
            int size = player.RoomImIn.id + 1;
            if (size >= 0)
                player.RoomImIn.id = size;
        }

        public override void ScrollUp()
        {
            int size = player.RoomImIn.id - 1;
            if (size >= 0)
                player.RoomImIn.id = size;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, player.RoomImIn.id.ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }

    /// <summary>
    /// Clears the entire room
    /// </summary>
    public class ClearOption : LevelEditorOption
    {
        public override string Name => "Clear";

        public override Color Color => Color.Blue;

        public override void ClickAction()
        {
            player.RoomImIn.Entities.RemoveAll((GameEntity g) => g is not TrapperPlayer);
        }
    }

    /// <summary>
    /// Changes the cursor mode to add items to the level
    /// </summary>
    public class AddItemOption : LevelEditorOption
    {
        public override string Name => "Add";

        public override Color Color => LevelEditor.CursorMode == 1 ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.CursorMode = 1;
        }
    }

    /// <summary>
    /// Changes the cursor mode to select a clicked item for editing
    /// </summary>
    public class SelectItemOption : LevelEditorOption
    {
        public override string Name => "Select";

        public override Color Color => LevelEditor.CursorMode == 0 ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.CursorMode = 0;
        }
    }

    /// <summary>
    /// Changes the cursor mode to delete any items touched 
    /// </summary>
    public class DeleteItemOption : LevelEditorOption
    {
        public override string Name => "Delete";

        public override Color Color => LevelEditor.CursorMode == 2 ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.CursorMode = 2;
        }
    }

    /// <summary>
    /// Saves the current room to CurLevel.txt
    /// </summary>
    public class SaveOption : LevelEditorOption
    {
        public override string Name => "Save";

        public override Color Color => Color.Blue;

        public override void ClickAction()
        {
            LevelEditor.ExportRoom();
        }
    }

    /// <summary>
    /// Loads a level from CurLevel.txt
    /// </summary>
    public class LoadOption : LevelEditorOption
    {
        public override string Name => "Load";

        public override Color Color => Color.Blue;

        public override void ClickAction()
        {
            LevelEditor.ImportRoom(false, clear: false);
        }
    }

    /// <summary>
    /// Displays the tile grid when enabled
    /// </summary>
    public class ShowGridOption : LevelEditorOption
    {
        public override string Name => "TileGrid";

        public override Color Color => LevelEditor.ShowGrid ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.ShowGrid = !LevelEditor.ShowGrid;
        }
    }

    /// <summary>
    /// Shows the cursor hitbox when enabled
    /// </summary>
    public class ShowCursorOption : LevelEditorOption
    {
        public override string Name => "Cursor";

        public override Color Color => LevelEditor.ShowCursor ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.ShowCursor = !LevelEditor.ShowCursor;
        }
    }

    /// <summary>
    /// Shows the player's hitbox when enabled
    /// </summary>
    public class ShowHitboxOption : LevelEditorOption
    {
        public override string Name => "Hitbox";

        public override Color Color => LevelEditor.ShowHitbox ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.ShowHitbox = !LevelEditor.ShowHitbox;
        }
    }

    /// <summary>
    /// Shows doors when enabled
    /// </summary>
    public class ShowDoorsOption : LevelEditorOption
    {
        public override string Name => "Show\nDoor";

        public override Color Color => LevelEditor.ShowDoors ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.ShowDoors = !LevelEditor.ShowDoors;
        }
    }

    /// <summary>
    /// Overwrites the room registery's version of this room to account for any edits made
    /// </summary>
    public class OverwriteOption : LevelEditorOption
    {
        public override string Name => "Overwrite";

        public override Color Color => Color.Blue;

        public override void ClickAction()
        {
            if (TQRoomPopulator.RoomData.Count > player.RoomImIn.id)
                TQRoomPopulator.RoomData[player.RoomImIn.id] = LevelEditor.ExportText(player.RoomImIn);
            else
                TQRoomPopulator.RoomData.Add(LevelEditor.ExportText(player.RoomImIn));
        }
    }
}
