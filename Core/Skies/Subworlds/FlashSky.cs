using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes.Subworlds
{
    public class FlashScreen
    {
        public int width;
        public int height;
        public int frameRate;
        public int cutOff;
        public string texture;
        public Point currentFrame = Point.Zero;
        public int frameTimer = 0;
        public Vector2 position = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        public int bounceCooldown = 0;

        public FlashScreen(int width, int height, int frameRate, int cutOff, string texture, Vector2 position, Vector2 velocity)
        {
            this.width = width;
            this.height = height;
            this.frameRate = frameRate;
            this.cutOff = cutOff;
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
        }
    }
    public class FlashSky : CustomSky
    {
        public List<FlashScreen> screens = new List<FlashScreen>();
        public float BackgroundIntensity;
        public static bool CanSkyBeActive
        {
            get
            {
                if (SubworldSystem.IsActive<FlashSubworld>())
                    return true;
                return false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!CanSkyBeActive)
            {
                BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity - 0.08f, 0f, 1f);
                Deactivate(Array.Empty<object>());
                return;
            }

            BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity + 0.01f, 0f, 1f);

            Opacity = BackgroundIntensity;

            if (Main.LocalPlayer.selectedItem == 2)
                screens.Clear();
            if (screens.Count <= 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    AddScreen("Aqua", 5, 3, cutOff: 2);
                    AddScreen("Brimmy", 5, 12, cutOff: 4);
                    AddScreen("CalamitasAlt", 5, 3, cutOff: 3);
                    AddScreen("BWCalamitasAlt", 5, 3, cutOff: 3);
                    AddScreen("Calclone", 5, 18, cutOff: 2);
                    AddScreen("Cirnogen", 5, 3, cutOff: 3);
                    AddScreen("Cory", 5, 8, cutOff: 4);
                    AddScreen("Crabulon", 5, 5, cutOff: 4);
                    AddScreen("Cryogen", 5, 7, cutOff: 3);
                    AddScreen("DesertScourge", 5, 3, cutOff: 4);
                    AddScreen("Deus", 5, 8, cutOff: 4);
                    AddScreen("Donut", 5, 3);
                    AddScreen("Dungeon", 1, 1, cutOff: 0);
                    AddScreen("EternalCrevi", 5, 17, cutOff: 3);
                    AddScreen("Geldon", 5, 7);
                    AddScreen("GodSlayers", 5, 4, cutOff: 1);
                    AddScreen("Goozma", 10, 10, cutOff: 2);
                    AddScreen("Halloween", 5, 3);
                    AddScreen("HiveMind", 10, 9, cutOff: 6);
                    AddScreen("Leviathan", 5, 8, cutOff: 3);
                    AddScreen("Mailman", 10, 11, cutOff: 9);
                    AddScreen("Mallah", 5, 14);
                    AddScreen("Noxus", 5, 12);
                    AddScreen("Oblivion", 10, 7, cutOff: 3);
                    AddScreen("PBG", 5, 5);
                    AddScreen("Perforators", 5, 8, cutOff: 1);
                    AddScreen("Providence", 5, 5, cutOff: 1);
                    AddScreen("Ravager", 7, 11, cutOff: 3);
                    AddScreen("Reaper", 5, 6, cutOff: 2);
                    AddScreen("SCal", 10, 10, cutOff: 6);
                    AddScreen("Sepulcher", 5, 3);
                    AddScreen("SHMT", 10, 10, cutOff: 4);
                    AddScreen("SlimeGod", 5, 3);
                    AddScreen("TundraBattle", 10, 13, cutOff: 4);
                    AddScreen("UngodlySerpent", 5, 17, cutOff: 3);
                    AddScreen("Yharim", 5, 7, cutOff: 1);
                    if (i == 0)
                    {
                        AddScreen("George", 10, 6, cutOff: 9);
                        AddScreen("Hataqau", 5, 11, cutOff: 1);
                    }
                }
            }

            foreach (FlashScreen screen in screens)
            {
                screen.position += screen.velocity;
                screen.bounceCooldown--;
                if (screen.bounceCooldown <= 0)
                {
                    if (screen.position.X < 130 || screen.position.X > (Main.maxTilesX - 70) * 16)
                    {
                        screen.velocity.X *= -1;
                        screen.bounceCooldown = 60;
                    }
                    if (screen.position.Y < 250 || screen.position.Y > (Main.maxTilesY - 70) * 16)
                    {
                        screen.velocity.Y *= -1;
                        screen.bounceCooldown = 60;
                    }
                }
                screen.frameTimer++;
                bool wrapBack = (screen.currentFrame.Y >= screen.height - 1 && screen.currentFrame.X >= screen.cutOff - 1);
                if (screen.frameTimer % screen.frameRate == 0)
                {
                    screen.currentFrame.X++;
                    if (screen.currentFrame.X >= screen.width || wrapBack)
                    {
                        if (!wrapBack)
                        {
                            screen.currentFrame.X = 0;
                            screen.currentFrame.Y++;
                        }
                        if (screen.currentFrame.Y >= screen.height || wrapBack)
                        {
                            screen.currentFrame.Y = 0;
                            screen.currentFrame.X = 0;
                        }
                    }
                }
            }
        }

        public void AddScreen(string texture, int width, int height, int frameRate = 6, int cutOff = -1)
        {
            if (cutOff == -1)
                cutOff = width;
            FlashScreen screen = new(width, height, frameRate, cutOff, "CalRemix/Core/Skies/Subworlds/Flash/" + texture, Main.LocalPlayer.Center, Main.rand.NextVector2CircularEdge(10, 10) * Main.rand.NextFloat(0.2f, 2.8f));
            screens.Add(screen);
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);
            if (minDepth < 0)
            {
                foreach (FlashScreen screen in screens)
                {
                    Rectangle screenie = new Rectangle((int)screen.position.X, (int)screen.position.Y, 800, 450);
                    Rectangle mainScreen = new Rectangle((int)Main.LocalPlayer.Center.X - (int)Main.screenWidth, (int)Main.LocalPlayer.Center.Y - (int)Main.screenHeight, Main.screenWidth * 2, Main.screenHeight * 2);
                    if (mainScreen.Intersects(screenie))
                    {
                        Texture2D texture = ModContent.Request<Texture2D>(screen.texture).Value;
                        Rectangle sourceRectangle = texture.Frame(screen.width, screen.height, screen.currentFrame.X, screen.currentFrame.Y);
                        Vector2 position = screen.position + screen.velocity - Main.screenPosition;
                        spriteBatch.Draw(TextureAssets.MagicPixel.Value, position, new Rectangle(0, 0, 800, 450), Color.White, 0, Vector2.Zero, 1, 0, 0);
                        spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0, Vector2.Zero, new Vector2(800 / (float)sourceRectangle.Width, 450 / (float)sourceRectangle.Height), 0, 0);
                    }
                }
            }
        }

        public override Color OnTileColor(Color inColor) => inColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { screens.Clear(); }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { screens.Clear(); }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
