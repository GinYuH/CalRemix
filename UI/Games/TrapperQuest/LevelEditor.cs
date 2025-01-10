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

namespace CalRemix.UI.Games.TrapperQuest
{
    public class LevelEditor
    {
        public static GameEntity currentType;

        public static Dictionary<int, GameEntity> types = new Dictionary<int, GameEntity>();

        public static void LoadTypes()
        {
            Type[] Ctypes = AssemblyManager.GetLoadableTypes(CalRemix.instance.Code);
            foreach (Type type in Ctypes)
            {
                if (!type.IsSubclassOf(typeof(GameEntity)) || type.IsSubclassOf(typeof(BoiEntity)) || type.IsAbstract)
                    continue;

                GameEntity entity = Activator.CreateInstance(type) as GameEntity;
                if (entity.Name == "")
                    continue;
                
                types.Add(entity.ID, entity);
            }
        }

        public static void Run()
        {

            Vector2 moused = ConvertToTileCords(Mouse.Location.ToVector2());
            string exportPath = Path.Combine(Main.SavePath + "/Data Dumps");
            string path = $@"{exportPath}\CurLevel.txt";
            if (Main.mouseLeft && Main.mouseRight && Main.mouseLeftRelease && Main.mouseRightRelease)
            {
                string ret = "";
                foreach (GameEntity g in player.RoomImIn.Entities)
                {
                    if (g.Name != "")
                        ret += g.ID + "," + g.Position.X + "," + g.Position.Y + "\n";
                }
                File.WriteAllText(path, ret, Encoding.UTF8);
                Main.NewText("Exported!");
            }
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
            {
                player.RoomImIn.Entities.RemoveAll((GameEntity g) => g is not TrapperPlayer);
                player.RoomImIn.Tiles.Clear();
                string dump = File.ReadAllText(path);
                string[] lines = dump.Split('\n');
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    string c = lines[i];
                    string[] elems = c.Split(',');

                    GameEntity spawnType = Activator.CreateInstance(types[(int)char.GetNumericValue(c[0])].GetType()) as GameEntity;

                    Vector2 pos = new Vector2(int.Parse(elems[1]), int.Parse(elems[2]));

                    Vector2 tileCords = ConvertToTileCords(pos);
                    if (!player.RoomImIn.Tiles.ContainsKey(((int)tileCords.X, (int)tileCords.Y)))
                    {
                        spawnType.Position = pos;
                        player.RoomImIn.Entities.Add(spawnType);
                        player.RoomImIn.Tiles.Add(((int)tileCords.X, (int)tileCords.Y), spawnType as TQRock);
                    }
                }
            }

            if (moused.X >= 0 && moused.X < RoomHeight && moused.Y >= 0 && moused.Y < RoomWidth)
                if (Main.mouseLeft && currentType != null)
                {
                    if (!player.RoomImIn.Tiles.ContainsKey(((int)moused.X, (int)moused.Y)))
                    {
                        GameEntity newE = Activator.CreateInstance(currentType.GetType()) as GameEntity;
                        //if (currentType.GetType().IsSubclassOf(typeof(TQRock)))
                        {
                            //Main.NewText("G");
                            newE.Position = moused;
                            player.RoomImIn.Tiles.Add(((int)moused.X, (int)moused.Y), newE as TQRock);
                        }

                        newE.Position = ConvertToScreenCords(moused);
                        player.RoomImIn.Entities.Add(newE);
                    }
                }
                else if (Main.mouseRight)
                {
                    if (player.RoomImIn.Tiles.ContainsKey(((int)moused.X, (int)moused.Y)))
                    {
                        int rock = player.RoomImIn.Entities.FindIndex(0, player.RoomImIn.Entities.Count, (GameEntity g) => ConvertToTileCords(g.Position) == moused);
                        if (rock != -1)
                        {
                            player.RoomImIn.Entities.RemoveAt(rock);
                            player.RoomImIn.Tiles.Remove((((int)moused.X, (int)moused.Y)));
                        }
                    }

                }
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

            sb.Draw(TextureAssets.MagicPixel.Value, GameManager.ScreenOffset - new Vector2(UIWidth + extraX, 0), new Rectangle(0, 0, UIWidth, (int)GameManager.playingField.Y), Color.AliceBlue);

            for (int i = 1; i < types.Count; i++)
            {
                if (i % 5 == 0)
                    curRow++;
                GameEntity g = types[i];
                float xWidth = FontAssets.MouseText.Value.MeasureString(g.Name).X;
                Vector2 pos = GameManager.ScreenOffset - new Vector2((UIWidth - (i % 5) * spacing + extraX) + padding - optionWidth, -curRow * ySpace);

                Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 10, 10);
                Color c = Color.White;
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

                Utils.DrawBorderString(sb, g.Name, pos, c, optionWidth / xWidth);
            }
        }
    }
}