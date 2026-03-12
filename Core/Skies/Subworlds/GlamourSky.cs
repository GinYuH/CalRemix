using CalamityMod;
using CalamityMod.Projectiles.Melee;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Core.Biomes;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using static CalamityMod.NPCs.VanillaNPCAIOverrides.RegularEnemies.RevengeanceAndDeathAI;

namespace CalRemix.Core.Scenes.Subworlds
{
    public class GlamourStar
    {
        public float idealSize;
        public int lifeTime;
        public float rotation;
        public Vector2 position;
        public int maxLife;
        public Color color;
        public GlamourStar(float idealSize, int lifeTime, float rotation, Vector2 position)
        {
            this.idealSize = idealSize;
            this.lifeTime = lifeTime;
            this.maxLife = lifeTime;
            this.rotation = rotation;
            this.position = position;
            color = Utils.SelectRandom<Color>(Main.rand, Color.PaleGoldenrod, Color.Magenta, Color.Cyan, Color.Gold, Color.HotPink, Color.LightYellow, Color.Orange);
        }
    }
    public class GlamourSky : CustomSky
    {
        public List<GlamourStar> stars = new List<GlamourStar>();
        public float BackgroundIntensity;
        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.IsActive<GlamourSubworld>();
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

            if (stars.Count < 22)
            {
                for (int i = 0; i < 100; i++)
                {
                    stars.Add(new GlamourStar(Main.rand.NextFloat(0.01f, 0.02f), Main.rand.Next(300, 1200), Main.rand.NextFloat(MathHelper.TwoPi), new Vector2(Main.rand.Next(Main.screenWidth), Main.rand.Next(Main.screenHeight))));
                }
            }
            else if (stars.Count < 200)
            {
                if (Main.rand.NextBool(5))
                    stars.Add(new GlamourStar(Main.rand.NextFloat(0.01f, 0.02f), Main.rand.Next(300, 1200), Main.rand.NextFloat(MathHelper.TwoPi), new Vector2(Main.rand.Next(Main.screenWidth), Main.rand.Next(Main.screenHeight))));
            }
            for (int i = 0; i < stars.Count; i++)
            {
                GlamourStar star = stars[i];
                star.lifeTime--;
                if (star.lifeTime <= 0)
                {
                    stars.RemoveAt(i);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (minDepth < 0)
            {
                float edge = Main.maxTilesX * 0.86f * 16;
                float edgeEnd = Main.maxTilesX * 0.91f * 16;
                float fadeOff = Utils.GetLerpValue(edgeEnd, edge, Main.LocalPlayer.Center.X, true);
                Texture2D starr = CalRemixAsset.BloomTexture;
                for (int i = 0; i < Main.screenHeight; i++)
                {
                    Color shade = Color.Lerp(new Color(6, 0, 64), new Color(0, 0, 0), 1 - (i / (float)Main.screenHeight));
                    Color final = Color.Lerp(Color.Black, shade, fadeOff);
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, i, Main.screenWidth, 1), final);
                }
                spriteBatch.EnterShaderRegion(BlendState.Additive);
                foreach (GlamourStar star in stars)
                {
                    float opacity = star.lifeTime > 60 ? Utils.GetLerpValue(star.maxLife, star.maxLife - 60, star.lifeTime, true) : Utils.GetLerpValue(0, 60, star.lifeTime, true);
                    spriteBatch.Draw(starr, star.position, null, star.color * opacity * fadeOff, star.rotation, starr.Size() / 2, star.idealSize, 0, 0);
                }
                spriteBatch.ExitShaderRegion();
            }
        }

        public override Color OnTileColor(Color inColor) => inColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { stars.Clear(); }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { stars.Clear(); }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
