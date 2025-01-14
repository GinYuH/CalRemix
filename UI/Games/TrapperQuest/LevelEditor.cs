using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using static CalRemix.UI.Games.TrapperQuest.TQHandler;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria.ModLoader.Core;
using CalRemix.UI.Games.Boi.BaseClasses;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent;
using System.Transactions;
using System.Text;
using System.IO;
using Terraria.GameContent.Creative;
using Microsoft.CodeAnalysis;
using System.Dynamic;

namespace CalRemix.UI.Games.TrapperQuest
{
    public class LevelEditor
    {
        public static GameEntity currentType;

        public static Dictionary<int, GameEntity> types = new Dictionary<int, GameEntity>();
        public static List<LevelEditorOption> options = new List<LevelEditorOption>();
        public static string exportPath = Path.Combine(Main.SavePath + "/Data Dumps");
        public static string path = $@"{exportPath}\CurLevel.txt";
        public static float scrollOld = 0;
        public static float scrollNew = 0;

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
                // place entities
                if (Main.mouseLeft && currentType != null)
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
                else if (Main.mouseRight)
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

        /// <summary>
        /// Exports all level editor-valid entities to a txt file
        /// </summary>
        public static void ExportRoom()
        {
            string ret = "";
            ret += player.RoomImIn.RoomSize.X + "," + player.RoomImIn.RoomSize.Y + "," + player.RoomImIn.id + "\n";
            foreach (GameEntity g in player.RoomImIn.Entities)
            {
                if (g is ICreative creative)
                {
                    if (creative.Name != "")
                    {
                        // Hold shift to automatically format it for code-use. This is NOT compatible with importation
                        bool shift = Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift);
                        if (shift)
                            ret += "                GameEntity.SpawnFromID(";
                        ret += creative.ID + "," + g.Position.X + "," + g.Position.Y;
                        if (shift)
                            ret += "),";
                        ret += "\n";
                    }
                }
            }
            File.WriteAllText(path, ret, Encoding.UTF8);
            Main.NewText("Exported!");            
        }

