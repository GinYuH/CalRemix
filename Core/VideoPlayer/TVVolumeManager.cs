using CalRemix.Content.Tiles.TVs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Core.VideoPlayer;

/// <summary>
/// Manages volume for shared channels with spatial audio.
/// For each channel, finds the closest TV and sets the channel's volume based on that distance.
/// TVs on different channels play independently.
/// Special override channel overrides all TVs and uses closest TV for volume.
/// </summary>
public class TVVolumeManager : ModSystem
{
    // Distance settings
    private const float MAX_HEARING_DISTANCE = 800f;
    private const float FULL_VOLUME_DISTANCE = 150f;

    // Performance optimization
    private int _frameCounter = 0;
    private const int UPDATE_INTERVAL = 2;

    public override void PostUpdateEverything()
    {
        _frameCounter++;
        if (_frameCounter % UPDATE_INTERVAL != 0)
            return;

        Player localPlayer = Main.LocalPlayer;
        if (!localPlayer.active || Main.gameMenu)
            return;

        UpdateChannelVolumes(localPlayer);
    }

    /// <summary>
    /// Update volume for each channel based on the closest TV to the player.
    /// </summary>
    private void UpdateChannelVolumes(Player player)
    {
        var manager = ModContent.GetInstance<VideoChannelManager>();
        if (manager == null) return;

        // Check if override channel is active
        bool overrideActive = manager.IsOverrideChannelActive();

        if (overrideActive)
        {
            // When override is active, find the closest TV (any TV, regardless of channel)
            // and use that distance to set the override channel volume
            UpdateOverrideChannelVolume(player, manager);
        }
        else
        {
            // Normal channel volume management
            UpdateRegularChannelVolumes(player, manager);
        }
    }

    /// <summary>
    /// Update volume for the override channel based on closest TV.
    /// </summary>
    private void UpdateOverrideChannelVolume(Player player, VideoChannelManager manager)
    {
        TVTileEntity closestTV = null;
        float closestDistance = float.MaxValue;

        // Find the closest ON TV (any channel)
        foreach (var kvp in TileEntity.ByID)
        {
            if (kvp.Value is not TVTileEntity tvEntity || !tvEntity.IsOn)
                continue;

            Vector2 tvCenter = GetTVCenter(tvEntity);
            float distance = Vector2.Distance(player.Center, tvCenter);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTV = tvEntity;
            }
        }

        // Get the override channel player
        var overridePlayer = manager.GetChannelPlayer(VideoChannelManager.DEFAULT_CHANNEL); // Will return override if active
        if (overridePlayer == null || !overridePlayer.IsPlaying)
            return;

        if (closestTV != null)
        {
            int distanceVolume = CalculateVolumeFromDistance(closestDistance);
            int targetVolume = (int)(distanceVolume * (closestTV.Volume / 100f));
            targetVolume = Math.Clamp(targetVolume, 0, 100);

            int currentVolume = overridePlayer.GetVolume();
            int smoothedVolume = (int)(currentVolume * 0.7f + targetVolume * 0.3f);

            if (Math.Abs(smoothedVolume - currentVolume) > 2)
            {
                overridePlayer.SetVolume(smoothedVolume);

                if (_frameCounter % 120 == 0 && smoothedVolume > 0)
                {
                    CalRemix.instance.Logger.Debug(
                        $"Override Channel: Volume {smoothedVolume}% " +
                        $"(closest TV at {closestDistance:F0} units)"
                    );
                }
            }

            if (targetVolume > 0)
                overridePlayer.SetMute(false);
            else if (targetVolume == 0)
                overridePlayer.SetMute(true);
        }
        else
        {
            // No TVs on, mute the override channel
            overridePlayer.SetMute(true);
        }
    }

    /// <summary>
    /// Update volume for regular channels (when override is NOT active).
    /// </summary>
    private void UpdateRegularChannelVolumes(Player player, VideoChannelManager manager)
    {
        var channelClosestTV = new Dictionary<int, (TVTileEntity TV, float Distance, int TargetVolume)>();

        foreach (var kvp in TileEntity.ByID)
        {
            if (kvp.Value is not TVTileEntity tvEntity || !tvEntity.IsOn)
                continue;

            int channelId = tvEntity.CurrentChannel;

            Vector2 tvCenter = GetTVCenter(tvEntity);
            float distance = Vector2.Distance(player.Center, tvCenter);

            int distanceVolume = CalculateVolumeFromDistance(distance);
            int targetVolume = (int)(distanceVolume * (tvEntity.Volume / 100f));
            targetVolume = Math.Clamp(targetVolume, 0, 100);

            if (!channelClosestTV.ContainsKey(channelId) ||
                distance < channelClosestTV[channelId].Distance)
            {
                channelClosestTV[channelId] = (tvEntity, distance, targetVolume);
            }
        }

        foreach (var kvp in channelClosestTV)
        {
            int channelId = kvp.Key;
            var (closestTV, distance, targetVolume) = kvp.Value;

            var channelPlayer = manager.GetChannelPlayer(channelId);
            if (channelPlayer == null || !channelPlayer.IsPlaying)
                continue;

            int currentVolume = channelPlayer.GetVolume();

            int smoothedVolume = (int)(currentVolume * 0.7f + targetVolume * 0.3f);

            if (Math.Abs(smoothedVolume - currentVolume) > 2)
            {
                channelPlayer.SetVolume(smoothedVolume);

                if (_frameCounter % 120 == 0 && smoothedVolume > 0)
                {
                    CalRemix.instance.Logger.Debug(
                        $"Channel {channelId}: Volume {smoothedVolume}% " +
                        $"(closest TV at {distance:F0} units)"
                    );
                }
            }

            if (targetVolume > 0)
                channelPlayer.SetMute(false);
            else if (targetVolume == 0)
                channelPlayer.SetMute(true);
        }

        for (int channelId = VideoChannelManager.MIN_CHANNEL;
             channelId <= VideoChannelManager.MAX_CHANNEL;
             channelId++)
        {
            if (!channelClosestTV.ContainsKey(channelId))
            {
                var channelPlayer = manager.GetChannelPlayer(channelId);
                if (channelPlayer != null && channelPlayer.IsPlaying)
                {
                    channelPlayer.SetMute(true);
                }
            }
        }
    }

    /// <summary>
    /// Get the world center position of a TV.
    /// </summary>
    private static Vector2 GetTVCenter(TVTileEntity tvEntity)
    {
        if (TVTileEntity.TileData.TryGetValue(
            Main.tile[tvEntity.Position.X, tvEntity.Position.Y].TileType,
            out var tileInfo))
        {
            return new Vector2(
                (tvEntity.Position.X + tileInfo.TileSize.X / 2f) * 16f,
                (tvEntity.Position.Y + tileInfo.TileSize.Y / 2f) * 16f
            );
        }

        return new Vector2(
            (tvEntity.Position.X + 0.5f) * 16f,
            (tvEntity.Position.Y + 0.5f) * 16f
        );
    }

    /// <summary>
    /// Calculate volume based on distance from player.
    /// </summary>
    private static int CalculateVolumeFromDistance(float distance)
    {
        if (distance <= FULL_VOLUME_DISTANCE)
            return 100;

        if (distance >= MAX_HEARING_DISTANCE)
            return 0;

        float t = (distance - FULL_VOLUME_DISTANCE) /
                 (MAX_HEARING_DISTANCE - FULL_VOLUME_DISTANCE);

        return (int)(100 * (1 - t));
    }

    public override void OnWorldUnload()
    {
        _frameCounter = 0;
    }
}