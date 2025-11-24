using CalamityMod;
using CalRemix.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using static CalRemix.CalRemixHelper;

namespace CalRemix.UI.SubworldMap
{
    public class NoiseTest : UIState
    {
        string selected = "";
        Vector2 trueBasePos = Vector2.Zero;
        Vector2 anchorPoint = Vector2.Zero;

        public const int Size = 100;

        public static bool[,] map = new bool[Size, Size];

        public static PerlinEase easingType = PerlinEase.None;

        public static float noiseThreshold = 0.56f;

        public static float noiseStrength = 0.3f;

        public static Vector2 noiseSize = new Vector2(180, 120f);

        public static float top = 0.3f;

        public static float bottom = 0.7f;

        public static float scrollOld = 0;
        public static float scrollNew = 0;

        public static int seed = 2222;

        public override void Draw(SpriteBatch spriteBatch)
        {
            //return;
            Main.blockInput = true;
            if (Main.mouseLeft)
            {
                map = PerlinGeneration(new Rectangle(0, 0, Size, Size), noiseThreshold, noiseStrength, noiseSize, ease: easingType, topStop: top, bottomStop: bottom, seed: seed);
            }
            Vector2 startPos = new Vector2(Main.screenWidth * 0.3f, Main.screenHeight * 0.1f);
            float UISize = Main.screenHeight - startPos.Y * 2;
            float squareSize = UISize / (float)Size;
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, startPos, new Rectangle(0, 0, (int)UISize, (int)UISize), Color.White);
            int qube = ModContent.TileType<IonCubePlaced>();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    bool filled = map[i, j];

