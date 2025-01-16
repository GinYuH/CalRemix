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

    public abstract class EntityOption
    {
        public virtual string Name => "";

        public virtual Color Color => Color.Green;

        public GameEntity BoundEntity = null;

        public virtual void ClickAction() { }

        public virtual void ScrollUp() { }

        public virtual void ScrollDown() { }

        public virtual bool IsValidEntity() => false;

        public virtual void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color);
        }
    }

    public class DoorRoomOption : EntityOption
    {
        public override string Name => "RoomGoto";

        public override bool IsValidEntity() => BoundEntity is TQDoor;

        public override void ScrollDown()
        {
            if (BoundEntity is not TQDoor door)
                return;
            int roomID = door.roomGoto + 1;
            if (roomID >= -1 && roomID < TQRoomPopulator.LoadedRooms.Count)
                door.roomGoto = roomID;
        }

        public override void ScrollUp()
        {
            if (BoundEntity is not TQDoor door)
                return;
            int roomID = door.roomGoto - 1;
            if (roomID >= -1 && roomID < TQRoomPopulator.LoadedRooms.Count)
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

    public class DoorXOption : EntityOption
    {
        public override string Name => "SpawnX";

        public override bool IsValidEntity() => BoundEntity is TQDoor;

        public override void ScrollDown()
        {
            if (BoundEntity is not TQDoor door)
                return;
            float size = door.playerTP.X + tileSize;
            if (size >= 0)
                door.playerTP.X = size;
        }

        public override void ScrollUp()
        {
            if (BoundEntity is not TQDoor door)
                return;
            float size = door.playerTP.X - tileSize;
            if (size >= 0)
                door.playerTP.X = size;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            if (BoundEntity is not TQDoor door)
                return;
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, (door.playerTP.X / 64 - 0.5f).ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }

    public class DoorYOption : EntityOption
    {
        public override string Name => "SpawnY";

        public override bool IsValidEntity() => BoundEntity is TQDoor;

        public override void ScrollDown()
        {
            if (BoundEntity is not TQDoor door)
                return;
            float size = door.playerTP.Y + tileSize;
            if (size >= 0)
                door.playerTP.Y = size;
        }

        public override void ScrollUp()
        {
            if (BoundEntity is not TQDoor door)
                return;
            float size = door.playerTP.Y - tileSize;
            if (size >= 0)
                door.playerTP.Y = size;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            if (BoundEntity is not TQDoor door)
                return;
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, (door.playerTP.Y / 64 - 0.5f).ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }

    public class FadeOption : EntityOption
    {
        public override string Name => "Fade";

        public override bool IsValidEntity() => BoundEntity is TQDoor;

        public override Color Color => (BoundEntity is TQDoor door) ? door.fade ? Microsoft.Xna.Framework.Color.Green : Microsoft.Xna.Framework.Color.Red : Microsoft.Xna.Framework.Color.White;

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
}