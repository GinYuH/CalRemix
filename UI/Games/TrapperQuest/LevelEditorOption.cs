using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using static CalRemix.UI.Games.TrapperQuest.TQHandler;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria.ModLoader.Core;
using CalRemix.UI.Games.Boi.BaseClasses;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent;
using System.Transactions;
using System.Text;
using System.IO;
using Terraria.GameContent.Creative;
using Microsoft.CodeAnalysis;
using System.Dynamic;
using Terraria.DataStructures;

namespace CalRemix.UI.Games.TrapperQuest
{

    public abstract class LevelEditorOption
    {
        public virtual string Name => "";

        public virtual Color Color => Color.Green;

        public virtual void ClickAction() { }

        public virtual void ScrollUp() { }

        public virtual void ScrollDown() { }

        public virtual void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color);
        }
    }

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

    public class ClearOption : LevelEditorOption
    {
        public override string Name => "Clear";

        public override Color Color => Color.Blue;

        public override void ClickAction()
        {
            player.RoomImIn.Entities.RemoveAll((GameEntity g) => g is not TrapperPlayer);
        }
    }

    public class AddItemOption : LevelEditorOption
    {
        public override string Name => "Add";

        public override Color Color => LevelEditor.CursorMode == 1 ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.CursorMode = 1;
        }
    }

    public class SelectItemOption : LevelEditorOption
    {
        public override string Name => "Select";

        public override Color Color => LevelEditor.CursorMode == 0 ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.CursorMode = 0;
        }
    }

    public class DeleteItemOption : LevelEditorOption
    {
        public override string Name => "Delete";

        public override Color Color => LevelEditor.CursorMode == 2 ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.CursorMode = 2;
        }
    }

    public class SaveOption : LevelEditorOption
    {
        public override string Name => "Save";

        public override Color Color => Color.Blue;

        public override void ClickAction()
        {
            LevelEditor.ExportRoom();
        }
    }

    public class LoadOption : LevelEditorOption
    {
        public override string Name => "Load";

        public override Color Color => Color.Blue;

        public override void ClickAction()
        {
            LevelEditor.ImportRoom(false, clear: false);
        }
    }

    public class ShowGridOption : LevelEditorOption
    {
        public override string Name => "TileGrid";

        public override Color Color => LevelEditor.ShowGrid ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.ShowGrid = !LevelEditor.ShowGrid;
        }
    }

    public class ShowCursorOption : LevelEditorOption
    {
        public override string Name => "Cursor";

        public override Color Color => LevelEditor.ShowCursor ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.ShowCursor = !LevelEditor.ShowCursor;
        }
    }

    public class ShowHitboxOption : LevelEditorOption
    {
        public override string Name => "Hitbox";

        public override Color Color => LevelEditor.ShowHitbox ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.ShowHitbox = !LevelEditor.ShowHitbox;
        }
    }

    public class ShowDoorsOption : LevelEditorOption
    {
        public override string Name => "ShowDoor";

        public override Color Color => LevelEditor.ShowDoors ? Color.Lime : Color.Red;

        public override void ClickAction()
        {
            LevelEditor.ShowDoors = !LevelEditor.ShowDoors;
        }
    }
}
