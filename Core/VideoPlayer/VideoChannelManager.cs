using CalRemix.Content.Tiles.TVs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalRemix.Core.VideoPlayer;

/// <summary>
/// Manages shared video channels that can be accessed by multiple players/tiles.
/// Each channel has its own VideoPlayerCore instance.
/// </summary>
public class VideoChannelManager : ModSystem
{
    private readonly Dictionary<int, VideoPlayerCore> _channels = [];
    private readonly HashSet<int> _activeChannels = []; // Channels being used this frame

    public const int DEFAULT_CHANNEL = 0;
    public const int MIN_CHANNEL = 0;
    public const int MAX_CHANNEL = 99;

    public VideoPlayerCore GetChannel(int channelId)
    {
        if (channelId < MIN_CHANNEL || channelId > MAX_CHANNEL)
        {
            CalRemix.instance.Logger.Warn($"Invalid channel ID: {channelId}");
            return null;
        }

        return _channels.TryGetValue(channelId, out var player) ? player : null;
    }

    public VideoPlayerCore GetOrCreateChannel(int channelId)
    {
        if (channelId < MIN_CHANNEL || channelId > MAX_CHANNEL)
        {
            CalRemix.instance.Logger.Warn($"Invalid channel ID: {channelId}");
            return null;
        }

        if (!_channels.TryGetValue(channelId, out var player))
        {
            player = new VideoPlayerCore(1280, 720);
            _channels[channelId] = player;
            CalRemix.instance.Logger.Info($"Created video channel {channelId}");
        }

        _activeChannels.Add(channelId);

        return player;
    }

    public void PlayOnChannel(int channelId, string input)
    {
        var channel = GetOrCreateChannel(channelId);
        if (channel != null)
        {
            channel.Play(input);
            CalRemix.instance.Logger.Info($"Playing on channel {channelId}: {input}");
        }
    }

    public void StopChannel(int channelId)
    {
        var channel = GetChannel(channelId);
        if (channel != null)
        {
            channel.Stop();
            CalRemix.instance.Logger.Info($"Stopped channel {channelId}");
        }
    }

    public void PauseChannel(int channelId) => GetChannel(channelId)?.Pause();
    public void ResumeChannel(int channelId) => GetChannel(channelId)?.Resume();
    public IEnumerable<int> GetActiveChannels() => _channels.Keys;
    public bool ChannelExists(int channelId) => _channels.ContainsKey(channelId);

    public void RemoveChannel(int channelId)
    {
        if (_channels.TryGetValue(channelId, out var player))
        {
            player.Dispose();
            _channels.Remove(channelId);
            CalRemix.instance.Logger.Info($"Removed channel {channelId}");
        }
    }

    /// <summary>
    /// Check if any TVs are still using a specific channel.
    /// </summary>
    public static bool IsChannelInUse(int channelId)
    {
        foreach (var kvp in TileEntity.ByID)
        {
            if (kvp.Value is TVTileEntity tvEntity && tvEntity.CurrentChannel == channelId && tvEntity.IsOn)
            {
                return true;
            }
        }
        return false;
    }

    #region ModSystem Overrides

    public override void OnWorldLoad()
    {
        CalRemix.instance.Logger.Info("VideoChannelManager: World loaded");
    }

    public override void OnWorldUnload()
    {
        foreach (var kvp in _channels)
        {
            kvp.Value.Dispose();
        }
        _channels.Clear();
        _activeChannels.Clear();

        CalRemix.instance.Logger.Info("VideoChannelManager: All channels disposed");
    }

    public override void PostUpdateEverything()
    {
        // Clear active channels from previous frame
        _activeChannels.Clear();

        // PERFORMANCE: Only update channels that are playing or paused
        foreach (var kvp in _channels)
        {
            if (kvp.Value.IsLoading || kvp.Value.IsPlaying || kvp.Value.IsPaused)
            {
                kvp.Value.Update(Main.gameTimeCache);
            }
        }
    }

    #endregion

    #region Save/Load

    public override void SaveWorldData(TagCompound tag)
    {
        var channelData = new List<TagCompound>();

        foreach (var kvp in _channels)
        {
            int channelId = kvp.Key;
            VideoPlayerCore player = kvp.Value;

            if (!string.IsNullOrEmpty(player.CurrentVideoPath))
            {
                channelData.Add(new TagCompound
                {
                    ["channelId"] = channelId,
                    ["videoPath"] = player.CurrentVideoPath,
                    ["position"] = player.GetPosition(),
                    ["volume"] = player.GetVolume()
                });
            }
        }

        tag["channels"] = channelData;
        CalRemix.instance.Logger.Info($"Saved {channelData.Count} channels");
    }

    public override void LoadWorldData(TagCompound tag)
    {
        if (tag.ContainsKey("channels"))
        {
            var channelData = tag.GetList<TagCompound>("channels");

            foreach (var data in channelData)
            {
                int channelId = data.GetInt("channelId");
                string videoPath = data.GetString("videoPath");
                float position = data.GetFloat("position");
                int volume = data.GetInt("volume");

                var channel = GetOrCreateChannel(channelId);
                if (channel != null)
                {
                    channel.SetVolume(volume);

                    EventHandler readyHandler = null;
                    readyHandler = (sender, e) =>
                    {
                        if (position > 0.01f)
                        {
                            channel.Seek(position);
                            CalRemix.instance.Logger.Info($"Restored channel {channelId} to position {position:P0}");
                        }

                        channel.PlaybackReady -= readyHandler;
                    };

                    channel.PlaybackReady += readyHandler;
                    channel.Play(videoPath);
                }
            }

            CalRemix.instance.Logger.Info($"Loaded {channelData.Count} channels");
        }
    }

    #endregion
}