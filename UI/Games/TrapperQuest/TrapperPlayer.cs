using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using Terraria.GameContent;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class TrapperPlayer : GameEntity, IColliding, IDrawable
    {
        public TQRoom RoomImIn;

        public int curFrame;
        public int frameCounter;
        public int direction = 0; // 0 = down, 1 = right, 2 = up, 3 = left
        public int nextRoom = -1;
        public Vector2 nextRoomPos = Vector2.Zero;

        public bool IsRunning => Main.keyState.IsKeyDown(Keys.LeftShift);

        public bool ControlLeft => Main.keyState.IsKeyDown(Keys.A);

        public bool ControlRight => Main.keyState.IsKeyDown(Keys.D);

        public bool ControlUp => Main.keyState.IsKeyDown(Keys.W);

        public bool ControlDown => Main.keyState.IsKeyDown(Keys.S);

        public TrapperPlayer()
        {

        }

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
        public CircleHitbox Hitbox => new CircleHitbox(Position, 24);

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
            int sprintSpeed = 10;
            int moveSpeed = IsRunning ? sprintSpeed : baseSpeed;

            if (ControlLeft && ControlRight)
            {
                Velocity.X = 0;
            }
            else if (ControlLeft || ControlRight)
            {
                if (ControlLeft)
                {
                    Velocity.X = -moveSpeed;
                    direction = 3;
                }
                if (ControlRight)
                {
                    Velocity.X = moveSpeed;
                    direction = 1;
                }
            }
            else
                Velocity.X = 0;

            if (ControlUp && ControlDown)
            {
                Velocity.Y = 0;
            }
            else if (ControlUp || ControlDown)
            {
                if (ControlUp)
                {
                    Velocity.Y = -moveSpeed;
                    direction = 2;
                }
                if (ControlDown)
                {
                    Velocity.Y = moveSpeed;
                    direction = 0;
                }
            }
            else
                Velocity.Y = 0;

            Velocity = Velocity.SafeNormalize(Vector2.Zero) * moveSpeed;

            if (Velocity.Length() > 0)
            {
                int animSpeed = IsRunning ? 6 : 12;
                frameCounter++;
                if (frameCounter > animSpeed)
                {
                    curFrame++;
                    frameCounter = 0;
                }
            }
            else
            {
                frameCounter = 0;
                curFrame = 0;
            }
            if (curFrame > 3)
                curFrame = 0;

            #endregion
        }

        public void ChangeRoom()
        {
            LevelEditor.selectedEntity = null;
            RoomImIn.Entities.Remove(this);
            //RoomImIn = TQRoom.Clone(TQRoomPopulator.LoadedRooms[nextRoom]);
            //RoomImIn = TQRoomPopulator.LoadedRooms[nextRoom];
            RoomImIn = LevelEditor.ImportRoom(true, TQRoomPopulator.RoomData[nextRoom], true);
            RoomImIn.Entities.Add(this);
            Position = nextRoomPos;
            nextRoom = -1;
            nextRoomPos = Vector2.Zero;
        }

        public int Layer => 3;

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Texture2D TBC = ModContent.Request<Texture2D>("CalRemix/UI/Games/TrapperQuest/TrapperPlayer").Value;

            int frame = 0;
            if (direction == 2)
                frame = 8;
            else if (direction == 1)
                frame = 4;
            else if (direction == 3)
                frame = 12;

            Vector2 drawPosition = Position + offset;

            Main.EntitySpriteDraw(TBC, drawPosition, TBC.Frame(1, 16, 0, frame + curFrame), Color.White, 0f, new Vector2(TBC.Width / 2, TBC.Height / 24), 1f, 0, 0);

            // Debug hitbox
            if (LevelEditor.ShowHitbox)
            {
                Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, drawPosition + GameManager.CameraPosition, new Rectangle(0, 0, 48, 48), Color.Orange * 0.8f, 0f, new Vector2(24, 24), 1f, 0, 0);
            }

        }
    }
}