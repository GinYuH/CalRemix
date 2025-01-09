using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using CalRemix.UI.Games.Boi.BaseClasses;
using CalamityMod.Graphics.Renderers;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class TQHandler
    {
        public static TrapperPlayer player;
        public static void Load()
        {
            TQRoom room = NewRoom(0, 0);
            player = new TrapperPlayer(GameManager.playingField / 2f, 5f, room);
        }

        public static void Unload()
        {
            player = null;
        }

        public static void Run()
        {
            if (Main.gameMenu || Main.gamePaused)
                return;
            //Process the players movement and actions
            player.ProcessControls();

            player.OldPosition = player.Position;
            player.Position += player.Velocity;


            //For each entity
            /*foreach (GameEntity entity in simulatedRoom.Entities)
            {
                //-Run their update function (and the UpdateEffect functions of their inventory slots
                entity.Update();
                if (entity.Inventory != null)
                {
                    foreach (BoiItem item in entity.Inventory)
                        item.UpdateEffect();
                }

                entity.OldPosition = entity.Position;
                entity.Position += entity.Velocity;

                //-If they are an active colliding entity, add them to the list.
                if (entity is IColliding colliding && colliding.CanCollide)
                {
                    CollidingEntities.Add(colliding, entity);
                }

                //-If they are an active collidable entity, add them to the list.
                if (entity is ICollidable collider && collider.CanCollide)
                {
                    CollidableEntities.Add(collider, entity);
                }

                //-If they are an active damageable entity, add them to the list.
                if (entity is IDamageable damageTaker && damageTaker.Vulnerable)
                {
                    DamageableEntities.Add(damageTaker, entity);
                }

                //-If they are an active damage dealing entity, add them to the list.
                if (entity is IDamageDealer damageDealer && damageDealer.ActiveHitbox)
                {
                    DamageDealingEntities.Add(damageDealer, entity);
                }

                //-If they are an active interactible entity and close enough to the player, add them to the list
                if (entity is IInteractable interactable && interactable.CanBeInteractedWith)
                {
                    if ((entity.Position - Ana.Position).Length() - Ana.Hitbox.radius < interactable.CollisionCircleRadius)
                    {
                        InteractibleEntities.Add(interactable, entity);
                    }
                }
            }*/
        }

        public static void Draw(SpriteBatch sb)
        {
            //Draw base screen
            Texture2D BackgroundTex = ModContent.Request<Texture2D>("CalRemix/UI/Games/Boi/PongBG").Value;
            Vector2 offset = BackgroundTex.Size() / 2f - GameManager.playingField / 2f;
            sb.Draw(BackgroundTex, GameManager.ScreenOffset - offset, null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);

            //Draw room border
            Texture2D BorderTex = ModContent.Request<Texture2D>("CalRemix/UI/Games/Boi/Room").Value;
            Vector2 BorderScale = new Vector2(GameManager.playingField.X / BorderTex.Width, GameManager.playingField.Y / BorderTex.Height);
            sb.Draw(BorderTex, GameManager.ScreenOffset, null, Color.White, 0f, Vector2.Zero, BorderScale, 0, 0f);

            player.Draw(sb, GameManager.ScreenOffset);
            /*//For each entity in Entities, if its an IDrawable, store them in new lists, separated on their Layer
            foreach (BoiEntity entity in player.RoomImIn.Entities)
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
                    drawer.Draw(spriteBatch, ScreenOffset);
                }
            }

            DrawLayers.Clear();*/
        }

        public static TQRoom NewRoom(int x, int y)
        {
            //Generate room sstuff
            TQRoom room = new TQRoom(x, y);

            //Fill the room with entities, i assume.
            room.Populate();

            return room;
        }
    }
}
