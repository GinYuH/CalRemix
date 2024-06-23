using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace CalRemix.UI.Title
{
    public class CalRemixMenu2 : ModMenu
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
            logoTexture = ModContent.Request<Texture2D>("CalRemix/UI/Title/Logo2");
        }
        public override Asset<Texture2D> SunTexture => blankTexture;
        public override Asset<Texture2D> MoonTexture => blankTexture;
        public override Asset<Texture2D> Logo => logoTexture;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CrazyLaPaint");
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<MenuBgStyle>();
        public override string DisplayName => "Ultimate Calamity Stylepilled Menu";
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
        public class Character
        {
            public Texture2D texture;
            public int direction;
            public Vector2 center;
            public Vector2 velocity;
            public Character(Texture2D _texture, int _direction, Vector2 _center, Vector2 _velocity)
            {
                texture = _texture;
                direction = _direction;
                center = _center;
                velocity = _velocity;
            }
        }
        private float FrameCounter = 0;
        private int Frame = 0;
        public static List<Star> Stars { get; internal set; } = new List<Star>();
        public static List<Character> Characters { get; internal set; } = new List<Character>();
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            // Menu
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

            // Characters
            if (Characters.Count < 4 && Main.rand.NextBool(550) && Main.WorldList.Exists((WorldFileData w) => w.IsHardMode))
            {
                bool rand = Main.rand.NextBool();
                Vector2 position = new((rand) ? Main.screenWidth + 100f : -100f, Main.screenHeight * Main.rand.NextFloat(0.1f, 0.9f));

                int rand2 = Main.rand.Next(10);
                Texture2D characterTexture = ModContent.Request<Texture2D>("CalRemix/Items/Weapons/Ogscule").Value;
                int direction = (rand) ? 0 : 1;
                Vector2 velocity = (rand) ? -Vector2.UnitX : Vector2.UnitX;

                bool evilFanny = !Characters.Exists((Character c) => c.texture == ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperEvilFannyIdle").Value) && (Main.WorldList.Exists((WorldFileData w) => w.IsHardMode) || Main.WorldList.Exists((WorldFileData w) => w.ZenithWorld));
                if (rand2 == 9 && (Main.WorldList.Exists((WorldFileData w) => w.DefeatedMoonlord)) && !Characters.Exists((Character c) => c.texture == ModContent.Request<Texture2D>("CalRemix/UI/Title/Blockhound").Value))
                {
                    characterTexture = ModContent.Request<Texture2D>("CalRemix/UI/Title/Blockhound").Value;
                    direction = (rand) ? 1 : 0;
                    velocity = (rand) ? -Vector2.UnitX * Main.rand.NextFloat(2f, 2.5f) : Vector2.UnitX * Main.rand.NextFloat(2f, 2.5f);
                }
                else if (rand2 == 7 && (Main.WorldList.Exists((WorldFileData w) => w.DefeatedMoonlord)) && !Characters.Exists((Character c) => c.texture == ModContent.Request<Texture2D>("CalRemix/UI/Title/Zero").Value))
                {
                    characterTexture = ModContent.Request<Texture2D>("CalRemix/UI/Title/Zero").Value;
                    direction = (rand) ? 1 : 0;
                    velocity = (rand) ? -Vector2.UnitX * Main.rand.NextFloat(3f, 3.5f) : Vector2.UnitX * Main.rand.NextFloat(3f, 3.5f);
                }
                else if (rand2 > 7 && (Main.WorldList.Exists((WorldFileData w) => w.DefeatedMoonlord)))
                {
                    characterTexture = Main.rand.Next(new Texture2D[]
                    {
                        ModContent.Request<Texture2D>("CalRemix/Items/SideGar").Value,
                        ModContent.Request<Texture2D>("CalRemix/UI/Title/FrontGar").Value,
                        ModContent.Request<Texture2D>("CalRemix/UI/Title/RearGar").Value
                    });
                    direction = (rand) ? 1 : 0;
                    velocity = (rand) ? -Vector2.UnitX * Main.rand.NextFloat(1.5f, 1.75f) : Vector2.UnitX * Main.rand.NextFloat(1.5f, 1.75f);
                }
                else if (rand2 < 4 && evilFanny)
                {
                    characterTexture = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/HelperEvilFannyIdle").Value;
                    direction = (rand) ? 1 : 0;
                }
                else if (rand2 == 4 && (Main.WorldList.Exists((WorldFileData w) => w.IsHardMode)) && !Characters.Exists((Character c) => c.texture == ModContent.Request<Texture2D>("CalRemix/NPCs/Minibosses/OnyxKinsman").Value))
                {
                    characterTexture = ModContent.Request<Texture2D>("CalRemix/NPCs/Minibosses/OnyxKinsman").Value;
                    direction = (rand) ? 0 : 1;
                    velocity = (rand) ? -Vector2.UnitX * Main.rand.NextFloat(4f, 4.5f) : Vector2.UnitX * Main.rand.NextFloat(4f, 4.5f);
                }
                else
                {
                    characterTexture = Main.rand.Next(new Texture2D[] 
                    { 
                        ModContent.Request<Texture2D>("CalRemix/Items/Weapons/Ogscule").Value,
                        ModContent.Request<Texture2D>("CalRemix/Items/Accessories/Baroclaw").Value,
                        ModContent.Request<Texture2D>("CalRemix/NPCs/Bosses/Acideye/MutatedEye").Value 
                    });
                    direction = (rand) ? 0 : 1;
                    velocity = (rand) ? -Vector2.UnitX * Main.rand.NextFloat(1.5f, 1.75f) : Vector2.UnitX * Main.rand.NextFloat(1.5f, 1.75f);
                }
                Characters.Add(new Character(characterTexture, direction, position, velocity));
            }
            for (int j = 0; j < Characters.Count; j++)
            {
                Characters[j].center += Characters[j].velocity;
            }
            Characters.RemoveAll((Character c) => c.center.X >= (Main.screenWidth + 120f) || c.center.X <= (-120f));
            for (int k = 0; k < Characters.Count; k++)
            {
                spriteBatch.Draw(Characters[k].texture, Characters[k].center, null, Color.White, 0f, Characters[k].texture.Size() * 0.5f, 1f, (SpriteEffects)Characters[k].direction, 0f);
            }

            // Stars
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

            // Logo and Final Stuff
            Main.time = 27000.0;
            Main.dayTime = true;
            Texture2D Glow = ModContent.Request<Texture2D>("CalRemix/UI/Title/LogoGlow2").Value;
            Rectangle rect = new(0, Frame * (Logo.Value.Height / 5), Logo.Value.Width, Logo.Value.Height / 5);
            spriteBatch.Draw(Glow, new Vector2((float)Main.screenWidth / 2f, 471f) + new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3)), rect, Main.DiscoColor, 0, Glow.Size() * 0.5f, 0.45f, SpriteEffects.None, 0f);
            spriteBatch.Draw(Logo.Value, new Vector2((float)Main.screenWidth / 2f, 471f), rect, drawColor, 0, Logo.Value.Size() * 0.5f, 0.45f, SpriteEffects.None, 0f);
            FrameCounter++;
            if (FrameCounter > 6)
            {
                if (Frame > 3)
                    Frame = 0;
                else
                    Frame++;
                FrameCounter = 0;
            }
            return false;
        }
    }
}
