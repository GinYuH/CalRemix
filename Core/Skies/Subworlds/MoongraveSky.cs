using System;
using System.Security.Policy;
using CalamityMod;
using CalamityMod.Projectiles.Melee;
using CalRemix.Content.NPCs.Subworlds.Sealed;
using CalRemix.Core.Biomes;
using CalRemix.Core.Subworlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs.Bosses.Carcinogen
{
    public class MoongraveSky : CustomSky
    {
        public float BackgroundIntensity;
        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.IsActive<MoonGraveyardSubworld>();
            }
        }

        public static Color current = new();

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
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (minDepth < 0)
            {
                Texture2D sun = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle").Value;
                //spriteBatch.EnterShaderRegion(BlendState.NonPremultiplied, matrix: Main.BackgroundViewMatrix);
                Vector2 sunPosition = new Vector2(Main.screenWidth / 2, Main.screenHeight * 0.65f);
                spriteBatch.Draw(sun, sunPosition, null, Color.MediumBlue with { A = 0 }, 0, sun.Size() / 2, 10, 0, 0);
                spriteBatch.Draw(sun, sunPosition, null, Color.White with { A = 0 }, 0, sun.Size() / 2, 9.5f, 0, 0);
                //spriteBatch.ExitShaderRegion();
            }
        }

        public override Color OnTileColor(Color inColor) => inColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
