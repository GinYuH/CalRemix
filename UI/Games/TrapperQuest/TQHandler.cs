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

        public const int RoomWidth = 7;
        public const int RoomHeight = 13;
        public const int tileSize = 64;

        public static Vector2 ConvertToTileCords(Vector2 position)
        {
            Vector2 preThing = (position - new Vector2(tileSize / 2)) / tileSize;
            preThing.X = MathF.Floor(preThing.X);
            preThing.Y = MathF.Floor(preThing.Y);
            return preThing;
        }

        public static Vector2 ConvertToScreenCords(Vector2 tilePos)
        {
            return tilePos * tileSize + new Vector2(tileSize / 2);
        }

        public static Rectangle Mouse => new Rectangle((int)Main.MouseScreen.X - (int)GameManager.ScreenOffset.X + tileSize / 2, (int)Main.MouseScreen.Y - (int)GameManager.ScreenOffset.Y + tileSize / 2, 10, 10);

        public static void Load()
        {
            TQRoom room = NewRoom(0, 0, 0);
            player = new TrapperPlayer(GameManager.playingField / 2f, 5f, room);

            DrawLayers = new List<List<IDrawable>>();
            DeadEntities = new List<GameEntity>();
            CollidingEntities = new Dictionary<IColliding, GameEntity>();
            CollidableEntities = new Dictionary<ICollidable, GameEntity>();
            InteractibleEntities = new Dictionary<IInteractable, GameEntity>();
        }

        public static void Unload()
        {
            player = null;
            DrawLayers.Clear();
        }

        public static void Run()
        {
            if (Main.gameMenu || Main.gamePaused)
                return;
            //Process the players movement and actions
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

                //-If they are an active interactible entity and close enough to the player, add them to the list
                if (entity is IInteractable interactable && interactable.CanBeInteractedWith)
                {
                    if ((entity.Position - player.Position).Length() - player.Hitbox.radius < interactable.CollisionCircleRadius)
                    {
                        InteractibleEntities.Add(interactable, entity);
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

            //Clear all the lists created earlier (since they were just here to store the entities that were being processed this frame.
            DeadEntities.Clear();
            CollidingEntities.Clear();
            CollidableEntities.Clear();
            InteractibleEntities.Clear();

            bool levelEditor = true;
            if (levelEditor)
                LevelEditor.Run();
        }

        public static void Draw(SpriteBatch sb)
        {
            //Draw base screen
            Texture2D BackgroundTex = ModContent.Request<Texture2D>("CalRemix/UI/Games/TrapperQuest/BG").Value;
            Vector2 offset = BackgroundTex.Size() / 2f - GameManager.playingField / 2f;
            sb.Draw(BackgroundTex, GameManager.ScreenOffset - offset, null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);

            //Draw room border
            Texture2D BorderTex = ModContent.Request<Texture2D>("CalRemix/UI/Games/TrapperQuest/Border").Value;
            Vector2 borderPos = GameManager.ScreenOffset - offset + (BackgroundTex.Size() - BorderTex.Size()) / 2;
            sb.Draw(BorderTex, borderPos, null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);

            bool debugDraw = true;
            if (debugDraw)
            {
                for (int i = 0; i < RoomHeight + 1; i++)
                {
                    sb.Draw(TextureAssets.MagicPixel.Value, borderPos + Vector2.UnitX * i * 64 + Vector2.One * 7, new Rectangle(0, 0, 2, BorderTex.Height - 16), Color.Red, 0f, Vector2.Zero, 1, 0, 0f);
                }
                for (int i = 0; i < RoomWidth + 1; i++)
                {
                    sb.Draw(TextureAssets.MagicPixel.Value, borderPos + Vector2.UnitY * i * 64 + Vector2.One * 7, new Rectangle(0, 0, BorderTex.Width - 16, 2), Color.Red, 0f, Vector2.Zero, 1, 0, 0f);
                }
            }

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

                if (entity.Position.X > GameManager.playingField.X - collisionRadius || entity.Position.X < collisionRadius)
                {
                    isOOB = true;
                    entity.Position.X = MathHelper.Clamp(entity.Position.X, collisionRadius, GameManager.playingField.X - collisionRadius);
                }

                if (entity.Position.Y > GameManager.playingField.Y - collisionRadius || entity.Position.Y < collisionRadius)
                {
                    isOOB = true;
                    entity.Position.Y = MathHelper.Clamp(entity.Position.Y, collisionRadius, GameManager.playingField.Y - collisionRadius);
                }

                if (isOOB)
                {
                    return collider.OnCollide(null);
                }

                return false;
            }

            else
            {
                if (entity.Position.X > GameManager.playingField.X + BoiHandler.OOBLeeway || entity.Position.X < -BoiHandler.OOBLeeway || entity.Position.Y > GameManager.playingField.Y + BoiHandler.OOBLeeway || entity.Position.Y < -BoiHandler.OOBLeeway)
                {
                    return true;
                }

                return false;
            }
        }

        public static TQRoom NewRoom(int x, int y, int ID)
        {
            //Generate room sstuff
            TQRoom room = new TQRoom(x, y, ID);

            //Fill the room with entities, i assume.
            room.Populate(ID);

            return room;
        }
    }
}
