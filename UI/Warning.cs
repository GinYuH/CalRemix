using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI
{
    public class WarningUI : UIState
    {
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.gameMenu)
                return;
            if (Main.LocalPlayer is null)
                return;
            if (!Main.LocalPlayer.TryGetModPlayer(out CalRemixPlayer p) || p.onFandom <= 0)
                return;
            base.Draw(spriteBatch);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            string nofandom = CalRemixHelper.LocalText("UI.FandomWarning").Value;
            float fandomwidth = FontAssets.MouseText.Value.MeasureString(nofandom).X;
            float fandomheight = FontAssets.MouseText.Value.MeasureString(nofandom).Y;
            float scale = (Main.screenWidth / fandomwidth) * 0.75f + (float)Math.Cos(Main.GlobalTimeWrappedHourly * 22) * 0.1f;
            float scaledWidth = fandomwidth * scale;
            float scaledHeight = fandomheight * scale;
            Utils.DrawBorderString(spriteBatch, nofandom, new Vector2((Main.screenWidth - scaledWidth) / 2, (Main.screenHeight - scaledHeight) / 2) + Main.rand.NextVector2Square(-10, 10), Color.Red, scale);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth * 4, Main.screenHeight * 4), null, Color.Red * 0.22f, 0f, TextureAssets.MagicPixel.Value.Size() * 0.5f, 0, 0f);
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class Warning : ModSystem
    {
        private UserInterface WarningUserInterface;
        internal WarningUI WarningState;
        public override void Load()
        {
            WarningState = new();
            WarningUserInterface = new();
            WarningUserInterface.SetState(WarningState);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            WarningUserInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int layerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Over"));
            if (layerIndex != -1)
            {
                layers.Insert(layerIndex, new LegacyGameInterfaceLayer("CalRemix: FandomWarning",
                    delegate
                    {
                        WarningUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }
        public static void CheckOnFandom(Player player)
        {
            bool fandomFound = false;
            CalRemixPlayer remixPlayer = player.GetModPlayer<CalRemixPlayer>();
            try
            {
                System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process p in processes)
                {
                    if (!string.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        if (p.MainWindowTitle.Contains("Calamity Mod Wiki") && !p.MainWindowTitle.Contains("Official"))
                        {
                            fandomFound = true;
                            break;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            remixPlayer.onFandom = (fandomFound) ? 300 : -1;
        }
    }
}
