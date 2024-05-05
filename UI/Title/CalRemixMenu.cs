using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalRemix.UI.Title
{
	public class CalRemixMenu : ModMenu
    {
        public static ModMenu Instance
        {
            get;
            private set;
        }
        private Asset<Texture2D> blankTexture;
        private Asset<Texture2D> logoTexture;
        public override void Load()
        {
            Instance = this;
            blankTexture = ModContent.Request<Texture2D>("CalRemix/ExtraTextures/Blank");
            logoTexture = ModContent.Request<Texture2D>("CalRemix/UI/Title/Logo");
        }
        public override Asset<Texture2D> SunTexture => blankTexture;
        public override Asset<Texture2D> MoonTexture => blankTexture;
        public override Asset<Texture2D> Logo => logoTexture;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CrazyLaPaint");
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<MenuBgStyle>();
        public override string DisplayName => "Remixed Calamity Style";
        public class Star
        {
            public int time;
            public int lifetime;
            public int id;
            public float scale;
            public Vector2 center;
            public Vector2 velocity;
            public Star(int _lifetime, int _id, float _scale, Vector2 _center, Vector2 _velocity)
            {
                lifetime = _lifetime;
                id = _id;
                scale = _scale;
                center = _center;
                velocity = _velocity;
            }
        }

        public static List<Star> Stars { get; internal set; } = new List<Star>();
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            Texture2D menuTexture = ModContent.Request<Texture2D>("CalRemix/UI/Title/Menu").Value;
            Vector2 zero = Vector2.Zero;
            Vector2 menuSize = new((float)Main.screenWidth / (float)menuTexture.Width, (float)Main.screenHeight / (float)menuTexture.Height);
            float scale = menuSize.X;
            if (menuSize.X != menuSize.Y)
            {
                if (menuSize.Y > menuSize.X)
                {
                    scale = menuSize.Y;
                    zero.X -= ((float)menuTexture.Width * scale - (float)Main.screenWidth) * 0.5f;
                }
                else
                    zero.Y -= ((float)menuTexture.Height * scale - (float)Main.screenHeight) * 0.5f;
            }
            spriteBatch.Draw(menuTexture, zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            if (Main.rand.NextBool(20))
            {
                Vector2 position = new((Main.screenWidth * 0.5f + (Main.screenWidth * Main.rand.NextFloat(-0.6f, 0.6f))), -256 - Main.screenHeight * Main.rand.NextFloat(0.1f, 0.15f));
                Vector2 velocity = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-0.9f, 0.9f)) * (4.5f + Main.rand.NextFloat());
                Stars.Add(new Star(480, Stars.Count, 0.5f + Main.rand.NextFloat(), position, velocity));
            }
            for (int j = 0; j < Stars.Count; j++)
            {
                Stars[j].center += Stars[j].velocity;
                Stars[j].time++;
            }
            Stars.RemoveAll((Star s) => s.time >= s.lifetime);
            Texture2D starTexture = ModContent.Request<Texture2D>("CalRemix/UI/Title/Star").Value;
            for (int k = 0; k < Stars.Count; k++)
            {
                spriteBatch.Draw(starTexture, Stars[k].center, null, Color.White, 0f, starTexture.Size() * 0.5f, Stars[k].scale, SpriteEffects.None, 0f);
            }
            Main.time = 27000.0;
            Main.dayTime = true;
            spriteBatch.Draw(Logo.Value, new Vector2((float)Main.screenWidth / 2f, 100f), null, drawColor, logoRotation, Logo.Value.Size() * 0.5f, logoScale * 0.45f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
