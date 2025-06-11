using CalamityMod;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Tiles;
using CalRemix.Content.Walls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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

namespace CalRemix
{
    public static class CalRemixHelper
    {
        public static CalRemixItem Remix(this Item item) => item.GetGlobalItem<CalRemixItem>();
        public static CalRemixNPC Remix(this NPC npc) => npc.GetGlobalNPC<CalRemixNPC>();
        public static CalRemixPlayer Remix(this Player player) => player.GetModPlayer<CalRemixPlayer>();
        public static CalRemixProjectile Remix(this Projectile projectile) => projectile.GetGlobalProjectile<CalRemixProjectile>();
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
        /// Generates a rectangle of tiles and/or walls using noise
        /// </summary>
        /// <param name="area">The rectangle</param>
        /// <param name="noiseThreshold">How open should caves be? Scales between 0f and 1f</param>
        /// <param name="noiseStrength">How strong the noise is. Weaker values look more like noodles</param>
        /// <param name="noiseSize">The zoom of the noise. Higher values means more zoomed in. Set to 120, 180 by default, the same as the Baron Strait</param>
        /// <param name="tileType">The tile to place</param>
        /// <param name="wallType">The wall to place</param>
        public static void PerlinGeneration(Rectangle area, float noiseThreshold = 0.56f, float noiseStrength = 0.1f, Vector2 noiseSize = default, int tileType = -1, int wallType = 0)
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
                    map[i, j] = MathHelper.Distance(noise, noiseThreshold) < noiseStrength;
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
                        if (t.HasTile)
                        {
                            continue;
                        }
                        // Cell stuff
                        int sur = SurroundingTileCounts(map, i, j, area.Width, area.Height);

                        // If there are more than 4 nearby nodes, place some
                        if (sur > 4 && (tileType != -1 || wallType != 0))
                        {
                            if (tileType != -1)
                            {
                                t.ResetToType((ushort)tileType);
                                WorldGen.SquareTileFrame(area.X + i, area.Y + j);
                            }
                            if (wallType != 0)
                            {
                                t.WallType = (ushort)wallType;
                                WorldGen.SquareWallFrame(area.X + i, area.Y + j);
                            }
                            map[i, j] = true;
                        }
                        // If there are less than 4 nodes nearby, remove
                        else if (sur < 4)
                        {
                            t.ClearEverything();
                            map[i, j] = false;
                        }
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
        public const int MoonLord = 10;
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
