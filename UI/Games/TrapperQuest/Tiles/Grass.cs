using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI.Games.TrapperQuest
{
    public abstract class TQFloor : TrapperTile, ICreative, IDrawable
    {
        public virtual string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/Grass";

        public virtual int SheetType => 0;

        public int frame = 0;

        public List<EntityOption> EditorOptions => new List<EntityOption>()
        {
            new FrameOption(),
        };

        public static TrapperTile NewTC(int x, int y)
        {
            return NewTC(new Vector2(x, y));
        }

        public static TrapperTile NewTC(Vector2 position)
        {
            TrapperTile newRok = new TrapperTile();
            newRok.tileX = (int)position.X;
            newRok.tileY = (int)position.Y;
            newRok.Position = TQHandler.ConvertToScreenCords(position);
            return newRok;
        }

        public int Layer => 1;

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D Rok = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 drawPosition = Position + offset;

            int frameX = 0;
            int frameY = 0;
            if (frame % 2 == 0)
                frameX = 1;
            frameY = frame / 2;

            Rectangle? rect = null;
            Vector2 origin = Rok.Size() / 2f;
            if (SheetType == 1)
            {
                rect = Rok.Frame(2, 3, frameX, frameY);
                origin = new Vector2(Rok.Width / 4, Rok.Height / 6);
            }
            Main.EntitySpriteDraw(Rok, drawPosition, rect, Color.White, 0f, origin, 2f, 0, 0);

        }
    }

    public class FloorWood : TQFloor, ICreative
    {
        public string Name => "WoodFloor";

        public int ID => 3;
        public override string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/Wood";
    }

    public class FloorCobblestone : TQFloor, ICreative
    {
        public string Name => "CobbleFloor";

        public int ID => 4;
        public override string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/Cobblestone";
    }

    public class FloorGrass : TQFloor, ICreative
    {
        public string Name => "GrassFloor";

        public int ID => 5;
        public override string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/Grass";

        public override int SheetType => 1;
    }

    public class FloorHill : TQFloor, ICreative
    {
        public string Name => "HillFloor";

        public int ID => 6;
        public override string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/Hill";

        public override int SheetType => 1;
    }

    public class FloorHole : TQFloor, ICreative
    {
        public string Name => "HoleFloor";

        public int ID => 7;
        public override string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/Hole";

        public override int SheetType => 1;
    }

    public class FloorRoad : TQFloor, ICreative
    {
        public string Name => "RoadFloor";

        public int ID => 8;
        public override string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/Road";

        public override int SheetType => 1;
    }

    public class FloorDirtPath : TQFloor, ICreative
    {
        public string Name => "PathFloor";

        public int ID => 9;
        public override string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/DirtPath";

        public override int SheetType => 1;
    }

    public class FloorFlower : TQFloor, ICreative
    {
        public string Name => "FlowerFloor";

        public int ID => 10;
        public override string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/Flowers";

        public override int SheetType => 1;
    }

    public class FloorTallGrass : TQFloor, ICreative
    {
        public string Name => "TallGrFloor";

        public int ID => 11;
        public override string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/TallGrass";

        public override int SheetType => 1;
    }

    public class FloorFloor : TQFloor, ICreative
    {
        public string Name => "FloorFloor";

        public int ID => 12;
        public override string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/Floor";

        public override int SheetType => 1;
    }
}