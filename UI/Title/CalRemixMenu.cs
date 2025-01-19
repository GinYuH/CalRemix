﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
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
            blankTexture = ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/Blank");
            logoTexture = ModContent.Request<Texture2D>("CalRemix/UI/Title/Logo");
        }
        public override Asset<Texture2D> SunTexture => blankTexture;
        public override Asset<Texture2D> MoonTexture => blankTexture;
        public override Asset<Texture2D> Logo => logoTexture;
        public override int Music => CalRemixMusic.Menu;
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<MenuBgStyle>();
        public override string DisplayName => CalRemixHelper.LocalText("UI.Title.1").Value;
        public class MenuItem
        {
            public int time;
            public int lifetime;
            public float rotation;
            public float rotationDirection;
            public float rotationIntensity;
            public Vector2 center;
            public Vector2 velocity;
            public Texture2D texture;

            public int alpha = 255;
            public MenuItem(int _lifetime, float _rotationDirection, float _rotationIntensity, Vector2 _center, Vector2 _velocity, Texture2D _texture)
            {
                lifetime = _lifetime;
                rotationDirection = _rotationDirection;
                rotationIntensity = _rotationIntensity;
                center = _center;
                velocity = _velocity;
                texture = _texture;
            }
        }
        public static List<MenuItem> MenuItems { get; internal set; } = new List<MenuItem>();
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            // Floating Items
            if (Main.rand.NextBool(5) && MenuItems.Count < 200)
            {
                int randomSide = Main.rand.Next(4);
                Vector2 position = DetermineSide(randomSide);
                Vector2 velocity = DetermineVelocity(randomSide).RotatedByRandom(MathHelper.ToRadians(135f));
                float rotate = MathHelper.ToRadians(1f + Main.rand.NextFloat(2f));
                Texture2D texturePath = (Texture2D)TextureAssets.Item[CalRemixAddon.Items[Main.rand.Next(CalRemixAddon.Items.Count)].Type];
                MenuItems.Add(new MenuItem(600, Main.rand.NextBool() ? -1 : 1, rotate, position, velocity, texturePath));
            }
            for (int j = 0; j < MenuItems.Count; j++)
            {
                MenuItem menuItem = MenuItems[j];
                menuItem.center += menuItem.velocity;
                menuItem.rotation += menuItem.rotationDirection * menuItem.rotationIntensity;
                if (menuItem.time <= menuItem.lifetime)
                    menuItem.time++;

            }
            MenuItems.RemoveAll((MenuItem m) => m.time >= 600 && ((m.center.X < -m.texture.Width && m.center.X < -m.texture.Height) || (m.center.X > Main.screenWidth + m.texture.Width && m.center.X > Main.screenWidth + m.texture.Height) || (m.center.Y < -m.texture.Width && m.center.Y < -m.texture.Height) || (m.center.Y > Main.screenHeight + m.texture.Width && m.center.Y > Main.screenHeight + m.texture.Height)));
            for (int k = 0; k < MenuItems.Count; k++)
            {
                spriteBatch.Draw(MenuItems[k].texture, MenuItems[k].center, null, Color.White, MenuItems[k].rotation, MenuItems[k].texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
            }

            // Logo and Final Stuff
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.dayTime = false;
                Main.raining = false;
                //Main.UseStormEffects = false;
                Main.time = Main.nightLength - (1800 * 3);
            }
            for (int i = 0; i < Main.cloud.Length; i++)
            {
                Main.cloud[i].Alpha = 255;
                Main.cloud[i].kill = true;
                Main.cloud[i].active = false;
            }
            if (Main.rand.NextBool(10) && Main.numStars < 200)
                Star.SpawnStars();
            if (Main.rand.NextBool(25))
                Main.star[Main.rand.Next(Main.numStars - 1)].Fall();
            Texture2D Glow = ModContent.Request<Texture2D>("CalRemix/UI/Title/LogoGlow").Value;
            spriteBatch.Draw(Logo.Value, new Vector2((float)Main.screenWidth / 2f, 111f), null, Color.White, 0, Logo.Value.Size() * 0.5f, 0.45f, SpriteEffects.None, 0f);
            spriteBatch.Draw(Glow, new Vector2((float)Main.screenWidth / 2f, 111f), null, Main.DiscoColor, 0, Logo.Value.Size() * 0.5f, 0.45f, SpriteEffects.None, 0f);
            return false;
        }
        private static Vector2 DetermineSide(int randomSide)
        {
            Vector2 position = Vector2.Zero;
            switch (randomSide)
            {
                case 0:
                    position = new((Main.screenWidth * 0.5f + (Main.screenWidth * Main.rand.NextFloat(-0.6f, 0.6f))), -256 - Main.screenHeight * Main.rand.NextFloat(0.1f, 0.15f));
                    break;
                case 1:
                    position = new((Main.screenWidth * 0.5f + (Main.screenWidth * Main.rand.NextFloat(-0.6f, 0.6f))), Main.screenHeight + 256 + Main.screenHeight * Main.rand.NextFloat(0.1f, 0.15f));
                    break;
                case 2:
                    position = new(-256 - Main.screenWidth * Main.rand.NextFloat(0.1f, 0.15f), (Main.screenHeight * 0.5f + (Main.screenHeight * Main.rand.NextFloat(-0.6f, 0.6f))));
                    break;
                default:
                    position = new(Main.screenWidth + 256 + Main.screenWidth * Main.rand.NextFloat(0.1f, 0.15f), (Main.screenHeight * 0.5f + (Main.screenHeight * Main.rand.NextFloat(-0.6f, 0.6f))));
                    break;
            }
            return position;
        }
        private static Vector2 DetermineVelocity(int randomSide)
        {
            Vector2 speed = Vector2.Zero;
            switch (randomSide)
            {
                case 0:
                    speed = Vector2.UnitY * (0.5f + Main.rand.NextFloat(6f));
                    break;
                case 1:
                    speed = Vector2.UnitY * -(0.5f + Main.rand.NextFloat(6f));
                    break;
                case 2:
                    speed = Vector2.UnitX * (0.5f + Main.rand.NextFloat(6f));
                    break;
                default:
                    speed = Vector2.UnitX * -(0.5f + Main.rand.NextFloat(6f));
                    break;
            }
            return speed;
        }
    }
}
