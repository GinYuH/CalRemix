using CalRemix.Content.Tiles.TVs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.ModLoader;

namespace CalRemix.Core.VideoPlayer;

public class TVDrawSystem : ModSystem
{
    private static List<TVTileEntity> _tvsToRender = new();

    public static List<TVTileEntity> GetActiveTVs() => _tvsToRender;

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
                    128, 80 // Approximate TV size
                );

                if (screenBounds.Intersects(tvBounds))
                {
                    _tvsToRender.Add(tvEntity);
                }
            }
        }
    }

    public override void OnModLoad()
    {
        On_Main.DrawNPCs += DrawTVScreens;
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
            // Calculate all screen areas first
            var tvData = new List<(Vector2 Position, Vector2 Size, Rectangle StaticArea, TVTileEntity Entity, bool NeedsStatic)>();

            foreach (var tvEntity in _tvsToRender)
            {
                var (position, size, staticArea) = tvEntity.CalculateScreenAreas();

                var player = tvEntity.GetVideoPlayer();
                bool needsStatic = tvEntity.IsOn &&
                    (player == null || (!player.IsPlaying && !player.IsLoading && !player.IsPreparing));

                tvData.Add((position, size, staticArea, tvEntity, needsStatic));
            }

            // Draw all videos first (no matrix)
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer);

            foreach (var (position, size, staticArea, tvEntity, needsStatic) in tvData)
            {
                if (!needsStatic)
                {
                    try
                    {
                        tvEntity.DrawVideoOrLoading(Main.spriteBatch, position, size);
                    }
                    catch (Exception ex)
                    {
                        CalRemix.instance.Logger.Error($"Error drawing video for TV at {tvEntity.Position}: {ex.Message}");
                    }
                }
            }

            // Then draw all static in one batch (with matrix)
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (var (position, size, staticArea, tvEntity, needsStatic) in tvData)
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
            // Calculate all screen areas first
            var tvData = new List<(Vector2 Position, Vector2 Size, Rectangle StaticArea, TVTileEntity Entity, bool NeedsStatic)>();

            foreach (var tvEntity in _tvsToRender)
            {
                var (position, size, staticArea) = tvEntity.CalculateScreenAreas();

                var player = tvEntity.GetVideoPlayer();
                bool needsStatic = tvEntity.IsOn &&
                    (player == null || (!player.IsPlaying && !player.IsLoading && !player.IsPreparing));

                tvData.Add((position, size, staticArea, tvEntity, needsStatic));
            }

            // Draw all videos first (no matrix)
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);

            foreach (var (position, size, staticArea, tvEntity, needsStatic) in tvData)
            {
                if (!needsStatic)
                {
                    try
                    {
                        tvEntity.DrawVideoOrLoading(spriteBatch, position, size);
                    }
                    catch (Exception ex)
                    {
                        CalRemix.instance.Logger.Error($"Error drawing video for TV at {tvEntity.Position}: {ex.Message}");
                    }
                }
            }

            spriteBatch.End();

            // Then draw all static in one batch (with matrix)
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (var (position, size, staticArea, tvEntity, needsStatic) in tvData)
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
}