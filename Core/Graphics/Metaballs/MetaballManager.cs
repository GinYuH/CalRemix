using System;
using System.Collections.Generic;
using System.Linq;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace CalRemix.Core.Graphics
{
    public class MetaballManager : ModSystem
    {
        private static readonly List<Metaball> metaballs = new();

        public override void OnModLoad()
        {
            Main.OnPreDraw += PrepareMetaballTargets;
            foreach (Type t in AssemblyManager.GetLoadableTypes(Mod.Code))
            {
                if (!t.IsSubclassOf(typeof(Metaball)) || t.IsAbstract)
                    continue;

                lock (metaballs)
                {
                    metaballs.Add((Metaball)Activator.CreateInstance(t));
                }
            }

            Terraria.On_Main.DrawProjectiles += DrawMetaballsAfterProjectiles;
            Terraria.On_Main.DrawNPCs += DrawMetaballsBeforeNPCs;
        }

        public override void OnModUnload()
        {
            Main.OnPreDraw -= PrepareMetaballTargets;
            Main.QueueMainThreadAction(() =>
            {
                foreach (Metaball metaball in metaballs)
                    metaball?.Dispose();
            });
        }

        private void PrepareMetaballTargets(GameTime obj)
        {
            // Prepare the sprite batch for drawing. Metaballs may restart the sprite batch if their implementation requires it.
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, Main.Rasterizer, null, Matrix.Identity);

            var gd = Main.instance.GraphicsDevice;
            lock (metaballs)
            {
                foreach (Metaball metaball in metaballs)
                {
                    // Update the metaball.
                    if (!Main.gamePaused)
                        metaball.Update();

                    // Prepare the sprite batch in accordance to the needs of the metaball instance. By default this does nothing, 
                    metaball.PrepareSpriteBatch(Main.spriteBatch);

                    // Draw the raw contents of the metaball to each of its render targets.
                    foreach (ManagedRenderTarget target in metaball.LayerTargets)
                    {
                        gd.SetRenderTarget(target.Target);
                        gd.Clear(Color.Transparent);
                        metaball.DrawInstances();
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, Main.Rasterizer, null, Matrix.Identity);
                }
            }

            // Return the backbuffer and end the sprite batch.
            gd.SetRenderTarget(null);
            Main.spriteBatch.End();
        }

        private void DrawMetaballsAfterProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
        {
            orig(self);

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            DrawMetaballs(MetaballDrawLayerType.AfterProjectiles);
            Main.spriteBatch.End();
        }

        private void DrawMetaballsBeforeNPCs(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
        {
            if (!behindTiles)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                DrawMetaballs(MetaballDrawLayerType.BeforeNPCs);
                Main.spriteBatch.ExitShaderRegion();
            }
            orig(self, behindTiles);
        }

        internal static void DrawMetaballs(MetaballDrawLayerType layerType)
        {
            var metaballShader = GameShaders.Misc[$"{CalRemix.instance.Name}:MetaballEdgeShader"];
            var gd = Main.instance.GraphicsDevice;

            foreach (Metaball metaball in metaballs.Where(m => m.DrawContext == layerType))
            {
                for (int i = 0; i < metaball.LayerTargets.Count; i++)
                {
                    gd.Textures[1] = metaball.Layers[i].Value;
                    metaballShader.Shader.Parameters["layerSize"].SetValue(metaball.Layers[i].Value.Size());
                    metaballShader.Shader.Parameters["screenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
                    metaballShader.Shader.Parameters["layerOffset"].SetValue(Main.screenPosition / new Vector2(Main.screenWidth, Main.screenHeight));
                    metaballShader.Shader.Parameters["edgeColor"].SetValue(metaball.EdgeColor.ToVector4());
                    metaballShader.Shader.Parameters["singleFrameScreenOffset"].SetValue((Main.screenLastPosition - Main.screenPosition) / new Vector2(Main.screenWidth, Main.screenHeight));
                    metaballShader.Apply();

                    Main.spriteBatch.Draw(metaball.LayerTargets[i].Target, Main.screenLastPosition - Main.screenPosition, Color.White);
                }
            }
        }
    }
}
