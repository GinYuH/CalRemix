using CalRemix.Content.Tiles.TVs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Core.VideoPlayer;

public class TVDrawSystem : ModSystem
{
    private static List<TVTileEntity> _tvsToRender = [];

    public static List<TVTileEntity> GetActiveTVs() => _tvsToRender;

    public override void OnModLoad()
    {
        On_Main.DrawNPCs += DrawTVScreens;
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _tvsToRender.Clear();

        Rectangle screenBounds = new(
            (int)Main.screenPosition.X - 200,
            (int)Main.screenPosition.Y - 200,
            Main.screenWidth + 400,
            Main.screenHeight + 400
        );

        foreach (var kvp in TileEntity.ByID)
        {
            if (kvp.Value is TVTileEntity tvEntity && tvEntity.IsOn)
            {
                // Check if TV is on screen
                Rectangle tvBounds = new Rectangle(
                    tvEntity.Position.X * 16,
                    tvEntity.Position.Y * 16,
                    tvEntity.Size.X * 16,
                    tvEntity.Size.Y * 16
                );

                if (screenBounds.Intersects(tvBounds))
                {
                    _tvsToRender.Add(tvEntity);
                }
            }
        }
    }

    private void DrawTVScreens(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
    {
        if (behindTiles)
        {
            orig(self, behindTiles);
            return;
        }

        if (_tvsToRender.Count == 0)
        {
            orig(self, behindTiles);
            return;
        }

        try
        {
            // Calculate all screen areas and shader info first
            var tvData = new List<(
                Vector2 Position,
                Vector2 Size,
                Rectangle StaticArea,
                TVTileEntity Entity,
                bool NeedsStatic,
                Effect Shader
            )>();

            foreach (var tvEntity in _tvsToRender)
            {
                var (position, size, staticArea) = tvEntity.CalculateScreenAreas();

                var player = tvEntity.GetVideoPlayer();
                bool needsStatic = tvEntity.IsOn &&
                    (player == null || (!player.IsPlaying && !player.IsLoading && !player.IsPreparing));

                // Get shader for this TV type
                Effect shader = GetShaderForTV(tvEntity);

                tvData.Add((position, size, staticArea, tvEntity, needsStatic, shader));
            }

            // Draw all videos (with shaders if applicable)
            Main.spriteBatch.End();

            foreach (var (position, size, staticArea, tvEntity, needsStatic, shader) in tvData)
            {
                if (!needsStatic)
                {
                    try
                    {
                        // Start spritebatch with shader (or without if null)
                        if (shader != null)
                        {
                            // Configure shader parameters
                            ConfigureShaderForTV(shader, tvEntity);

                            Main.spriteBatch.Begin(
                                SpriteSortMode.Immediate,
                                BlendState.AlphaBlend,
                                Main.DefaultSamplerState,
                                DepthStencilState.None,
                                Main.Rasterizer,
                                shader
                            );

                            // Apply shader pass
                            shader.CurrentTechnique.Passes[0].Apply();
                        }
                        else
                        {
                            Main.spriteBatch.Begin(
                                SpriteSortMode.Deferred,
                                BlendState.AlphaBlend,
                                Main.DefaultSamplerState,
                                DepthStencilState.None,
                                Main.Rasterizer
                            );
                        }

                        tvEntity.DrawVideoOrLoading(Main.spriteBatch, position, size);
                        Main.spriteBatch.End();
                    }
                    catch (Exception ex)
                    {
                        CalRemix.instance.Logger.Error($"Error drawing video for TV at {tvEntity.Position}: {ex.Message}");
                        try { Main.spriteBatch.End(); } catch { }
                    }
                }
            }

            // Then draw all static in one batch (with matrix)
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );

            foreach (var (position, size, staticArea, tvEntity, needsStatic, shader) in tvData)
            {
                if (needsStatic)
                {
                    try
                    {
                        tvEntity.DrawStatic(Main.spriteBatch, staticArea);
                    }
                    catch (Exception ex)
                    {
                        CalRemix.instance.Logger.Error($"Error drawing static for TV at {tvEntity.Position}: {ex.Message}");
                    }
                }
            }

            orig(self, behindTiles);
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Critical error in DrawTVScreens: {ex.Message}");
            // Ensure we still call orig to prevent breaking the game
            orig(self, behindTiles);
        }
    }

