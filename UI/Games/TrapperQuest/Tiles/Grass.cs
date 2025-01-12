using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI.Games.TrapperQuest
{
    public abstract class TQFloor : TrapperTile, ICreative, IDrawable
    {
        public virtual string Texture => "CalRemix/UI/Games/TrapperQuest/Tiles/Grass";

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

            Main.EntitySpriteDraw(Rok, drawPosition, null, Color.White, 0f, Rok.Size() / 2f, 2f, 0, 0);

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
    }
}