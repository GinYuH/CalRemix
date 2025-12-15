using CalRemix.Content.Tiles.TVs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Core.VideoPlayer;

/// <summary>
/// Centralized TV volume management to avoid per-tile calculations.
/// </summary>
public class TVVolumeManager : ModSystem
{
    private struct TVInfo
    {
        public Point16 Position;
        public Point TileSize;
        public TVTileEntity Entity;
    }

    private List<TVInfo> _activeTVs = [];
    private const int UPDATE_INTERVAL = 5;
    private int _frameCounter = 0;

    // Distance settings
    private const float MAX_HEARING_DISTANCE = 800f;
    private const float FULL_VOLUME_DISTANCE = 150f;
    private const float MAX_HEARING_DISTANCE_SQ = MAX_HEARING_DISTANCE * MAX_HEARING_DISTANCE;
    private const float FULL_VOLUME_DISTANCE_SQ = FULL_VOLUME_DISTANCE * FULL_VOLUME_DISTANCE;

    public override void PostUpdateEverything()
    {
        _frameCounter++;
        if (_frameCounter < UPDATE_INTERVAL)
            return;
        _frameCounter = 0;

        _activeTVs.Clear();

        foreach (var kvp in TileEntity.ByID)
        {
            if (kvp.Value is TVTileEntity tvEntity)
            {
                var player = tvEntity.GetVideoPlayer();
                if (player != null && player.IsPlaying)
                {
                    if (TVTileEntity.TileData.TryGetValue(Main.tile[tvEntity.Position.X, tvEntity.Position.Y].TileType, out var tileInfo))
                    {
                        _activeTVs.Add(new TVInfo
                        {
                            Position = tvEntity.Position,
                            TileSize = tileInfo.TileSize,
                            Entity = tvEntity
                        });
                    }
                }
            }
        }

        if (_activeTVs.Count == 0)
            return;

        Player closestPlayer = null;
        float closestDistanceSq = float.MaxValue;

        foreach (Player p in Main.player)
        {
            if (!p.active)
                continue;

            float dx = p.Center.X - Main.LocalPlayer.Center.X;
            float dy = p.Center.Y - Main.LocalPlayer.Center.Y;
            float distSq = dx * dx + dy * dy;

            if (distSq < closestDistanceSq)
            {
                closestDistanceSq = distSq;
                closestPlayer = p;
            }
        }

        if (closestPlayer == null)
            return;

        foreach (var tv in _activeTVs)
        {
            Vector2 tvCenter = new Vector2(
                (tv.Position.X + tv.TileSize.X / 2f) * 16f,
                (tv.Position.Y + tv.TileSize.Y / 2f) * 16f
            );

            float dx = closestPlayer.Center.X - tvCenter.X;
            float dy = closestPlayer.Center.Y - tvCenter.Y;
            float distanceSq = dx * dx + dy * dy;

            int volume = CalculateVolumeFromDistanceSq(distanceSq);

            tv.Entity.GetVideoPlayer()?.SetVolume(volume);
        }
    }

    /// <summary>
    /// Calculate volume using squared distance.
    /// </summary>
    private static int CalculateVolumeFromDistanceSq(float distanceSq)
    {
        if (distanceSq <= FULL_VOLUME_DISTANCE_SQ)
            return 100;

        if (distanceSq >= MAX_HEARING_DISTANCE_SQ)
            return 0;

        // Convert to normalized value [0, 1]
        float normalizedDist = (distanceSq - FULL_VOLUME_DISTANCE_SQ) /
                               (MAX_HEARING_DISTANCE_SQ - FULL_VOLUME_DISTANCE_SQ);

        // Apply curve (sqrt for smoother falloff since we're working with squared distances)
        float volume = 1f - (float)System.Math.Sqrt(normalizedDist);

        return (int)(volume * 100f);
    }
}