                    if (filled)
                        spriteBatch.Draw(TextureAssets.Tile[qube].Value, startPos + new Vector2(i, j) * squareSize, null, filled ? Color.White : Color.White, 0, Vector2.Zero, squareSize / 16f, 0, 0);

                }
            }

            int optionAmount = 8;
            int spacing = 20;
            int optionsize = (int)(UISize / optionAmount) - (int)(spacing / optionAmount);
            for (int i = 0; i < optionAmount; i++)
            {
                Vector2 pos = startPos + new Vector2(0, i) * optionsize - Vector2.UnitX * (optionsize + spacing);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, pos, new Rectangle(0, 0, (int)optionsize + 1, (int)optionsize + 1), Color.White, 0, Vector2.Zero, 1, 0, 0);
                Action<bool?> whatToDo = null;
                string text = "";
                switch (i)
                {
                    case 0:
                        {
                            text = "Noise Threshold: " + noiseThreshold;
                            whatToDo = (scrollingup) =>
                            {
                                if (scrollingup.HasValue)
                                {
                                    if (scrollingup.Value)
                                    {
                                        noiseThreshold += 0.05f;
                                    }
                                    else
                                    {
                                        noiseThreshold -= 0.05f;
                                    }
                                }
                                noiseThreshold = MathHelper.Clamp(noiseThreshold, 0, 1f);
                            };
                        }
                        break;
                    case 1:
                        {
                            text = "Noise Strength: " + noiseStrength;
                            whatToDo = (scrollingup) =>
                            {
                                if (scrollingup.HasValue)
                                {
                                    if (scrollingup.Value)
                                    {
                                        noiseStrength += 0.05f;
                                    }
                                    else
                                    {
                                        noiseStrength -= 0.05f;
                                    }
                                    noiseStrength = MathHelper.Clamp(noiseStrength, 0, 1f);
                                }
                            };
                        }
                        break;
                    case 2:
                        {
                            text = "Noise Size X: " + noiseSize.X;
                            whatToDo = (scrollingup) =>
                            {
                                if (scrollingup.HasValue)
                                {
                                    if (scrollingup.Value)
                                    {
                                        noiseSize.X += 50;
                                    }
                                    else
                                    {
                                        noiseSize.X -= 50;
                                    }
                                    noiseSize.X = MathHelper.Clamp(noiseSize.X, 50, 1000f);
                                }
                            };
                        }
                        break;
                    case 3:
                        {
                            text = "Noise Size Y: " + noiseSize.Y;
                            whatToDo = (scrollingup) =>
                            {
                                if (scrollingup.HasValue)
                                {
                                    if (scrollingup.Value)
                                    {
                                        noiseSize.Y += 50;
                                    }
                                    else
                                    {
                                        noiseSize.Y -= 50;
                                    }
                                    noiseSize.Y = MathHelper.Clamp(noiseSize.Y, 50, 1000f);
                                }
                            };
                        }
                        break;
                    case 4:
                        {
                            text = "Top: " + top;
                            whatToDo = (scrollingup) =>
                            {
                                if (scrollingup.HasValue)
                                {
                                    if (scrollingup.Value)
                                    {
                                        top += 0.05f;
                                    }
                                    else
                                    {
                                        top -= 0.05f;
                                    }
                                    top = MathHelper.Clamp(top, 0, 1f);
                                }
                            };
                        }
                        break;
                    case 5:
                        {
                            text = "Bottom: " + bottom;
                            whatToDo = (scrollingup) =>
                            {
                                if (scrollingup.HasValue)
                                {
                                    if (scrollingup.Value)
                                    {
                                        bottom += 0.05f;
                                    }
                                    else
                                    {
                                        bottom -= 0.05f;
                                    }
                                    bottom = MathHelper.Clamp(bottom, 0, 1f);
                                }
                            };
                        }
                        break;
                    case 6:
                        {
                            string easeType = easingType switch
                            {
                                PerlinEase.EaseInTop => "Ease in top",
                                PerlinEase.EaseInBottom => "Ease in bottom",
                                PerlinEase.EaseOutIn => "Ease out in",
                                PerlinEase.EaseInOut => "Ease in out",
                                PerlinEase.EaseOutTop => "Ease out top",
                                PerlinEase.EaseOutBottom => "Ease out bottom",
                                PerlinEase.EaseAirTopSolidBottom => "Ease Air Top\nSolid Bottom",
                                PerlinEase.EaseSolidTopAirBottom => "Ease Solid Top\nAir Bottom",
                                _ => "None",

                            };
                            text = "Easing Type: " + easeType;
                            whatToDo = (scrollingup) =>
                            {
                                if (scrollingup.HasValue)
                                {
                                    if (scrollingup.Value)
                                    {
                                        easingType = (PerlinEase)((int)easingType + 1);
                                    }
                                    else
                                    {
                                        easingType = (PerlinEase)((int)easingType - 1);
                                    }
                                    easingType = (PerlinEase)MathHelper.Clamp((int)easingType, 0, 8);
                                }
                            };
                        }
                        break;
                    case 7:
                        {
                            text = "Seed: " + seed;
                            whatToDo = (scrollingup) =>
                            {
                                seed = Main.rand.Next(0, 22222222);
                            };
                        }
                        break;

                }

                bool? scrollstate = null;
                if (scrollOld != scrollNew)
                {
                    scrollstate = scrollNew > scrollOld;
                }

                Color textColor = Color.White;
                Rectangle maus = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 10, 10);
                if (maus.Intersects(new Rectangle((int)pos.X, (int)pos.Y, optionsize, optionsize)))
                {
                    if (scrollstate.HasValue)
                    {
                        whatToDo.Invoke(scrollstate);
                    }
                    textColor = Color.Yellow;
                }

                Utils.DrawBorderString(spriteBatch, text, pos + new Vector2(optionsize) * 0.5f, textColor, anchorx: 0.5f, anchory: 0.5f);
            }

            MouseState state = Mouse.GetState();
            // Set both scroll states to the mouse's current scroll value
            if (scrollOld == 0 && scrollNew == 0)
            {
                scrollOld = state.ScrollWheelValue;
                scrollNew = state.ScrollWheelValue;
            }
            // Update scroll values
            else
            {
                scrollOld = scrollNew;
                scrollNew = state.ScrollWheelValue;
            }
        }

        [Autoload(Side = ModSide.Client)]
        internal class NoiseUISystem : ModSystem
        {
            private UserInterface UserInter;

            internal NoiseTest SUI;

            public override void Load()
            {
                SUI = new();
                UserInter = new();
                UserInter.SetState(SUI);
            }

            public override void UpdateUI(GameTime gameTime)
            {
                UserInter?.Update(gameTime);
            }

            public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
            {
                int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
                if (resourceBarIndex != -1)
                {
                    layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                        "CalRemix:NoiseTest",
                        delegate
                        {
                            UserInter.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }


        public static bool[,] PerlinGeneration(Rectangle area, float noiseThreshold = 0.56f, float noiseStrength = 0.1f, Vector2 noiseSize = default, PerlinEase ease = PerlinEase.None, float topStop = 0.3f, float bottomStop = 0.7f, Predicate<Point> tileCondition = null, int seed = 2222)
        {

            int sizeX = area.Width;
            int sizeY = area.Height;

            // Map to store what blocks should be converted
            bool[,] map = new bool[sizeX, sizeY];

            // default Baron Strait numbers
            if (noiseSize == default)
            {
                noiseSize.X = 180f;
                noiseSize.Y = 120f;
            }

            // Create a perlin noise map
            for (int i = 0; i < area.Width; i++)
            {
                for (int j = 0; j < area.Height; j++)
                {
                    float noise = CalamityUtils.PerlinNoise2D(i / noiseSize.X, j / noiseSize.Y, 3, seed) * 0.5f + 0.5f;

                    float endPoint = noiseThreshold;
                    switch (ease)
                    {
                        case PerlinEase.EaseInTop:
                            endPoint = MathHelper.Lerp(noise, noiseThreshold, Utils.GetLerpValue(0, topStop, (j / (float)area.Height), true));
                            break;
                        case PerlinEase.EaseOutBottom:
                            endPoint = MathHelper.Lerp(noiseThreshold, 0, Utils.GetLerpValue(bottomStop, 1f, (j / (float)area.Height), true));
                            break;
                        case PerlinEase.EaseInOut:
                            if (j / (float)area.Height < 0.5f)
                            {
                                endPoint = MathHelper.Lerp(noise, noiseThreshold, Utils.GetLerpValue(0, topStop, (j / (float)area.Height), true));
                            }
                            else
                            {
                                endPoint = MathHelper.Lerp(noise, noiseThreshold, Utils.GetLerpValue(1f, bottomStop, (j / (float)area.Height), true));
                            }
                            break;
                        case PerlinEase.EaseOutTop:
                            endPoint = MathHelper.Lerp(0, noiseThreshold, Utils.GetLerpValue(0f, topStop, (j / (float)area.Height), true));
                            break;
                        case PerlinEase.EaseInBottom:
                            endPoint = MathHelper.Lerp(noiseThreshold, noise, Utils.GetLerpValue(bottomStop, 1f, (j / (float)area.Height), true));
                            break;
                        case PerlinEase.EaseOutIn:
                            if (j / (float)area.Height < 0.5f)
                            {
                                endPoint = MathHelper.Lerp(noise, noiseThreshold, Utils.GetLerpValue(0.5f, topStop, (j / (float)area.Height), true));
                            }
                            else
                            {
                                endPoint = MathHelper.Lerp(noise, noiseThreshold, Utils.GetLerpValue(0.5f, bottomStop, (j / (float)area.Height), true));
                            }
                            break;
                        case PerlinEase.EaseAirTopSolidBottom:
                            if (j / (float)area.Height < 0.5f)
                            {
                                endPoint = MathHelper.Lerp(0, noiseThreshold, Utils.GetLerpValue(0, topStop, (j / (float)area.Height), true));
                            }
                            else
                            {
                                endPoint = MathHelper.Lerp(noiseThreshold, noise, Utils.GetLerpValue(bottomStop, 1f, (j / (float)area.Height), true));
                            }
                            break;
                        case PerlinEase.EaseSolidTopAirBottom:
                            if (j / (float)area.Height < 0.5f)
                            {
                                endPoint = MathHelper.Lerp(noise, noiseThreshold, Utils.GetLerpValue(0, topStop, (j / (float)area.Height), true));
                            }
                            else
                            {
                                endPoint = MathHelper.Lerp(noiseThreshold, 0, Utils.GetLerpValue(bottomStop, 1f, (j / (float)area.Height), true));
                            }
                            break;
                    }

                    map[i, j] = MathHelper.Distance(noise, endPoint) < noiseStrength;
                }
            }

            return map;
        }
    }
}