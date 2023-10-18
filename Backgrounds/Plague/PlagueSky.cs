using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Utilities;
using Terraria.GameContent;

namespace CalRemix.Backgrounds.Plague
{
    public class PlagueSky : CustomSky
    {
        private class PlagueStar
		{
            public float velocityY;
			public Vector2 Position;
			public Vector2 SpawnPosition;
			public float Depth;
			public int TextureIndex;
			public float SinOffset;
			public float AlphaFrequency;
			public float AlphaAmplitude;

            public void Randomize(float num4, float num5, UnifiedRandom rand)
            {
                SpawnPosition.X = num4 * Main.maxTilesX * 16f;
                SpawnPosition.Y = num5 * ((float)Main.worldSurface * 16f + 2000f) - 1000f;
                Position = SpawnPosition;
                Depth = rand.NextFloat() * 8f + 1.5f;
                TextureIndex = rand.Next(_starTextures.Length);
                SinOffset = rand.NextFloat() * 6.28f;
                AlphaAmplitude = rand.NextFloat() * 5f;
                AlphaFrequency = rand.NextFloat() + 1f;
                velocityY = rand.NextFloat(2f) + 0.25f;
            }

            public float GetIntensity()
            {
                float num3 = ((float)Math.Sin((AlphaFrequency * Main.GlobalTimeWrappedHourly) + SinOffset) * AlphaAmplitude) + AlphaAmplitude;
                num3 = MathHelper.Clamp(num3, 0f, 1f);
                return num3;
            }

            public void ResetPosition(UnifiedRandom rand)
            {
                Position = SpawnPosition;
                Position.Y += rand.NextFloat(-60f, 60f);
            }
        }

		private static PlagueStar[] _stars;
		private static Texture2D[] _starTextures;

        public bool Active;
        public static float Intensity;

        public static Texture2D SkyTexture;
        public static Texture2D DesertSkyTexture;

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                Intensity = Math.Min(1f, 0.01f + Intensity);
            }
            else
            {
                Intensity = Math.Max(0f, Intensity - 0.01f);
            }
            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i].Position.Y -= _stars[i].velocityY;
            }
        }

        public override Color OnTileColor(Color inColor)
        {
            byte a = inColor.A;
            inColor *= 0.5f + (0.5f * (1f - Intensity));
            inColor.A = a;
            return inColor;
        }

        public override void OnLoad()
		{
            SkyTexture = ModContent.Request<Texture2D>("CalRemix/Backgrounds/Plague/PlagueSky").Value;
            DesertSkyTexture = ModContent.Request<Texture2D>("CalRemix/Backgrounds/Plague/PlagueDesertSky").Value;

            _starTextures = new Texture2D[2];
			for (int i = 0; i < _starTextures.Length; i++)
			{
				_starTextures[i] = ModContent.Request<Texture2D>("CalRemix/Backgrounds/Plague/PlagueStar" + i).Value;
			}
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
            {
                if (Main.player[Main.myPlayer].GetModPlayer<CalRemixPlayer>().ZonePlague)
                {
                    float height = (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 22 + Main.screenHeight;
                    //Draw the sky box texture
                    spriteBatch.Draw(ModContent.Request<Texture2D>("CalRemix/Backgrounds/Plague/PlagueSky").Value, new Rectangle(0, 0, Main.screenWidth, (int)height), Color.White * Intensity);
                }

                if (Main.player[Main.myPlayer].GetModPlayer<CalRemixPlayer>().ZonePlagueDesert)
                { 
                    //Draw the sky box texture
                    spriteBatch.Draw(DesertSkyTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * Intensity);
                }
            }

            if (!Main.dayTime)
            {
                int num = -1;
                int num2 = 0;
                for (int i = 0; i < _stars.Length; i++)
                {
                    float depth = _stars[i].Depth;
                    if (num == -1 && depth < maxDepth)
                    {
                        num = i;
                    }
                    if (depth <= minDepth)
                    {
                        break;
                    }
                    num2 = i;
                }
                if (num == -1)
                {
                    return;
                }
                float scale = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
                Vector2 value3 = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
                Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
                for (int j = num; j < num2; j++)
                {
                    Vector2 value4 = new Vector2(1f / _stars[j].Depth, 1.1f / _stars[j].Depth);
                    Vector2 position = (_stars[j].Position - value3) * value4 + value3 - Main.screenPosition;
                    if (rectangle.Contains((int)position.X, (int)position.Y))
                    {
                        float starIntensity = _stars[j].GetIntensity();
                        if (starIntensity <= 0.01f)
                        {
                            _stars[j].ResetPosition(Main.rand);
                        }
                        float num4 = (float)Math.Sin(_stars[j].AlphaFrequency * Main.GlobalTimeWrappedHourly * 5f + _stars[j].SinOffset) * 0.1f - 0.1f;
                        Texture2D texture2D = _starTextures[_stars[j].TextureIndex];
                        spriteBatch.Draw(texture2D, position, null, Color.White * scale * starIntensity * 0.8f * (1f - num4) * Intensity, 0f, new Vector2(texture2D.Width >> 1, texture2D.Height >> 1), (value4.X * 0.5f + 0.5f) * (starIntensity * 0.3f + 0.7f), SpriteEffects.None, 0f);
                    }
                }
            }

            //deactivate the sky if in the menu
            if (Main.gameMenu || !Main.LocalPlayer.active)  
            {
                Active = false;
            }
        }

        public override float GetCloudAlpha()
        {
            return 1f - Intensity;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            int num = 200;
			int num2 = 10;
			_stars = new PlagueStar[num * num2];
			int num3 = 0;
			for (int i = 0; i < num; i++)
			{
				float num4 = i / (float)num;
				for (int j = 0; j < num2; j++)
				{
					float num5 = j / (float)num2;
                    _stars[num3] = new PlagueStar();
                    _stars[num3].Randomize(num4, num5, Main.rand);
					num3++;
				}
			}
			Array.Sort(_stars, SortMethod);

            Intensity = 0.002f;
            Active = true;
        }

        private int SortMethod(PlagueStar meteor1, PlagueStar meteor2)
		{
			return meteor2.Depth.CompareTo(meteor1.Depth);
		}

        public override void Deactivate(params object[] args)
        {
            Active = false;
        }

        public override void Reset()
        {
            Active = false;
        }

        public override bool IsActive()
        {
            return Active || Intensity > 0.001f;
        }
    }
    
    public class PlagueSkyData : ScreenShaderData
    {
        public PlagueSkyData(string passName)
            : base(passName)
        {
        }
        private void UpdatePlagueIndex()
        {
           
        }
        
        public override void Apply()
        {
            UpdatePlagueIndex();
            if (Main.player[Main.myPlayer].GetModPlayer<CalRemixPlayer>().ZonePlague || Main.player[Main.myPlayer].GetModPlayer<CalRemixPlayer>().ZonePlagueDesert)
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    UseTargetPosition(Main.player[(int)Player.FindClosest(Main.npc[i].position, Main.npc[i].width, Main.npc[i].height)].Center);
                }
            }
            base.Apply();
        }
    }
}