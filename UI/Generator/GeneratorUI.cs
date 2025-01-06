using static Terraria.ModLoader.ModContent;
using static CalRemix.Core.CustomGen;
using CalamityMod;
using CalRemix.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Audio;
using Terraria.ID;

namespace CalRemix.UI.Generator
{
    public class GeneratorUI : ModSystem
    {
        private UserInterface UI;
        internal GeneratorMenu Menu;
        public override void Load()
        {
            Menu = new();
            UI = new();
            UI.SetState(Menu);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            UI?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().generatingGen)
            {
                int layerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
                if (layerIndex != -1)
                {
                    layers.Insert(layerIndex, new LegacyGameInterfaceLayer("CalRemix: GenMakerUI",
                        delegate
                        {
                            UI.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI));
                }
            }
        }
    }
    public partial class GeneratorMenu : UIState
    {
        private CustomGen pendingGen;

        private static string TextInput = "";
        private static string Cursor = "|";
        private static int CursorTimer = 0;
        private static int BackDelay = 45;
        private static short Mode = 0;

        private static bool Inputting = false;

        private static readonly int bgWidth = Main.screenWidth;
        private static readonly int bgHeight = Main.screenHeight;
        private static readonly Vector2 Center = new(bgWidth / 2, bgHeight / 2);
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.gameMenu)
                return;
            if (!Main.myPlayer.WithinBounds(Main.player.Length))
                return;
            if (Main.LocalPlayer is null)
                return;
            if (Main.LocalPlayer.dead || !Main.LocalPlayer.TryGetModPlayer(out CalRemixPlayer p) || !p.generatingGen)
                return;
            Main.blockInput = true;
            pendingGen = Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().customGen;
            Main.playerInventory = false;
            if (BackDelay > 0)
                BackDelay--;

            if (Main.keyState.IsKeyDown(Keys.Escape))
            {
                if (Inputting)
                    SetInput(pendingGen);
                Main.blockInput = false;
                Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().generatingGen = false;
                return;
            }
            base.Draw(spriteBatch);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            DrawMenu(spriteBatch);

            DrawModeButton(spriteBatch);
            DrawMusicButton(spriteBatch);
            DrawStyleButton(spriteBatch, pendingGen);

            DrawVisibleButton(spriteBatch, pendingGen);
            DrawHexButton(spriteBatch, pendingGen);
            DrawGlowButton(spriteBatch, pendingGen);

