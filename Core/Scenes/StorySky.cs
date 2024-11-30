using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;

namespace CalRemix.Core.Scenes
{
    public class StorySky : CustomSky
    {
        private bool _isActive;
        public static float strength = 0;

        public static bool CanSkyBeActive
        {
            get
            {
                return CalRemixWorld.trueStory < CalRemixWorld.maxStoryTime;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!CanSkyBeActive)
            {
                Deactivate(Array.Empty<object>());
                return;
            }
            if (CalRemixWorld.trueStory > 180 && CalRemixWorld.trueStory < 240)
            {
                strength += (1 / 60f);
            }
            else if (CalRemixWorld.trueStory > (CalRemixWorld.maxStoryTime - 120))
            {
                strength -= (1 / 120f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            string text = "Based on a true story";
            float height = FontAssets.MouseText.Value.MeasureString(text).Y;
            float width = FontAssets.MouseText.Value.MeasureString(text).X;

            float scale = 6f * (Main.screenWidth / 1920f);
            height *= scale;
            width *= scale;
            float remaindingSpace = (Main.screenWidth - width) * 0.5f;
            Utils.DrawBorderString(spriteBatch, "Based on a true story", new Vector2(remaindingSpace, Main.screenHeight / 2 - height * 1.5f), Color.White * strength, scale);
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            _isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            _isActive = false;
        }

        public override void Reset()
        {
            _isActive = false;
        }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
