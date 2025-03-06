using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.Graphics
{
    public class TileOverlaysSystem : ModSystem
    {
        public static ManagedRenderTarget OverlayableTarget
        {
            get;
            private set;
        }

        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            Main.QueueMainThreadAction(() => OverlayableTarget = new(true, RenderTargetManager.CreateScreenSizedTarget));
            Main.OnPreDraw += PrepareOverlayTarget;
            Terraria.On_Main.DrawProjectiles += DrawOverlayTarget;
        }

        public override void OnModUnload()
        {
            Main.OnPreDraw -= PrepareOverlayTarget;
        }

        private void PrepareOverlayTarget(GameTime obj)
        {
            var gd = Main.instance.GraphicsDevice;

            gd.SetRenderTarget(OverlayableTarget.Target);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // Draw all projectiles that have the relevant interface.
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.ModProjectile is IDrawsOverTiles drawer)
                    drawer.Draw(Main.spriteBatch);
            }

            Main.spriteBatch.End();
            gd.SetRenderTarget(null);
        }

        private void DrawOverlayTarget(Terraria.On_Main.orig_DrawProjectiles orig, Main self)
        {
            orig(self);

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // Prepare the overlay shader and supply it with tile information.
            var gd = Main.instance.GraphicsDevice;
            var shader = GameShaders.Misc[$"{Mod.Name}:TileOverlayShader"];
            shader.Shader.Parameters["uZoom"].SetValue(new Vector2(1.15f, 1.27f));
            shader.Shader.Parameters["tileOverlayOffset"].SetValue((Main.sceneTilePos - Main.screenPosition) / new Vector2(Main.screenWidth, Main.screenHeight) * -1f);
            shader.Shader.Parameters["inversionZoom"].SetValue(Main.GameViewMatrix.Zoom);
            gd.Textures[1] = Main.instance.tileTarget;
            gd.Textures[2] = Main.instance.blackTarget;
            shader.Apply();

            Main.spriteBatch.Draw(OverlayableTarget.Target, Vector2.Zero, Color.White);
            Main.spriteBatch.End();
        }
    }
}
