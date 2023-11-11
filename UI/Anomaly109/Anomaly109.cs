using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Placeables.FurnitureAbyss;
using CalamityMod.Items.Potions;
using CalamityMod.NPCs.Yharon;
using CalRemix.Items.Materials;
using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI
{
    public class Anomaly109UI : UIState
    {
        public static int CurrentPage = 0;
        public static int ClickCooldown = 0;
        public override void Update(GameTime gameTime)
        {
            bool shouldShow = !Main.gameMenu;


            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            // This prevents drawing unless we are using an ExampleCustomResourceWeapon
            if (Main.gameMenu)
                return;

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float bgWidth = Main.screenWidth * 0.6f;
            float bgHeight = Main.screenHeight * 0.7f;
            DrawBackground(spriteBatch, bgWidth, bgHeight, out Rectangle borderframe, out Rectangle mainframe);
            BlockClicks(borderframe);
            Anomaly109Option selected = new Anomaly109Option("aa", "aaa", "aaaaa", () => { });
            Rectangle barrect2 = new Rectangle();
            DrawOptions(spriteBatch, mainframe, out barrect2, out selected, bgWidth, bgHeight);
            DrawArrows(spriteBatch, mainframe);
            SelectOption(spriteBatch, selected, barrect2);
        }

        private static void BlockClicks(Rectangle borderframe)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
            if (maus.Intersects(borderframe))
            {
                Main.blockMouse = true;
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

        private static void DrawOptions(SpriteBatch spriteBatch, Rectangle mainframe, out Rectangle optionRect, out Anomaly109Option selected, float bgWidth, float bgHeight)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
            int individualLength = (int)(bgWidth / 2 * 0.6f);
            int individualHeight = (int)(bgHeight / 4 * 0.4f);
            int spacingX = (int)(bgHeight / 1.9f);
            int spacingY = (int)(bgHeight / 7);
            int row = 1;
            int column = 1;
            selected = new Anomaly109Option("aa", "aaa", "aaaa", () => { });
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
                Rectangle barframe = new Rectangle((int)(mainframe.X + mainframe.Width / 28 + 2) + spacingX * column, (int)(mainframe.Y + mainframe.Height / 16 + 2) + spacingY * row, individualLength, individualHeight);
                Rectangle barbg = new Rectangle((int)(mainframe.X + mainframe.Width / 28) + spacingX * column, (int)(mainframe.Y + mainframe.Height / 16) + spacingY * row, individualLength + 4, individualHeight + 4);

                spriteBatch.Draw(TextureAssets.MagicPixel.Value, barbg, Color.Lime);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, barframe, Color.Black);
                string address = "C:\\remix\\";

                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, address, barbg.X + barbg.Width / 32, barbg.Y + barbg.Height / 4, Color.Lime * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, Anomaly109Manager.options[i].title, barbg.X + barbg.Width / 32 + (float)FontAssets.MouseText.Value.MeasureString(address).X * (float)Main.screenWidth / (float)1745, barbg.Y + barbg.Height / 4, Color.MediumBlue * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);

                if (maus.Intersects(barbg))
                {
                    selected = Anomaly109Manager.options[i];
                    optionRect = barbg;
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
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, option.message, (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 20, Color.White * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                    if (Main.mouseLeft && ClickCooldown <= 0)
                    {
                        option.toggle();
                        ClickCooldown = 8;
                    }
                }
            }
        }

        private static void DrawArrows(SpriteBatch spriteBatch, Rectangle mainframe)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
            int maxPages = Anomaly109Manager.options.Count() / 12;
            Rectangle arrowframer = new Rectangle(mainframe.Right - (int)(mainframe.Width * 0.1f), mainframe.Bottom - (int)(mainframe.Height * 0.1f), (int)(mainframe.Width * 0.05f), (int)(mainframe.Width * 0.05f));
            Rectangle arrowframel = new Rectangle(mainframe.Left + (int)(mainframe.Width * 0.1f), mainframe.Bottom - (int)(mainframe.Height * 0.1f), (int)(mainframe.Width * 0.05f), (int)(mainframe.Width * 0.05f));
            if (CurrentPage < maxPages)
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, arrowframer, Color.Lime);
            if (CurrentPage > 0)
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, arrowframel, Color.Lime);
            if (maus.Intersects(arrowframer))
            {
                if (CurrentPage < maxPages)
                {
                    if (Main.mouseLeft && ClickCooldown <= 0)
                    {
                        Main.LocalPlayer.releaseUseItem = true;
                        CurrentPage++;
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
                        ClickCooldown = 8;
                    }
                }
            }
            if (ClickCooldown > 0)
            {
                ClickCooldown--;
            }
        }
    }

    public class Anomaly109Manager : ModSystem
    {
        public static List<Anomaly109Option> options = new List<Anomaly109Option> { };
        public override void OnWorldLoad()
        {
            if (options.Count == 0)
            {
                options.Add(new Anomaly109Option("934832020", "alloy_bars", "Toggles Alloy Bars from recipes", () => { CalRemixWorld.alloyBars = !CalRemixWorld.alloyBars; }));
                options.Add(new Anomaly109Option("302588271", "essential_essence_bars", "Toggles Essential Essence Bars from recipes", () => { CalRemixWorld.essenceBars = !CalRemixWorld.essenceBars; }));
                options.Add(new Anomaly109Option("034683710", "yharim_bars", "Toggles Yharim Bars from recipes", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "shimmer_essences", "Toggles Shimmer Essences from recipes", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "cosmilite_slag", "Toggles tiershifted Cosmilite", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "rear_gars", "Toggles Rear Gars and Uelibloom Ore removal", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "side_gars", "Toggles Side Gars and Galactica Singularity recipe removal",  () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "front_gars", "Toggles Front Gars and Reaper Tooth drops", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "meld_gunk", "Toggles Meld Gunk", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "crocodile_scales", "Toggles Crocodile Scales from recipes", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "permanent_upgrades", "Toggles permanent upgrade recipes and alt obtainment methods", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "starbuster_core", "Toggles the Starbuster Core's strange obtainment method", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "plagued_jungle", "Toggles generation of the Plagued Jungle and related requirements", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "hardmode_shrines", "Toggles generation for hardmode shrines", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "life_ore", "Toggles generation for Life Ore", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "resprites", "Toggles resprites for bosses and items", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "boss_dialogue", "Toggles boss dialogue", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "grimesand", "Toggles generation of Grimesand and its requirement for evil 2 bosses", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "banana_clown", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "primal_aspid", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "clamitas", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "coyote_venom", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "parched_scales", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "fearmonger_retier", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "idkrn", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "idkrn2", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "idkrn3", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
                options.Add(new Anomaly109Option("034683710", "la_ruga", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
            }
        }
    }

    public class Anomaly109Option
    {
        public string title { get; set; }
        public string message { get; set; }
        public string key { get; set; }

        public Action toggle { get; set; }

        public Anomaly109Option(string key, string title, string message, Action toggle)
        {
            this.key = key;
            this.title = title;
            this.message = message;
            this.toggle = toggle;
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