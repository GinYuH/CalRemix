using CalRemix.Content.NPCs.PandemicPanic;
using CalRemix.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Core.Scenes
{
    public class Eater
    {
        public bool variant;
        public float size;
        public float depth;
        public int lifeTime;
        public float rotation;
        public Vector2 position;
        public float opacity = 0;
        public Eater(bool variant, float size, float depth, int lifeTime, float rotation, Vector2 position)
        {
            this.variant = variant;
            this.size = size;
            this.depth = depth;
            this.lifeTime = lifeTime;
            this.rotation = rotation;
            this.position = position;
        }
    }
    public class EaterSky : CustomSky
    {
        private bool _isActive;
        public float BackgroundIntensity;
        public float LightningIntensity;
        public List<Eater> worms = new List<Eater>();

        public static bool CanSkyBeActive
        {
            get
            {
                return CalRemixWorld.eaterTimer > 0;
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

            LightningIntensity = MathHelper.Clamp(LightningIntensity * 0.95f - 0.025f, 0f, 1f);
            BackgroundIntensity = MathHelper.Clamp(BackgroundIntensity + 0.01f, 0f, 1f);

            Opacity = BackgroundIntensity;
            for (int i = 0; i < worms.Count; i++)
            {
                worms[i].position += Vector2.UnitY * 5;
                worms[i].lifeTime -= 1;
                if (worms[i].lifeTime > 60)
                {
                    if (worms[i].opacity < 1)
                    {
                        worms[i].opacity += 0.1f;
                    }
                }
                else
                {
                    worms[i].opacity -= 0.04f;
                }
                worms[i].rotation += (worms[i].variant).ToDirectionInt() * 0.1f;
                if (worms[i].lifeTime <= 0)
                {
                    worms.RemoveAt(i);
                }
            }
            if (Main.rand.NextBool(3))
            {
                SpawnWorm();
            }
        }

        public void SpawnWorm()
        {
            for (int i = 0; i < 1; i++)
            {
                Vector2 spawnPos = Main.LocalPlayer.position + new Vector2(Main.rand.Next(-Main.screenWidth * 2, Main.screenWidth * 2), -Main.screenHeight);
                float scale = Main.rand.NextFloat(0.3f, 1.1f);
                float layer = MathHelper.Lerp(6, -1, Utils.GetLerpValue(0.3f, 1.1f, scale, true));
                Eater c = new Eater(Main.rand.NextBool(), scale, layer, Main.rand.Next(320, 480), Main.rand.NextFloat(0, MathHelper.TwoPi), spawnPos);
                worms.Add(c);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            /*bool drawBack = minDepth < 6;
            bool drawAfterBack = minDepth < 5;
            bool drawAfterMid = minDepth < 2.9f;
            bool drawAfterFront = minDepth < 2.1f;
            bool drawFront = minDepth < 0;*/
            Texture2D cell = ModContent.Request<Texture2D>("CalRemix/Assets/ExtraTextures/Eater").Value;
            for (int i = 0; i < worms.Count; i++)
            {
                Eater c = worms[i];
                if (minDepth > c.depth)
                {
                    spriteBatch.Draw(cell, c.position - Main.screenPosition, null, Color.White * c.opacity * Utils.GetLerpValue(0, 20, CalRemixWorld.eaterTimer, true), c.rotation, cell.Size() / 2, c.size, SpriteEffects.None, 0f);
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return 1f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            _isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            _isActive = false;
            worms.Clear();
        }

        public override void Reset()
        {
            _isActive = false;
            worms.Clear();
        }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
