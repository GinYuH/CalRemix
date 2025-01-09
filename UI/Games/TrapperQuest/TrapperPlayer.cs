using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class TrapperPlayer : GameEntity, IColliding, IDrawable
    {
        public TQRoom RoomImIn;

        public TrapperPlayer(Vector2 position, float health, TQRoom room)
        {
            Position = position;
            RoomImIn = null;
            RoomImIn = room;
            RoomImIn.Entities.Add(this);
        }


        public static float InteractionRadius = 5;

        public int RoomCooldown = 0;

        //The general hitbox of the player. Its the same for collisions and damage.
        public CircleHitbox Hitbox => new CircleHitbox(Position, 25);

        //IColliding stuff
        public bool CanCollide => true;
        public CircleHitbox CollisionHitbox => Hitbox;


        public override void Update()
        {
            RoomCooldown--;
        }

        public void ProcessControls()
        {
            KeyboardState state = Main.keyState;

            #region movement

            int baseSpeed = 3;
            int sprintSpeed = 5;
            int moveSpeed = state.IsKeyDown(Keys.LeftShift) ? sprintSpeed : baseSpeed;

            if (state.IsKeyDown(Keys.A) && state.IsKeyDown(Keys.D))
            {
                Velocity.X = 0;
            }
            else if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.D))
            {
                if (state.IsKeyDown(Keys.A))
                    Velocity.X = -moveSpeed;
                if (state.IsKeyDown(Keys.D))
                    Velocity.X = moveSpeed;
            }
            else
                Velocity.X = 0;

            if (state.IsKeyDown(Keys.W) && state.IsKeyDown(Keys.S))
            {
                Velocity.Y = 0;
            }
            else if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.S))
            {
                if (state.IsKeyDown(Keys.W))
                    Velocity.Y = -moveSpeed;
                if (state.IsKeyDown(Keys.S))
                    Velocity.Y = moveSpeed;
            }
            else
                Velocity.Y = 0;

            Velocity = Velocity.SafeNormalize(Vector2.Zero) * moveSpeed;

            #endregion
        }

        public int Layer => 3;

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D TBC = ModContent.Request<Texture2D>("CalRemix/UI/Games/TrapperQuest/TrapperPlayer").Value;

            int frame = 0;
            if (Velocity.Y < 0)
                frame = 2;
            else if (Velocity.X > 0)
                frame = 1;
            else if (Velocity.X < 0)
                frame = 3;

            Vector2 drawPosition = Position + offset;

            Main.EntitySpriteDraw(TBC, drawPosition, TBC.Frame(1, 4, 0, frame), Color.White, 0f, new Vector2(TBC.Width / 2, TBC.Height / 8), 1f, 0, 0);

        }
    }
}