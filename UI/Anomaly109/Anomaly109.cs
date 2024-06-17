using CalamityMod.Items.Fishing.SulphurCatches;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.TreasureBags;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.DevourerofGods;
using CalRemix.Retheme;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace CalRemix.UI
{
    public class Anomaly109UI : UIState
    {
        public static int CurrentPage = 0;
        public static int ClickCooldown = 0;
        public static string TextInput = "";
        private static string Underscore = "_";
        private static int UnderscoreTimer = 0;
        public static bool TryUnlock = false;
        private static int HeldRightTimer = 0;
        private static Vector2 fannyOffset = Vector2.Zero;
        public static int fannyFreezeTime = 0;

        public enum InputType
        {
            text,
            integer,
            number
        }
        public static InputType inputType;
        public override void Update(GameTime gameTime)
        {
            bool shouldShow = !Main.gameMenu;


            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!ScreenHelperManager.screenHelpersEnabled)
            {
                fannyFreezeTime++;
            }
            // This prevents drawing unless we are using an ExampleCustomResourceWeapon
            if (Main.gameMenu)
                return;
            if (!(Main.LocalPlayer.TryGetModPlayer<CalRemixPlayer>(out CalRemixPlayer p) && p.anomaly109UI))
            {
                return;
            }
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Main.blockInput = false;
                Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().anomaly109UI = false;
                return;
            }

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float bgWidth = Main.screenWidth * 0.6f;
            float bgHeight = Main.screenHeight * 0.7f;
            Anomaly109Option selectedOption = new Anomaly109Option("aa", "aaa", "aaaaa", () => { }, Condition.ZenithWorld);
            Rectangle selectedRectangle = new Rectangle();

            DrawBackground(spriteBatch, bgWidth, bgHeight, out Rectangle borderframe, out Rectangle mainframe);

            BlockClicks(borderframe);
            if (CurrentPage <= Anomaly109Manager.options.Count() / 12)
            {
                DrawOptions(spriteBatch, mainframe, out selectedRectangle, out selectedOption, bgWidth, bgHeight);
                DrawPrompt(spriteBatch, mainframe);
            }
            else
                DrawFanny(spriteBatch, mainframe);
            DrawArrows(spriteBatch, mainframe);
            if (CurrentPage <= Anomaly109Manager.options.Count() / 12)
            {
                DrawHelp(spriteBatch, mainframe);
                SelectOption(spriteBatch, selectedOption, selectedRectangle);
            }
        }

        private static void DrawBackground(SpriteBatch spriteBatch, float bgWidth, float bgHeight, out Rectangle borderframe, out Rectangle mainframe)
        {
            int borderwidth = 4;
            mainframe = new Rectangle((int)(Main.screenWidth - bgWidth) / 2, (int)(Main.screenHeight - bgHeight) / 2, (int)bgWidth, (int)bgHeight);
            borderframe = new Rectangle((int)(Main.screenWidth - bgWidth) / 2 - borderwidth, (int)(Main.screenHeight - bgHeight) / 2 - borderwidth, (int)bgWidth + borderwidth * 2, (int)bgHeight + borderwidth * 2);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, borderframe, Color.Lime);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, mainframe, Color.Black);
        }

        private static void DrawPrompt(SpriteBatch spriteBatch, Rectangle mainframe)
        {
            int borderwidth = 4;
            Rectangle promptframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.04f), mainframe.Y + (int)(mainframe.Height * 0.1f), (int)(mainframe.Width * 0.93f), (int)(mainframe.Height * 0.1f));
            Rectangle promptborderframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.04f) - borderwidth, mainframe.Y + (int)(mainframe.Height * 0.1f) - borderwidth, (int)(mainframe.Width * 0.93f) + borderwidth * 2, (int)(mainframe.Height * 0.1f) + borderwidth * 2);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, promptborderframe, Color.Lime);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, promptframe, Color.Black);

            UnderscoreTimer++;
            if (UnderscoreTimer > 30)
            {
                if (Underscore == "_")
                {
                    Underscore = " ";
                }
                else if (Underscore == " ")
                {
                    Underscore = "_";
                }
                UnderscoreTimer = 0;
            }

            ProcessInput();
            string textwithoutspaces = TextInput.Replace(" ", string.Empty);
            for (int i = 0; i < Anomaly109Manager.options.Count; i++)
            {
                if (!Anomaly109Manager.options[i].unlocked)
                {
                    if ((textwithoutspaces == "welive4hypnos" || textwithoutspaces == "weliveforhypnos"))
                    {
                        SoundEngine.PlaySound(SoundID.Item4, Main.LocalPlayer.Center);
                        Anomaly109Manager.options[i].unlocked = true;
                        continue;
                    }
                    if (Anomaly109Manager.options[i].key == textwithoutspaces)
                    {
                        SoundEngine.PlaySound(SoundID.Item4, Main.LocalPlayer.Center);
                        Anomaly109Manager.options[i].unlocked = true;
                        break;
                    }
                }
            }
            Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, ">C:\\remix\\" + TextInput + Underscore, promptframe.X + promptframe.Width / 64, promptframe.Y + promptframe.Height / 3f, Color.Lime * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);
        }

        private static void ProcessInput()
        {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();

            string newText = Main.GetInputText(TextInput);
            if (TextInput.Length >= 64 && !Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back))
                return;

            // input here more or less referenced from dragonlens
            if (inputType == InputType.integer && Regex.IsMatch(newText, "[0-9]*$"))
            {
                if (newText != TextInput)
                {
                    TextInput = newText;
                }
            }
            else if (inputType == InputType.number && Regex.IsMatch(newText, "(?<=^| )[0-9]+(.[0-9]+)?(?=$| )|(?<=^| ).[0-9]+(?=$| )"))
            {
                if (newText != TextInput)
                {
                    TextInput = newText;
                }
            }
            else
            {
                if (newText != TextInput)
                {
                    TextInput = newText;
                }
            }
        }

        private static void DrawHelp(SpriteBatch spriteBatch, Rectangle mainframe)
        {
            int borderwidth = 4;
            Color bordercolor = Anomaly109Manager.helpUnlocked ? Color.Yellow : Color.DarkSlateGray;
            Rectangle promptframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.775f), mainframe.Bottom - (int)(mainframe.Height * 0.125f), (int)(mainframe.Width * 0.1f), (int)(mainframe.Height * 0.08f));
            Rectangle promptborderframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.775f) - borderwidth, mainframe.Bottom - (int)(mainframe.Height * 0.125f) - borderwidth, (int)(mainframe.Width * 0.1f) + borderwidth * 2, (int)(mainframe.Height * 0.08f) + borderwidth * 2);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, promptborderframe, bordercolor);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, promptframe, Color.Black);
            Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, "Help", promptframe.X + promptframe.Width / 3, promptframe.Y + promptframe.Height / 3f, bordercolor * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);
            Rectangle maus = getMouse();

            string textwithoutspaces = TextInput.Replace(" ", string.Empty);
            if (TextInput.ToLower().Contains("gg") && !Anomaly109Manager.helpUnlocked && !TextInput.ToLower().Contains("fandom"))
            {
                SoundEngine.PlaySound(SoundID.Item4, Main.LocalPlayer.Center);
                Anomaly109Manager.helpUnlocked = true;
                CalRemixWorld.UpdateWorldBool();
            }
            if (maus.Intersects(promptborderframe))
            {
                if (Anomaly109Manager.helpUnlocked)
                {
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, "Click to open browser", (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 20, Color.White * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);

                    if (Main.mouseLeft && ClickCooldown <= 0)
                    {
                        ClickCooldown = 30;
                        SoundEngine.PlaySound(SoundID.MenuOpen);
                        Utils.OpenToURL("https://calamitymod.wiki.gg/wiki/User:Lilsigtum/Sandbox2");
                    }
                }
                else
                {
                    string prom = "To unlock help, answer the following: ";
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, prom, (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 20, Color.White * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, "Where is the Calamity Wiki hosted?", (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 20 + FontAssets.MouseText.Value.MeasureString(prom).Y + 4, Color.White * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                }
            }

        }

        private static void DrawOptions(SpriteBatch spriteBatch, Rectangle mainframe, out Rectangle optionRect, out Anomaly109Option selected, float bgWidth, float bgHeight)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
            int individualLength = (int)(bgWidth / 2 * 0.6f);
            int individualHeight = (int)(bgHeight / 4 * 0.4f);
            int spacingX = (int)(bgWidth / 3.15f);
            int spacingY = (int)(bgHeight / 7);
            int row = 1;
            int column = 1;
            selected = new Anomaly109Option("aa", "aaa", "aaaa", () => { }, Condition.ZenithWorld);
            optionRect = new Rectangle();
            for (int i = CurrentPage * 12; i < Anomaly109Manager.options.Count(); i++)
            {
                if (i >= CurrentPage * 12 + CurrentPage * 12 && i > 11)
                {
                    break;
                }
                column++;
                if (i % 3 == 0)
                {
                    column = 0;
                    row++;
                }
                Rectangle barframe = new Rectangle((int)(mainframe.X + mainframe.Width / 28 + 2) + spacingX * column, (int)(mainframe.Y + mainframe.Height / 48 + 2) + spacingY * row, individualLength, individualHeight);
                Rectangle barbg = new Rectangle((int)(mainframe.X + mainframe.Width / 28) + spacingX * column, (int)(mainframe.Y + mainframe.Height / 48) + spacingY * row, individualLength + 4, individualHeight + 4);

                Color outlineColor = Anomaly109Manager.options[i].unlocked ? Color.Lime : Color.Gray;
                Color pathColor = Anomaly109Manager.options[i].unlocked ? Color.Lime : Color.Gray;
                Color nameColor = Anomaly109Manager.options[i].unlocked ? new(83, 83, 249) : Color.DarkGray;
                bool putOnLock = false;
                if (!Anomaly109Manager.options[i].check.IsMet() && Anomaly109Manager.options[i].unlocked)
                {
                    outlineColor = Color.Red;
                }
                if (!Anomaly109Manager.options[i].unlocked)
                {
                    putOnLock = true;
                }
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, barbg, outlineColor);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, barframe, Color.Black);
                string address = "C:\\remix\\";

                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, address, barbg.X + barbg.Width / 32, barbg.Y + barbg.Height / 4, pathColor * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, Anomaly109Manager.options[i].title, barbg.X + barbg.Width / 32 + (float)FontAssets.MouseText.Value.MeasureString(address).X * (float)Main.screenWidth / (float)1745, barbg.Y + barbg.Height / 4, nameColor * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);

                if (maus.Intersects(barbg))
                {
                    selected = Anomaly109Manager.options[i];
                    optionRect = barbg;
                }

                if (putOnLock)
                {
                    Texture2D locke = ModContent.Request<Texture2D>("CalRemix/UI/Anomaly109/Anomalock").Value;
                    Rectangle lockrect = new Rectangle((int)(barbg.X + barbg.Width / 2.175f), (int)(barbg.Y + barbg.Height / 5), locke.Width * 2, locke.Height * 2);
                    spriteBatch.Draw(locke, lockrect, Color.White);
                }
            }
        }

        private static void SelectOption(SpriteBatch spriteBatch, Anomaly109Option option, Rectangle optionRect)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
            if (option.key != "aa")
            {
                if (maus.Intersects(optionRect))
                {
                    string status = "Enabled";
                    Color statusColor = Color.Lime;
                    string statusLiteral = "Status: ";
                    if (!option.unlocked)
                    {
                        status = "Locked";
                        statusColor = Color.Gray;
                    }
                    else if (!option.check.IsMet())
                    {
                        status = "Disabled";
                        statusColor = Color.Red;
                    }
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, option.message, (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 20, Color.White * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, statusLiteral, (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 52, Color.White * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, status, (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20 + (float)FontAssets.MouseText.Value.MeasureString(statusLiteral).X, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 52, statusColor * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                    if (Main.mouseLeft && ClickCooldown <= 0 && option.unlocked)
                    {
                        SoundEngine.PlaySound(CalamityMod.UI.DraedonSummoning.ExoMechSelectionUI.TwinsHoverSound);
                        option.toggle();
                        CalRemixWorld.UpdateWorldBool();
                        ClickCooldown = 8;
                    }
                }
            }
        }

        private static void DrawArrows(SpriteBatch spriteBatch, Rectangle mainframe)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
            int maxPages = Anomaly109Manager.options.Count() / 12;
            Rectangle arrowframer = new Rectangle(mainframe.Right - (int)(mainframe.Width * 0.0775f), mainframe.Bottom - (int)(mainframe.Height * 0.125f), (int)(mainframe.Width * 0.05f), (int)(mainframe.Width * 0.05f));
            Rectangle arrowframel = new Rectangle(mainframe.Left + (int)(mainframe.Width * 0.036f), mainframe.Bottom - (int)(mainframe.Height * 0.125f), (int)(mainframe.Width * 0.05f), (int)(mainframe.Width * 0.05f));
            if (CurrentPage < maxPages)
            {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, arrowframer, Color.Lime);
                //Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, "→", arrowframer.X + (int)(arrowframer.Width * 0.1f), arrowframer.Y + (int)(arrowframer.Height * 0.1f), Color.Black * (Main.mouseTextColor / 255f), Color.Lime, Vector2.Zero, 2 * (float)Main.screenWidth / (float)1745);
            }
            if (CurrentPage > 0)
            {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, arrowframel, Color.Lime);
                //Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, "←", arrowframel.X + (int)(arrowframel.Width * 0.1f), arrowframel.Y + (int)(arrowframel.Height * 0.1f), Color.Black * (Main.mouseTextColor / 255f), Color.Lime, Vector2.Zero, 2 * (float)Main.screenWidth / (float)1745);
            }
            bool holdingRight = false;
            if (maus.Intersects(arrowframer))
            {
                if (CurrentPage < maxPages)
                {
                    if (Main.mouseLeft && ClickCooldown <= 0)
                    {
                        Main.LocalPlayer.releaseUseItem = true;
                        CurrentPage++;
                        SoundEngine.PlaySound(CalamityMod.UI.DraedonSummoning.ExoMechSelectionUI.TwinsHoverSound);
                        ClickCooldown = 8;
                    }
                }
            }
            else
            if (maus.Intersects(arrowframel))
            {
                if (CurrentPage > 0)
                {
                    if (Main.mouseLeft && CurrentPage > 0 && ClickCooldown <= 0)
                    {
                        Main.LocalPlayer.releaseUseItem = true;
                        CurrentPage--;
                        SoundEngine.PlaySound(CalamityMod.UI.DraedonSummoning.ExoMechSelectionUI.TwinsHoverSound);
                        ClickCooldown = 8;
                    }
                }
                else if (Main.mouseLeft)
                {
                    Main.LocalPlayer.releaseUseItem = true;
                    HeldRightTimer++;
                    holdingRight = true;
                    if (HeldRightTimer > 300)
                    {
                        CurrentPage = maxPages + 1;
                        SoundEngine.PlaySound(CalamityMod.UI.DraedonSummoning.ExoMechSelectionUI.TwinsHoverSound with { Pitch = -1f });
                        HeldRightTimer = 0;
                        ClickCooldown = 40;
                    }
                }
            }
            if (ClickCooldown > 0)
            {
                ClickCooldown--;
            }
            if (HeldRightTimer > 0 && !holdingRight)
            {
                HeldRightTimer = 0;
            }
        }

        private static void DrawFanny(SpriteBatch spriteBatch, Rectangle mainframe)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);

            Texture2D cage = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Magic/IceBlock").Value;
            Texture2D fanny = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/FannyIdle").Value;

            Rectangle cageframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.375f), mainframe.Y + (int)(mainframe.Height * 0.2f), (int)(mainframe.Width * 0.25f), (int)(mainframe.Height * 0.4f));
            Rectangle bgframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.375f), mainframe.Y + (int)(mainframe.Height * 0.65f), (int)(mainframe.Width * 0.25f), (int)(mainframe.Height * 0.1f));
            Rectangle borderframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.375f) - 2, mainframe.Y + (int)(mainframe.Height * 0.65f) - 2, (int)(mainframe.Width * 0.25f) + 4, (int)(mainframe.Height * 0.1f) + 4);
            Rectangle fannyframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.375f + fannyOffset.X), mainframe.Y + (int)(mainframe.Height * 0.22f + fannyOffset.Y), (int)(mainframe.Width * 0.25f), (int)(mainframe.Height * 0.35f));
            Rectangle fannytheFrame = fanny.Frame(1, 8, 0, 0);

            string FannyStatus = ScreenHelperManager.screenHelpersEnabled ? "Fanny is currently free" : "Fanny is currently sealed";
            Color FannyColor = ScreenHelperManager.screenHelpersEnabled ? Color.Lime : Color.Red;

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, borderframe, FannyColor);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, bgframe, Color.Black);
            if (!ScreenHelperManager.screenHelpersEnabled)
            {
                spriteBatch.Draw(fanny, fannyframe, fannytheFrame, Color.White);
            }
            spriteBatch.Draw(cage, cageframe, Color.White * 0.6f);
            Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, FannyStatus, bgframe.X + (int)(bgframe.Height * 0.3f), bgframe.Y + (int)(bgframe.Height * 0.3f), FannyColor * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);
            fannyOffset = Vector2.Zero;
            if (maus.Intersects(bgframe))
            {
                if (ScreenHelperManager.screenHelpersEnabled)
                {
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, "Toggles Fanny", (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 20, Color.White * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                }
                else
                {
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, "KILL KILL KILL KILL KILL", (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 20, Color.DarkRed * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                }
                if (Main.mouseLeft && ClickCooldown <= 0)
                {
                    ScreenHelperManager.screenHelpersEnabled = !ScreenHelperManager.screenHelpersEnabled;
                    CalRemixWorld.UpdateWorldBool();
                    ClickCooldown = 12;
                    if (ScreenHelperManager.screenHelpersEnabled)
                    {
                        fannyFreezeTime = 0;
                        SoundEngine.PlaySound(SoundID.Cockatiel with { MaxInstances = 0, Volume = 0.3f, Pitch = -0.8f });
                        SoundEngine.PlaySound(SoundID.Item27);
                        SoundEngine.PlaySound(SoundID.Item62);
                    }
                    else
                    {
                        SoundEngine.PlaySound(SoundID.DD2_GoblinScream);
                        SoundEngine.PlaySound(SoundID.Item28);
                        SoundEngine.PlaySound(CalamityMod.NPCs.Cryogen.Cryogen.ShieldRegenSound);
                    }

                }
            }
            if (maus.Intersects(fannyframe) && !ScreenHelperManager.screenHelpersEnabled && Main.mouseLeft && ClickCooldown <= 0)
            {
                SoundEngine.PlaySound(CalamityMod.NPCs.Cryogen.Cryogen.HitSound);
                fannyOffset += new Vector2(Main.rand.NextFloat(-2.5f, 2.5f), Main.rand.NextFloat(-2.5f, 2.5f));
                ClickCooldown = 4;
            }
        }

        private static void BlockClicks(Rectangle borderframe)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
            if (maus.Intersects(borderframe))
            {
                Main.blockMouse = true;
            }
        }

        private static Rectangle getMouse()
        {
            return new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
        }
    }

    public class Anomaly109Manager : ModSystem
    {
        public static List<Anomaly109Option> options = new List<Anomaly109Option> { };
        public static bool helpUnlocked = false;
        public override void OnWorldLoad()
        {
            if (options.Count == 0)
            {
                options.Add(new Anomaly109Option("emitem", "item_changes", "Toggles visual changes for certain items and projectiles", () =>
                {
                    if (!CalRemixWorld.itemChanges)
                    {
                        foreach (KeyValuePair<int, string> p in RethemeList.Items)
                        {
                            TextureAssets.Item[p.Key] = ModContent.Request<Texture2D>("CalRemix/Retheme/" + p.Value);
                        }
                        foreach (KeyValuePair<int, string> p in RethemeList.Projs)
                        {
                            TextureAssets.Projectile[p.Key] = ModContent.Request<Texture2D>("CalRemix/Retheme/" + p.Value);
                        }
                        Main.RegisterItemAnimation(ModContent.ItemType<WulfrumMetalScrap>(), new DrawAnimationVertical(6, 16));
                    }
                    else
                    {
                        foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.Items)
                        {
                            TextureAssets.Item[p.Key] = p.Value;
                        }
                        foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.Projs)
                        {
                            TextureAssets.Projectile[p.Key] = p.Value;
                        }
                        Main.RegisterItemAnimation(ModContent.ItemType<WulfrumMetalScrap>(), new DrawAnimationVertical(1, 1));
                    }
                    CalRemixWorld.itemChanges = !CalRemixWorld.itemChanges;
                }, new Condition("", () => CalRemixWorld.itemChanges)));
                options.Add(new Anomaly109Option("creativefreedom", "npc_changes", "Toggles visual changes for some (not all) NPCs and bosses", () =>
                {
                    if (!CalRemixWorld.npcChanges)
                    {
                        foreach (KeyValuePair<int, string> p in RethemeList.NPCs)
                        {
                            TextureAssets.Npc[p.Key] = ModContent.Request<Texture2D>("CalRemix/Retheme/" + p.Value);
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<int, Asset<Texture2D>> p in RethemeMaster.NPCs)
                        {
                            TextureAssets.Npc[p.Key] = p.Value;
                        }
                    }
                    CalRemixWorld.npcChanges = !CalRemixWorld.npcChanges;
                }, new Condition("", () => CalRemixWorld.npcChanges)));
                options.Add(new Anomaly109Option("talkywalky", "boss_dialogue", "Toggles boss dialogue", () => { CalRemixWorld.bossdialogue = !CalRemixWorld.bossdialogue; }, new Condition("", () => CalRemixWorld.bossdialogue)));
                options.Add(new Anomaly109Option("space", "remix_jump", "Toggles the default double jump", () => { CalRemixWorld.remixJump = !CalRemixWorld.remixJump; }, new Condition("", () => CalRemixWorld.remixJump)));

                options.Add(new Anomaly109Option("bloodorange", "permanent_upgrades", "Toggles permanent upgrade recipe removals and alt obtainment methods", () => { CalRemixWorld.permanenthealth = !CalRemixWorld.permanenthealth; }, new Condition("", () => CalRemixWorld.permanenthealth)));
                options.Add(new Anomaly109Option("terragrim", "alloy_bars", "Toggles Alloy Bars from recipes", () => { Recipes.MassModifyIngredient(CalRemixWorld.alloyBars, Recipes.alloyBarCrafts); CalRemixWorld.alloyBars = !CalRemixWorld.alloyBars; }, new Condition("", () => CalRemixWorld.alloyBars)));
                options.Add(new Anomaly109Option("starfury", "essential_essence_bars", "Toggles Essential Essence Bars from recipes", () => { Recipes.MassModifyIngredient(CalRemixWorld.essenceBars, Recipes.essenceBarCrafts); CalRemixWorld.essenceBars = !CalRemixWorld.essenceBars; }, new Condition("", () => CalRemixWorld.essenceBars)));
                options.Add(new Anomaly109Option("defiledgreatsword", "yharim_bars", "Toggles Yharim Bars from recipes", () => { Recipes.MassModifyIngredient(CalRemixWorld.yharimBars, Recipes.yharimBarCrafts); CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, new Condition("", () => CalRemixWorld.yharimBars)));
                options.Add(new Anomaly109Option("babilzot", "shimmer_essences", "Toggles Shimmer Essences from recipes", () => { Recipes.MassModifyIngredient(CalRemixWorld.shimmerEssences, Recipes.shimmerEssenceCrafts); CalRemixWorld.shimmerEssences = !CalRemixWorld.shimmerEssences; }, new Condition("", () => CalRemixWorld.shimmerEssences)));
                options.Add(new Anomaly109Option("leviathan", "crocodile_scales", "Toggles Crocodile Scales from recipes", () => { Recipes.MassModifyIngredient(CalRemixWorld.crocodile, Recipes.crocodileCrafts); CalRemixWorld.crocodile = !CalRemixWorld.crocodile; }, new Condition("", () => CalRemixWorld.crocodile)));
                options.Add(new Anomaly109Option("ceaselessvoid", "coyote_venom", "Toggles Coyote Venom drops and recipe injections", () => { Recipes.MassModifyIngredient(CalRemixWorld.wolfvenom, Recipes.venomCrafts); CalRemixWorld.wolfvenom = !CalRemixWorld.wolfvenom; }, new Condition("", () => CalRemixWorld.wolfvenom)));
                options.Add(new Anomaly109Option("flashdrive", "rear_gars", "Toggles Rear Gars and Uelibloom Ore removal", () =>
                {
                    if (!CalRemixWorld.reargar)
                    {
                        CalRemixWorld.RemoveLoot(ItemID.JungleFishingCrate, ModContent.ItemType<UelibloomOre>(), false);
                        CalRemixWorld.RemoveLoot(ItemID.JungleFishingCrateHard, ModContent.ItemType<UelibloomOre>(), false);
                        CalRemixWorld.RemoveLoot(ItemID.JungleFishingCrate, ModContent.ItemType<UelibloomBar>(), false);
                        CalRemixWorld.RemoveLoot(ItemID.JungleFishingCrateHard, ModContent.ItemType<UelibloomBar>(), false);
                    }
                    else
                    {
                        CalRemixWorld.AddLootDynamically(ItemID.JungleFishingCrate);
                    }
                    CalRemixWorld.reargar = !CalRemixWorld.reargar; 
                }, new Condition("", () => CalRemixWorld.reargar)));
                options.Add(new Anomaly109Option("driveflash", "side_gars", "Toggles Side Gars and Galactica Singularity recipe removal",  () => { CalRemixWorld.sidegar = !CalRemixWorld.sidegar; }, new Condition("", () => CalRemixWorld.sidegar)));
                options.Add(new Anomaly109Option("reapershark", "front_gars", "Toggles Front Gars and Reaper Tooth drop removal", () => 
                { 
                    if (!CalRemixWorld.frontgar) 
                    { 
                        CalRemixWorld.RemoveLoot(ModContent.NPCType<ReaperShark>(), ModContent.ItemType<ReaperTooth>(), true); 
                        CalRemixWorld.RemoveLoot(ModContent.ItemType<SulphurousCrate>(), ModContent.ItemType<ReaperTooth>(), false); } 
                    else 
                    { 
                        CalRemixWorld.AddLootDynamically(ModContent.NPCType<ReaperShark>(), true); 
                        CalRemixWorld.AddLootDynamically(ModContent.ItemType<SulphurousCrate>()); 
                    } 
                    CalRemixWorld.frontgar = !CalRemixWorld.frontgar; 
                }, new Condition("", () => CalRemixWorld.frontgar)));
                options.Add(new Anomaly109Option("passive", "seafood", "Toggles Seafood becoming a normal food item", () => { CalRemixWorld.seafood = !CalRemixWorld.seafood; }, new Condition("", () => CalRemixWorld.seafood)));
                options.Add(new Anomaly109Option("stellarculex", "starbuster_core", "Toggles the Starbuster Core's strange obtainment method", () => { CalRemixWorld.starbuster = !CalRemixWorld.starbuster; }, new Condition("", () => CalRemixWorld.starbuster)));
                options.Add(new Anomaly109Option("summoner", "fearmonger_retier", "Toggles the Fearmonger set's tiershifted stats", () => { CalRemixWorld.fearmonger = !CalRemixWorld.fearmonger; }, new Condition("", () => CalRemixWorld.fearmonger)));

                options.Add(new Anomaly109Option("meldosaurus", "meld_gunk", "Toggles Meld Gunk initial generation and spread", () => { CalRemixWorld.meldGunk = !CalRemixWorld.meldGunk; }, new Condition("", () => CalRemixWorld.meldGunk)));
                options.Add(new Anomaly109Option("beewasp", "plagued_jungle", "Toggles the initial generation of the Plagued Jungle and related requirements", () => { CalRemixWorld.plaguetoggle = !CalRemixWorld.plaguetoggle; }, new Condition("", () => CalRemixWorld.plaguetoggle)));
                options.Add(new Anomaly109Option("shrineys", "hardmode_shrines", "Toggles the initial generation for Hardmode shrines", () => { CalRemixWorld.shrinetoggle = !CalRemixWorld.shrinetoggle; }, new Condition("", () => CalRemixWorld.shrinetoggle)));
                options.Add(new Anomaly109Option("livinglife", "life_ore", "Toggles the initial generation for Life Ore", () => { CalRemixWorld.lifeoretoggle = !CalRemixWorld.lifeoretoggle; }, new Condition("", () => CalRemixWorld.lifeoretoggle)));
                options.Add(new Anomaly109Option("grimethegame", "grimesand", "Toggles generation of Grimesand and its requirement for evil 2 bosses", () => { CalRemixWorld.grimesandToggle = !CalRemixWorld.grimesandToggle; }, new Condition("", () => CalRemixWorld.grimesandToggle)));
                options.Add(new Anomaly109Option("thedevourerofgods", "cosmilite_slag", "Toggles initial generation of Cosmilite Slag and nerfed Cosmilite gear", () =>
                {
                    if (!CalRemixWorld.cosmislag)
                    {
                        CalRemixWorld.RemoveLoot(ModContent.NPCType<DevourerofGodsHead>(), ModContent.ItemType<CosmiliteBar>(), true);
                        CalRemixWorld.RemoveLoot(ModContent.ItemType<DevourerofGodsBag>(), ModContent.ItemType<CosmiliteBar>(), false);
                    }
                    else
                    {
                        CalRemixWorld.AddLootDynamically(ModContent.NPCType<DevourerofGodsHead>(), true);
                        CalRemixWorld.AddLootDynamically(ModContent.ItemType<DevourerofGodsBag>());
                    }
                    CalRemixWorld.cosmislag = !CalRemixWorld.cosmislag;
                }, new Condition("", () => CalRemixWorld.cosmislag)));

                options.Add(new Anomaly109Option("eyespy", "acidsighter", "Toggles initial acid rain requirement change", () => { CalRemixWorld.acidsighter = !CalRemixWorld.acidsighter; }, new Condition("", () => CalRemixWorld.acidsighter)));
                options.Add(new Anomaly109Option("eleum", "primal_aspid", "Toggles Primal Aspids and the Cryo Key recipe removal", () => { CalRemixWorld.aspids = !CalRemixWorld.aspids; }, new Condition("", () => CalRemixWorld.aspids)));
                options.Add(new Anomaly109Option("havoc", "clamitas", "Toggles the Clamitas miniboss and the Eye of Desolation recipe removal", () => { CalRemixWorld.clamitas = !CalRemixWorld.clamitas; }, new Condition("", () => CalRemixWorld.clamitas)));
                options.Add(new Anomaly109Option("applesand", "banana_clown", "Toggles Banana Clowns", () => { CalRemixWorld.clowns = !CalRemixWorld.clowns; }, new Condition("", () => CalRemixWorld.clowns)));
                options.Add(new Anomaly109Option("banban", "green_demon", "Toggles Green Demons", () => { CalRemixWorld.greenDemon = !CalRemixWorld.greenDemon; }, new Condition("", () => CalRemixWorld.greenDemon)));
                options.Add(new Anomaly109Option("thesealed", "la_ruga", "...", () => { CalRemixWorld.laruga = !CalRemixWorld.laruga; }, new Condition("", () => CalRemixWorld.laruga)));
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            for (int i = 0; i < options.Count; i++)
            {
                Anomaly109Option msg = options[i];
                if (msg.unlocked)
                    tag["Anomaly109" + msg.title] = true;
            }
            tag["help109"] = helpUnlocked;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            for (int i = 0; i < options.Count; i++)
            {
                Anomaly109Option msg = options[i];
                options[i].unlocked = tag.ContainsKey("Anomaly109" + msg.title);
            }
            helpUnlocked = tag.ContainsKey("help109");
        }
    }

    public class Anomaly109Option
    {
        public string title { get; set; }
        public string message { get; set; }
        public string key { get; set; }

        public Action toggle { get; set; }

        public Condition check { get; set; }

        public bool unlocked {  get; set; }

        public Anomaly109Option(string key, string title, string message, Action toggle, Condition check, bool unlocked = false)
        {
            this.key = key;
            this.title = title;
            this.message = message;
            this.toggle = toggle;
            this.unlocked = unlocked;
            this.check = check;
        }


    }

    [Autoload(Side = ModSide.Client)]
    internal class Anomaly109System : ModSystem
    {
        private UserInterface Anomaly109UserInterface;

        internal Anomaly109UI Anoui;

        public static LocalizedText ExampleResourceText { get; private set; }

        public override void Load()
        {
            Anoui = new();
            Anomaly109UserInterface = new();
            Anomaly109UserInterface.SetState(Anoui);

            string category = "UI";
            ExampleResourceText ??= Mod.GetLocalization($"{category}.ExampleResource");
        }

        public override void UpdateUI(GameTime gameTime)
        {
            Anomaly109UserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "ExampleMod: Example Resource Bar",
                    delegate {
                        Anomaly109UserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}