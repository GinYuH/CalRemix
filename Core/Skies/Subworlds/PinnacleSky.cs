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
    public class PinnacleAsh
    {
        public float idealSize;
        public int lifeTime;
        public float rotation;
        public Vector2 position;
        public int maxLife;
        public int frame;
        public Vector2 velocity;
        public PinnacleAsh(float idealSize, int lifeTime, float rotation, Vector2 position)
        {
            this.idealSize = idealSize;
            this.lifeTime = lifeTime;
            this.maxLife = lifeTime;
            this.rotation = rotation;
            this.position = position;
            frame = Main.rand.Next(4);
            velocity = new Vector2(Main.rand.NextFloat(-4, -2), Main.rand.NextFloat(3, 4));
        }
    }
    public class PinnacleSky : CustomSky
    {
        public static List<PinnacleAsh> ashes = new List<PinnacleAsh>();
        public float BackgroundIntensity;
        public static bool CanSkyBeActive
        {
            get
            {
                if (SubworldSystem.IsActive<PinnaclesSubworld>())
                    return true;
                /*if (Main.LocalPlayer.TryGetModPlayer(out CalRemixPlayer mp))
                    return mp.PinnacleMonolith;*/
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

            int width = Main.screenWidth * 2;
            int spawnPosY = -Main.screenHeight;
            int height = Main.screenHeight - spawnPosY;

            Rectangle spawnRect = new Rectangle((int)Main.screenPosition.X + (int)Main.LocalPlayer.velocity.X, (int)Main.screenPosition.Y - Main.screenHeight + (int)Main.LocalPlayer.velocity.Y, Main.screenWidth * 2, Main.screenHeight);

            float completion = Utils.GetLerpValue(Main.maxTilesX * 8, Main.maxTilesX * 16, Main.LocalPlayer.Center.X, true);
            
            int max = (int)MathHelper.Lerp(2000, 5000, completion);
            int frequency = (int)MathHelper.Lerp(1, 10, completion);

            max = (int)MathHelper.Min(max, CalamityClientConfig.Instance.ParticleLimit);

            if (ashes.Count < 22)
            {
                for (int i = 0; i < 100; i++)
                {
                    ashes.Add(new PinnacleAsh(Main.rand.NextFloat(0.75f, 1f), Main.rand.Next(300, 1200), Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.NextVector2FromRectangle(spawnRect)));
                }
            }
            else if (ashes.Count < max)
            {
                for (int i = 0; i < frequency; i++)
                    ashes.Add(new PinnacleAsh(Main.rand.NextFloat(0.75f, 1f), Main.rand.Next(300, 1200), Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.NextVector2FromRectangle(spawnRect)));
            }
            for (int i = 0; i < ashes.Count; i++)
            {
                PinnacleAsh speck = ashes[i];
                speck.position += speck.velocity;
                speck.lifeTime--;
                speck.rotation += Main.rand.NextFloat(0.01f, 0.2f);

                if (speck.position.Y > (Main.screenPosition.Y + Main.screenHeight * 2) || speck.lifeTime <= 0)
                {
                    ashes.RemoveAt(i);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (minDepth < 0)
            {
                Texture2D starr = ModContent.Request<Texture2D>("CalRemix/Content/Particles/AshFlake").Value;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new(161, 161, 161));
                foreach (PinnacleAsh speck in ashes)
                {
                    if (speck.idealSize <= 0.9f)
                    {
                        float opacity = speck.lifeTime > 60 ? Utils.GetLerpValue(speck.maxLife, speck.maxLife - 60, speck.lifeTime, true) : Utils.GetLerpValue(0, 60, speck.lifeTime, true);
                        spriteBatch.Draw(starr, speck.position - Main.screenPosition, starr.Frame(1, 4, 0, speck.frame), Lighting.GetColor(speck.position.ToTileCoordinates()) * opacity, speck.rotation, new Vector2(starr.Width / 2, starr.Height / 8), speck.idealSize, 0, 0);
                    }
                }
            }
        }

        public override Color OnTileColor(Color inColor) => inColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { ashes.Clear(); }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { ashes.Clear(); }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
