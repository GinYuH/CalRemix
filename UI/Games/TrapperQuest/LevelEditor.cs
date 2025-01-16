using CalRemix.UI.Games.Boi.BaseClasses;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader.Core;
using static CalRemix.UI.Games.TrapperQuest.TQHandler;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class LevelEditor
    {
        /// <summary>
        /// The current type to add for the addition tool
        /// </summary>
        public static GameEntity currentType;

        /// <summary>
        /// The current selected entity for the selection tool
        /// </summary>
        public static GameEntity selectedEntity;

        /// <summary>
        /// All types available to spawn in the spawn window
        /// </summary>
        public static Dictionary<int, GameEntity> types = new Dictionary<int, GameEntity>();

        /// <summary>
        /// All available options in the tool window
        /// </summary>
        public static List<LevelEditorOption> options = new List<LevelEditorOption>();

        /// <summary>
        /// The path to export room data to
        /// </summary>
        public static string exportPath = Path.Combine(Main.SavePath + "/Data Dumps");

        /// <summary>
        /// The file to export room data to
        /// </summary>
        public static string path = $@"{exportPath}\CurLevel.txt";

        /// <summary>
        /// Old mouse scroll value
        /// </summary>
        public static float scrollOld = 0;

        /// <summary>
        /// Current mouse scroll value
        /// </summary>
        public static float scrollNew = 0;

        /// <summary>
        /// Should the grid be shown
        /// </summary>
        public static bool ShowGrid = false;

        /// <summary>
        /// Should the cursor hitbox be shown
        /// </summary>
        public static bool ShowCursor = false;

        /// <summary>
        /// Should the player hitbox be shown
        /// </summary>
        public static bool ShowHitbox = false;

        /// <summary>
        /// Should doors be made visible
        /// </summary>
        public static bool ShowDoors = false;

        /// <summary>
        /// Which left window is currently being looked at
        /// All other windows will have their options locked
        /// </summary>
        public static int CurrentMenu = 0;

        /// <summary>
        /// What does clicking the level do?
        /// 0 = Select
        /// 1 = Add
        /// 2 = Delete
        /// </summary>
        public static int CursorMode = 0;

        /// <summary>
        /// Loads all game entities for use in the level editor
        /// </summary>
        public static void LoadTypes()
        {
            Type[] Ctypes = AssemblyManager.GetLoadableTypes(CalRemix.instance.Code);
            foreach (Type type in Ctypes)
            {
                // This check is probably worthless with the next check but I don't feel like testing if removing this will break anything so I'm just leaving this here
                if (!type.IsSubclassOf(typeof(GameEntity)) || type.IsSubclassOf(typeof(BoiEntity)) || type.IsAbstract)
                    continue;

                GameEntity entity = Activator.CreateInstance(type) as GameEntity;
                // Only add valid entities that are marked as ICreative
                if (entity is ICreative create)
                {
                    if (create.Name == "")
                        continue;
                    if (create.ID == -1)
                        continue;

                    types.Add(create.ID, entity);
                }
            }
        }

        /// <summary>
        /// Loads all custom level editor options
        /// </summary>
        public static void LoadOptions()
        {
            Type[] Ctypes = AssemblyManager.GetLoadableTypes(CalRemix.instance.Code);
            foreach (Type type in Ctypes)
            {
                // This check is probably worthless with the next check but I don't feel like testing if removing this will break anything so I'm just leaving this here
                if (!type.IsSubclassOf(typeof(LevelEditorOption)) || type.IsAbstract)
                    continue;

                LevelEditorOption entity = Activator.CreateInstance(type) as LevelEditorOption;
                options.Add(entity);                
            }
        }

        public static void Run()
        {
            Vector2 moused = ConvertToTileCords(Mouse.Location.ToVector2());
            // check if the mouse is inside of the UI
            if (moused.X >= 0 && moused.X < RoomWidth && moused.Y >= 0 && moused.Y < RoomHeight)
            {
                if (Main.mouseLeft)
                {
                    // select entities
                    if (CursorMode == 0)
                    {
                        // place an entity if no other non-floor entity is there. if the entity is a floor allow placing unless it's another floor
                        if (player.RoomImIn.Entities.Any((GameEntity g) => (ConvertToTileCords(g.Position) == moused && g is not TrapperPlayer && g is not TQFloor)))
                        {
                            selectedEntity = player.RoomImIn.Entities.Find((GameEntity g) => (ConvertToTileCords(g.Position) == moused && g is not TrapperPlayer && g is not TQFloor));
                        }
                    }
                    // place entities
                    else if (CursorMode == 1 && currentType != null)
                    {
                        GameEntity newE = Activator.CreateInstance(currentType.GetType()) as GameEntity;

                        // if the entity is a tile and no tile exists at the current coordinate, place the tile
                        if ((!player.RoomImIn.Tiles.ContainsKey(((int)moused.X, (int)moused.Y)) && newE is ITile))
                        {
                            newE.Position = moused;
                            player.RoomImIn.Tiles.Add(((int)moused.X, (int)moused.Y), newE as TQRock);
                        }
                        // place an entity if no other non-floor entity is there. if the entity is a floor allow placing unless it's another floor
                        if (!player.RoomImIn.Entities.Any((GameEntity g) => (ConvertToTileCords(g.Position) == moused && g is not TQFloor) || (ConvertToTileCords(g.Position) == moused && g is TQFloor && newE is TQFloor)))
                        {
                            newE.Position = ConvertToScreenCords(moused);
                            player.RoomImIn.Entities.Add(newE);
                        }
                    }
                    // kill entities
                    else if (CursorMode == 2)
                    {
                        // kill any tiles at the coordinates
                        if (player.RoomImIn.Tiles.ContainsKey(((int)moused.X, (int)moused.Y)))
                        {
                            int rock = player.RoomImIn.Entities.FindIndex(0, player.RoomImIn.Entities.Count, (GameEntity g) => ConvertToTileCords(g.Position) == moused);
                            if (rock != -1)
                            {
                                player.RoomImIn.Tiles.Remove((((int)moused.X, (int)moused.Y)));
                            }
                        }
                        // kill anything else at the coordinates that isn't the player
                        if (player.RoomImIn.Entities.Any((GameEntity g) => (ConvertToTileCords(g.Position) == moused) && g is not TrapperPlayer))
                        {
                            int idx = player.RoomImIn.Entities.FindIndex((GameEntity g) => ConvertToTileCords(g.Position) == moused);
                            if (idx != -1)
                            {
                                player.RoomImIn.Entities.RemoveAt(idx);
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Exports all level editor-valid entities to a txt file
        /// </summary>
        public static void ExportRoom()
        {
            string ret = ExportText(player.RoomImIn);
            File.WriteAllText(path, ret, Encoding.UTF8);
            Main.NewText("Exported!");            
        }

        /// <summary>
        /// Generates a string of room data
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static string ExportText(TQRoom room)
        {
            string ret = "";
            ret += room.RoomSize.X + "," + room.RoomSize.Y + "," + room.id + "\n";
            foreach (GameEntity g in room.Entities)
            {
                if (g is ICreative creative)
                {
                    if (creative.Name != "")
                    {
                        // Hold shift to automatically format it for code-use. This is NOT compatible with importation
                        bool shift = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);
                        if (shift)
                            ret += "                GameEntity.SpawnFromID(";
                        if (g is TQDoor d)
                            ret += creative.ID + "," + g.Position.X + "," + g.Position.Y + "," + d.roomGoto + "," + d.playerTP.X + "," + d.playerTP.Y + "," + d.fade.ToInt();
                        else
                            ret += creative.ID + "," + g.Position.X + "," + g.Position.Y;
                        if (shift)
                            ret += "),";
                        ret += "\n";
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Import rooms from a text file
        /// </summary>
        public static TQRoom ImportRoom(bool load = true, string text = "", bool clear = true)
        {
            if (clear)
            {
                player.RoomImIn.Entities.RemoveAll((GameEntity g) => g is not TrapperPlayer);
                player.RoomImIn.Tiles.Clear();
            }
            string dump = load ? text : File.ReadAllText(path);
            string[] lines = dump.Split('\n');
            string[] genericData = lines[0].Split(',');

            // Elements 1 and 2 are room size
            Vector2 size = Vector2.Zero;
            // Element 3 is room ID
            int roomID = -1;
            try
            {
                size = new Vector2(int.Parse(genericData[0]), int.Parse(genericData[1]));
            }
            catch
            {
                CalRemix.instance.Logger.Error("Failed to load a Trapper Quest room due to improper coordinates");
            }
            try
            {
                roomID = int.Parse(genericData[2]);
            }
            catch
            {
                CalRemix.instance.Logger.Error("Failed to load a Trapper Quest room due to improper room ID");
            }

            // Create a room
            TQRoom newRoom = new TQRoom(0, 0, roomID, size);

            if (!load)
                player.RoomImIn.RoomSize = size;

            newRoom.RoomSize = size;
            newRoom.id = roomID;

            for (int i = 1; i < lines.Length - 1; i++)
            {
                string c = lines[i];
                string[] elems = c.Split(',');

                // Element 1 is the ID of the entity
                GameEntity spawnType = Activator.CreateInstance(types[(int)char.GetNumericValue(c[0])].GetType()) as GameEntity;

                // Elements 2 and 3 are screen position
                Vector2 pos = new Vector2(int.Parse(elems[1]), int.Parse(elems[2]));
                
                if (spawnType is TQDoor door)
                {
                    door.roomGoto = int.Parse(elems[3]);
                    door.playerTP = new Vector2(int.Parse(elems[4]), int.Parse(elems[5]));
                    door.fade = int.Parse(elems[6]) == 1 ? true : false;
                }

                Vector2 tileCords = ConvertToTileCords(pos);
                if (clear || load)
                {
                    spawnType.Position = pos;
                    newRoom.Entities.Add(spawnType);
                    //TODO: UNCOMMENT THIS LATER
                    //if (spawnType is ITile)
                        //newRoom.Tiles.Add(((int)tileCords.X, (int)tileCords.Y), spawnType as TQRock);
                }
                else if (!player.RoomImIn.Entities.Any((GameEntity g) => ConvertToTileCords(g.Position) == pos))
                {
                    spawnType.Position = pos;
                    player.RoomImIn.Entities.Add(spawnType);
                    if (spawnType is ITile)
                        player.RoomImIn.Tiles.Add(((int)tileCords.X, (int)tileCords.Y), spawnType as TQRock);
                }
            }
            if (!load)
                Main.NewText("Imported!");
            return load ? newRoom : player.RoomImIn;
        }

        public static void DrawUI(SpriteBatch sb)
        {
            Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 10, 10);

            int UIWidth = 300;
            int extraX = 60;

            Vector2 pose = GameManager.ScreenOffset - new Vector2(UIWidth + extraX, 0) + GameManager.CameraPosition;

            Vector2 objectsPos = pose - Vector2.UnitY * 40;
            Rectangle objectsRect = new Rectangle((int)objectsPos.X, (int)objectsPos.Y, UIWidth / 2, 40);
            Vector2 toolsPos = objectsPos + Vector2.UnitX * objectsRect.Width;
            Rectangle toolsRect = new Rectangle((int)toolsPos.X, (int)toolsPos.Y, UIWidth / 2, 40);

            sb.Draw(TextureAssets.MagicPixel.Value, objectsPos, new Rectangle(0, 0, UIWidth / 2, 40), Color.AliceBlue);
            Utils.DrawBorderString(sb, "Objects", objectsPos, CurrentMenu == 0 ? Color.Lime : Color.White);

            sb.Draw(TextureAssets.MagicPixel.Value, toolsPos, new Rectangle(0, 0, UIWidth / 2, 40), Color.PaleGoldenrod);
            Utils.DrawBorderString(sb, "Tools", toolsPos, CurrentMenu == 1 ? Color.Lime : Color.White);

            // Switch between tabs
            if (Main.mouseLeft && Main.mouseLeftRelease)
            {
                if (maus.Intersects(toolsRect))
                {
                    CurrentMenu = 1;
                }
                else if (maus.Intersects(objectsRect))
                {
                    CurrentMenu = 0;
                }
            }

            // Draw the menu that contains all the spawnable entities
            if (CurrentMenu == 0)
            {
                DrawSpawnerMenu(sb, UIWidth, extraX);
            }
            // Draw the menu that contains all the options
            else if (CurrentMenu == 1)
            {
                DrawToolMenu(sb, UIWidth, extraX);
            }
            DrawInstanceMenu(sb, UIWidth, extraX);
            
            // Debug cursor
            if (ShowCursor)
                sb.Draw(TextureAssets.MagicPixel.Value, Main.MouseScreen, new Rectangle(0, 0, 22, 22), Color.Red * 0.8f, 0, Vector2.Zero, 1, 0, 0);

            // Initialize scroll values
            if (scrollOld == 0 && scrollNew == 0)
            {
                scrollOld = Microsoft.Xna.Framework.Input.Mouse.GetState().ScrollWheelValue;
                scrollNew = Microsoft.Xna.Framework.Input.Mouse.GetState().ScrollWheelValue;
            }
            // Update scroll values
            else
            {
                scrollOld = scrollNew;
                scrollNew = Microsoft.Xna.Framework.Input.Mouse.GetState().ScrollWheelValue;
            }
        }

        public static void DrawInstanceMenu(SpriteBatch sb, int UIWidth, int extraX)
        {
            Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 10, 10);
            int padding = 22;
            int xSpace = 6;
            int curRow = 0;
            int spacing = 50;
            int ySpace = 32;
            float optionWidth = (UIWidth - (padding * 2)) / 5 - xSpace;
            Vector2 pose = Vector2.UnitX * (GameManager.playingField.X + 50) + GameManager.ScreenOffset + GameManager.CameraPosition;

            sb.Draw(TextureAssets.MagicPixel.Value, pose, new Rectangle(0, 0, UIWidth, (int)GameManager.playingField.Y), Color.Teal);
            string title = "No entity selected";
            bool validEntity = selectedEntity is ICreative creative && creative.EditorOptions.Count > 0;
            if (selectedEntity != null)
            {
                ICreative temp = selectedEntity as ICreative;
                title = "Selected entity: " + temp.Name + " " + selectedEntity.TilePosition;
            }
            Utils.DrawBorderString(sb, title, pose, Color.White);
            if (selectedEntity == null)
                return;
            if (selectedEntity is not ICreative)
                return;
            if (!validEntity)
                return;

            ICreative editingEntity = selectedEntity as ICreative;

            // draw options
            for (int i = 0; i < editingEntity.EditorOptions.Count; i++)
            {
                if (i % 5 == 0)
                    curRow++;
                EntityOption g = editingEntity.EditorOptions[i];
                if (g.BoundEntity == null)
                    g.BoundEntity = selectedEntity;
                float xWidth = FontAssets.MouseText.Value.MeasureString(g.Name).X;
                Vector2 pos = pose + new Vector2(i % 5 * spacing + extraX, curRow * ySpace + 30) - new Vector2(30);
                
                Color c = Color.White;
                // click on the option to select it
                if (maus.Intersects(new Rectangle((int)pos.X, (int)pos.Y, 40, 40)))
                {
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        g.ClickAction();
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                    else if (scrollOld > scrollNew)
                    {
                        g.ScrollUp();
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                    else if (scrollOld < scrollNew)
                    {
                        g.ScrollDown();
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }
                g.Draw(sb, pos, new Rectangle((int)pos.X, (int)pos.Y, 40, 40));
                Utils.DrawBorderString(sb, g.Name, pos + Vector2.UnitY * 5, c, optionWidth / xWidth);
            }
        }

        public static void DrawSpawnerMenu(SpriteBatch sb, int UIWidth, int extraX)
        {
            Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 10, 10);
            int padding = 22;
            int xSpace = 6;
            int curRow = 0;
            int spacing = 50;
            int ySpace = 32;
            float optionWidth = (UIWidth - (padding * 2)) / 5 - xSpace;
            Vector2 pose = GameManager.ScreenOffset - new Vector2(UIWidth + extraX, 0) + GameManager.CameraPosition;

            sb.Draw(TextureAssets.MagicPixel.Value, pose, new Rectangle(0, 0, UIWidth, (int)GameManager.playingField.Y), Color.AliceBlue);

            // draw options
            for (int i = 1; i < types.Count + 1; i++)
            {
                if ((i - 1) % 5 == 0)
                    curRow++;
                GameEntity g = types[i];
                ICreative cr = g as ICreative;
                float xWidth = FontAssets.MouseText.Value.MeasureString(cr.Name).X;
                Vector2 pos = pose + new Vector2(i % 5 * spacing + extraX, curRow * ySpace) - new Vector2(30);

                Color c = Color.White;
                // click on the option to select it
                if (maus.Intersects(new Rectangle((int)pos.X, (int)pos.Y, 40, 40)))
                {
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        if (currentType != g)
                            currentType = g;
                        else
                            currentType = null;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }
                if (currentType == g)
                    c = Color.Lime;

                cr.DrawCreative(sb, pos);

                Utils.DrawBorderString(sb, cr.Name, pos, c, optionWidth / xWidth);
            }
        }

        public static void DrawToolMenu(SpriteBatch sb, int UIWidth, int extraX)
        {
            int curRow = 0;
            int spacing = 50;
            int ySpace = 52;

            float optionWidth = 36;

            Vector2 basePos = GameManager.ScreenOffset - new Vector2(UIWidth + extraX, 0) + GameManager.CameraPosition;

            sb.Draw(TextureAssets.MagicPixel.Value, basePos, new Rectangle(0, 0, UIWidth, (int)GameManager.playingField.Y), Color.PaleGoldenrod);

            Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 10, 10);
            // draw options
            for (int i = 0; i < options.Count; i++)
            {
                if (i % 5 == 0)
                    curRow++;
                LevelEditorOption g = options[i];
                float xWidth = FontAssets.MouseText.Value.MeasureString(g.Name).X;
                Vector2 pos = basePos + new Vector2(i % 5 * spacing + extraX, curRow * ySpace) - new Vector2(30);

                Color c = Color.White;
                // click on the option to select it
                if (maus.Intersects(new Rectangle((int)pos.X, (int)pos.Y, 40, 40)))
                {
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        g.ClickAction();
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                    else if (scrollOld > scrollNew)
                    {
                        g.ScrollUp();
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                    else if (scrollOld < scrollNew)
                    {
                        g.ScrollDown();
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }
                g.Draw(sb, pos, new Rectangle((int)pos.X, (int)pos.Y, 40, 40));
                Utils.DrawBorderString(sb, g.Name, pos + Vector2.UnitY * 5, c, optionWidth / xWidth);
            }
        }
    }
}