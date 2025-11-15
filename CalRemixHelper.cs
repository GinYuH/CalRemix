using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.World;
using CalRemix.Content.Items.Ammo;
using CalRemix.Content.Projectiles;
using CalRemix.UI;
using CalRemix.UI.Anomaly109;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix
{
    public static class CalRemixHelper
    {
        public static CalRemixItem Remix(this Item item) => item.GetGlobalItem<CalRemixItem>();
        public static CalRemixNPC Remix(this NPC npc) => npc.GetGlobalNPC<CalRemixNPC>();
        public static CalRemixPlayer Remix(this Player player) => player.GetModPlayer<CalRemixPlayer>();
        public static CalRemixProjectile Remix(this Projectile projectile) => projectile.GetGlobalProjectile<CalRemixProjectile>();

        public static Player Target(this NPC n)
        {
            if (n.target == -1)
            {
                n.TargetClosest(false);
            }
            return Main.player[n.target];
        }

        /// <summary>
        /// Checks if the player has a stack of an item.
        /// </summary>
        public static bool HasStack(this Player player, int itemType, int stackNum)
        {
            for (int i = 0; i < 58; i++)
            {
                Item item = player.inventory[i];
                if (item.type == itemType) { if (item.stack >= stackNum) return true; }
            }
            return false;
        }
        /// Checks if the player has a light pet active.
        /// </summary>
        public static bool HasLightPet(this Player player)
        {
            for (int i = 0; i < player.buffType.Length; i++)
            {
                if (Main.lightPet[player.buffType[i]])
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Consumes an item stack from the player.
        /// </summary>
        public static void ConsumeStack(this Player player, int itemType, int stackNum)
        {
            for(int i = 0; i < 58; i++)
            {
                ref Item item = ref player.inventory[i];
                if (player.HasStack(itemType, stackNum)) item.stack -= stackNum;
            }
        }
        /// <summary>
        /// Checks if the player has every item of a given list.
        /// </summary>
        public static bool HasItems(this Player player, List<int> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!player.HasItem(items[i]))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Checks if the player has an item from a mod.
        /// </summary>
        public static bool HasCrossModItem(Player player, Mod Mod, string ItemName)
        {
            if (Mod != null)
            {
                if (player.HasItem(Mod.Find<ModItem>(ItemName).Type))
                    return true;
            }
            return false;
        }
        public static bool HasCrossModItem(Player player, string ModName, string ItemName)
        {
            if (ModLoader.HasMod(ModName))
            {
                if (player.HasItem(ModLoader.GetMod(ModName).Find<ModItem>(ItemName).Type))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Changes item to the specified type. If stack is non-zero, change to the stack parameter.
        /// </summary>
        public static Item ChangeItemWithStack(this Item item, int to, int stack = 0)
        {
            bool flag = item.favorited;
            stack = (stack <= 0) ? item.stack : stack;
            item.SetDefaults(to);
            item.favorited = flag;
            item.stack = stack;
            return item;
        }
        /// <summary>
        /// Updates buff and checks if the player is dead. If dead, sets minion bool to false. If not, update timeLeft.
        /// </summary>
        public static void CheckMinionCondition(this Projectile projectile, int buff, bool minionBool)
        {
            Player player = Main.player[projectile.owner];
            player.AddBuff(buff, 3600);
            if (player.dead)
                minionBool = false;
            if (minionBool)
                projectile.timeLeft = 2;
        }
        /// <summary>
        /// Get the localized text using the provided key
        /// </summary>
        public static LocalizedText LocalText(string key) => Language.GetOrRegister("Mods.CalRemix." + key);

        /// <summary>
        /// Get the localized dialog using the provided key and send to all players in chat
        /// </summary>
        public static void GetNPCDialog(string key, Color? textColor = null)
        {
            if (!textColor.HasValue)
                textColor = Color.White;
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(LocalText($"Dialog.{key}").Value, textColor.Value);
            else
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey($"Mods.CalRemix.Dialog.{key}"), textColor.Value);
        }
        private static readonly FieldInfo shaderTextureField = typeof(MiscShaderData).GetField("_uImage1", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly FieldInfo shaderTextureField2 = typeof(MiscShaderData).GetField("_uImage2", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly FieldInfo shaderTextureField3 = typeof(MiscShaderData).GetField("_uImage3", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Uses reflection to set the _uImage1. Its underlying data is private and the only way to change it publicly is via a method that only accepts paths to vanilla textures.
        /// </summary>
        /// <param name="shader">The shader</param>
        /// <param name="texture">The texture to use</param>
        public static void SetShaderTexture(this MiscShaderData shader, Asset<Texture2D> texture) => shaderTextureField.SetValue(shader, texture);

        /// <summary>
        /// Uses reflection to set the _uImage2. Its underlying data is private and the only way to change it publicly is via a method that only accepts paths to vanilla textures.
        /// </summary>
        /// <param name="shader">The shader</param>
        /// <param name="texture">The texture to use</param>
        public static void SetShaderTexture2(this MiscShaderData shader, Asset<Texture2D> texture) => shaderTextureField2.SetValue(shader, texture);

        /// <summary>
        /// Uses reflection to set the _uImage3. Its underlying data is private and the only way to change it publicly is via a method that only accepts paths to vanilla textures.
        /// </summary>
        /// <param name="shader">The shader</param>
        /// <param name="texture">The texture to use</param>
        public static void SetShaderTexture3(this MiscShaderData shader, Asset<Texture2D> texture) => shaderTextureField3.SetValue(shader, texture);

        /// <summary>
        /// Sets a <see cref="SpriteBatch"/>'s <see cref="BlendState"/> arbitrarily.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="blendState">The blend state to use.</param>
        public static void SetBlendState(this SpriteBatch spriteBatch, BlendState blendState)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, blendState, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        /// <summary>
        /// Reset's a <see cref="SpriteBatch"/>'s <see cref="BlendState"/> based to a typical <see cref="BlendState.AlphaBlend"/>.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="blendState">The blend state to use.</param>
        public static void ResetBlendState(this SpriteBatch spriteBatch) => spriteBatch.SetBlendState(BlendState.AlphaBlend);

        public static void DrawBloomLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float width)
        {
            // Draw nothing if the start and end are equal, to prevent division by 0 problems.
            if (start == end)
                return;

            start -= Main.screenPosition;
            end -= Main.screenPosition;

            Texture2D line = ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/Lines/BloomLine").Value;
            float rotation = (end - start).ToRotation() + MathHelper.PiOver2;
            Vector2 scale = new Vector2(width, Vector2.Distance(start, end)) / line.Size();
            Vector2 origin = new(line.Width / 2f, line.Height);

            spriteBatch.Draw(line, start, null, color, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        public static void SwapToRenderTarget(this RenderTarget2D renderTarget, Color? flushColor = null)
        {
            // Local variables for convinience.
            GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;
            SpriteBatch spriteBatch = Main.spriteBatch;

            // If we are in the menu, a server, or any of these are null, return.
            if (Main.gameMenu || Main.dedServ || renderTarget is null || graphicsDevice is null || spriteBatch is null)
                return;

            // Otherwise set the render target.
            graphicsDevice.SetRenderTarget(renderTarget);

            // "Flush" the screen, removing any previous things drawn to it.
            flushColor ??= Color.Transparent;
            graphicsDevice.Clear(flushColor.Value);
        }
        public static void ChatMessage(string text, Color color, NetworkText netText = null)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(text, color);
            else if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(netText ?? NetworkText.FromLiteral(text), color);
        }
        public static void SpawnNPCOnPlayer(int playerWhoAmI, int type)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.SpawnOnPlayer(playerWhoAmI, type);
            else
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: playerWhoAmI, number2: type);
        }
        /// <summary>
        /// Spawns a new npc with multiplayer and action invocation support.
        /// </summary>
        /// <param name="source">The source of the npc.</param>
        /// <param name="x">The x spawn position of the npc.</param>
        /// <param name="y">The y spawn position of the npc.</param>
        /// <param name="type">The id of the npc type that should be spawned.</param>
        /// <param name="minSlot">The lowest slot in <see cref="Main.npc"/> the npc can use.</param>
        /// <param name="ai0">The <see cref="NPC.ai"/>[0] value.</param>
        /// <param name="ai1">The <see cref="NPC.ai"/>[1] value.</param>
        /// <param name="ai2">The <see cref="NPC.ai"/>[0] value.</param>
        /// <param name="ai3">The <see cref="NPC.ai"/>[1] value.</param>
        /// <param name="target">The player index to use for the <see cref="NPC.target"/> value.</param>
        /// <param name="npcTasks">The actions the <see cref="NPC"/> executes.</param>
        /// <param name="awakenMessage">Sends a boss awaken message in chat</param>
        public static NPC SpawnNewNPC(IEntitySource source, int x, int y, int type, int minSlot = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int target = 255, Action<NPC> npcTasks = null, bool awakenMessage = false)
        {
            NPC npc = Main.npc[0];
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc = NPC.NewNPCDirect(source, x, y, type, minSlot, ai0, ai1, ai2, ai3, target);
                if (Main.npc.IndexInRange(npc.whoAmI))
                {
                    npcTasks?.Invoke(npc);
                    if (Main.dedServ)
                        npc.netUpdate = true;
                }
            }
            if (awakenMessage)
                ChatMessage(Language.GetTextValue("Announcement.HasAwoken", npc.TypeName), new Color(175, 75, 255), NetworkText.FromKey("Announcement.HasAwoken", npc.GetTypeNetName()));
            return npc;
        }
        /// <summary>
        /// Spawns a new npc with multiplayer and action invocation support.
        /// </summary>
        /// <param name="source">The source of the npc.</param>
        /// <param name="position">The spawn position of the npc.</param>
        /// <param name="type">The id of the npc type that should be spawned.</param>
        /// <param name="minSlot">The lowest slot in <see cref="Main.npc"/> the npc can use.</param>
        /// <param name="ai0">The <see cref="NPC.ai"/>[0] value.</param>
        /// <param name="ai1">The <see cref="NPC.ai"/>[1] value.</param>
        /// <param name="ai2">The <see cref="NPC.ai"/>[0] value.</param>
        /// <param name="ai3">The <see cref="NPC.ai"/>[1] value.</param>
        /// <param name="target">The player index to use for the <see cref="NPC.target"/> value.</param>
        /// <param name="npcTasks">The actions the <see cref="NPC"/> executes.</param>
        /// <param name="awakenMessage">Sends a boss awaken message in chat</param>
        public static NPC SpawnNewNPC(IEntitySource source, Vector2 position, int type, int minSlot = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int target = 255, Action<NPC> npcTasks = null, bool awakenMessage = false)
        {
            return SpawnNewNPC(source, (int)position.X, (int)position.Y, type, minSlot, ai0, ai1, ai2, ai3, target, npcTasks, awakenMessage);
        }
        public static void DestroyTile(int i, int j, bool fail = false, bool effectOnly = false, bool noItem = false)
        {
            WorldGen.KillTile(i, j, fail, effectOnly, noItem);
            if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
        }
        /// <summary>
        /// Summons a projectile of a specific type while also adjusting damage for vanilla spaghetti regarding hostile projectiles.
        /// </summary>
        /// <param name="spawnX">The x spawn position of the projectile.</param>
        /// <param name="spawnY">The y spawn position of the projectile.</param>
        /// <param name="velocityX">The x velocity of the projectile.</param>
        /// <param name="velocityY">The y velocity of the projectile</param>
        /// <param name="type">The id of the projectile type that should be spawned.</param>
        /// <param name="damage">The damage of the projectile.</param>
        /// <param name="knockback">The knockback of the projectile.</param>
        /// <param name="owner">The owner index of the projectile.</param>
        /// <param name="ai0">An optional <see cref="NPC.ai"/>[0] fill value. Defaults to 0.</param>
        /// <param name="ai1">An optional <see cref="NPC.ai"/>[1] fill value. Defaults to 0.</param>
        public static int NewProjectileBetter(float spawnX, float spawnY, float velocityX, float velocityY, int type, int damage, float knockback, int owner = -1, float ai0 = 0f, float ai1 = 0f)
        {
            if (owner == -1)
                owner = Main.myPlayer;
            damage = (int)(damage * 0.5);
            if (Main.expertMode)
                damage = (int)(damage * 0.5);
            int index = Projectile.NewProjectile(new EntitySource_WorldEvent(), spawnX, spawnY, velocityX, velocityY, type, damage, knockback, owner, ai0, ai1);
            if (index >= 0 && index < Main.maxProjectiles)
                Main.projectile[index].netUpdate = true;

            return index;
        }

        /// <summary>
        /// Summons a projectile of a specific type while also adjusting damage for vanilla spaghetti regarding hostile projectiles.
        /// </summary>
        /// <param name="center">The spawn position of the projectile.</param>
        /// <param name="velocity">The velocity of the projectile</param>
        /// <param name="type">The id of the projectile type that should be spawned.</param>
        /// <param name="damage">The damage of the projectile.</param>
        /// <param name="knockback">The knockback of the projectile.</param>
        /// <param name="owner">The owner index of the projectile.</param>
        /// <param name="ai0">An optional <see cref="NPC.ai"/>[0] fill value. Defaults to 0.</param>
        /// <param name="ai1">An optional <see cref="NPC.ai"/>[1] fill value. Defaults to 0.</param>
        public static int NewProjectileBetter(Vector2 center, Vector2 velocity, int type, int damage, float knockback, int owner = -1, float ai0 = 0f, float ai1 = 0f)
        {
            return NewProjectileBetter(center.X, center.Y, velocity.X, velocity.Y, type, damage, knockback, owner, ai0, ai1);
        }

        /// <summary>
        /// Returns all projectiles present of a specific type.
        /// </summary>
        /// <param name="desiredTypes">The projectile type to check for.</param>
        public static IEnumerable<Projectile> AllProjectilesByID(params int[] desiredTypes)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && desiredTypes.Contains(Main.projectile[i].type))
                    yield return Main.projectile[i];
            }
        }



        /// <summary>
        /// Returns a readable projectile damage number
        /// </summary>
        /// <param name="normal">The damage in normal mode</param>
        /// <param name="expert">The damage in expert mode. If left 0, defaults to normal damage</param>
        /// <param name="master">The damage in master mode. If left 0, defaults to expert damage, then normal damage</param>
        /// <returns></returns>
        public static int ProjectileDamage(int normal, int expert = 0, int master = 0)
        {
            if (Main.masterMode && master != 0)
                return (int)(master / 6f);
            else if (Main.masterMode && master == 0 && expert != 0)
                return (int)(expert / 6f);
            else if (Main.masterMode && master == 0 && expert == 0)
                return (int)(normal / 6f);
            else if (Main.expertMode && expert != 0)
                return (int)(expert / 4f);
            else if (Main.masterMode && expert == 0)
                return (int)(normal / 4f);
            else
                return (int)(normal / 2f);
        }

        /// <summary>
        /// Checks if a point is inside of a given elipse area and position
        /// </summary>
        /// <param name="x">X to checkt</param>
        /// <param name="y">Y to check</param>
        /// <param name="h">Elipse center x</param>
        /// <param name="k">Elipse center y</param>
        /// <param name="a">X size</param>
        /// <param name="b">Y size</param>
        /// <returns>true if the point is inside</returns>
        public static bool WithinElipse(float x, float y, float h, float k, float a, float b)
        {
            double p = (MathF.Pow((x - h), 2) / MathF.Pow(a, 2))
                    + (MathF.Pow((y - k), 2) / MathF.Pow(b, 2));

            return p < 1;
        }

        /// <summary>
        /// Checks if a point is inside of a given heart area and position
        /// </summary>
        /// <param name="origin">The center of the heart</param>
        /// <param name="roughDimensions">The width and height of the heart</param>
        /// <param name="point">The point to check</param>
        /// <returns>true if the point is inside</returns>
        public static bool WithinHeart(Point origin, Point roughDimensions, Point point)
        {
            float x = (point.X - origin.X) / (roughDimensions.X / 2f);
            float y = -(point.Y - origin.Y) / (roughDimensions.Y / 2f);

            return (MathF.Pow(MathF.Pow(x, 2) + MathF.Pow(y, 2) - 1f, 3f) - (MathF.Pow(x, 2) * MathF.Pow(y, 3))) <= 0f;
        }

        /// <summary>
        /// Checks if a point is inside of a given rhombus area and position
        /// </summary>
        /// <param name="origin">The center of the rhombus</param>
        /// <param name="dimensions">The width and height of the rhombus</param>
        /// <param name="point">The point to check</param>
        /// <returns>true if the point is inside</returns>
        public static bool WithinRhombus(Point origin, Point dimensions, Point point)
        {
            float val = ((Math.Abs(point.X - origin.X)) / (float)dimensions.X) + ((Math.Abs(point.Y - origin.Y)) / (float)dimensions.Y);
            return val < 1;
        }

        /// <summary>
        /// Gets the area of a triangle given 3 points
        /// </summary>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <param name="point3">The third point</param>
        /// <returns>the area of the triangle</returns>
        public static float TriangleArea(Point point1, Point point2, Point point3)
        {
            return Math.Abs(point1.X * (point2.Y - point3.Y) + point2.X * (point3.Y - point1.Y) + point3.X * (point1.Y - point2.Y)) / 2f;
        }

        /// <summary>
        /// Checks if a point is inside of a triangle with 3 given points
        /// </summary>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <param name="point3">The third point</param>
        /// <param name="toCheck">The point to check</param>
        /// <returns>true if the point is inside</returns>
        public static bool WithinTriangle(Point point1, Point point2, Point point3, Point toCheck)
        {
            float mainArea = TriangleArea(point1, point2, point3);

            float areaOneTwo = TriangleArea(point1, point2, toCheck);
            float areaTwoThree = TriangleArea(point2, point3, toCheck);
            float areaOneThreeLikeTheUser = TriangleArea(point3, point1, toCheck);

            return mainArea == areaOneTwo + areaTwoThree + areaOneThreeLikeTheUser;
        }

        public enum PerlinEase
        {
            /// <summary>
            /// No easing, all of the noise is consistent
            /// </summary>
            None = 0,
            /// <summary>
            /// Top starts solid and becomes noise
            /// </summary>
            EaseInTop = 1,
            /// <summary>
            /// Top is noise, bottom is air
            /// </summary>
            EaseOutBottom = 2,
            /// <summary>
            /// Top and bottom are solid, middle is noise
            /// </summary>
            EaseInOut = 3,
            /// <summary>
            /// Top and bottom are noise, middle is solid
            /// </summary>
            EaseOutIn = 4,
            /// <summary>
            /// Top is noise, bottom is solid
            /// </summary>
            EaseInBottom = 5,
            /// <summary>
            /// Top is air, bottom is noise
            /// </summary>
            EaseOutTop = 6
        }

        /// <summary>
        /// Generates a rectangle of tiles and/or walls using noise
        /// </summary>
        /// <param name="area">The rectangle</param>
        /// <param name="noiseThreshold">How open should caves be? Scales between 0f and 1f</param>
        /// <param name="noiseStrength">How strong the noise is. Weaker values look more like noodles</param>
        /// <param name="noiseSize">The zoom of the noise. Higher values means more zoomed in. Set to 120, 180 by default, the same as the Baron Strait</param>
        /// <param name="tileType">The tile to place</param>
        /// <param name="wallType">The wall to place</param>
        /// <param name="tileCondition">A condition for if a tile can be placed. Usually used in conjunction with shapes</param>
        public static void PerlinGeneration(Rectangle area, float noiseThreshold = 0.56f, float noiseStrength = 0.1f, Vector2 noiseSize = default, int tileType = -1, int wallType = 0, PerlinEase ease = PerlinEase.None, float topStop = 0.3f, float bottomStop = 0.7f, Predicate<Point> tileCondition = null, bool overrideTiles = false, bool eraseWalls = true)
        {

            int sizeX = area.Width;
            int sizeY = area.Height;

            // Map to store what blocks should be converted
            bool[,] map = new bool[sizeX, sizeY];

            // default Baron Strait numbers
            if (noiseSize == default)
            {
                noiseSize.X = 180f;
                noiseSize.Y = 120f;
            }

            // Create a perlin noise map
            for (int i = 0; i < area.Width; i++)
            {
                for (int j = 0; j < area.Height; j++)
                {
                    float noise = CalamityUtils.PerlinNoise2D(i / noiseSize.X, j / noiseSize.Y, 3, (int)Main.GlobalTimeWrappedHourly) * 0.5f + 0.5f;

                    float endPoint = noiseThreshold;
                    switch (ease)
                    {
                        case PerlinEase.EaseInTop:
                            endPoint = MathHelper.Lerp(noise, noiseThreshold, Utils.GetLerpValue(0, topStop, (j / (float)area.Height), true));
                            break;
                        case PerlinEase.EaseOutBottom:
                            endPoint = MathHelper.Lerp(noiseThreshold, 0, Utils.GetLerpValue(bottomStop, 1f, (j / (float)area.Height), true));
                            break;
                        case PerlinEase.EaseInOut:
                            if (j / (float)area.Height < 0.5f)
                            {
                                endPoint = MathHelper.Lerp(noise, noiseThreshold, Utils.GetLerpValue(0, topStop, (j / (float)area.Height), true));
                            }
                            else
                            {
                                endPoint = MathHelper.Lerp(noise, noiseThreshold, Utils.GetLerpValue(1f, bottomStop, (j / (float)area.Height), true));
                            }
                            break;
                        case PerlinEase.EaseOutTop:
                            endPoint = MathHelper.Lerp(0, noiseThreshold, Utils.GetLerpValue(0f, topStop, (j / (float)area.Height), true));
                            break;
                        case PerlinEase.EaseInBottom:
                            endPoint = MathHelper.Lerp(noiseThreshold, noise, Utils.GetLerpValue(bottomStop, 1f, (j / (float)area.Height), true));
                            break;
                        case PerlinEase.EaseOutIn:
                            if (j / (float)area.Height < 0.5f)
                            {
                                endPoint = MathHelper.Lerp(noise, noiseThreshold, Utils.GetLerpValue(0.5f, topStop, (j / (float)area.Height), true));
                            }
                            else
                            {
                                endPoint = MathHelper.Lerp(noise, noiseThreshold, Utils.GetLerpValue(0.5f, bottomStop, (j / (float)area.Height), true));
                            }
                            break;
                    }

                    map[i, j] = MathHelper.Distance(noise, endPoint) < noiseStrength;
                }
            }
            if (overrideTiles)
            {
                // Iterate through the map and remove blocks/walls accordingly
                for (int gdv = 0; gdv < 1; gdv++)
                {
                    for (int i = 0; i < area.Width; i++)
                    {
                        for (int j = 0; j < area.Height; j++)
                        {
                            if (!WorldGen.InWorld(area.X + i, area.Y + j))
                            {
                                continue;
                            }
                            Tile t = Main.tile[area.X + i, area.Y + j];

                            if (tileCondition != null)
                            {
                                if (!tileCondition.Invoke(new Point(area.X + i, area.Y + j)))
                                {
                                    continue;
                                }
                            }

                            t.ClearEverything();
                        }
                    }
                }
            }
            // Iterate through the map and add blocks/walls accordingly
            for (int gdv = 0; gdv < 1; gdv++)
            {
                for (int i = 0; i < area.Width; i++)
                {
                    for (int j = 0; j < area.Height; j++)
                    {
                        if (!WorldGen.InWorld(area.X + i, area.Y + j))
                        {
                            continue;
                        }
                        Tile t = Main.tile[area.X + i, area.Y + j];
                        if (t.HasTile && !overrideTiles)
                        {
                            continue;
                        }

                        if (tileCondition != null)
                        {
                            if (!tileCondition.Invoke(new Point(area.X + i, area.Y + j)))
                            {
                                continue;
                            }
                        }

                        // Cell stuff
                        int sur = SurroundingTileCounts(map, i, j, area.Width, area.Height);

                        // If there are more than 4 nearby nodes, place some
                        if (sur > 4 && (tileType != -1 || (wallType != 0 && eraseWalls)))
                        {
                            if (tileType != -1)
                            {
                                t.ResetToType((ushort)tileType);
                                //WorldGen.SquareTileFrame(area.X + i, area.Y + j);
                            }
                            if (wallType != 0)
                            {
                                t.WallType = (ushort)wallType;
                                //WorldGen.SquareWallFrame(area.X + i, area.Y + j);
                            }
                            map[i, j] = true;
                        }
                        // If there are less than 4 nodes nearby, remove
                        else if (sur < 4)
                        {
                            t.ClearEverything();
                            map[i, j] = false;
                        }

                        if (!eraseWalls && wallType != 0)
                        {
                            t.WallType = (ushort)wallType;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generations perlin-based surface terrain
        /// </summary>
        /// <param name="area">The area to generate it</param>
        /// <param name="tileType">The tile type</param>
        /// <param name="iterations">How many iterations should be done</param>
        /// <param name="variance">Height variance in tiles</param>
        /// <param name="perlinBottom">Smoothen the bottom like the top</param>
        public static void PerlinSurface(Rectangle area, int tileType, int iterations = 3, int variance = 20, bool perlinBottom = false)
        {
            int baseHeight = area.Y;
            int noiseSeed = WorldGen.genRand.Next(0, int.MaxValue);
            int noiseSeedBottom = WorldGen.genRand.Next(0, int.MaxValue);
            for (int i = area.X; i < area.X + area.Width; i++)
            {
                float height = CalamityUtils.PerlinNoise2D(i / 380f, 0, iterations, noiseSeed);
                float heightBottom = CalamityUtils.PerlinNoise2D(i / 380f, 0, iterations, noiseSeedBottom);

                for (int j = area.Y; j < area.Y + area.Height; j++)
                {
                    bool bottom = perlinBottom ? (j < area.Y + area.Height - (int)(heightBottom * variance)) : true;
                    if (j > baseHeight + 2 + (int)(height * variance) && bottom)
                    {
                        Tile t = Main.tile[i, j];
                        t.ResetToType((ushort)tileType);
                    }
                }
            }
        }

        // thank you random youtube video
        // https://www.youtube.com/watch?v=v7yyZZjF1z4
        public static int SurroundingTileCounts(bool[,] map, int x, int y, int checkDistX = 0, int chestDistY = 0)
        {
            int wallCount = 0;
            for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
            {
                for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < checkDistX && neighbourY >= 0 && neighbourY < chestDistY)
                    {
                        if (neighbourX != x || neighbourY != y)
                        {
                            wallCount += map[neighbourX, neighbourY].ToInt();
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }
            return wallCount;
        }

        /// <summary>
        /// Creates a basic verlet chain if one doesn't exist
        /// </summary>
        /// <param name="list">The list instance</param>
        /// <param name="segmentAmount">The amount of segments</param>
        /// <param name="basePosition">The position to hook from</param>
        /// <param name="locks">Indices that should be locked</param>
        /// <returns>The segment list</returns>
        public static List<VerletSimulatedSegment> CreateVerletChain(ref List<VerletSimulatedSegment> list, int segmentAmount, Vector2 basePosition, List<int> locks = default)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                if (list == null || list.Count < segmentAmount)
                {
                    list = new List<VerletSimulatedSegment>(segmentAmount);
                    for (int i = 0; i < segmentAmount; i++)
                    {
                        VerletSimulatedSegment segment = new VerletSimulatedSegment(basePosition);
                        list.Add(segment);
                    }
                    // Lock segments
                    if (locks != default)
                    {
                        for (int i = 0; i < locks.Count; i++)
                        {
                            // Assure the index isnt too high
                            int trueIndex = (locks[i] >= list.Count) ? (list.Count - 1) : locks[i];
                            list[trueIndex].locked = true;
                        }
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// Gives a specified player a specified number of coins.
        /// </summary>
        /// <param name="coinValue">The value of the coins to give the player.</param>
        /// <param name="player">The player to be given the coins.</param>
        public static void GiveCoins(int coinValue, Player player)
        {
            int[] coinsList = { ItemID.CopperCoin, ItemID.SilverCoin, ItemID.GoldCoin, ItemID.PlatinumCoin, ModContent.ItemType<CosmiliteCoin>(), ModContent.ItemType<Klepticoin>() };
            for(int i = 5; i >= 0; i--)
            {
                if (coinValue >= Math.Pow(100, i))
                {
                    player.QuickSpawnItem(player.GetSource_DropAsItem(), coinsList[i], (int)(coinValue / Math.Pow(100, i)));
                    coinValue %= (int)Math.Pow(100, i);
                }
            }
        }

        public static void DrawChain(Texture2D texture, Vector2 start, Vector2 end, float angleAdditive = 0, Color color = default)
        {
            Vector2 center = start;
            float rotation = start.AngleTo(end) - MathF.PI / 2f;
            bool doDraw = true;
            float increment = angleAdditive == MathHelper.PiOver2 ? texture.Width : texture.Height;
            while (doDraw)
            {
                float dist = (end - center).Length();
                if (dist < (float)increment + 1f)
                {
                    doDraw = false;
                    continue;
                }

                if (float.IsNaN(dist))
                {
                    doDraw = false;
                    continue;
                }

                center += start.DirectionTo(end) * increment;
                Color finalColor = (color == new Color(0, 0, 0, 0) ? Lighting.GetColor((int)((center.X + Main.screenPosition.X) / 16), (int)((center.Y + Main.screenPosition.Y) / 16f)) : color);
                Main.spriteBatch.Draw(texture, center, new Rectangle(0, 0, texture.Width, texture.Height), finalColor, rotation + angleAdditive, texture.Size() / 2f, 1f, SpriteEffects.None, 0f);
            }
        }

        public static void DustExplosionOutward(Vector2 position, int dustID, float speed, int amount = 50, Color color = default, int alpha = 0, float scaleMin = 1, float scaleMax = 1.001f)
        {
            for (int i = 0; i < amount; i++)
            {
                Dust dust = Dust.NewDustPerfect(position, dustID, Vector2.Zero, alpha, color, Main.rand.NextFloat(scaleMin, scaleMax));
                dust.noGravity = true;
                dust.position += Main.rand.NextVector2Square(-5, 5);
                dust.velocity = position.DirectionTo(dust.position) * speed;
            }
        }

        public static void DustExplosionOutward(Vector2 position, int dustID, float speedMin, float speedMax, int amount = 50, Color color = default, int alpha = 0, float scaleMin = 1, float scaleMax = 1.001f)
        {
            for (int i = 0; i < amount; i++)
            {
                Dust dust = Dust.NewDustPerfect(position, dustID, Vector2.Zero, alpha, color, Main.rand.NextFloat(scaleMin, scaleMax));
                dust.noGravity = true;
                dust.position += Main.rand.NextVector2Square(-5, 5);
                dust.velocity = position.DirectionTo(dust.position) * Main.rand.NextFloat(speedMin, speedMax);
            }
        }

        public static void DustExplosionOutward(Vector2 position, int dustID, float speedMin, float speedMax, int amount = 50, Color color = default, int alpha = 0, float scale = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                Dust dust = Dust.NewDustPerfect(position, dustID, Vector2.Zero, alpha, color, scale);
                dust.noGravity = true;
                dust.position += Main.rand.NextVector2Square(-5, 5);
                dust.velocity = position.DirectionTo(dust.position) * Main.rand.NextFloat(speedMin, speedMax);
            }
        }

        public static void DustExplosionOutward(Vector2 position, int dustID, float speed, int amount = 50, Color color = default, int alpha = 0, float scale = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                Dust dust = Dust.NewDustPerfect(position, dustID, Vector2.Zero, alpha, color, scale);
                dust.noGravity = true;
                dust.position += Main.rand.NextVector2Square(-5, 5);
                dust.velocity = position.DirectionTo(dust.position) * speed;
            }
        }


        public static bool ForceGrowTree(int i, int y, int height = 0)
        {
            int j = y;

            if (!Main.tile[i, j].IsHalfBlock && Main.tile[i, j].Slope == 0)
            {
                TileColorCache cache = Main.tile[i, j].BlockColorAndCoating();
                if (Main.tenthAnniversaryWorld && !WorldGen.gen)
                    cache.Color = (byte)WorldGen.genRand.Next(1, 13);

                int treeHeight = WorldGen.genRand.Next(5, 17);

                if (height != 0)
                    treeHeight = height;

                int extraHeight = treeHeight + 4;

                bool canGenerate = false;
                if (WorldGen.EmptyTileCheck(i - 2, i + 2, j - extraHeight, j - 1, 20))
                {
                    canGenerate = true;
                }
                if (canGenerate)
                {
                    bool remixUnderground = Main.remixWorld && (double)j < Main.worldSurface;
                    bool topMaybeIDK = false;
                    bool branchIThinkIDK = false;
                    int treeFrame;
                    for (int k = j - treeHeight; k < j; k++)
                    {
                        Main.tile[i, k].TileType = TileID.Trees;
                        Main.tile[i, k].Get<TileWallWireStateData>().HasTile = true;
                        Main.tile[i, k].TileFrameX = ((byte)WorldGen.genRand.Next(3));
                        Main.tile[i, k].UseBlockColors(cache);
                        treeFrame = WorldGen.genRand.Next(3);
                        int randomFrame = WorldGen.genRand.Next(10);
                        if (k == j - 1 || k == j - treeHeight)
                            randomFrame = 0;

                        while (((randomFrame == 5 || randomFrame == 7) && topMaybeIDK) || ((randomFrame == 6 || randomFrame == 7) && branchIThinkIDK))
                        {
                            randomFrame = WorldGen.genRand.Next(10);
                        }

                        topMaybeIDK = false;
                        branchIThinkIDK = false;
                        if (randomFrame == 5 || randomFrame == 7)
                            topMaybeIDK = true;

                        if (randomFrame == 6 || randomFrame == 7)
                            branchIThinkIDK = true;

                        switch (randomFrame)
                        {
                            case 1:
                                if (treeFrame == 0)
                                {
                                    Main.tile[i, k].TileFrameX = 0;
                                    Main.tile[i, k].TileFrameY = 66;
                                }
                                if (treeFrame == 1)
                                {
                                    Main.tile[i, k].TileFrameX = 0;
                                    Main.tile[i, k].TileFrameY = 88;
                                }
                                if (treeFrame == 2)
                                {
                                    Main.tile[i, k].TileFrameX = 0;
                                    Main.tile[i, k].TileFrameY = 110;
                                }
                                break;
                            case 2:
                                if (treeFrame == 0)
                                {
                                    Main.tile[i, k].TileFrameX = 22;
                                    Main.tile[i, k].TileFrameY = 0;
                                }
                                if (treeFrame == 1)
                                {
                                    Main.tile[i, k].TileFrameX = 22;
                                    Main.tile[i, k].TileFrameY = 22;
                                }
                                if (treeFrame == 2)
                                {
                                    Main.tile[i, k].TileFrameX = 22;
                                    Main.tile[i, k].TileFrameY = 44;
                                }
                                break;
                            case 3:
                                if (treeFrame == 0)
                                {
                                    Main.tile[i, k].TileFrameX = 44;
                                    Main.tile[i, k].TileFrameY = 66;
                                }
                                if (treeFrame == 1)
                                {
                                    Main.tile[i, k].TileFrameX = 44;
                                    Main.tile[i, k].TileFrameY = 88;
                                }
                                if (treeFrame == 2)
                                {
                                    Main.tile[i, k].TileFrameX = 44;
                                    Main.tile[i, k].TileFrameY = 110;
                                }
                                break;
                            case 4:
                                if (treeFrame == 0)
                                {
                                    Main.tile[i, k].TileFrameX = 22;
                                    Main.tile[i, k].TileFrameY = 66;
                                }
                                if (treeFrame == 1)
                                {
                                    Main.tile[i, k].TileFrameX = 22;
                                    Main.tile[i, k].TileFrameY = 88;
                                }
                                if (treeFrame == 2)
                                {
                                    Main.tile[i, k].TileFrameX = 22;
                                    Main.tile[i, k].TileFrameY = 110;
                                }
                                break;
                            case 5:
                                if (treeFrame == 0)
                                {
                                    Main.tile[i, k].TileFrameX = 88;
                                    Main.tile[i, k].TileFrameY = 0;
                                }
                                if (treeFrame == 1)
                                {
                                    Main.tile[i, k].TileFrameX = 88;
                                    Main.tile[i, k].TileFrameY = 22;
                                }
                                if (treeFrame == 2)
                                {
                                    Main.tile[i, k].TileFrameX = 88;
                                    Main.tile[i, k].TileFrameY = 44;
                                }
                                break;
                            case 6:
                                if (treeFrame == 0)
                                {
                                    Main.tile[i, k].TileFrameX = 66;
                                    Main.tile[i, k].TileFrameY = 66;
                                }
                                if (treeFrame == 1)
                                {
                                    Main.tile[i, k].TileFrameX = 66;
                                    Main.tile[i, k].TileFrameY = 88;
                                }
                                if (treeFrame == 2)
                                {
                                    Main.tile[i, k].TileFrameX = 66;
                                    Main.tile[i, k].TileFrameY = 110;
                                }
                                break;
                            case 7:
                                if (treeFrame == 0)
                                {
                                    Main.tile[i, k].TileFrameX = 110;
                                    Main.tile[i, k].TileFrameY = 66;
                                }
                                if (treeFrame == 1)
                                {
                                    Main.tile[i, k].TileFrameX = 110;
                                    Main.tile[i, k].TileFrameY = 88;
                                }
                                if (treeFrame == 2)
                                {
                                    Main.tile[i, k].TileFrameX = 110;
                                    Main.tile[i, k].TileFrameY = 110;
                                }
                                break;
                            default:
                                if (treeFrame == 0)
                                {
                                    Main.tile[i, k].TileFrameX = 0;
                                    Main.tile[i, k].TileFrameY = 0;
                                }
                                if (treeFrame == 1)
                                {
                                    Main.tile[i, k].TileFrameX = 0;
                                    Main.tile[i, k].TileFrameY = 22;
                                }
                                if (treeFrame == 2)
                                {
                                    Main.tile[i, k].TileFrameX = 0;
                                    Main.tile[i, k].TileFrameY = 44;
                                }
                                break;
                        }

                        if (randomFrame == 5 || randomFrame == 7)
                        {
                            Main.tile[i - 1, k].TileType = TileID.Trees;
                            Main.tile[i - 1, k].Get<TileWallWireStateData>().HasTile = true;
                            Main.tile[i - 1, k].UseBlockColors(cache);
                            treeFrame = WorldGen.genRand.Next(3);
                            if (WorldGen.genRand.Next(3) < 2 && !remixUnderground)
                            {
                                if (treeFrame == 0)
                                {
                                    Main.tile[i - 1, k].TileFrameX = 44;
                                    Main.tile[i - 1, k].TileFrameY = 198;
                                }

                                if (treeFrame == 1)
                                {
                                    Main.tile[i - 1, k].TileFrameX = 44;
                                    Main.tile[i - 1, k].TileFrameY = 220;
                                }

                                if (treeFrame == 2)
                                {
                                    Main.tile[i - 1, k].TileFrameX = 44;
                                    Main.tile[i - 1, k].TileFrameY = 242;
                                }
                            }
                            else
                            {
                                if (treeFrame == 0)
                                {
                                    Main.tile[i - 1, k].TileFrameX = 66;
                                    Main.tile[i - 1, k].TileFrameY = 0;
                                }

                                if (treeFrame == 1)
                                {
                                    Main.tile[i - 1, k].TileFrameX = 66;
                                    Main.tile[i - 1, k].TileFrameY = 22;
                                }

                                if (treeFrame == 2)
                                {
                                    Main.tile[i - 1, k].TileFrameX = 66;
                                    Main.tile[i - 1, k].TileFrameY = 44;
                                }
                            }
                        }

                        if (randomFrame != 6 && randomFrame != 7)
                            continue;

                        Main.tile[i + 1, k].TileType = TileID.Trees;
                        Main.tile[i + 1, k].Get<TileWallWireStateData>().HasTile = true;
                        Main.tile[i + 1, k].UseBlockColors(cache);
                        treeFrame = WorldGen.genRand.Next(3);
                        if (WorldGen.genRand.Next(3) < 2 && !remixUnderground)
                        {
                            if (treeFrame == 0)
                            {
                                Main.tile[i + 1, k].TileFrameX = 66;
                                Main.tile[i + 1, k].TileFrameY = 198;
                            }

                            if (treeFrame == 1)
                            {
                                Main.tile[i + 1, k].TileFrameX = 66;
                                Main.tile[i + 1, k].TileFrameY = 220;
                            }

                            if (treeFrame == 2)
                            {
                                Main.tile[i + 1, k].TileFrameX = 66;
                                Main.tile[i + 1, k].TileFrameY = 242;
                            }
                        }
                        else
                        {
                            if (treeFrame == 0)
                            {
                                Main.tile[i + 1, k].TileFrameX = 88;
                                Main.tile[i + 1, k].TileFrameY = 66;
                            }

                            if (treeFrame == 1)
                            {
                                Main.tile[i + 1, k].TileFrameX = 88;
                                Main.tile[i + 1, k].TileFrameY = 88;
                            }

                            if (treeFrame == 2)
                            {
                                Main.tile[i + 1, k].TileFrameX = 88;
                                Main.tile[i + 1, k].TileFrameY = 110;
                            }
                        }
                    }

                    int rootType = WorldGen.genRand.Next(3);
                    bool flag5 = false;
                    bool flag6 = false;
                    if (!Main.tile[i - 1, j].IsHalfBlock && Main.tile[i - 1, j].Slope == 0)
                        flag5 = true;

                    if (!Main.tile[i + 1, j].IsHalfBlock && Main.tile[i + 1, j].Slope == 0)
                        flag6 = true;

                    if (!flag5)
                    {
                        if (rootType == 0)
                            rootType = 2;

                        if (rootType == 1)
                            rootType = 3;
                    }

                    if (!flag6)
                    {
                        if (rootType == 0)
                            rootType = 1;

                        if (rootType == 2)
                            rootType = 3;
                    }

                    if (flag5 && !flag6)
                        rootType = 2;

                    if (flag6 && !flag5)
                        rootType = 1;

                    if (rootType == 0 || rootType == 1)
                    {
                        Main.tile[i + 1, j - 1].TileType = TileID.Trees;
                        Main.tile[i + 1, j - 1].Get<TileWallWireStateData>().HasTile = true;
                        Main.tile[i + 1, j - 1].UseBlockColors(cache);
                        treeFrame = WorldGen.genRand.Next(3);
                        if (treeFrame == 0)
                        {
                            Main.tile[i + 1, j - 1].TileFrameX = 22;
                            Main.tile[i + 1, j - 1].TileFrameY = 132;
                        }

                        if (treeFrame == 1)
                        {
                            Main.tile[i + 1, j - 1].TileFrameX = 22;
                            Main.tile[i + 1, j - 1].TileFrameY = 154;
                        }

                        if (treeFrame == 2)
                        {
                            Main.tile[i + 1, j - 1].TileFrameX = 22;
                            Main.tile[i + 1, j - 1].TileFrameY = 176;
                        }
                    }

                    if (rootType == 0 || rootType == 2)
                    {
                        Main.tile[i - 1, j - 1].TileType = TileID.Trees;
                        Main.tile[i - 1, j - 1].Get<TileWallWireStateData>().HasTile = true;
                        Main.tile[i - 1, j - 1].UseBlockColors(cache);
                        treeFrame = WorldGen.genRand.Next(3);
                        if (treeFrame == 0)
                        {
                            Main.tile[i - 1, j - 1].TileFrameX = 44;
                            Main.tile[i - 1, j - 1].TileFrameY = 132;
                        }

                        if (treeFrame == 1)
                        {
                            Main.tile[i - 1, j - 1].TileFrameX = 44;
                            Main.tile[i - 1, j - 1].TileFrameY = 154;
                        }

                        if (treeFrame == 2)
                        {
                            Main.tile[i - 1, j - 1].TileFrameX = 44;
                            Main.tile[i - 1, j - 1].TileFrameY = 176;
                        }
                    }

                    treeFrame = WorldGen.genRand.Next(3);
                    switch (rootType)
                    {
                        case 0:
                            if (treeFrame == 0)
                            {
                                Main.tile[i, j - 1].TileFrameX = 88;
                                Main.tile[i, j - 1].TileFrameY = 132;
                            }
                            if (treeFrame == 1)
                            {
                                Main.tile[i, j - 1].TileFrameX = 88;
                                Main.tile[i, j - 1].TileFrameY = 154;
                            }
                            if (treeFrame == 2)
                            {
                                Main.tile[i, j - 1].TileFrameX = 88;
                                Main.tile[i, j - 1].TileFrameY = 176;
                            }
                            break;
                        case 1:
                            if (treeFrame == 0)
                            {
                                Main.tile[i, j - 1].TileFrameX = 0;
                                Main.tile[i, j - 1].TileFrameY = 132;
                            }
                            if (treeFrame == 1)
                            {
                                Main.tile[i, j - 1].TileFrameX = 0;
                                Main.tile[i, j - 1].TileFrameY = 154;
                            }
                            if (treeFrame == 2)
                            {
                                Main.tile[i, j - 1].TileFrameX = 0;
                                Main.tile[i, j - 1].TileFrameY = 176;
                            }
                            break;
                        case 2:
                            if (treeFrame == 0)
                            {
                                Main.tile[i, j - 1].TileFrameX = 66;
                                Main.tile[i, j - 1].TileFrameY = 132;
                            }
                            if (treeFrame == 1)
                            {
                                Main.tile[i, j - 1].TileFrameX = 66;
                                Main.tile[i, j - 1].TileFrameY = 154;
                            }
                            if (treeFrame == 2)
                            {
                                Main.tile[i, j - 1].TileFrameX = 66;
                                Main.tile[i, j - 1].TileFrameY = 176;
                            }
                            break;
                    }

                    if (!WorldGen.genRand.NextBool(13) && !remixUnderground)
                    {
                        treeFrame = WorldGen.genRand.Next(3);
                        if (treeFrame == 0)
                        {
                            Main.tile[i, j - treeHeight].TileFrameX = 22;
                            Main.tile[i, j - treeHeight].TileFrameY = 198;
                        }

                        if (treeFrame == 1)
                        {
                            Main.tile[i, j - treeHeight].TileFrameX = 22;
                            Main.tile[i, j - treeHeight].TileFrameY = 220;
                        }

                        if (treeFrame == 2)
                        {
                            Main.tile[i, j - treeHeight].TileFrameX = 22;
                            Main.tile[i, j - treeHeight].TileFrameY = 242;
                        }
                    }
                    else
                    {
                        treeFrame = WorldGen.genRand.Next(3);
                        if (treeFrame == 0)
                        {
                            Main.tile[i, j - treeHeight].TileFrameX = 0;
                            Main.tile[i, j - treeHeight].TileFrameY = 198;
                        }

                        if (treeFrame == 1)
                        {
                            Main.tile[i, j - treeHeight].TileFrameX = 0;
                            Main.tile[i, j - treeHeight].TileFrameY = 220;
                        }

                        if (treeFrame == 2)
                        {
                            Main.tile[i, j - treeHeight].TileFrameX = 0;
                            Main.tile[i, j - treeHeight].TileFrameY = 242;
                        }
                    }

                    WorldGen.RangeFrame(i - 2, j - treeHeight - 1, i + 2, j + 1);
                    if (Main.netMode == 2)
                        NetMessage.SendTileSquare(-1, i - 1, j - treeHeight, 3, treeHeight);

                    return true;
                }
            }

            return false;
        }
        public static void MakeTag(ref TagCompound savedWorldData, string key, int status)
        {
            savedWorldData[key] = status;
        }
        public static void MakeTag(ref TagCompound savedWorldData, string key, bool status)
        {
            if (status)
            {
                savedWorldData[key] = status;
            }
        }

        /// <summary>
        /// Saves difficulties and Fanny togglage across subworlds
        /// The returned TagComound has the name "RemixCommonBools" MAKE SURE to insert "SubworldSystem.CopyWorldData("RemixCommonBools_<WorldName>", savedWorldData);" after all further data saving!
        /// </summary>
        /// <returns></returns>
        public static TagCompound SaveCommonSubworldBools()
        {
            TagCompound savedWorldData = [];
            MakeTag(ref savedWorldData, "RevengeanceMode", CalamityWorld.revenge);
            MakeTag(ref savedWorldData, "DeathMode", CalamityWorld.death);
            MakeTag(ref savedWorldData, "Fanny", ScreenHelperManager.screenHelpersEnabled);
            /*foreach (Anomaly109Option option in Anomaly109Manager.options)
            {
                MakeTag(ref savedWorldData, "Anomaly" + option.key, option.check.Invoke());
            }*/
            return savedWorldData;
        }

        /// <summary>
        /// Loads difficulties and Fanny togglage across subworlds
        /// </summary>
        /// <returns></returns>
        public static TagCompound LoadCommonSubworldBools(string world)
        {
            TagCompound savedWorldData = SubworldSystem.ReadCopiedWorldData<TagCompound>("RemixCommonBools_" + world);
            CalamityWorld.revenge = savedWorldData.GetBool("RevengeanceMode");
            CalamityWorld.death = savedWorldData.GetBool("DeathMode");
            ScreenHelperManager.screenHelpersEnabled = savedWorldData.GetBool("Fanny");
            /*for (int i = 0; i < Anomaly109Manager.options.Count; i++)
            {
                Anomaly109Option option = Anomaly109Manager.options[i];
                if (option.check.Invoke() != savedWorldData.GetBool("Anomaly" + option.key))
                    option.toggle();
            }*/
            return savedWorldData;
        }

        /// <summary>
        /// Quickly returns SpriteEffects based on the Entity's direction. Defaults to flipping if the entity faces left (-1)
        /// </summary>
        /// <param name="proj">The projectile</param>
        /// <param name="reverse">Flip when facing right instead</param>
        /// <returns></returns>
        public static SpriteEffects FlippedEffects(this Projectile proj, bool reverse = false)
        {
            if ((proj.spriteDirection == -1 && !reverse) || (reverse && proj.spriteDirection == 1))
            {
                return SpriteEffects.None;
            }
            return SpriteEffects.FlipHorizontally;
        }

        /// <summary>
        /// Quickly returns SpriteEffects based on the Entity's direction. Defaults to flipping if the entity faces left (-1)
        /// </summary>
        /// <param name="npc">The NPC</param>
        /// <param name="reverse">Flip when facing right instead</param>
        /// <returns></returns>
        public static SpriteEffects FlippedEffects(this NPC npc, bool reverse = false)
        {
            if ((npc.spriteDirection == -1 && !reverse) || (reverse && npc.spriteDirection == 1))
            {
                return SpriteEffects.None;
            }
            return SpriteEffects.FlipHorizontally;
        }
    }

    public static class RarityHelper
    {
        public const int Origen = 1;
        public const int WulfrumExcavator = 1;
        public const int KingSlime = 1;
        public const int DesertScourge = 1;
        public const int EyeofCthulhu = 1;
        public const int Crabulon = 2;
        public const int EaterofWorlds = 2;
        public const int BrainofCthulhu = 2;
        public const int HiveMind = 3;
        public const int Perforators = 3;
        public const int QueenBee = 3;
        public const int Deerclops = 3;
        public const int Skeletron = 3;
        public const int Carcinogen = 3;
        public const int SlimeGod = 4;
        public const int WallofFlesh = 4;
        public const int Hardmode = 4;
        public const int Cryogen = 5;
        public const int AquaticScourge = 5;
        public const int BrimstoneElemental = 5;
        public const int Mechs = 5;
        public const int Twins = 5;
        public const int SkeletronPrime = 5;
        public const int Destroyer = 5;
        public const int CalamitasClone = 5;
        public const int Ionogen = 5;
        public const int Plantera = 6;
        public const int Leviathan = 7;
        public const int Oxygen = 7;
        public const int AstrumAureus = 7;
        public const int Golem = 8;
        public const int Phytogen = 8;
        public const int EmpressofLight = 8;
        public const int PlaguebringerGoliath = 8;
        public const int DukeFishron = 8;
        public const int Hydrogen = 8;
        public const int Ravager = 8;
        public const int Pathogen = 8;
        public const int LunaticCultist = 9;
        public const int AstrumDeus = 9;
        public const int SealedMain = 9;
        public const int Void = 10;
        public const int Disilphia = 10;
        public const int GastropodA = 10;
        public const int Oneguy = 10;
        public const int MoonLord = 10;
        public const int Crevi = 11;
    }

    public static class BetterSoundID
    {
        public static SoundStyle ItemSwing => SoundID.Item1;
        public static SoundStyle ItemEat => SoundID.Item2;
        public static SoundStyle ItemDrink => SoundID.Item3;
        public static SoundStyle ItemLifeCrystal => SoundID.Item4;
        public static SoundStyle ItemBow => SoundID.Item5;
        public static SoundStyle ItemTeleportMirror => SoundID.Item6;
        public static SoundStyle ItemCast => SoundID.Item8;
        public static SoundStyle ItemMagicStar => SoundID.Item9;
        public static SoundStyle ItemWeakLaunch => SoundID.Item10;
        public static SoundStyle ItemBasicGun => SoundID.Item11;
        public static SoundStyle ItemSpaceLaser => SoundID.Item12;
        public static SoundStyle ItemMagicStream => SoundID.Item13;
        public static SoundStyle ItemExplosion => SoundID.Item14;
        public static SoundStyle ItemPhaseblade => SoundID.Item15;
        public static SoundStyle ItemFart => SoundID.Item16;
        public static SoundStyle ItemSpit => SoundID.Item17;
        public static SoundStyle ItemSpit2 => SoundID.Item18;
        public static SoundStyle ItemSpit3 => SoundID.Item19;
        public static SoundStyle ItemFireball => SoundID.Item20;
        public static SoundStyle ItemWaterBolt => SoundID.Item21;
        public static SoundStyle ItemDrill => SoundID.Item22;
        public static SoundStyle ItemDrill2 => SoundID.Item23;
        public static SoundStyle ItemRocketHover => SoundID.Item24;
        public static SoundStyle ItemMagicMount => SoundID.Item25;
        public static SoundStyle ItemHarp => SoundID.Item26;
        public static SoundStyle ItemIceBreak => SoundID.Item27;
        public static SoundStyle ItemRainbowRodIce => SoundID.Item28;
        public static SoundStyle ItemManaCrystal => SoundID.Item29;
        public static SoundStyle ItemMagicIceBlock => SoundID.Item30;
        public static SoundStyle ItemClockworkGun => SoundID.Item31;
        public static SoundStyle ItemWingFlap => SoundID.Item32;
        public static SoundStyle ItemThisStupidFuckingLaser => SoundID.Item33;
        public static SoundStyle ItemFlamethrower => SoundID.Item34;
        public static SoundStyle ItemBell => SoundID.Item35;
        public static SoundStyle ItemShotgun => SoundID.Item36;
        public static SoundStyle ItemReforge => SoundID.Item37;
        public static SoundStyle ItemExplosiveShotgun => SoundID.Item38;
        public static SoundStyle ItemFlingRazorpine => SoundID.Item39;
        public static SoundStyle ItemSniperGun => SoundID.Item40;
        public static SoundStyle ItemMachineGun => SoundID.Item41;
        public static SoundStyle ItemMissileFireSqueak => SoundID.Item42;
        public static SoundStyle ItemMagicStaff => SoundID.Item43;
        public static SoundStyle ItemSummonWeapon => SoundID.Item44;
        public static SoundStyle ItemFireballImpact => SoundID.Item45;
        public static SoundStyle ItemSentrySummon => SoundID.Item46;
        public static SoundStyle ItemAxeGuitar => SoundID.Item47;
        public static SoundStyle ItemSnowHit => SoundID.Item48;
        public static SoundStyle ItemSnowHit2 => SoundID.Item49;
        public static SoundStyle ItemIceHit => SoundID.Item50;
        public static SoundStyle ItemSnowballHit => SoundID.Item51;
        public static SoundStyle ItemMinecartHit => SoundID.Item52;
        public static SoundStyle ItemMinecartCling => SoundID.Item53;
        public static SoundStyle ItemBubblePop => SoundID.Item54;
        public static SoundStyle ItemMinecartSlowdown => SoundID.Item55;
        public static SoundStyle ItemMinecartBounce => SoundID.Item56;
        public static SoundStyle ItemMeowBounce => SoundID.Item57;
        public static SoundStyle ItemMeowBounce2 => SoundID.Item58;
        public static SoundStyle ItemPigOink => SoundID.Item59;
        public static SoundStyle ItemTerraBeam => SoundID.Item60;
        public static SoundStyle ItemGrenadeChuck => SoundID.Item61;
        public static SoundStyle ItemGrenadeExplosion => SoundID.Item62;
        public static SoundStyle ItemBlowpipe => SoundID.Item63;
        public static SoundStyle ItemBlowgunGrandDesign => SoundID.Item64;
        public static SoundStyle ItemBlowReload => SoundID.Item65;
        public static SoundStyle ItemNimbusRain => SoundID.Item66;
        public static SoundStyle ItemRainbowGun => SoundID.Item67;
        public static SoundStyle ItemRainbowGun2 => SoundID.Item68;
        public static SoundStyle ItemStaffofEarth => SoundID.Item69;
        public static SoundStyle ItemBoulderImpact => SoundID.Item70;
        public static SoundStyle ItemDeathSickle => SoundID.Item71;
        public static SoundStyle ItemShadowbeamStaff => SoundID.Item72;
        public static SoundStyle ItemInfernoFork => SoundID.Item73;
        public static SoundStyle ItemInfernoExplosion => SoundID.Item74;
        public static SoundStyle ItemPulseBowLaser => SoundID.Item75;
        public static SoundStyle ItemHornetSummon => SoundID.Item76;
        public static SoundStyle ItemImpSummon => SoundID.Item77;
        public static SoundStyle ItemSentrySummonStrong => SoundID.Item78;
        public static SoundStyle ItemBunnyMountSummon => SoundID.Item79;
        public static SoundStyle ItemTruffleMountSummon => SoundID.Item80;
        public static SoundStyle ItemSlimeMountSummon => SoundID.Item81;
        public static SoundStyle ItemOpticStaffSummon => SoundID.Item82;
        public static SoundStyle ItemSpiderStaffSummon => SoundID.Item83;
        public static SoundStyle ItemRazorbladeTyphoon => SoundID.Item84;
        public static SoundStyle ItemBubbleGun => SoundID.Item85;
        public static SoundStyle ItemBubbleGun2 => SoundID.Item86;
        public static SoundStyle ItemBubbleGun3 => SoundID.Item87;
        public static SoundStyle ItemMeteorStaffLunarFlare => SoundID.Item88;
        public static SoundStyle ItemMeteorImpact => SoundID.Item89;
        public static SoundStyle ItemBrainScrambler => SoundID.Item90;
        public static SoundStyle ItemLaserMachinegun => SoundID.Item91;
        public static SoundStyle ItemElectrospherePetSummon => SoundID.Item92;
        public static SoundStyle ItemElectricFizzleExplosion => SoundID.Item93;
        public static SoundStyle ItemElectricMineSettle => SoundID.Item94;
        public static SoundStyle ItemXenopopper => SoundID.Item95;
        public static SoundStyle ItemXenopopperPop => SoundID.Item96;
        public static SoundStyle ItemBeesKnees => SoundID.Item97;
        public static SoundStyle ItemDartPistol => SoundID.Item98;
        public static SoundStyle ItemDartRifle => SoundID.Item99;
        public static SoundStyle ItemClingerStaff => SoundID.Item100;
        public static SoundStyle ItemCrystalVileShard => SoundID.Item101;
        public static SoundStyle ItemAerialBane => SoundID.Item102;
        public static SoundStyle ItemShadowflameHexDoll => SoundID.Item103;
        public static SoundStyle ItemShadowflameHexDoll2 => SoundID.Item104;
        public static SoundStyle ItemStarWrath => SoundID.Item105;
        public static SoundStyle ItemToxicFlaskThrow => SoundID.Item106;
        public static SoundStyle ItemToxicFlaskImpact => SoundID.Item107;
        public static SoundStyle ItemNailGun => SoundID.Item108;
        public static SoundStyle ItemCrystalSerpent => SoundID.Item109;
        public static SoundStyle ItemCrystalSerpentImpact => SoundID.Item110;
        public static SoundStyle ItemToxikarp => SoundID.Item111;
        public static SoundStyle ItemToxikarp2 => SoundID.Item112;
        public static SoundStyle ItemDeadlySphereVroom => SoundID.Item113;
        public static SoundStyle ItemPortalGun => SoundID.Item114;
        public static SoundStyle ItemPortalGun2 => SoundID.Item115;
        public static SoundStyle ItemSolarEruption => SoundID.Item116;
        public static SoundStyle ItemNebulaArcanum => SoundID.Item117;
        public static SoundStyle ItemCrystalChargeImpact => SoundID.Item118;
        public static SoundStyle ItemPhantasmDragon => SoundID.Item119;
        public static SoundStyle ItemIceMistCultist => SoundID.Item120;
        public static SoundStyle ItemLightningOrbCultist => SoundID.Item121;
        public static SoundStyle ItemCultistUnused => SoundID.Item122;
        public static SoundStyle ItemLightningRitualCultist => SoundID.Item123;
        public static SoundStyle ItemPhantasmalBolt => SoundID.Item124;
        public static SoundStyle ItemPhantasmalBolt2 => SoundID.Item125;
        public static SoundStyle ItemGolfClubSwing => SoundID.Item126;
        public static SoundStyle ItemCrackedDungeonBricks => SoundID.Item127;
        public static SoundStyle ItemGolfWhistle => SoundID.Item128;
        public static SoundStyle ItemGolfWin => SoundID.Item129;
        public static SoundStyle ItemVoidBag => SoundID.Item130;
        public static SoundStyle ItemVoidSuckThingUnusedIdk => SoundID.Item131;
        public static SoundStyle ItemLaserDrill => SoundID.Item132;
        public static SoundStyle ItemStringInstrument => SoundID.Item133;
        public static SoundStyle ItemStringInstrument2 => SoundID.Item134;
        public static SoundStyle ItemStringInstrument3 => SoundID.Item135;
        public static SoundStyle ItemStringInstrument4 => SoundID.Item136;
        public static SoundStyle ItemStringInstrument5 => SoundID.Item137;
        public static SoundStyle ItemStringInstrument6 => SoundID.Item138;
        public static SoundStyle ItemDrumInstrument => SoundID.Item139;
        public static SoundStyle ItemDrumInstrument2 => SoundID.Item140;
        public static SoundStyle ItemDrumInstrument3 => SoundID.Item141;
        public static SoundStyle ItemDrumInstrument4 => SoundID.Item142;
        public static SoundStyle ItemDrumInstrument5 => SoundID.Item143;
        public static SoundStyle ItemDrumInstrument6 => SoundID.Item144;
        public static SoundStyle ItemDrumInstrument7 => SoundID.Item145;
        public static SoundStyle ItemDrumInstrument8 => SoundID.Item146;
        public static SoundStyle ItemDrumInstrument9 => SoundID.Item147;
        public static SoundStyle ItemDrumInstrument10 => SoundID.Item148;
        public static SoundStyle ItemAmmoBox => SoundID.Item149;
        public static SoundStyle ItemBoingReflectProjectile => SoundID.Item150;
        public static SoundStyle ItemSnakeCharmersFluteHiss => SoundID.Item151;
        public static SoundStyle ItemWhipSwing => SoundID.Item152;
        public static SoundStyle ItemWhipLash => SoundID.Item153;
        public static SoundStyle ItemQueenSlimeWobble => SoundID.Item154;
        public static SoundStyle ItemQueenSlimeProjectileShoot => SoundID.Item155;
        public static SoundStyle ItemCelebration => SoundID.Item156;
        public static SoundStyle ItemSpaceGun => SoundID.Item157;
        public static SoundStyle ItemZapinator => SoundID.Item158;
        public static SoundStyle ItemEmpressofLightSunDance => SoundID.Item159;
        public static SoundStyle ItemEmpressofLightDash => SoundID.Item160;
        public static SoundStyle ItemEmpressofLightPhase2 => SoundID.Item161;
        public static SoundStyle ItemEmpressofLightEtherealLances => SoundID.Item162;
        public static SoundStyle ItemEmpressofLightEverlastingRainbow => SoundID.Item163;
        public static SoundStyle ItemEmpressofLightPrismaticBolts => SoundID.Item164;
        public static SoundStyle ItemEmpressofLightBoltWoosh => SoundID.Item165;
        public static SoundStyle ItemMusicBoxRecord => SoundID.Item166;
        public static SoundStyle ItemQueenSlimeSlam => SoundID.Item167;
        public static SoundStyle ItemPogoStick => SoundID.Item168;
        public static SoundStyle ItemZenith => SoundID.Item169;
        public static SoundStyle ItemDreadnautBubbling => SoundID.Item170;
        public static SoundStyle ItemDreadnautSpit => SoundID.Item171;
        public static SoundStyle ItemDreadnautCharge => SoundID.Item172;
        public static SoundStyle ItemQueenBeeScree => SoundID.Item173;
        public static SoundStyle ItemKOCannon => SoundID.Item174;
        public static SoundStyle ItemSlapHandSmack => SoundID.Item175;
        public static SoundStyle ItemShimmered => SoundID.Item176;
        public static SoundStyle ItemPoopSquish => SoundID.Item177;
        public static SoundStyle ItemWafflesIronTink => SoundID.Item178;

    }
}
