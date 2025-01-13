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

namespace CalRemix.UI.Games.TrapperQuest
{
    public class LevelEditor
    {
        public static GameEntity currentType;

        public static Dictionary<int, GameEntity> types = new Dictionary<int, GameEntity>();
        public static string exportPath = Path.Combine(Main.SavePath + "/Data Dumps");
        public static string path = $@"{exportPath}\CurLevel.txt";

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
        public static void ImportRoom()
        {
            player.RoomImIn.Entities.RemoveAll((GameEntity g) => g is not TrapperPlayer);
            player.RoomImIn.Tiles.Clear();
            string dump = File.ReadAllText(path);
            string[] lines = dump.Split('\n');
            for (int i = 0; i < lines.Length - 1; i++)
            {
                string c = lines[i];
                string[] elems = c.Split(',');

                // Element 1 is the ID of the entity
                GameEntity spawnType = Activator.CreateInstance(types[(int)char.GetNumericValue(c[0])].GetType()) as GameEntity;

                // Elements 2 and 3 are screen position
                Vector2 pos = new Vector2(int.Parse(elems[1]), int.Parse(elems[2]));

                Vector2 tileCords = ConvertToTileCords(pos);
                if (!player.RoomImIn.Entities.Any((GameEntity g) => ConvertToTileCords(g.Position) == pos))
                {
                    spawnType.Position = pos;
                    player.RoomImIn.Entities.Add(spawnType);
                    if (spawnType is ITile)
                        player.RoomImIn.Tiles.Add(((int)tileCords.X, (int)tileCords.Y), spawnType as TQRock);
                }
            }
            Main.NewText("Imported!");            
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

            // save and load buttons
            int saveX = UIWidth + extraX - 20;
            int loadX = UIWidth + extraX - 40 - UIWidth / 3;
            float saveloadY = -GameManager.playingField.Y + 40;
            sb.Draw(TextureAssets.MagicPixel.Value, GameManager.ScreenOffset - new Vector2(saveX, saveloadY), new Rectangle(0, 0, UIWidth / 3, 20), Color.Green);
            sb.Draw(TextureAssets.MagicPixel.Value, GameManager.ScreenOffset - new Vector2(loadX, saveloadY), new Rectangle(0, 0, UIWidth / 3, 20), Color.Blue);

            int saveText = saveX - UIWidth / 10;
            int loadText = loadX - UIWidth / 10;
            Utils.DrawBorderString(sb, "Save", GameManager.ScreenOffset - new Vector2(saveText, saveloadY), Color.White);
            Utils.DrawBorderString(sb, "Load", GameManager.ScreenOffset - new Vector2(loadText, saveloadY), Color.White);

            Rectangle saveRect = new Rectangle((int)GameManager.ScreenOffset.X - saveText, (int)GameManager.ScreenOffset.Y - (int)saveloadY, UIWidth / 3, 20);
            Rectangle loadRect = new Rectangle((int)GameManager.ScreenOffset.X - loadText, (int)GameManager.ScreenOffset.Y - (int)saveloadY, UIWidth / 3, 20);

            // click to export
            if (maus.Intersects(saveRect) && Main.mouseLeft && Main.mouseLeftRelease)
            {
                ExportRoom();
            }
            // click to import
            if (maus.Intersects(loadRect) && Main.mouseLeft && Main.mouseLeftRelease)
            {
                ImportRoom();
            }
        }
    }
}