    public override void PostDrawTiles()
    {
        if (_tvsToRender.Count == 0)
            return;

        SpriteBatch spriteBatch = Main.spriteBatch;

        try
        {
            // Calculate all screen areas and shader info first
            var tvData = new List<(
                Vector2 Position,
                Vector2 Size,
                Rectangle StaticArea,
                TVTileEntity Entity,
                bool NeedsStatic,
                Effect Shader
            )>();

            foreach (var tvEntity in _tvsToRender)
            {
                var (position, size, staticArea) = tvEntity.CalculateScreenAreas();

                var player = tvEntity.GetVideoPlayer();
                bool needsStatic = tvEntity.IsOn &&
                    (player == null || (!player.IsPlaying && !player.IsLoading && !player.IsPreparing));

                // Get shader for this TV type
                Effect shader = GetShaderForTV(tvEntity);

                tvData.Add((position, size, staticArea, tvEntity, needsStatic, shader));
            }

            // Draw each video with its shader
            foreach (var (position, size, staticArea, tvEntity, needsStatic, shader) in tvData)
            {
                if (!needsStatic)
                {
                    try
                    {
                        // Start spritebatch with shader (or without if null)
                        if (shader != null)
                        {
                            // Configure shader parameters
                            ConfigureShaderForTV(shader, tvEntity);

                            spriteBatch.Begin(
                                SpriteSortMode.Immediate,
                                BlendState.AlphaBlend,
                                SamplerState.LinearClamp,
                                DepthStencilState.None,
                                RasterizerState.CullNone,
                                shader
                            );

                            // Apply shader pass
                            shader.CurrentTechnique.Passes[0].Apply();
                        }
                        else
                        {
                            spriteBatch.Begin(
                                SpriteSortMode.Deferred,
                                BlendState.AlphaBlend,
                                SamplerState.LinearClamp,
                                DepthStencilState.None,
                                RasterizerState.CullNone
                            );
                        }

                        tvEntity.DrawVideoOrLoading(spriteBatch, position, size);
                        spriteBatch.End();
                    }
                    catch (Exception ex)
                    {
                        CalRemix.instance.Logger.Error($"Error drawing video for TV at {tvEntity.Position}: {ex.Message}");
                        try { spriteBatch.End(); } catch { }
                    }
                }
            }

            // Then draw all static in one batch (with matrix)
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );

            foreach (var (position, size, staticArea, tvEntity, needsStatic, shader) in tvData)
            {
                if (needsStatic)
                {
                    try
                    {
                        tvEntity.DrawStatic(spriteBatch, staticArea);
                    }
                    catch (Exception ex)
                    {
                        CalRemix.instance.Logger.Error($"Error drawing static for TV at {tvEntity.Position}: {ex.Message}");
                    }
                }
            }

            spriteBatch.End();
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Critical error in PostDrawTiles: {ex.Message}");
            // Ensure spritebatch is ended to prevent issues
            if (spriteBatch != null)
            {
                try { spriteBatch.End(); } catch { }
            }
        }
    }

    /// <summary>
    /// Get the shader effect for a TV based on its tile type.
    /// Returns null if the TV has no shader.
    /// </summary>
    private static Effect GetShaderForTV(TVTileEntity tvEntity)
    {
        try
        {
            int tileType = Main.tile[tvEntity.Position.X, tvEntity.Position.Y].TileType;
            var modTile = TileLoader.GetTile(tileType);

            if (modTile is BaseTVTile baseTVTile)
            {
                Asset<Effect> shaderAsset = baseTVTile.GetShaderEffect();
                return shaderAsset?.Value;
            }
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Error getting shader for TV at {tvEntity.Position}: {ex.Message}");
        }

        return null;
    }

    /// <summary>
    /// Configure shader parameters for a TV.
    /// Calls the tile's ConfigureShader method if available.
    /// </summary>
    private static void ConfigureShaderForTV(Effect shader, TVTileEntity tvEntity)
    {
        try
        {
            int tileType = Main.tile[tvEntity.Position.X, tvEntity.Position.Y].TileType;
            var modTile = TileLoader.GetTile(tileType);

            if (modTile is BaseTVTile baseTVTile)
            {
                baseTVTile.ConfigureShader(shader, tvEntity);
            }
        }
        catch (Exception ex)
        {
            CalRemix.instance.Logger.Error($"Error configuring shader for TV at {tvEntity.Position}: {ex.Message}");
        }
    }
}