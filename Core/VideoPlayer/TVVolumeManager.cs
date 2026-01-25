using CalamityMod;
using CalRemix.Content.Tiles.TVs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using YoutubeExplode.Channels;

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
        if (manager == null) 
            return;

        if (manager.IsOverrideChannelActive())
            UpdateOverrideChannelVolume(player, manager);
        else
            UpdateRegularChannelVolumes(player, manager);
    }

    /// <summary>
    /// Update volume for the override channel based on closest TV.
    /// </summary>
    private void UpdateOverrideChannelVolume(Player player, VideoChannelManager manager)
    {
        TVTileEntity closestTV = FindClosestTV(player, tvEntity => tvEntity.IsOn);

        var overridePlayer = manager.GetChannelPlayer(VideoChannelManager.DEFAULT_CHANNEL);
        if (overridePlayer == null || !overridePlayer.IsPlaying)
            return;

        if (closestTV != null)
        {
            float closestDistance = Vector2.Distance(player.Center, GetTVCenter(closestTV));
            ApplyVolumeToPlayer(overridePlayer, closestTV, closestDistance, "Override Channel");
        }
        else
            overridePlayer.SetMute(true);
    }

    /// <summary>
    /// Update volume for regular channels (when override is NOT active).
    /// </summary>
    private void UpdateRegularChannelVolumes(Player player, VideoChannelManager manager)
    {
        var channelClosestTV = FindClosestTVPerChannel(player, tvEntity => tvEntity.IsOn);

        foreach (var kvp in channelClosestTV)
        {
            int channelId = kvp.Key;
            var (closestTV, distance) = kvp.Value;

            var channelPlayer = manager.GetChannelPlayer(channelId);
            if (channelPlayer == null || !channelPlayer.IsPlaying)
                continue;

            ApplyVolumeToPlayer(channelPlayer, closestTV, distance, $"Channel {channelId}");
        }

        // Mute channels that don't have any TVs on
        MuteInactiveChannels(manager, channelClosestTV);

        foreach(TileEntity te in TileEntity.ByID.Values)
        {
            if (te is not MediaPlayerEntity mpe)
                continue;

            if (!mpe.player.IsPlaying)
                continue;

            var closestTV = FindClosestTV(player, (TVTileEntity e) => e.MediaPlayerPosition == mpe.Position);
            if (closestTV == null)
            {
                mpe.player.SetMute(true);
                continue;
            }
            float closestDistance = Vector2.Distance(player.Center, GetTVCenter(closestTV));
            ApplyVolumeToPlayer(mpe.player, closestTV, closestDistance, $"Media Player at {mpe.Position}");
        }
    }

    /// <summary>
    /// Find the closest TV that matches the filter condition.
    /// </summary>
    private static TVTileEntity FindClosestTV(Player player, Func<TVTileEntity, bool> filter)
    {
        TVTileEntity closestTV = null;
        float closestDistance = float.MaxValue;

        foreach (var kvp in TileEntity.ByID)
        {
            if (kvp.Value is not TVTileEntity tvEntity || !filter(tvEntity))
                continue;

            Vector2 tvCenter = GetTVCenter(tvEntity);
            float distance = Vector2.Distance(player.Center, tvCenter);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTV = tvEntity;
            }
        }

        return closestTV;
    }

    /// <summary>
    /// Find the closest TV for each channel.
    /// </summary>
    private static Dictionary<int, (TVTileEntity TV, float Distance)> FindClosestTVPerChannel(Player player, Func<TVTileEntity, bool> filter)
    {
        var channelClosestTV = new Dictionary<int, (TVTileEntity TV, float Distance)>();

        foreach (var kvp in TileEntity.ByID)
        {
            if (kvp.Value is not TVTileEntity tvEntity || !filter(tvEntity))
                continue;

            int channelId = tvEntity.CurrentChannel;
            Vector2 tvCenter = GetTVCenter(tvEntity);
            float distance = Vector2.Distance(player.Center, tvCenter);

            if (!channelClosestTV.TryGetValue(channelId, out (TVTileEntity TV, float Distance) value) || distance < value.Distance)
                channelClosestTV[channelId] = (tvEntity, distance);
        }

        return channelClosestTV;
    }

    /// <summary>
    /// Calculate and apply volume to a media player based on TV distance and settings.
    /// </summary>
    private void ApplyVolumeToPlayer(VideoPlayerCore player, TVTileEntity tv, float distance, string debugLabel)
    {
        int distanceVolume = CalculateVolumeFromDistance(distance);

        int targetVolume = (int)(distanceVolume * (tv.Volume / 100f) * CalamityUtils.CircOutEasing(Main.soundVolume, 1));
        targetVolume = Math.Clamp(targetVolume, 0, 100);

        int currentVolume = player.GetVolume();
        int smoothedVolume = (int)(currentVolume * 0.7f + targetVolume * 0.3f);

        if (Math.Abs(smoothedVolume - currentVolume) > 2)
        {
            player.SetVolume(smoothedVolume);

            if (_frameCounter % 120 == 0 && smoothedVolume > 0)
            {
                CalRemix.instance.Logger.Debug(
                    $"{debugLabel}: Volume {smoothedVolume}% " +
                    $"(closest TV at {distance:F0} units)"
                );
            }
        }

        if (targetVolume > 0)
            player.SetMute(false);
        else
            player.SetMute(true);
    }

    /// <summary>
    /// Mute all channels that don't have active TVs.
    /// </summary>
    private static void MuteInactiveChannels(VideoChannelManager manager, Dictionary<int, (TVTileEntity TV, float Distance)> activeChannels)
    {
        for (int channelId = VideoChannelManager.MIN_CHANNEL; channelId <= VideoChannelManager.MAX_CHANNEL; channelId++)
        {
            if (!activeChannels.ContainsKey(channelId))
            {
                var channelPlayer = manager.GetChannelPlayer(channelId);
                if (channelPlayer != null && channelPlayer.IsPlaying)
                    channelPlayer.SetMute(true);
            }
        }
    }

    /// <summary>
    /// Get the world center position of a TV.
    /// </summary>
    private static Vector2 GetTVCenter(TVTileEntity tvEntity)
    {
        if (TVTileEntity.TileData.TryGetValue(Main.tile[tvEntity.Position.X, tvEntity.Position.Y].TileType, out var tileInfo))
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

        float t = (distance - FULL_VOLUME_DISTANCE) / (MAX_HEARING_DISTANCE - FULL_VOLUME_DISTANCE);

        return (int)(100 * (1 - t));
    }

    public override void OnWorldUnload()
    {
        _frameCounter = 0;
    }
}