            DrawGenToSpritebatch(spriteBatch, pendingGen, Center);
        }
        private static void DrawMenu(SpriteBatch spriteBatch)
        {
            Rectangle background = RectFromCenter((int)(bgWidth * 0.5f), (int)(bgHeight * 0.5f), 440, 440);
            Rectangle border = RectFromCenter((int)(bgWidth * 0.5f), (int)(bgHeight * 0.5f), background.Width + 8, background.Height + 8);

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, border, Color.White);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, background, new Color(66, 80, 96));
        }
        private static void DrawModeButton(SpriteBatch spriteBatch)
        {
            Vector2 pos = new((int)Center.X - 144, (int)Center.Y - 88);
            Texture2D texture = Request<Texture2D>("CalRemix/UI/Generator/Mode").Value;

            Rectangle button = RectFromCenter((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            int frameX = Mode;
            int frameY = button.Contains(Main.mouseX, Main.mouseY) ? 1 : 0;
            Rectangle rect = new(frameX * (texture.Width / 2), frameY * (texture.Height / 2), (texture.Width / 2 - 1), (texture.Height / 2 - 1));
            spriteBatch.Draw(texture, pos, rect, Color.White, 0, rect.Size() * 0.5f, 1, SpriteEffects.None, 0);

            if (button.Contains(Main.mouseX, Main.mouseY))
            {
                Main.hoverItemName = CalRemixHelper.LocalText("UI.Generator.Mode").Value;
                if (Main.mouseLeftRelease && Main.mouseLeft)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    Mode = (short)((Mode == 0) ? 1 : 0);
                }
            }
        }
        private static void DrawMusicButton(SpriteBatch spriteBatch)
        {
            Vector2 pos = new((int)Center.X, (int)Center.Y - 144);
            Texture2D texture = Request<Texture2D>("CalRemix/UI/Generator/Music").Value;

            Rectangle button = RectFromCenter((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            int frameX = (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().genMusic) ? 0 : 1;
            int frameY = (button.Contains(Main.mouseX, Main.mouseY)) ? 1 : 0;
            Rectangle rect = new(frameX * (texture.Width / 2), frameY * (texture.Height / 2), (texture.Width / 2 - 1), (texture.Height / 2 - 1));
            spriteBatch.Draw(texture, pos, rect, Color.White, 0, rect.Size() * 0.5f, 1, SpriteEffects.None, 0);

            if (button.Contains(Main.mouseX, Main.mouseY))
            {
                Main.hoverItemName = CalRemixHelper.LocalText("UI.Generator.Music").Value;
                if (Main.mouseLeftRelease && Main.mouseLeft)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().genMusic = (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().genMusic == false);
                }
            }
        }
        private static void DrawStyleButton(SpriteBatch spriteBatch, CustomGen pendingGen)
        {
            Vector2 pos = new((int)Center.X + 144, (int)Center.Y - 88);
            Texture2D texture = Request<Texture2D>("CalRemix/UI/Generator/Style").Value;

            Rectangle button = RectFromCenter((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            int frameY = (button.Contains(Main.mouseX, Main.mouseY)) ? 1 : 0;
            Rectangle rect = new(0, frameY * (texture.Height / 2), texture.Width, (texture.Height / 2 - 1));
            spriteBatch.Draw(texture, pos, rect, Color.White, 0, rect.Size() * 0.5f, 1, SpriteEffects.None, 0);

            if (button.Contains(Main.mouseX, Main.mouseY))
            {
                Main.hoverItemName = CalRemixHelper.LocalText("UI.Generator.Style").Value;
                if (Main.mouseLeftRelease && Main.mouseLeft)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    if (Mode == 0)
                        pendingGen.CoreTexture = (pendingGen.CoreTexture + 1 >= TextureList.Count) ? 0 : pendingGen.CoreTexture + 1;
                    else
                        pendingGen.ShieldTexture = (pendingGen.ShieldTexture + 1 >= TextureList.Count) ? 0 : pendingGen.ShieldTexture + 1;
                }
            }
        }
        private static void DrawVisibleButton(SpriteBatch spriteBatch, CustomGen pendingGen)
        {
            Vector2 pos = new((int)Center.X - 144, (int)Center.Y + 88);
            Texture2D texture = Request<Texture2D>("CalRemix/UI/Generator/Visibility").Value;

            Rectangle button = RectFromCenter((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            int frameX;
            if (Mode == 0)
                frameX = pendingGen.CoreVisible ? 0 : 1;
            else
                frameX = pendingGen.ShieldVisible ? 0 : 1;
            int frameY = (button.Contains(Main.mouseX, Main.mouseY)) ? 1 : 0;
            Rectangle rect = new(frameX * (texture.Width / 2), frameY * (texture.Height / 2), (texture.Width / 2 - 1), (texture.Height / 2 - 1));
            spriteBatch.Draw(texture, pos, rect, Color.White, 0, rect.Size() * 0.5f, 1, SpriteEffects.None, 0);

            if (button.Contains(Main.mouseX, Main.mouseY))
            {
                Main.hoverItemName = CalRemixHelper.LocalText("UI.Generator.Visibility").Value;
                if (Main.mouseLeftRelease && Main.mouseLeft)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    if (Mode == 0)
                        pendingGen.CoreVisible = !pendingGen.CoreVisible;
                    else
                        pendingGen.ShieldVisible = !pendingGen.ShieldVisible;
                }
            }
        }
        private static void DrawGlowButton(SpriteBatch spriteBatch, CustomGen pendingGen)
        {
            Vector2 pos = new((int)Center.X + 144, (int)Center.Y + 88);
            Texture2D texture = Request<Texture2D>("CalRemix/UI/Generator/Glow").Value;

            Rectangle button = RectFromCenter((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            int frameX;
            if (Mode == 0)
                frameX = pendingGen.CoreGlow ? 1 : 0;
            else
                frameX = pendingGen.ShieldGlow ? 1 : 0;
            int frameY = (button.Contains(Main.mouseX, Main.mouseY)) ? 1 : 0;
            Rectangle rect = new(frameX * (texture.Width / 2), frameY * (texture.Height / 2), (texture.Width / 2 - 1), (texture.Height / 2 - 1));
            spriteBatch.Draw(texture, pos, rect, Color.White, 0, rect.Size() * 0.5f, 1, SpriteEffects.None, 0);

            if (button.Contains(Main.mouseX, Main.mouseY))
            {
                Main.hoverItemName = CalRemixHelper.LocalText("UI.Generator.Glow").Value;
                if (Main.mouseLeftRelease && Main.mouseLeft)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    if (Mode == 0)
                        pendingGen.CoreGlow = !pendingGen.CoreGlow;
                    else
                        pendingGen.ShieldGlow = !pendingGen.ShieldGlow;
                }
            }
        }
        private static void DrawHexButton(SpriteBatch spriteBatch, CustomGen pendingGen)
        {
            Vector2 pos = new((int)Center.X, (int)Center.Y + 144);

            Rectangle button = RectFromCenter((int)pos.X, (int)pos.Y, 88, 22);
            if (button.Contains(Main.mouseX, Main.mouseY))
                Main.hoverItemName = CalRemixHelper.LocalText("UI.Generator.Color").Value;
            if (!Inputting)
                TextInput = (Mode == 0) ? pendingGen.CoreColor.Hex3() : pendingGen.ShieldColor.Hex3();
            if (Main.mouseLeftRelease && Main.mouseLeft && button.Contains(Main.mouseX, Main.mouseY))
                Inputting = true;
            if (Inputting)
            {
                ProcessInput(pendingGen, button);
                CursorTimer++;
                if (CursorTimer > 15)
                {
                    Cursor = (Cursor == "|") ? " " : "|";
                    CursorTimer = 0;
                }
            }
            else
                Cursor = " ";
            Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, "#" + TextInput + Cursor, button.X, button.Y, Color.White, Color.Black, Vector2.Zero);
        }
        private static void ProcessInput(CustomGen pendingGen, Rectangle button)
        {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();
            if (Main.keyState.IsKeyDown(Keys.Delete))
            {
                TextInput = string.Empty;
                return;
            }
            if (Main.keyState.IsKeyDown(Keys.Back) && TextInput.Length > 0 && BackDelay <= 0)
            {
                TextInput = TextInput.Remove(TextInput.Length - 1);
                BackDelay = 45;
                return;
            }
            string newText = Main.GetInputText(TextInput);
            if (newText.Length > 6 || Regex.IsMatch(newText, "[^0-9a-fA-F]+"))
                return;
            TextInput = newText;
            if (Main.mouseLeftRelease && Main.mouseLeft && !button.Contains(Main.mouseX, Main.mouseY))
                SetInput(pendingGen);
            if (Main.keyState.IsKeyDown(Keys.Enter))
            {
                Main.drawingPlayerChat = false;
                SetInput(pendingGen);
            }
        }
        private static Rectangle RectFromCenter(int x, int y, int w, int h) => new((x - w / 2), (y - h / 2), w, h);
        private static void SetInput(CustomGen pendingGen)
        {
            if (TextInput.Equals(string.Empty))
                TextInput = "ffffff";
            int i = int.Parse(TextInput, System.Globalization.NumberStyles.HexNumber);
            if (i > 0xffffff)
                i = 0xffffff;
            Color c = new((i & 0xff0000) >> 0x10, (i & 0xff00) >> 0x8, i & 0xff);
            if (Mode == 0)
                pendingGen.CoreColor = c;
            else
                pendingGen.ShieldColor = c;
            Inputting = false;
        }
    }
}
