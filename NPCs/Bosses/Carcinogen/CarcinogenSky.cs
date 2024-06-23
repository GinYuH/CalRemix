using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalRemix.NPCs.Bosses.Carcinogen
{
    public class CarcinogenSky : CustomSky
    {
        public float BackgroundIntensity;
        public float LightningIntensity;
        public static bool CanSkyBeActive
        {
            get
            {
                return NPC.AnyNPCs(ModContent.NPCType<Carcinogen>());
            }
        }

        public static readonly Color DrawColor = Color.Tan;
        public float fannyPos = 0;
        public float fannyPos2 = Main.screenWidth;
        public float fannyPos3 = Main.screenWidth;

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
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
        }

        public override Color OnTileColor(Color inColor) => new Color(Vector4.Lerp(DrawColor.ToVector4(), inColor.ToVector4(), 1f - BackgroundIntensity));

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
