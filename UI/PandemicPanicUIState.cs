using CalRemix.Content.NPCs.PandemicPanic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalamityMod.UI;

namespace CalRemix.UI
{
    public static class PandemicPanicUIState
    {
        public static bool IsActive => PandemicPanic.CountsAsActive;
        public static float CompletionRatio => PandemicPanic.InvadersWinning ? (PandemicPanic.DefendersKilled / PandemicPanic.MaxRequired) : PandemicPanic.DefendersWinning ? (PandemicPanic.InvadersKilled / PandemicPanic.MinToSummonPathogen) : 0;

        public static string InvasionName => PandemicPanic.InvadersWinning ? CalRemixHelper.LocalText("UI.PandemicPanic.Invader").Value : PandemicPanic.DefendersWinning ? CalRemixHelper.LocalText("UI.PandemicPanic.Defender").Value : CalRemixHelper.LocalText("UI.PandemicPanic.None").Value;
        public static Color InvasionBarColor => PandemicPanic.InvadersWinning ? Color.Red : PandemicPanic.DefendersWinning ? Color.Lime : Color.White;
        public static Texture2D IconTexture => PandemicPanic.InvadersWinning ? ModContent.Request<Texture2D>("CalRemix/Content/NPCs/PandemicPanic/RedBloodCell").Value : PandemicPanic.DefendersWinning ? ModContent.Request<Texture2D>("CalRemix/Content/NPCs/PandemicPanic/Ecolium").Value : ModContent.Request<Texture2D>("CalRemix/Content/NPCs/PandemicPanic/RedBloodCell").Value;

