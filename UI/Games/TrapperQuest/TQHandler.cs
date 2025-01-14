using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using CalRemix.UI.Games.Boi.BaseClasses;
using CalamityMod.Graphics.Renderers;
using Terraria.GameContent;
using log4net.Layout;
using Microsoft.Xna.Framework.Input;
using CalamityMod;
using Terraria.GameInput;
using Newtonsoft.Json.Linq;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class TQHandler
    {
        public static TrapperPlayer player;
        public static List<GameEntity> DeadEntities = new List<GameEntity>();
        public static Dictionary<IColliding, GameEntity> CollidingEntities = new Dictionary<IColliding, GameEntity>();
        public static Dictionary<ICollidable, GameEntity> CollidableEntities = new Dictionary<ICollidable, GameEntity>();
        public static Dictionary<IInteractable, GameEntity> InteractibleEntities = new Dictionary<IInteractable, GameEntity>();
        public static List<List<IDrawable>> DrawLayers = new List<List<IDrawable>>();

        public static int RoomHeight => (int)player.RoomImIn.RoomSize.Y;
        public static int RoomWidth => (int) player.RoomImIn.RoomSize.X;
        public const int tileSize = 64;
        public const int RoomTransitionTime = 90;
        public const int RoomWidthDefault = 13;
        public const int RoomHeightDefault = 7;
        public static Vector2 RoomSizeDefault = new Vector2(RoomWidthDefault, RoomHeightDefault);

        public static int roomTransitionCounter;

        public static Vector2 ConvertToTileCords(int x, int y)
        {
            return ConvertToTileCords(new Vector2(x, y));
        }

        public static Vector2 ConvertToTileCords(Vector2 position)
        {
            Vector2 preThing = (position - new Vector2(tileSize / 2)) / tileSize;
            preThing.X = MathF.Floor(preThing.X);
            preThing.Y = MathF.Floor(preThing.Y);
            return preThing;
        }

        public static Vector2 ConvertToScreenCords(int x, int y)
        {
            return ConvertToScreenCords(new Vector2(x, y));
        }

        public static Vector2 ConvertToScreenCords(Vector2 tilePos)
        {
            return tilePos * tileSize + new Vector2(tileSize / 2);
        }

        public static Rectangle Mouse => 
            new Rectangle((int)Main.MouseScreen.X - (int)GameManager.ScreenOffset.X + tileSize / 2, 
                (int)Main.MouseScreen.Y - (int)GameManager.ScreenOffset.Y + tileSize / 2, 
                10, 
                10);

        public static void Load()
        {
            TQRoom room = new TQRoom(Vector2.Zero, 0);                
            room = TQRoom.Clone(TQRoomPopulator.LoadedRooms[0]);

            player = new TrapperPlayer(ConvertToScreenCords(new Vector2(6, 3)), 5f, room);

            DrawLayers = new List<List<IDrawable>>();
            DeadEntities = new List<GameEntity>();
            CollidingEntities = new Dictionary<IColliding, GameEntity>();
            CollidableEntities = new Dictionary<ICollidable, GameEntity>();
            InteractibleEntities = new Dictionary<IInteractable, GameEntity>();
        }

        public static void Unload()
        {
            player.RoomImIn.Entities.Remove(player);
            player = null;
            DrawLayers.Clear();
            roomTransitionCounter = 0;
            GameManager.CameraPosition = Vector2.Zero;
        }

        public static void Run()
        {
            if (Main.gameMenu || Main.gamePaused)
                return;
            //Process the players movement and actions
            if (roomTransitionCounter > 0)
            {
                roomTransitionCounter--;
                player.Velocity = Vector2.Zero;
                if (roomTransitionCounter == (RoomTransitionTime / 2))
                {
                    player.ChangeRoom();
                    GameManager.CameraPosition = Vector2.Zero;
                }
            }
            else
            {
                player.ProcessControls();

                TQRoom simulatedRoom = player.RoomImIn;

                //For each entity
                foreach (GameEntity entity in simulatedRoom.Entities)
                {
                    entity.OldPosition = entity.Position;
                    entity.Position += entity.Velocity;

                    //-If they are an active colliding entity, add them to the list.
                    if (entity is IColliding colliding && colliding.CanCollide)
                    {
                        //CollidingEntities.Add(colliding, entity);
                    }

                    //-If they are an active collidable entity, add them to the list.
                    if (entity is ICollidable collider && collider.CanCollide)
                    {
                        //CollidableEntities.Add(collider, entity);
                    }

                    bool enteredDoor = false;
                    //-If they are an active interactible entity and close enough to the player, add them to the list
                    if (entity is IInteractable interactable && interactable.CanBeInteractedWith)
                    {
                        if (enteredDoor && entity is TQDoor)
                            continue;
                        if ((entity.Position - player.Position).Length() - player.Hitbox.radius < interactable.CollisionCircleRadius)
                        {
                            InteractibleEntities.Add(interactable, entity);
                            if (entity is TQDoor t)
                                enteredDoor = true;
                        }
                    }
                }

                bool found = false;
                //For each entity that is IColliding and has CanCollide set to true (as listed above)
                foreach (IColliding colliding in CollidingEntities.Keys)
                {
                    GameEntity collidingEntity = CollidingEntities[colliding];

                    //Check for all IColliders, and grab their SimulationDistance.
                    foreach (ICollidable collider in CollidableEntities.Keys)
                    {
                        GameEntity colliderEntity = CollidableEntities[collider];

                        //If the simulation distance + the CollisionCircleRadius of the IColliding is higher than the distance between the two entities, thats awesome, don't even do anything about it and go to the next one
                        if ((colliderEntity.Position - collidingEntity.Position).Length() > collider.SimulationDistance + colliding.CollisionHitbox.radius)
                            continue;

                        Vector2 ogPos = collidingEntity.Position;

                        //If it ISNT, call the MovementCheck function and displace the colliding entity by the provided vector
                        collidingEntity.Position += collider.MovementCheck(colliding.CollisionHitbox);

                        if (ogPos != collidingEntity.Position)
                            found = true;

                        //Call both their onCollide function. Kill the colliding entity if its onCollide returns true
                        collider.OnCollide(collidingEntity);
                        if (colliding.OnCollide(colliderEntity))
                        {
                            DeadEntities.Add(collidingEntity);
                            break;
                        }
                    }
                }

                foreach (IInteractable interactible in InteractibleEntities.Keys)
                {
                    if (interactible.Interact(player))
                        DeadEntities.Add(InteractibleEntities[interactible]);
                }

                //Out of bounds checks & entityn interaction
                foreach (GameEntity entity in simulatedRoom.Entities)
                {
                    if (OOBCheck(entity, entity is IColliding collider))
                        DeadEntities.Add(entity);
                }


                //Remove all the dead entities from the list of simulated entities
                simulatedRoom.Entities.RemoveAll(n => DeadEntities.Contains(n));
            }

            //Clear all the lists created earlier (since they were just here to store the entities that were being processed this frame.
            DeadEntities.Clear();
            CollidingEntities.Clear();
            CollidableEntities.Clear();
            InteractibleEntities.Clear();

            // Camera controls for larger rooms
            if (player.RoomImIn.RoomSize != RoomSizeDefault)
            {
                GameManager.CameraPosition.X = player.Position.X;
                GameManager.CameraPosition.Y = player.Position.Y;

                GameManager.CameraPosition = Vector2.Clamp(GameManager.CameraPosition - ConvertToScreenCords(new Vector2(6, 3)), Vector2.Zero, TQHandler.ConvertToScreenCords(new Vector2(RoomWidth - RoomWidthDefault - 1, RoomHeight - RoomHeightDefault - 1)) + new Vector2(TQHandler.tileSize) / 2);
            }

            bool levelEditor = true;
            if (levelEditor)
                LevelEditor.Run();
        }

        public static void Draw(SpriteBatch sb)
        {
            Texture2D pickle = TextureAssets.MagicPixel.Value;
            Vector2 innerScreenSize = RoomSizeDefault * tileSize;
            int frameThickness = 6;
            Vector2 borderSize = innerScreenSize + new Vector2(frameThickness) * 2 + Vector2.UnitX * 2;
            int bgPadding = 36;
            Vector2 bgSize = borderSize + new Vector2(bgPadding) * 2;
            
            Rectangle cut = new Rectangle((int)GameManager.ScreenOffset.X + (int)GameManager.CameraPosition.X + 52, (int)GameManager.ScreenOffset.Y + (int)GameManager.CameraPosition.Y + 26, (int)(RoomWidthDefault * tileSize + 103), (int)(RoomHeightDefault * tileSize + 52));

            sb.Draw(pickle, GameManager.ScreenOffset + GameManager.CameraPosition - new Vector2(frameThickness + bgPadding), new Rectangle(0, 0, (int)bgSize.X, (int)bgSize.Y), new Color(21, 37, 46), 0f, Vector2.Zero, 1f, 0, 0f);

            //Draw room border
            sb.Draw(pickle, GameManager.ScreenOffset + GameManager.CameraPosition - new Vector2(frameThickness), new Rectangle(0, 0, (int)borderSize.X, (int)borderSize.Y), new Color(0, 255, 255), 0f, Vector2.Zero, 1f, 0, 0f);

            //Cut off everything outside of the screen
            RasterizerState rasterizer = Main.Rasterizer;
            rasterizer.ScissorTestEnable = true;
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, null, Main.UIScaleMatrix);
            sb.GraphicsDevice.ScissorRectangle = cut;

            //For each entity in Entities, if its an IDrawable, store them in new lists, separated on their Layer
            foreach (GameEntity entity in player.RoomImIn.Entities)
            {
                if (entity is IDrawable drawableEntity)
                {
                    int layer = drawableEntity.Layer;

                    //If they are just enough layers minus the one we're looking at 
                    //For example, if currently we have zero layers and we want to put an entity on layer zero. Or if there are two layers (the layers zero and one) and we want to put an entity on the layer two.
                    //In this case, we simply create a new layer after the already existing ones
                    if (DrawLayers.Count == layer)
                    {
                        DrawLayers.Add(new List<IDrawable>());
                    }

                    //If there are less layers than the amount needed 
                    //For example, if we want to add an entity to layer 2 but currently, there is just one layer (The layer zero)
                    if (DrawLayers.Count < layer)
                    {
                        while (!(DrawLayers.Count == layer + 1))
                            DrawLayers.Add(new List<IDrawable>());
                    }

                    //If there are more layers than the layer we want to draw on (this means the layer already got initialized) (which it should have , because of the previous checks.
                    if (DrawLayers.Count > layer)
                    {
                        DrawLayers[layer].Add(drawableEntity);
                    }
                }
            }

            //Then for all these lists, draw the IDrawables with the proper scale and offset.
            for (int i = 0; i < DrawLayers.Count; i++)
            {
                foreach (IDrawable drawer in DrawLayers[i])
                {
                    drawer.Draw(sb, GameManager.ScreenOffset);
                }
            }
            DrawLayers.Clear();
            Color black = Color.Black;
            if (roomTransitionCounter > RoomTransitionTime / 2)
            {
                black *= MathHelper.Lerp(0, 1, Utils.GetLerpValue(RoomTransitionTime, RoomTransitionTime / 2, roomTransitionCounter));
            }
            else
            {
                black *= MathHelper.Lerp(1, 0, Utils.GetLerpValue(RoomTransitionTime / 2, 0, roomTransitionCounter));
            }
            sb.Draw(TextureAssets.MagicPixel.Value, GameManager.ScreenOffset, new Rectangle(0, 0, cut.Width * 2, cut.Height * 2), black, 0f, Vector2.Zero, 1f, 0, 0f);

            bool debugDraw = true;
            if (debugDraw)
            {
                for (int i = 0; i < RoomWidth + 1; i++)
                {
                    Vector2 pos = GameManager.ScreenOffset + Vector2.UnitX * i * 64 + Vector2.UnitY * GameManager.CameraPosition.Y;
                    sb.Draw(TextureAssets.MagicPixel.Value, pos, new Rectangle(0, 0, 2, tileSize * RoomHeight - 16), Color.Black, 0f, Vector2.Zero, 1, 0, 0f);
                    if (i < RoomWidth)
                        Utils.DrawBorderString(sb, i.ToString(), pos, Color.Red);
                }
                for (int i = 0; i < RoomHeight + 1; i++)
                {
                    Vector2 pos = GameManager.ScreenOffset + Vector2.UnitY * i * 64 + Vector2.UnitX * GameManager.CameraPosition.X;
                    sb.Draw(TextureAssets.MagicPixel.Value, pos, new Rectangle(0, 0, tileSize * RoomWidth, 2), Color.Black, 0f, Vector2.Zero, 1, 0, 0f);
                    if (i < RoomHeight)
                        Utils.DrawBorderString(sb, i.ToString(), pos, Color.Cyan);
                }
            }
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, null, Main.UIScaleMatrix);
            sb.ReleaseCutoffRegion(Main.UIScaleMatrix);

            LevelEditor.DrawUI(sb);
        }

        /// <summary>
        /// Do the necessary processes to make sure an entity doesnt end up out of bound, and kills it if necessary. If the entity needs to die, this function returns true
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <param name="Collision">Wether or not the entity can collide with walls. If false, the entity will be able to go out of bounds for a while before getting cleared</param>
        /// <returns>Wether or not the entity died from the oob check</returns>
        public static bool OOBCheck(GameEntity entity, bool Collision)
        {
            if (Collision)
            {
                IColliding collider = entity as IColliding;
                bool isOOB = false;
                float collisionRadius = collider.CollisionHitbox.radius;

                if (entity.Position.X > player.RoomImIn.RoomSize.X * tileSize - collisionRadius || entity.Position.X * tileSize < collisionRadius)
                {
                    isOOB = true;
                    entity.Position.X = MathHelper.Clamp(entity.Position.X, collisionRadius, player.RoomImIn.RoomSize.X * tileSize - collisionRadius);
                }

                if (entity.Position.Y > player.RoomImIn.RoomSize.Y * tileSize - collisionRadius || entity.Position.Y * tileSize < collisionRadius)
                {
                    isOOB = true;
                    entity.Position.Y = MathHelper.Clamp(entity.Position.Y, collisionRadius, player.RoomImIn.RoomSize.Y * tileSize - collisionRadius);
                }

                if (isOOB)
                {
                    return collider.OnCollide(null);
                }

                return false;
            }

            else
            {
                if (entity.Position.X > player.RoomImIn.RoomSize.X * tileSize + BoiHandler.OOBLeeway || entity.Position.X < -BoiHandler.OOBLeeway || entity.Position.Y > player.RoomImIn.RoomSize.Y * tileSize + BoiHandler.OOBLeeway || entity.Position.Y < -BoiHandler.OOBLeeway)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