        /// <summary>
        /// Import rooms from a text file
        /// </summary>
        public static TQRoom ImportRoom(bool load = true, string text = "")
        {
            if (!load)
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
                if (load)
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
            int UIWidth = 300;
            int extraX = 60;
            int curRow = 0;
            int spacing = 50;
            int padding = 22;
            int ySpace = 32;
            int xSpace = 6;

            float optionWidth = (UIWidth - (padding * 2)) / 5 - xSpace;

            sb.Draw(TextureAssets.MagicPixel.Value, GameManager.ScreenOffset - new Vector2(UIWidth + extraX, 0) + GameManager.CameraPosition, new Rectangle(0, 0, UIWidth, (int)GameManager.playingField.Y), Color.AliceBlue);

            Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 10, 10);
            // draw options
            for (int i = 1; i < types.Count + 1; i++)
            {
                if ((i - 1) % 5 == 0)
                    curRow++;
                GameEntity g = types[i];
                ICreative cr = g as ICreative;
                float xWidth = FontAssets.MouseText.Value.MeasureString(cr.Name).X;
                Vector2 pos = GameManager.ScreenOffset - new Vector2((UIWidth - ((i - 1) % 5) * spacing + extraX) + padding - optionWidth, -curRow * ySpace)+ GameManager.CameraPosition;

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
            sb.Draw(TextureAssets.MagicPixel.Value, Main.MouseScreen, new Rectangle(0, 0, 22, 22), Color.Red * 0.8f, 0, Vector2.Zero, 1, 0, 0);

            DrawSaveLoad(sb, UIWidth, extraX);
            DrawTechyStuff(sb, UIWidth, extraX);

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

        public static void DrawTechyStuff(SpriteBatch sb, int UIWidth, int extraX)
        {
            int curRow = 0;
            int spacing = 50;
            int padding = 22;
            int ySpace = 32;
            int xSpace = 6;

            float optionWidth = (UIWidth - (padding * 2)) / 5 - xSpace;

            Vector2 basePos = Vector2.UnitX * 60 + Vector2.UnitX * GameManager.playingField.X + GameManager.ScreenOffset + GameManager.CameraPosition;

            sb.Draw(TextureAssets.MagicPixel.Value, basePos, new Rectangle(0, 0, UIWidth, (int)GameManager.playingField.Y), Color.AliceBlue);

            Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 10, 10);
            // draw options
            for (int i = 0; i < options.Count; i++)
            {
                if (i % 5 == 0)
                    curRow++;
                LevelEditorOption g = options[i];
                float xWidth = FontAssets.MouseText.Value.MeasureString(g.Name).X;
                Vector2 pos = basePos + new Vector2((i % 5) * spacing + extraX, curRow * ySpace);

                Color c = Color.White;
                // click on the option to select it
                if (maus.Intersects(new Rectangle((int)pos.X, (int)pos.Y, 40, 40)))
                {
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        g.ClickAction();
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                    if (scrollOld > scrollNew)
                    {
                        g.ScrollUp();
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                    if (scrollOld < scrollNew)
                    {
                        g.ScrollDown();
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                }
                g.Draw(sb, pos, new Rectangle((int)pos.X, (int)pos.Y, 40, 40));
                Utils.DrawBorderString(sb, g.Name, pos, c, optionWidth / xWidth);
            }
        }

        public static void DrawSaveLoad(SpriteBatch sb, int UIWidth, int extraX)
        {
            Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 10, 10);
            // save and load buttons
            int saveX = UIWidth + extraX - 20;
            int loadX = UIWidth + extraX - 40 - UIWidth / 3;
            float saveloadY = -GameManager.playingField.Y + 40;
            sb.Draw(TextureAssets.MagicPixel.Value, GameManager.ScreenOffset - new Vector2(saveX, saveloadY) + GameManager.CameraPosition, new Rectangle(0, 0, UIWidth / 3, 20), Color.Green);
            sb.Draw(TextureAssets.MagicPixel.Value, GameManager.ScreenOffset - new Vector2(loadX, saveloadY) + GameManager.CameraPosition, new Rectangle(0, 0, UIWidth / 3, 20), Color.Blue);

            int saveText = saveX - UIWidth / 10;
            int loadText = loadX - UIWidth / 10;
            Utils.DrawBorderString(sb, "Save", GameManager.ScreenOffset - new Vector2(saveText, saveloadY) + GameManager.CameraPosition, Color.White);
            Utils.DrawBorderString(sb, "Load", GameManager.ScreenOffset - new Vector2(loadText, saveloadY) + GameManager.CameraPosition, Color.White);

            Rectangle saveRect = new Rectangle((int)GameManager.ScreenOffset.X - saveText + (int)GameManager.CameraPosition.X, (int)GameManager.ScreenOffset.Y - (int)saveloadY, UIWidth / 3, 20);
            Rectangle loadRect = new Rectangle((int)GameManager.ScreenOffset.X - loadText + (int)GameManager.CameraPosition.X, (int)GameManager.ScreenOffset.Y - (int)saveloadY, UIWidth / 3, 20);

            // click to export
            if (maus.Intersects(saveRect) && Main.mouseLeft && Main.mouseLeftRelease)
            {
                ExportRoom();
            }
            // click to import
            if (maus.Intersects(loadRect) && Main.mouseLeft && Main.mouseLeftRelease)
            {
                ImportRoom(false);
            }
        }
    }

    public abstract class LevelEditorOption
    {
        public virtual string Name => "";

        public virtual void ClickAction() { }

        public virtual void ScrollUp() { }

        public virtual void ScrollDown() { }

        public virtual void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color.Black);
        }
    }

    public class SizeXOption : LevelEditorOption
    {
        public override string Name => "SizeX";

        public override void ScrollUp()
        {
            float size = player.RoomImIn.RoomSize.X + 1;
            if (size >= RoomWidthDefault)
                player.RoomImIn.RoomSize.X = size;
        }

        public override void ScrollDown()
        {
            float size = player.RoomImIn.RoomSize.X - 1;
            if (size >= RoomWidthDefault)
                player.RoomImIn.RoomSize.X = size;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color.Green, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, player.RoomImIn.RoomSize.X.ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }

    public class SizeYOption : LevelEditorOption
    {
        public override string Name => "SizeY";

        public override void ScrollUp()
        {
            float size = player.RoomImIn.RoomSize.Y + 1;
            if (size >= RoomHeightDefault)
                player.RoomImIn.RoomSize.Y = size;
        }

        public override void ScrollDown()
        {
            float size = player.RoomImIn.RoomSize.Y - 1;
            if (size >= RoomHeightDefault)
                player.RoomImIn.RoomSize.Y = size;
        }

        public override void Draw(SpriteBatch sb, Vector2 position, Rectangle rect)
        {
            sb.Draw(TextureAssets.MagicPixel.Value, position, rect, Color.Green, 0, Vector2.Zero, 1, 0, 0);
            Utils.DrawBorderString(sb, player.RoomImIn.RoomSize.Y.ToString(), position + Vector2.UnitY * 22, Color.White);
        }
    }
}