        public static void DrawBlueBar(SpriteBatch spriteBatch, Vector2 barDrawPosition, int barOffsetY)
        {
            int barWidth = 200;
            int barHeight = 45;

            barDrawPosition.Y += barOffsetY;

            Rectangle screenCoordsRectangle = new Rectangle((int)barDrawPosition.X - barWidth / 2, (int)barDrawPosition.Y - barHeight / 2, barWidth, barHeight);
            Texture2D barTexture = TextureAssets.ColorBar.Value;

            Utils.DrawInvBG(spriteBatch, screenCoordsRectangle, new Color(63, 65, 151, 255) * 0.785f);
            spriteBatch.Draw(barTexture, barDrawPosition, null, Color.White, 0f, new Vector2(barTexture.Width / 2, 0f), 1f, SpriteEffects.None, 0f);
        }
        public static void DrawProgressText(SpriteBatch spriteBatch, float yScale, Vector2 baseBarDrawPosition, int barOffsetY, out Vector2 newBarPosition)
        {
            Color c = PandemicPanic.SummonedPathogen && PandemicPanic.LockedFinalSide == 1 ? Main.DiscoColor : Color.White;
            string progressText = !PandemicPanic.DefendersWinning && !PandemicPanic.InvadersWinning ? "???" : (100 * CompletionRatio).ToString($"N{0}") + "%";
            progressText = Language.GetTextValue("Game.WaveCleared", progressText);

            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(progressText);
            float progressTextScale = 1f;
            if (textSize.Y > 22f)
                progressTextScale *= 22f / textSize.Y;

            newBarPosition = baseBarDrawPosition + Vector2.UnitY * (yScale + barOffsetY);
            Utils.DrawBorderString(spriteBatch, progressText, newBarPosition - Vector2.UnitY * 4f, c, progressTextScale, 0.5f, 1f, -1);
            string defKills = PandemicPanic.DefendersKilled.ToString();
            string invKills = PandemicPanic.InvadersKilled.ToString();
            Utils.DrawBorderString(spriteBatch, defKills, newBarPosition - 80 * Vector2.UnitX, Color.Lime, progressTextScale, 0.5f, 1f, -1);
            Utils.DrawBorderString(spriteBatch, invKills, newBarPosition + 80 * Vector2.UnitX, Color.Red, progressTextScale, 0.5f, 1f, -1);
        }
        public static void DrawBackground(SpriteBatch spriteBatch, float yScale, Vector2 baseBarDrawPosition, int barOffsetY)
        {
            float barDrawOffsetX = 169f;
            bool neutral = !PandemicPanic.DefendersWinning && !PandemicPanic.InvadersWinning;
            float comp = neutral ? 0.5f : CompletionRatio;
            Vector2 barDrawPosition = baseBarDrawPosition + Vector2.UnitX * (CompletionRatio - 0.5f) * barDrawOffsetX;
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, barDrawPosition, new Rectangle(0, 0, 1, 1), new Color(255, 241, 51), 0f, new Vector2(1f, 0.5f), new Vector2(barDrawOffsetX * CompletionRatio, yScale), SpriteEffects.None, 0f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, barDrawPosition, new Rectangle(0, 0, 1, 1), new Color(255, 165, 0, 127), 0f, new Vector2(1f, 0.5f), new Vector2(2f, yScale), SpriteEffects.None, 0f);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, barDrawPosition, new Rectangle(0, 0, 1, 1), Color.Black, 0f, Vector2.UnitY * 0.5f, new Vector2(barDrawOffsetX * (1f - CompletionRatio), yScale), SpriteEffects.None, 0f);
        }
        public static void DrawProgressTextAndIcons(SpriteBatch spriteBatch, int barOffsetY)
        {
            Vector2 textMeasurement = FontAssets.MouseText.Value.MeasureString(InvasionName);
            float x = 120f;
            if (textMeasurement.X > 200f)
            {
                x += textMeasurement.X - 200f;
            }
            Rectangle iconRectangle = Utils.CenteredRectangle(new Vector2(Main.screenWidth - x, Main.screenHeight - 80 + barOffsetY), textMeasurement + new Vector2(IconTexture.Width + 12, 6f));
            Utils.DrawInvBG(spriteBatch, iconRectangle, InvasionBarColor * 0.5f);
            spriteBatch.Draw(IconTexture, iconRectangle.Left() + Vector2.UnitX * 8f, null, Color.White, 0f, Vector2.UnitY * IconTexture.Height / 2, 0.8f, SpriteEffects.None, 0f);
            Utils.DrawBorderString(spriteBatch, InvasionName, iconRectangle.Right() + Vector2.UnitX * -16f, Color.White, 0.9f, 1f, 0.4f, -1);
        }
        public static void Drawe(SpriteBatch spriteBatch)
        {
            if (!IsActive || Main.invasionProgressMode == 0)
                return;

            int barOffsetY = 0;
            int totalBars = 0;
            if (Main.invasionProgressNearInvasion || Main.invasionProgressAlpha > 0f)
                totalBars++;

            // Incorporate the MGR boss health bar so that the two do not overlap.
            // Also, keep the offset active during boss rush, so+= InvasionProgressUIManager.TotalGUIsActive; that it doesn't flip positions during cool-down periods.
            if (CalamityPlayer.areThereAnyDamnBosses || BossRushEvent.BossRushActive)
                totalBars++;
            totalBars += InvasionProgressUIManager.TotalGUIsActive;
            totalBars++;
            // Rise somewhat if there's other invasions going on.
            barOffsetY -= 85 * (totalBars - 1);

            Vector2 barDrawPosition = new Vector2(Main.screenWidth - 120, Main.screenHeight - 40);
            DrawBlueBar(spriteBatch, barDrawPosition, barOffsetY);

            float yScale = 8f;
            DrawProgressText(spriteBatch, yScale, barDrawPosition, barOffsetY, out Vector2 newBarPosition);
            DrawBackground(spriteBatch, yScale, newBarPosition, barOffsetY);
            DrawProgressTextAndIcons(spriteBatch, barOffsetY);
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class BioUI : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (PandemicPanic.IsActive)
            {
                int layerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Diagnose Net"));
                if (layerIndex != -1)
                {
                    layers.Insert(layerIndex, new LegacyGameInterfaceLayer("CalRemix: Pandemic Panic Invasion",
                        delegate
                        {
                            PandemicPanicUIState.Drawe(Main.spriteBatch);
                            return true;
                        },
                        InterfaceScaleType.UI));
                }
            }
        }
    }
}
