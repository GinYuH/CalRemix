using System.Collections.Generic;
using System.Linq;
using CalamityMod.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.UI
{
    public abstract class TerminalUI : PopupGUI
    {
        public int Page = 0;
        public int ArrowClickCooldown;
        public bool HoveringOverBook = false;
        public int TotalLinesPerPage => 16;
        public abstract int TotalPages { get; }

        public const int TextStartOffsetX = 40;

        public override void Update()
        {
            if (Active)
            {
                if (FadeTime < FadeTimeMax)
                    FadeTime++;
            }
            else if (FadeTime > 0)
            {
                FadeTime--;
            }

            if (Main.mouseLeft && !HoveringOverBook && FadeTime >= 30)
            {
                Page = 0;
                Active = false;
            }

            if (ArrowClickCooldown > 0)
                ArrowClickCooldown--;
            HoveringOverBook = false;
        }

        public abstract string GetTextByPage();

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D pageTexture = ModContent.Request<Texture2D>("CalRemix/UI/Logs/TerminalSlab").Value;
            float xScale = MathHelper.Lerp(0.004f, 1f, FadeTime / (float)FadeTimeMax);
            Vector2 scale = new Vector2(xScale, 1f) * new Vector2(Main.screenWidth, Main.screenHeight) / pageTexture.Size();
            scale.Y *= 1.5f;
            scale *= 0.5f;

            float xResolutionScale = Main.screenWidth / 2060f;
            float yResolutionScale = Main.screenHeight / 2560f;
            float bookScale = 0.75f;
            scale *= bookScale;

            float yPageTop = MathHelper.Lerp(Main.screenHeight * 2, Main.screenHeight * 0.25f, FadeTime / (float)FadeTimeMax);

            Rectangle mouseRectangle = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 2, 2);
            float drawPositionX = Main.screenWidth * 0.66f;
            Vector2 drawPosition = new Vector2(drawPositionX, yPageTop);
            Rectangle pageRectangle = new Rectangle((int)drawPosition.X - (int)(pageTexture.Width * scale.X), (int)yPageTop, (int)(pageTexture.Width * scale.X) * 2, (int)(pageTexture.Height * scale.Y));
            
            spriteBatch.Draw(pageTexture, drawPosition, null, Color.White, 0f, new Vector2(pageTexture.Width, 0f), scale, SpriteEffects.None, 0f);

            if (!HoveringOverBook)
                HoveringOverBook = mouseRectangle.Intersects(new Rectangle(pageRectangle.X, pageRectangle.Y, pageRectangle.Width / 2, pageRectangle.Height));
            

            // Create text and arrows.
            if (FadeTime >= FadeTimeMax - 4 && Active)
            {
                int textWidth = (int)(xScale * pageTexture.Width) - TextStartOffsetX;
                textWidth = (int)(textWidth * xResolutionScale);
                List<string> dialogLines = Utils.WordwrapString(GetTextByPage(), FontAssets.MouseText.Value, (int)(textWidth / xResolutionScale), 250, out _).ToList();
                dialogLines.RemoveAll(text => string.IsNullOrEmpty(text));

                int trimmedTextCharacterCount = string.Concat(dialogLines).Length;
                float yOffsetPerLine = 28f;
                if (dialogLines.Count > TotalLinesPerPage)
                    yOffsetPerLine *= string.Concat(dialogLines).Length / GetTextByPage().Length;

                // Ensure the page number doesn't become nonsensical as a result of a resolution change (such as by resizing the game).
                if (Page < 0)
                    Page = 0;
                if (Page > TotalPages)
                    Page = TotalPages;

                DrawArrows(spriteBatch, xResolutionScale * 2, yResolutionScale * 2, yPageTop + 1106f * yResolutionScale, mouseRectangle);

                int textDrawPositionX = (int)(pageTexture.Width * xResolutionScale + 240 * xResolutionScale);
                int yScale = (int)(42 * yResolutionScale);
                int yScale2 = (int)(yOffsetPerLine * yResolutionScale * 2f);
                for (int i = 0; i < dialogLines.Count; i++)
                {
                    if (dialogLines[i] != null)
                    {
                        int textDrawPositionY = yScale + i * yScale2 + (int)yPageTop + 122;
                        Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, dialogLines[i], textDrawPositionX, textDrawPositionY, Color.Orange, Color.Black, Vector2.Zero, xResolutionScale);
                    }
                }
            }
        }

        public void DrawArrows(SpriteBatch spriteBatch, float xResolutionScale, float yResolutionScale, float yPageBottom, Rectangle mouseRectangle)
        {
            float arrowScale = 0.6f;
            Texture2D arrowTexture = ModContent.Request<Texture2D>("CalamityMod/UI/DraedonLogs/DraedonsLogArrow").Value;
            if (Page > 0)
            {
                Vector2 drawPosition = new Vector2(Main.screenWidth / 2 - 80f, yPageBottom);
                Rectangle arrowRectangle = new Rectangle((int)drawPosition.X, (int)drawPosition.Y, arrowTexture.Width, arrowTexture.Height);
                arrowRectangle.Width = (int)(arrowRectangle.Width * xResolutionScale * arrowScale);
                arrowRectangle.Height = (int)(arrowRectangle.Height * yResolutionScale * arrowScale);

                if (mouseRectangle.Intersects(arrowRectangle))
                {
                    arrowTexture = ModContent.Request<Texture2D>("CalamityMod/UI/DraedonLogs/DraedonsLogArrowHover").Value;
                    if (ArrowClickCooldown <= 0 && Main.mouseLeft)
                    {
                        Page--;
                        ArrowClickCooldown = 8;
                    }
                    Main.blockMouse = true;
                }

                spriteBatch.Draw(arrowTexture, drawPosition, null, Color.Red, 0f, Vector2.Zero, new Vector2(xResolutionScale, yResolutionScale) * arrowScale, SpriteEffects.FlipHorizontally, 0f);
            }

            if (Page < TotalPages - 1)
            {
                Vector2 drawPosition = new Vector2(Main.screenWidth / 2 + 40f, yPageBottom);
                Rectangle arrowRectangle = new Rectangle((int)drawPosition.X, (int)drawPosition.Y, arrowTexture.Width, arrowTexture.Height);
                arrowRectangle.Width = (int)(arrowRectangle.Width * xResolutionScale);
                arrowRectangle.Height = (int)(arrowRectangle.Height * yResolutionScale);

                if (mouseRectangle.Intersects(arrowRectangle))
                {
                    arrowTexture = ModContent.Request<Texture2D>("CalamityMod/UI/DraedonLogs/DraedonsLogArrowHover").Value;
                    if (ArrowClickCooldown <= 0 && Main.mouseLeft)
                    {
                        Page++;
                        ArrowClickCooldown = 8;
                    }
                    Main.blockMouse = true;
                }

                spriteBatch.Draw(arrowTexture, drawPosition, null, Color.Red, 0f, Vector2.Zero, new Vector2(xResolutionScale, yResolutionScale) * arrowScale, SpriteEffects.None, 0f);
            }
        }
    }
}
