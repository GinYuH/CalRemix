using CalamityMod;
using CalRemix.Content.Items.ZAccessories;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI
{
    public class WordleUI : UIState
    {

        public static List<LetterState> playedLetters = new();

        public static string correctWord = "APPLE";

        public static string currentWord = "";

        public static int currentLine = 0;

        public enum InputType
        {
            text,
            integer,
            number
        }
        public static InputType inputType;
        private static void ProcessInput()
        {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();

            string newText = Main.GetInputText(currentWord).ToUpper();
            if (currentWord.Length >= 5 && !Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back))
                return;

            // input here more or less referenced from dragonlens
            if (inputType == InputType.integer && Regex.IsMatch(newText, "[0-9]*$"))
            {
                if (newText != currentWord)
                {
                    currentWord = newText;
                }
            }
            else if (inputType == InputType.number && Regex.IsMatch(newText, "(?<=^| )[0-9]+(.[0-9]+)?(?=$| )|(?<=^| ).[0-9]+(?=$| )"))
            {
                if (newText != currentWord)
                {
                    currentWord = newText;
                }
            }
            else
            {
                if (newText != currentWord)
                {
                    currentWord = newText;
                }
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            return;
            ProcessInput();
            if (Main.mouseLeft && Main.mouseLeftRelease)
            {
                if (currentWord.Length == 5)
                {
                    Dictionary<int, int> states = new();
                    char[] dummy = currentWord.ToCharArray();
                    for (int i = 0; i < 5; i++)
                    {
                        if (states.ContainsKey(i))
                            continue;
                        if (dummy[i] == correctWord[i])
                        {
                            states.Add(i, 2);
                            dummy[i] = ' ';
                        }
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (states.ContainsKey(i))
                            continue;
                        for (int j = 0; j < 5; j++)
                        {
                            if (dummy[j] == currentWord[i])
                            {
                                states.Add(i, 1);
                                dummy[j] = ' ';
                                continue;
                            }
                        }
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (states.ContainsKey(i))
                            continue;
                        states.Add(i, 0);
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        int state = states[i];
                        if (state == 0)
                        {
                            playedLetters.Add(new LetterState(currentWord[i], false, false));
                        }
                        if (state == 1)
                        {
                            playedLetters.Add(new LetterState(currentWord[i], true, false));
                        }
                        if (state == 2)
                        {
                            playedLetters.Add(new LetterState(currentWord[i], true, true));
                        }
                    }

                }
                currentWord = "";
            }
            if (Main.mouseRight && Main.mouseRightRelease)
            {
                playedLetters.Clear();
            }
            int curIdx = 0;
            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 pos = new Vector2(200 + i * 60, 100 + j * 60);
                    Texture2D boxTexture = TextureAssets.InventoryBack.Value;
                    Color color = Color.Gray;
                    if (playedLetters.Count > 0 && curIdx < playedLetters.Count)
                    {
                        color = playedLetters[curIdx].CorrectPosition ? Color.Cyan : playedLetters[curIdx].ValidLetter ? Color.Yellow : Color.Gray;
                    }
                    spriteBatch.Draw(boxTexture, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    if (playedLetters.Count > 0 || currentWord.Length > 0)
                    {
                        int lettersSoFar = playedLetters.Count;
                        if (playedLetters.Count % 5 == 0 && (curIdx + 1 - playedLetters.Count) > currentWord.Length && curIdx >= lettersSoFar)
                        {
                            continue;
                        }
                        int currrIdx = curIdx - playedLetters.Count;
                        bool isOld = curIdx < playedLetters.Count;
                        char letter = !isOld ? currentWord[currrIdx] : playedLetters[curIdx].Letter;                        
                        Vector2 letterSize = FontAssets.MouseText.Value.MeasureString(letter.ToString());
                        Vector2 letterPos = pos + TextureAssets.InventoryBack.Size() / 2;
                        Utils.DrawBorderString(spriteBatch, letter.ToString(), letterPos, color, anchorx: 0.5f, anchory: 0.5f);
                    }
                    curIdx++;
                }
            }
        }
    }

    public class LetterState
    {
        public char Letter;
        public bool ValidLetter;
        public bool CorrectPosition;
        public LetterState(char letter, bool validLetter, bool correctPosition)
        {
            Letter = letter;
            ValidLetter = validLetter;
            CorrectPosition = correctPosition;
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class WordleUISystem : ModSystem
    {
        private UserInterface UserInter;

        internal WordleUI SUI;

        public override void Load()
        {
            SUI = new();
            UserInter = new();
            UserInter.SetState(SUI);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            UserInter?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "CalRemix:WordleUI",
                    delegate
                    {
                        UserInter.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}