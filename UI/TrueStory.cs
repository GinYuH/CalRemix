using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.UI;

[Autoload(Side = ModSide.Client)]
public class TrueStory : ModSystem
{
    public const int MaxStoryTime = 660;
    private static float strength = 0;
    public override void Load()
    {
        On_Main.DoDraw += DrawTrueStory;
    }
    private static void DrawTrueStory(On_Main.orig_DoDraw orig, Main self, GameTime gameTime)
    {
        orig(self, gameTime);
        SpriteBatch spriteBatch = Main.spriteBatch;
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
        if (!Main.gameMenu && Main.LocalPlayer != null)
        {
            Player player = Main.LocalPlayer;
            if (player.TryGetModPlayer(out CalRemixPlayer p) && !CalRemixWorld.playerSawTrueStory.Contains(player.name))
            {
                if (p.trueStory > 180 && p.trueStory < 240)
                    strength += (1 / 60f);
                else if (p.trueStory > MaxStoryTime - 120)
                    strength -= (1 / 120f);

                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth * 4, Main.screenHeight * 4), null, Color.Black * strength, 0f, TextureAssets.MagicPixel.Value.Size() * 0.5f, 0, 0f);
                string text = CalRemixHelper.LocalText("UI.TrueStory").Value;
                float height = FontAssets.MouseText.Value.MeasureString(text).Y;
                float width = FontAssets.MouseText.Value.MeasureString(text).X;

                float scale = 6f * (Main.screenWidth / 1920f);
                height *= scale;
                width *= scale;
                float remaindingSpace = (Main.screenWidth - width) * 0.5f;
                Utils.DrawBorderString(spriteBatch, text, new Vector2(remaindingSpace, Main.screenHeight / 2 - height * 1.5f), Color.White * strength, scale);
            }
            else
                strength = 0;
        }
        spriteBatch.End();
    }
}
