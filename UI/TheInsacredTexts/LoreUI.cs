using CalRemix.Content.Items.Lore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI.TheInsacredTexts
{
    public class LoreUIState : UIState
    {
        private static readonly SoundStyle PageFlip = new("CalRemix/Assets/Sounds/PageFlip");
        public static readonly string LocalKey = "Lore.TheInsacredTexts.";
        public static readonly string CKey = $"{LocalKey}Chapters.";
        public static int inputCooldown = 0;
        public static int page = 0;
        public override void Update(GameTime gameTime)
        {
            bool shouldShow = !Main.gameMenu;
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.gameMenu)
                return;
            if (Main.LocalPlayer is null)
                return;
            if (Main.LocalPlayer.dead || !Main.LocalPlayer.TryGetModPlayer(out TheInsacredTextsPlayer p) || !p.reading)
                return;
            if (Main.keyState.IsKeyDown(Keys.Escape))
            {
                Main.blockInput = false;
                Main.LocalPlayer.GetModPlayer<TheInsacredTextsPlayer>().reading = false;
                return;
            }
            base.Draw(spriteBatch);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (inputCooldown > 0)
                inputCooldown--;
            if (Main.keyState.IsKeyDown(Keys.Right) && page < 38 - 1 & inputCooldown <= 0)
            {
                SoundEngine.PlaySound(PageFlip, Main.LocalPlayer.Center);
                page++;
                inputCooldown = 12;
            }
            else if (Main.keyState.IsKeyDown(Keys.Left) && page > 0 & inputCooldown <= 0)
            {
                SoundEngine.PlaySound(PageFlip, Main.LocalPlayer.Center);
                page--;
                inputCooldown = 12;
            }
            DrawPage(spriteBatch);
        }

        private static void DrawPage(SpriteBatch spriteBatch)
        {
            Texture2D texture = (page % 2 == 0) ? ModContent.Request<Texture2D>("CalRemix/UI/TheInsacredTexts/Page1").Value : ModContent.Request<Texture2D>("CalRemix/UI/TheInsacredTexts/Page2").Value;
            spriteBatch.Draw(texture, new Vector2((Main.screenWidth / 2) - texture.Width / 2, (Main.screenHeight / 2) - texture.Height / 2), Color.White);
            Vector2 topCenter = new(Main.screenWidth / 2, (Main.screenHeight / 2) - texture.Height / 2);
            Color color = new(197, 179, 174);
            string title = CalRemixHelper.LocalText($"{LocalKey}Title").Value;
            string subtitle = CalRemixHelper.LocalText($"{LocalKey}Subtitle").Value;
            if (page == 0)
            {
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, title, (int)topCenter.X - (FontAssets.MouseText.Value.MeasureString(title).X / 2), (int)topCenter.Y + (texture.Height / 2) - 80, Color.DarkRed, color, Vector2.Zero, 1.2f);
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, subtitle, (int)topCenter.X - (FontAssets.MouseText.Value.MeasureString(subtitle).X / 2), (int)topCenter.Y + (texture.Height / 2) - 40, Color.DarkRed, color, Vector2.Zero);
                Texture2D logo = ModContent.Request<Texture2D>("CalRemix/UI/TheInsacredTexts/Logo").Value;
                spriteBatch.Draw(logo, new Vector2((Main.screenWidth / 2) - logo.Width / 2, (Main.screenHeight / 2) - (logo.Height / 2) + 60), Color.White);
            }
            else
            {
                string chapter = (page) switch
                {
                    1 => CalRemixHelper.LocalText($"{CKey}1").Value,
                    5 => CalRemixHelper.LocalText($"{CKey}2").Value,
                    9 => CalRemixHelper.LocalText($"{CKey}3").Value,
                    15 => CalRemixHelper.LocalText($"{CKey}4").Value,
                    18 => CalRemixHelper.LocalText($"{CKey}5").Value,
                    24 => CalRemixHelper.LocalText($"{CKey}6").Value,
                    28 => CalRemixHelper.LocalText($"{CKey}7").Value,
                    31 => CalRemixHelper.LocalText($"{CKey}8").Value,
                    35 => CalRemixHelper.LocalText($"{CKey}9").Value,
                    _ => string.Empty
                };
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, chapter, (int)topCenter.X - (FontAssets.MouseText.Value.MeasureString(chapter).X / 2), (int)topCenter.Y + (FontAssets.MouseText.Value.MeasureString(chapter).Y / 2) + 10, Color.DarkRed, color, Vector2.Zero);
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, LoreUI.PageText[page], (int)topCenter.X - (texture.Width / 2) + 56, (int)(topCenter.Y + FontAssets.MouseText.Value.MeasureString(title).Y / 2) + 40, Color.DarkRed, color, Vector2.Zero, 0.85f);
                /*
                string[] text = LoreDeath.Chapter[page].Split(' ');
                string line = string.Empty;
                float height = (int)(topCenter.Y + FontAssets.MouseText.Value.MeasureString(title).Y / 2) + 20;
                foreach (string s in text)
                {
                    if (line.Length < 100)
                        line += s + " ";
                    else
                    {
                        Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, line, (int)topCenter.X - (texture.Width / 2) + 56, height, Color.DarkRed, Color.LightSalmon, Vector2.Zero, 0.8f);
                        height += (FontAssets.MouseText.Value.MeasureString(title).Y / 2) + 5;
                        line = string.Empty;
                    }
                }
                */

            }
        }
        private static Rectangle Mouse()
        {
            return new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class LoreUI : ModSystem
    {
        private UserInterface LoreUserInterface;
        internal LoreUIState LoreState;
        internal static string[] PageText;
        internal static int LoreSize = 38;
        public override void Load()
        {
            PageText = new string[LoreSize];
            LoreState = new();
            LoreUserInterface = new();
            LoreUserInterface.SetState(LoreState);
        }
        public override void PostSetupContent()
        {
            string[] page = new string[LoreSize];
            for (int n = 0; n < LoreSize; n++)
            {
                page[n] = CalRemixHelper.LocalText($"Lore.TheInsacredTexts.Pages.{n}").Value;
            }
            for (int i = 0; i < PageText.Length; i++)
            {
                string[] text = page[i].Split(' ');
                string line = string.Empty;
                string final = string.Empty;
                foreach (string s in text)
                {
                    if (line.Length < 50)
                        line += s + " ";
                    else
                    {
                        line += s + "\n";
                        final += line;
                        line = string.Empty;
                    }
                }
                final += line;
                PageText[i] = final;
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            LoreUserInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (Main.LocalPlayer.GetModPlayer<TheInsacredTextsPlayer>().reading)
            {
                int layerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
                if (layerIndex != -1)
                {
                    layers.Insert(layerIndex, new LegacyGameInterfaceLayer("CalRemix: InsacredTexts",
                        delegate
                        {
                            LoreUserInterface.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI));
                }
            }
        }
    }
}
