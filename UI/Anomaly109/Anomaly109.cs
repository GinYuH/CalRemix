using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Potions;
using CalamityMod.NPCs.Yharon;
using CalRemix.Items.Materials;
using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using static System.Net.Mime.MediaTypeNames;

namespace CalRemix.UI
{
    public class Anomaly109UI : UIState
    {
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
            int borderwidth = 4;
            float bgWidth = Main.screenWidth * 0.6f;
            float bgHeight = Main.screenHeight * 0.7f;
            Rectangle mainframe = new Rectangle((int)(Main.screenWidth - bgWidth) / 2, (int)(Main.screenHeight - bgHeight) / 2, (int)bgWidth, (int)bgHeight);
            Rectangle borderframe = new Rectangle((int)(Main.screenWidth - bgWidth) / 2 - borderwidth, (int)(Main.screenHeight - bgHeight) / 2 - borderwidth, (int)bgWidth + borderwidth * 2, (int)bgHeight + borderwidth * 2);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, borderframe, Color.Lime);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, mainframe, Color.Black);

            int individualLength = (int)(bgWidth / 2 * 0.8f);
            int individualHeight = (int)(bgWidth / 7 * 0.4f);
            int spacingY = (int)(bgHeight / 7);
            for (int i = 0; i < Anomaly109Manager.options.Count(); i++)
            {
                Rectangle barframe = new Rectangle((int)(mainframe.X + mainframe.Width / 22 + 2), (int)(mainframe.Y + mainframe.Height / 14 + 2) + spacingY * i, individualLength, individualHeight);
                Rectangle barbg = new Rectangle((int)(mainframe.X + mainframe.Width / 22), (int)(mainframe.Y + mainframe.Height / 14) + spacingY * i, individualLength + 4, individualHeight + 4);
                int count = Main.itemAnimations[Anomaly109Manager.options[i].icon] != null ? Main.itemAnimations[Anomaly109Manager.options[i].icon].FrameCount : 1;
                Texture2D itemSprite = TextureAssets.Item[Anomaly109Manager.options[i].icon].Value;

                float iwidth = itemSprite.Width;
                float iheight = itemSprite.Height / count;
                bool crunchTop = true;
                if (iwidth > iheight)
                {
                    crunchTop = false;
                }
                if (crunchTop)
                {
                    float divisor = iheight / barbg.Height / 0.6f;
                    iwidth /= divisor;
                    iheight /= divisor;
                }
                else
                {
                    float divisor = iwidth / barbg.Height / 0.6f;
                    iwidth /= divisor;
                    iheight /= divisor;
                }
                Rectangle itemframe = new Rectangle(barbg.Right - barbg.Width / 8, (int)(barbg.Top + barbg.Height / 3.5f), (int)iwidth, (int)iheight);
                Rectangle nframe = itemSprite.Frame(1, count, 0, 0);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, barbg, Color.Lime);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, barframe, Color.Black);
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, Anomaly109Manager.options[i].title, barbg.X + barbg.Width / 32, barbg.Y + barbg.Height / 4, Color.Lime * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);
                spriteBatch.Draw(TextureAssets.Item[Anomaly109Manager.options[i].icon].Value, itemframe, nframe, Color.White);

                Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 20, 20);
                if (maus.Intersects(barframe))
                {
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, Anomaly109Manager.options[i].message, (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 20, Color.White * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                    if (Main.LocalPlayer.controlUseItem)
                    {
                        Anomaly109Manager.options[i].toggle(true);
                    }
                }
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
                options.Add(new Anomaly109Option("934832020", "Alloy Bars", "Toggles Alloy Bars from recipes", ModContent.ItemType<AlloyBar>(), (bool enabled) => { CalRemixWorld.alloyBars = !CalRemixWorld.alloyBars; }));
                options.Add(new Anomaly109Option("302588271", "Essential Essence Bars", "Toggles Essential Essence Bars from recipes", ModContent.ItemType<EssentialEssenceBar>(), (bool enabled) => { CalRemixWorld.essenceBars = !CalRemixWorld.essenceBars; }));
                options.Add(new Anomaly109Option("034683710", "Yharim Bars", "Toggles Yharim Bars from recipes", ModContent.ItemType<YharimBar>(), (bool enabled) => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }));
            }
        }
    }

    public class Anomaly109Option
    {
        public string title { get; set; }
        public string message { get; set; }
        public int icon { get; set; }
        public string key { get; set; }

        public Action<bool> toggle { get; set; }

        public Anomaly109Option(string key, string title, string message, int icon, Action<bool> toggle)
        {
            this.key = key;
            this.title = title;
            this.message = message;
            this.icon = icon;
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