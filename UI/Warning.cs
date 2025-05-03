using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    [Autoload(Side = ModSide.Client)]
    public class Warning : ModSystem
    {
        public override void Load()
        {
            On_Main.DoDraw += DrawWarning;
        }
        private static void DrawWarning(On_Main.orig_DoDraw orig, Main self, GameTime gameTime)
        {
            orig(self, gameTime);
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
            if (!Main.gameMenu && Main.LocalPlayer != null)
            {
                if (Main.LocalPlayer.TryGetModPlayer(out CalRemixPlayer p) && p.onFandom > 0)
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
            spriteBatch.End();
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
            if (fandomFound)
                remixPlayer.onFandom = 300;
        }
    }
}
