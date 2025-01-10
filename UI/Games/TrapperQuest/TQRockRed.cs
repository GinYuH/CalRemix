using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using System.Collections.Generic;
using CalRemix.UI.Games.Boi.BaseClasses;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class TQRockRed : TQRock, IDrawable
    {
        public override string Name => "Red Rock";
        public string Texture => "CalRemix/UI/Games/TrapperQuest/Rock";

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D Rok = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 drawPosition = Position + offset;

            Main.EntitySpriteDraw(Rok, drawPosition, null, Color.Red, 0f, Rok.Size() / 2f, 1f, 0, 0);
        }
    }
    public class TQRockGreen : GameEntity, IDrawable
    {
        public override string Name => "Green Rock";
        public string Texture => "CalRemix/UI/Games/TrapperQuest/Rock";

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D Rok = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 drawPosition = Position + offset;

            Main.EntitySpriteDraw(Rok, drawPosition, null, Color.Green, 0f, Rok.Size() / 2f, 1f, 0, 0);

        }
    }
    public class TQRockBlue : GameEntity, IDrawable
    {
        public override string Name => "Blue Rock";
        public string Texture => "CalRemix/UI/Games/TrapperQuest/Rock";

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D Rok = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 drawPosition = Position + offset;

            Main.EntitySpriteDraw(Rok, drawPosition, null, Color.Blue, 0f, Rok.Size() / 2f, 1f, 0, 0);

        }
    }
    public class TQRockPurple : GameEntity, IDrawable
    {
        public override string Name => "Purple Rock";
        public string Texture => "CalRemix/UI/Games/TrapperQuest/Rock";

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D Rok = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 drawPosition = Position + offset;

            Main.EntitySpriteDraw(Rok, drawPosition, null, Color.Purple, 0f, Rok.Size() / 2f, 1f, 0, 0);

        }
    }
    public class TQRockOrange : GameEntity, IDrawable
    {
        public override string Name => "Orange Rock";
        public string Texture => "CalRemix/UI/Games/TrapperQuest/Rock";

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D Rok = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 drawPosition = Position + offset;

            Main.EntitySpriteDraw(Rok, drawPosition, null, Color.Orange, 0f, Rok.Size() / 2f, 1f, 0, 0);

        }
    }
    public class TQRockYellow : GameEntity, IDrawable
    {
        public override string Name => "Yellow Rock";
        public string Texture => "CalRemix/UI/Games/TrapperQuest/Rock";

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D Rok = ModContent.Request<Texture2D>(Texture).Value;

            Vector2 drawPosition = Position + offset;

            Main.EntitySpriteDraw(Rok, drawPosition, null, Color.Yellow, 0f, Rok.Size() / 2f, 1f, 0, 0);

        }
    }
}