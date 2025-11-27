using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using static Terraria.Main;

namespace CalRemix.Core.Subworlds
{
    public class ScreamingFaceSky : CustomSky
    {
        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.Current == ModContent.GetInstance<ScreamingSubworld>();
            }
        }

        public static readonly Color DrawColor = Color.DarkGray;

        public int offset = 0;

        public override void Update(GameTime gameTime)
        {
            if (!CanSkyBeActive)
            {
                Deactivate(Array.Empty<object>());
                return;
            }

            Opacity = 1;
            offset++;
            if (offset > Main.screenWidth / 10)
                offset = 0;

        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Texture2D face = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Subworlds/ScreamingMummifiedFace").Value;
            int screenHeight5 = Main.screenHeight / 5;
            //if (maxDepth >= float.MaxValue)
            {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                bool flip = false;
                int height = 0;
                int xPos = 0;
                for (int i = 0; i < 201; i++)
                {
                    spriteBatch.Draw(face, Vector2.Zero + new Vector2(xPos + offset * flip.ToDirectionInt() - 100, height), null, Color.White * 0.6f, 0, face.Size() / 2, 1, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    if (CalamityClientConfig.Instance.Afterimages)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            Vector2 dp = (Vector2.Zero + new Vector2(xPos + offset * flip.ToDirectionInt() - 100, height)) + Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, j / 4f)) * 8 * MathF.Sin(GlobalTimeWrappedHourly);
                            spriteBatch.Draw(face, dp, null, Color.White * 0.1f, 0, face.Size() / 2, 1, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                        }
                    }
                    xPos += Main.screenWidth / 10;
                    if (i % 15 == 0 && i > 0)
                    {
                        flip = !flip;
                        height += screenHeight5;
                        xPos = 0;
                    }
                }
            }
        }

        public override Color OnTileColor(Color inColor) => DrawColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
