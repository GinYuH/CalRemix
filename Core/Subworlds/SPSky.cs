using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Summon;
using CalRemix.Content.NPCs.Subworlds.SingularPoint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using static Terraria.Main;

namespace CalRemix.Core.Subworlds
{
    public class SPSky : CustomSky
    {
        public static bool CanSkyBeActive
        {
            get
            {
                return SubworldSystem.Current == ModContent.GetInstance<SingularPointSubworld>();
            }
        }

        public static float SkyOpacity = 0;

        public static readonly Color DrawColor = Color.White;

        public override void Update(GameTime gameTime)
        {
            if (!CanSkyBeActive)
            {
                Deactivate(Array.Empty<object>());
                return;
            }
            if (!NPC.AnyNPCs(ModContent.NPCType<AnomalyTwo>()))
                SkyOpacity = 0;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, (int)Main.screenWidth, (int)Main.screenHeight), Color.Black);

            var shader = GameShaders.Misc["CalRemix:AnomalyBorder"];
            Vector2 center = new Vector2(Main.maxTilesX, Main.maxTilesY) * 8f - Main.screenPosition;
            Vector2 dimensions = new Vector2(8000, 4000);   
            shader.UseColor(new Color(0, 255, 123));
            shader.Shader.Parameters["rectangle"].SetValue(new Vector4(center.X - dimensions.X / 2, center.Y - dimensions.Y / 2, center.X + dimensions.X / 2, center.Y + dimensions.Y / 2));
            shader.Shader.Parameters["topLeft"].SetValue(dimensions / 3);
            shader.Shader.Parameters["bottomRight"].SetValue(dimensions * 0.66f);
            shader.Shader.Parameters["opacity"].SetValue(SkyOpacity * 0.66f);
            shader.SetShaderTexture(ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Perlin"));
            shader.Apply();
            Main.spriteBatch.EnterShaderRegion(BlendState.AlphaBlend, shader.Shader);

            Texture2D portalTexture = TextureAssets.Item[ModContent.ItemType<Baroclaw>()].Value;
            Main.spriteBatch.Draw(portalTexture, center, new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y), Color.White, 0, dimensions / 2, 1, 0, 0);

            Main.spriteBatch.ExitShaderRegion();
        }

        public override Color OnTileColor(Color inColor) => DrawColor;

        public override float GetCloudAlpha() => 0f;

        public override void Reset() { }

        public override void Activate(Vector2 position, params object[] args) { }

        public override void Deactivate(params object[] args) { }

        public override bool IsActive() => CanSkyBeActive && !Main.gameMenu;
    }
}
