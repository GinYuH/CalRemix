using CalRemix.NPCs.Bosses.BossScule;
using CalRemix.NPCs.PandemicPanic;
using CalRemix.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Scenes
{
    public class BloodCell
    {
        public bool variant;
        public float size;
        public int lifeTime;
        public float rotation;
        public Vector2 position;
        public float opacity = 0;
        public BloodCell(bool variant, float size, int lifeTime, float rotation, Vector2 position)
        {
            this.variant = variant;
            this.size = size;
            this.lifeTime = lifeTime;
            this.rotation = rotation;
            this.position = position;
        }
    }
    public class PandemicSky : CustomSky
    {
        private bool _isActive;
        public float BackgroundIntensity;
        public float LightningIntensity;
        public List<BloodCell> cells = new List<BloodCell>();

        public static bool CanSkyBeActive
        {
            get
            {
                return PandemicPanic.IsActive;
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
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].position -= Vector2.UnitY * 5;
                cells[i].lifeTime -= 1;
                if (cells[i].lifeTime > 60)
                {
                    if (cells[i].opacity < 1)
                    {
                        cells[i].opacity += 0.04f;
                    }
                }
                else
                {
                    cells[i].opacity -= 0.04f;
                }
                if (cells[i].lifeTime <= 0)
                {
                    cells.RemoveAt(i);
                }
            }
            if (Main.rand.NextBool(4))
            {
                SpawnCell();
            }
        }

        public void SpawnCell()
        {
            Vector2 spawnPos = Main.LocalPlayer.position + new Vector2(Main.rand.Next(-Main.screenWidth * 2, Main.screenWidth * 2), Main.screenHeight);
            BloodCell c = new BloodCell(Main.rand.NextBool(), Main.rand.NextFloat(0.6f, 1.4f), Main.rand.Next(320, 480), Main.rand.NextFloat(0, MathHelper.TwoPi), spawnPos);
            cells.Add(c);
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (!PandemicPanic.IsActive)
                return;
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.DarkRed * 0.4f);
            for (int i = 0; i < cells.Count; i++)
            {
                BloodCell c = cells[i];
                Texture2D cell = c.variant ? ModContent.Request<Texture2D>("CalRemix/NPCs/PandemicPanic/RedBloodCell").Value : ModContent.Request<Texture2D>("CalRemix/NPCs/PandemicPanic/RedBloodCell2").Value;
                spriteBatch.Draw(cell, c.position- Main.screenPosition, null, Color.White * 0.2f * c.opacity, c.rotation, cell.Size() / 2, c.size, SpriteEffects.None, 0f);
            }
        }
        public override Color OnTileColor(Color inColor) => new Color(Vector4.Lerp(Color.Red.ToVector4(), inColor.ToVector4(), 1f - BackgroundIntensity));

        public override float GetCloudAlpha()
        {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            _isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            _isActive = false;
            cells.Clear();
        }

        public override void Reset()
        {
            _isActive = false;
            cells.Clear();
        }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
